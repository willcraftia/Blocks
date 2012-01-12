#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    /// <summary>
    /// 永続化用データ表現の Block から 3D モデル描画のための BlockModel を生成するクラスです。
    /// </summary>
    public sealed class BlockModelFactory
    {
        #region BlockMesh

        /// <summary>
        /// Material ごとに Element をまとめるクラスです。
        /// </summary>
        class BlockMesh
        {
            /// <summary>
            /// 参照する Material のインデックス。
            /// </summary>
            public int MaterialIndex;

            /// <summary>
            /// Element のリスト。
            /// </summary>
            public List<Element> Elements = new List<Element>();

            /// <summary>
            /// インスタンスを生成します。
            /// </summary>
            /// <param name="materialIndex">参照する Material のインデックス。</param>
            public BlockMesh(int materialIndex)
            {
                MaterialIndex = materialIndex;
            }
        }

        #endregion

        #region ElementClassifier

        /// <summary>
        /// Element を BlockMesh へ分類するクラスです。
        /// このクラスでは、IndexBuffer で使えるインデックスの最大値を考慮し、
        /// その最大値を越える場合には、同じ Material を参照する Element であっても、
        /// 新たに作成する BlockMesh へ分類します。
        /// </summary>
        class ElementClassifier
        {
            /// <summary>
            /// Material のインデックスをキーとし、BlockMesh のリストを値とするディクショナリ。
            /// </summary>
            public Dictionary<int, List<BlockMesh>> MeshListMap = new Dictionary<int, List<BlockMesh>>();

            /// <summary>
            /// Element を BlockMesh へ分類します。
            /// </summary>
            /// <param name="element">分類する Element。</param>
            public void Classify(Element element)
            {
                // 対象とする BlockMesh のリストを取得します。
                List<BlockMesh> meshList;
                if (!MeshListMap.TryGetValue(element.MaterialIndex, out meshList))
                {
                    meshList = new List<BlockMesh>();
                    meshList.Add(new BlockMesh(element.MaterialIndex));
                    MeshListMap[element.MaterialIndex] = meshList;
                }

                // リストの最後の BlockMesh を取得します。
                var lastMesh = meshList[meshList.Count - 1];

                // IndexBuffer の最大サイズを超えるならば、
                // 新たな BlockMesh へ Element を追加するようにします。
                // Reach Profile では 16 ビットが最大サイズ (ushort) であり、
                // また、立方体の表現に必要なインデックス数は 36 です。
                if (ushort.MaxValue < (lastMesh.Elements.Count + 1) * 36)
                {
                    lastMesh = new BlockMesh(element.MaterialIndex);
                    meshList.Add(lastMesh);
                }

                // リストの最後の BlockMesh へ Element を追加します。
                lastMesh.Elements.Add(element);
            }
        }

        #endregion

        /// <summary>
        /// GraphicsDevice を取得します。
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// CubeVertexSourceFactory を取得します。
        /// </summary>
        public CubeVertexSourceFactory CubeVertexSourceFactory { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice。</param>
        public BlockModelFactory(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null) throw new ArgumentNullException("graphicsDevice");
            GraphicsDevice = graphicsDevice;

            CubeVertexSourceFactory = new CubeVertexSourceFactory();
        }

        /// <summary>
        /// BlockModel を生成します。
        /// </summary>
        /// <param name="block">Block。</param>
        /// <returns>生成された BlockModel。</returns>
        public BlockModel CreateBlockModel(Block block)
        {
            // BlockModel を生成します。
            var model = new BlockModel();

            // BlockModelMaterial を生成して登録します。
            foreach (var material in block.Materials)
            {
                var modelMaterial = CreateBlockModelMaterial(material);
                model.InternalMaterials.Add(modelMaterial);
            }

            // Element を BlockMesh へ分類します。
            var elementClassifier = new ElementClassifier();
            foreach (var element in block.Elements) elementClassifier.Classify(element);

            // BlockModelMesh を生成して BlockModel へ登録します。
            using (var vertexSource = CubeVertexSourceFactory.CreateVertexSource())
            {
                foreach (var meshList in elementClassifier.MeshListMap.Values)
                {
                    foreach (var mesh in meshList)
                    {
                        // BlockModelMesh を生成します。
                        var modelMesh = CreateBlockModelMesh(mesh, vertexSource);
                        // BlockModelMaterial への参照を設定します。
                        modelMesh.Material = model.Materials[mesh.MaterialIndex];
                        // BlockModel へ登録します。
                        model.InternalMeshes.Add(modelMesh);
                    }
                }
            }

            return model;
        }

        /// <summary>
        /// BlockModelMaterial を生成します。
        /// </summary>
        /// <param name="material">Material。</param>
        /// <returns>生成された BlockModelMaterial。</returns>
        BlockModelMaterial CreateBlockModelMaterial(Material material)
        {
            var modelMaterial = new BlockModelMaterial();

            modelMaterial.DiffuseColor = material.DiffuseColor.ToColor().ToVector3();
            modelMaterial.EmissiveColor = material.EmissiveColor.ToColor().ToVector3();
            modelMaterial.SpecularColor = material.SpecularColor.ToColor().ToVector3();
            modelMaterial.SpecularPower = material.SpecularPower;
            modelMaterial.Alpha = material.Alpha;

            return modelMaterial;
        }

        /// <summary>
        /// BlockModelMesh を生成します。
        /// </summary>
        /// <param name="mesh">BlockMesh。</param>
        /// <param name="vertexSource">立方体の頂点データを持つ VertexSource。</param>
        /// <returns>生成された BlockModelMesh。</returns>
        BlockModelMesh CreateBlockModelMesh(BlockMesh mesh, VertexSource<VertexPositionNormal, ushort> vertexSource)
        {
            // BlockModelMesh 用 VertexSource に頂点データを詰めていきます。
            using (var meshVertexSource = new VertexSource<VertexPositionNormal, ushort>())
            {
                var elementSize = CubeVertexSourceFactory.Size;

                // Block は最小位置を原点とするモデルであり、一方、立方体の VertexSource は立方体の中心が原点にあるため、
                // 立方体の最小位置を原点とするための移動行列を作成し、立方体の頂点データの変換に利用します。
                Matrix elementOriginTranslation = Matrix.CreateTranslation(new Vector3(elementSize * 0.5f));

                for (int i = 0; i < mesh.Elements.Count; i++)
                {
                    var element = mesh.Elements[i];

                    // グリッド内位置へ移動させるための移動行列を作成します。
                    var gridPosition = new Vector3(element.Position.X, element.Position.Y, element.Position.Z) * elementSize;
                    Matrix gridTransform = Matrix.CreateTranslation(gridPosition);

                    // 立方体の最終的な移動行列を作成します。
                    Matrix finalTransform;
                    Matrix.Multiply(ref elementOriginTranslation, ref gridTransform, out finalTransform);

                    // インデックスを追加します。
                    var startIndex = meshVertexSource.Vertices.Count;
                    foreach (var index in vertexSource.Indices)
                    {
                        meshVertexSource.Indices.Add((ushort) (startIndex + index));
                    }

                    // 頂点位置を変換させつつ立方体の頂点データを BlockModelMesh 用 VertexSource に追加します。
                    for (int cubeVertexIndex = 0; cubeVertexIndex < vertexSource.Vertices.Count; cubeVertexIndex++)
                    {
                        var cubeVertex = vertexSource.Vertices[cubeVertexIndex];
                        Vector3 transformedVertexPosition;
                        Vector3.Transform(ref cubeVertex.Position, ref finalTransform, out transformedVertexPosition);
                        meshVertexSource.Vertices.Add(new VertexPositionNormal(transformedVertexPosition, cubeVertex.Normal));
                    }
                }

                return BlockModelMesh.Create(GraphicsDevice, meshVertexSource.Vertices.ToArray(), meshVertexSource.Indices.ToArray());
            }
        }
    }
}
