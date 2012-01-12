#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    using MeshVertexSource = VertexSource<VertexPositionNormal, ushort>;
    using SurfaceVertexSource = VertexSource<VertexPositionNormal, ushort>;

    /// <summary>
    /// 永続化用データ表現の Block から 3D モデル描画のための BlockModel を生成するクラスです。
    /// </summary>
    public sealed class BlockModelFactory
    {
        #region ResolvedElement

        /// <summary>
        /// グリッド内位置をキーとして Element を管理するコレクションです。
        /// </summary>
        class PositionedElementCollection : KeyedCollection<Position, Element>
        {
            protected override Position GetKeyForItem(Element item)
            {
                return item.Position;
            }
        }

        #endregion

        #region ResolvedElement

        /// <summary>
        /// 他の Element との隣接状態と共に Element を管理する構造体です。
        /// </summary>
        struct ResolvedElement
        {
            /// <summary>
            /// Element を取得します。
            /// </summary>
            public Element Element { get; private set; }

            /// <summary>
            /// 各面が他の Element に隣接しているかどうかを示す値の配列。
            /// インデックスは面の方向に対応。
            /// </summary>
            /// <value>
            /// true (面が他の Element に隣接していない場合)、false (それ以外の場合)。
            /// </value>
            public bool[] SurfaceVisible;

            /// <summary>
            /// Element が他の Element で完全に囲まれているかどうかを示す値を取得します。
            /// </summary>
            /// <value>
            /// true (Element が他の Element で完全に囲まれている場合)、false (それ以外の場合)。
            /// </value>
            public bool Enclosed
            {
                get
                {
                    return !SurfaceVisible[0] && !SurfaceVisible[1] &&
                        !SurfaceVisible[2] && !SurfaceVisible[3] &&
                        !SurfaceVisible[4] && !SurfaceVisible[5];
                }
            }

            /// <summary>
            /// インスタンスを生成します。
            /// </summary>
            /// <param name="element">Element。</param>
            ResolvedElement(Element element)
                : this()
            {
                Element = element;
                SurfaceVisible = new bool[6];
            }

            /// <summary>
            /// 指定の Element コレクション内について、指定の Element を解析します。
            /// </summary>
            /// <param name="block">Element を保持する Block。</param>
            /// <param name="elements">Element コレクション。</param>
            /// <param name="target">解析対象の Element。</param>
            /// <returns>解析結果の ResolvedElement。</returns>
            public static ResolvedElement Resolve(Block block, PositionedElementCollection elements, Element target)
            {
                var resolvedElement = new ResolvedElement(target);

                var targetPosition = target.Position;
                var testPosition = targetPosition;

                testPosition.X = targetPosition.X + 1;
                resolvedElement.SurfaceVisible[(int) CubeSurfaces.East] = !elements.Contains(testPosition);

                testPosition.X = targetPosition.X - 1;
                resolvedElement.SurfaceVisible[(int) CubeSurfaces.West] = !elements.Contains(testPosition);

                testPosition.X = targetPosition.X;

                testPosition.Y = targetPosition.Y + 1;
                resolvedElement.SurfaceVisible[(int) CubeSurfaces.Top] = !elements.Contains(testPosition);

                testPosition.Y = targetPosition.Y - 1;
                resolvedElement.SurfaceVisible[(int) CubeSurfaces.Bottom] = !elements.Contains(testPosition);

                testPosition.Y = targetPosition.Y;

                testPosition.Z = targetPosition.Z + 1;
                resolvedElement.SurfaceVisible[(int) CubeSurfaces.North] = !elements.Contains(testPosition);

                testPosition.Z = targetPosition.Z - 1;
                resolvedElement.SurfaceVisible[(int) CubeSurfaces.South] = !elements.Contains(testPosition);

                return resolvedElement;
            }
        }

        #endregion

        #region BlockMesh

        /// <summary>
        /// Material ごとに ResolvedElement をまとめるクラスです。
        /// </summary>
        class BlockMesh
        {
            /// <summary>
            /// 参照する Material のインデックス。
            /// </summary>
            public int MaterialIndex;

            /// <summary>
            /// ResolvedElement のリスト。
            /// </summary>
            public List<ResolvedElement> ResolvedElements = new List<ResolvedElement>();

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
        /// ResolvedElement を BlockMesh へ分類するクラスです。
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
            /// インスタンスを生成します。
            /// </summary>
            ElementClassifier() { }

            /// <summary>
            /// Element を分類します。
            /// </summary>
            /// <param name="elements">分類する Element を持つ Block。</param>
            /// <returns>分類結果を管理する ElementClassifier。</returns>
            public static ElementClassifier ClassifyElements(Block block)
            {
                var instance = new ElementClassifier();

                // グリッド位置で参照できる Element のコレクションを生成します。
                // これは ResolvedElement の処理で、特定の位置に Element が存在するかどうかを調べる際に、
                // リストに対する全件検索ではなく、高速化のためにハッシュ テーブルに対する検索とするためです。
                var positionedElements = new PositionedElementCollection();
                foreach (var element in block.Elements) positionedElements.Add(element);

                foreach (var element in block.Elements)
                {
                    // 面の結合状態を解析します。
                    var resolvedElement = ResolvedElement.Resolve(block, positionedElements, element);

                    // 立方体が完全に囲まれているのではないならば分類を開始します。
                    if (!resolvedElement.Enclosed) instance.Classify(resolvedElement);
                }

                return instance;
            }

            /// <summary>
            /// ResolvedElement を BlockMesh へ分類します。
            /// </summary>
            /// <param name="resolvedElement">分類する ResolvedElement。</param>
            void Classify(ResolvedElement resolvedElement)
            {
                // 対象とする BlockMesh のリストを取得します。
                List<BlockMesh> meshList;
                if (!MeshListMap.TryGetValue(resolvedElement.Element.MaterialIndex, out meshList))
                {
                    meshList = new List<BlockMesh>();
                    meshList.Add(new BlockMesh(resolvedElement.Element.MaterialIndex));
                    MeshListMap[resolvedElement.Element.MaterialIndex] = meshList;
                }

                // リストの最後の BlockMesh を取得します。
                var lastMesh = meshList[meshList.Count - 1];

                // IndexBuffer の最大サイズを超えるならば、
                // 新たな BlockMesh へ Element を追加するようにします。
                // Reach Profile では 16 ビットが最大サイズ (ushort) であり、
                // また、立方体の表現に必要なインデックス数は 36 です。
                if (ushort.MaxValue < (lastMesh.ResolvedElements.Count + 1) * 36)
                {
                    lastMesh = new BlockMesh(resolvedElement.Element.MaterialIndex);
                    meshList.Add(lastMesh);
                }

                // リストの最後の BlockMesh へ Element を追加します。
                lastMesh.ResolvedElements.Add(resolvedElement);
            }
        }

        #endregion

        /// <summary>
        /// GraphicsDevice を取得します。
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// Element の立方体の辺のサイズを取得または設定します。
        /// </summary>
        public float ElementSize { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice。</param>
        public BlockModelFactory(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null) throw new ArgumentNullException("graphicsDevice");
            GraphicsDevice = graphicsDevice;

            ElementSize = 0.1f;
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

            // Element を分類します。
            var elementClassifier = ElementClassifier.ClassifyElements(block);

            // BlockModelMesh を生成して登録します。
            using (var cubeSurfaceVertexSource = new CubeSurfaceVertexSource(ElementSize))
            {
                foreach (var meshList in elementClassifier.MeshListMap.Values)
                {
                    foreach (var mesh in meshList)
                    {
                        // BlockModelMesh を生成します。
                        var modelMesh = CreateBlockModelMesh(mesh, cubeSurfaceVertexSource);
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

            return modelMaterial;
        }

        /// <summary>
        /// BlockModelMesh を生成します。
        /// </summary>
        /// <param name="mesh">BlockMesh。</param>
        /// <param name="cubeSurfaceVertexSource">立方体の面の頂点データを提供する VertexSource。</param>
        /// <returns>生成された BlockModelMesh。</returns>
        BlockModelMesh CreateBlockModelMesh(BlockMesh mesh, CubeSurfaceVertexSource cubeSurfaceVertexSource)
        {
            // BlockModelMesh 用 VertexSource に頂点データを詰めていきます。
            using (var meshVertexSource = new MeshVertexSource())
            {
                // Block は最小位置を原点とするモデルであり、一方、立方体の VertexSource は立方体の中心が原点にあるため、
                // 立方体の最小位置を原点とするための移動行列を作成し、立方体の頂点データの変換に利用します。
                Matrix elementOriginTranslation = Matrix.CreateTranslation(new Vector3(ElementSize * 0.5f));

                for (int i = 0; i < mesh.ResolvedElements.Count; i++)
                {
                    var resolvedElement = mesh.ResolvedElements[i];

                    // グリッド内位置へ移動させるための移動行列を作成します。
                    var gridPosition = resolvedElement.Element.Position.ToVector3() * ElementSize;
                    Matrix gridTransform = Matrix.CreateTranslation(gridPosition);

                    // 立方体の最終的な移動行列を作成します。
                    Matrix finalTransform;
                    Matrix.Multiply(ref elementOriginTranslation, ref gridTransform, out finalTransform);

                    // 表示すべき面の頂点データを VertexSource へ設定します。
                    for (int surfaceIndex = 0; surfaceIndex < 6; surfaceIndex++)
                    {
                        if (resolvedElement.SurfaceVisible[surfaceIndex])
                        {
                            AddSurfaceVertices(meshVertexSource, cubeSurfaceVertexSource.Surfaces[surfaceIndex], ref finalTransform);
                        }
                    }
                }

                return BlockModelMesh.Create(GraphicsDevice, meshVertexSource.Vertices.ToArray(), meshVertexSource.Indices.ToArray());
            }
        }

        /// <summary>
        /// 面の頂点データをメッシュの頂点データへ設定します。
        /// </summary>
        /// <param name="meshVertexSource">メッシュの頂点データを提供する VertexSource。</param>
        /// <param name="surfaceVertexSource">面の頂点データを提供する VertexSource。</param>
        /// <param name="transform">設定前に適用する変換行列。</param>
        void AddSurfaceVertices(MeshVertexSource meshVertexSource, SurfaceVertexSource surfaceVertexSource, ref Matrix transform)
        {
            var startIndex = meshVertexSource.Vertices.Count;
            var vertexSource = surfaceVertexSource;
            foreach (var index in vertexSource.Indices)
            {
                meshVertexSource.Indices.Add((ushort) (startIndex + index));
            }
            for (int i = 0; i < vertexSource.Vertices.Count; i++)
            {
                var cubeVertex = vertexSource.Vertices[i];
                Vector3 transformedVertexPosition;
                Vector3.Transform(ref cubeVertex.Position, ref transform, out transformedVertexPosition);
                meshVertexSource.Vertices.Add(new VertexPositionNormal(transformedVertexPosition, cubeVertex.Normal));
            }
        }
    }
}
