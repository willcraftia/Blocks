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
    using MeshPartVertexSource = VertexSource<VertexPositionNormal, ushort>;
    using SurfaceVertexSource = VertexSource<VertexPositionNormal, ushort>;

    /// <summary>
    /// 永続化用データ表現の Block から 3D モデル描画のための BlockMesh を生成するクラスです。
    /// </summary>
    public sealed class BlockMeshFactory
    {
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
            /// <param name="elements">Element コレクション。</param>
            /// <param name="target">解析対象の Element。</param>
            /// <returns>解析結果の ResolvedElement。</returns>
            public static ResolvedElement Resolve(InterElementCollection elements, Element target)
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

        #region Part

        /// <summary>
        /// Material ごとに ResolvedElement をまとめるクラスです。
        /// </summary>
        class Part
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
            public Part(int materialIndex)
            {
                MaterialIndex = materialIndex;
            }
        }

        #endregion

        #region ElementClassifier

        /// <summary>
        /// ResolvedElement を Part へ分類するクラスです。
        /// 基本的には、同じ Material を参照する ResolvedElement を 1 つの Part へ纏めますが、
        /// IndexBuffer で使えるインデックスの最大値を越える場合には、新たに作成する Part へ分類します。
        /// </summary>
        class ElementClassifier
        {
            /// <summary>
            /// Material のインデックスをキーとし、Part のリストを値とするディクショナリ。
            /// </summary>
            public Dictionary<int, List<Part>> PartsMap = new Dictionary<int, List<Part>>();

            /// <summary>
            /// インスタンスを生成します。
            /// </summary>
            ElementClassifier() { }

            /// <summary>
            /// Element を分類します。
            /// </summary>
            /// <param name="elements">Element のリスト。</param>
            /// <returns>分類結果を管理する ElementClassifier。</returns>
            public static ElementClassifier Classify(InterElementCollection elements)
            {
                var instance = new ElementClassifier();

                foreach (var element in elements)
                {
                    // 面の結合状態を解析します。
                    var resolvedElement = ResolvedElement.Resolve(elements, element);

                    // 立方体が完全に囲まれているのではないならば分類を開始します。
                    if (!resolvedElement.Enclosed) instance.Classify(resolvedElement);
                }

                return instance;
            }

            /// <summary>
            /// ResolvedElement を Part へ分類します。
            /// </summary>
            /// <param name="resolvedElement">分類する ResolvedElement。</param>
            void Classify(ResolvedElement resolvedElement)
            {
                // 対象とする Part のリストを取得します。
                List<Part> partList;
                if (!PartsMap.TryGetValue(resolvedElement.Element.MaterialIndex, out partList))
                {
                    partList = new List<Part>();
                    partList.Add(new Part(resolvedElement.Element.MaterialIndex));
                    PartsMap[resolvedElement.Element.MaterialIndex] = partList;
                }

                // リストの最後の Part を取得します。
                var part = partList[partList.Count - 1];

                // IndexBuffer の最大サイズを超えるならば、
                // 新たな Part へ Element を追加するようにします。
                // Reach Profile では 16 ビットが最大サイズ (ushort) であり、
                // また、立方体の表現に必要なインデックス数は 36 です。
                if (ushort.MaxValue < (part.ResolvedElements.Count + 1) * 36)
                {
                    part = new Part(resolvedElement.Element.MaterialIndex);
                    partList.Add(part);
                }

                // リストの最後の Part へ Element を追加します。
                part.ResolvedElements.Add(resolvedElement);
            }
        }

        #endregion

        /// <summary>
        /// GraphicsDevice を取得します。
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// IBlockEffectFactory を取得します。
        /// </summary>
        public IBlockEffectFactory BlockEffectFactory { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice。</param>
        /// <param name="blockEffectFactory">IBlockEffectFactory。</param>
        public BlockMeshFactory(GraphicsDevice graphicsDevice, IBlockEffectFactory blockEffectFactory)
        {
            if (graphicsDevice == null) throw new ArgumentNullException("graphicsDevice");
            if (blockEffectFactory == null) throw new ArgumentNullException("blockEffectFactory");
            GraphicsDevice = graphicsDevice;
            BlockEffectFactory = blockEffectFactory;
        }

        /// <summary>
        /// 指定された LOD サイズの分だけ BlockMesh を生成します。
        /// </summary>
        /// <param name="block">Block。</param>
        /// <param name="levelSize">LOD のサイズ。</param>
        /// <returns>生成された BlockMesh の配列。</returns>
        public BlockMesh[] CreateBlockMeshes(Block block, int levelSize)
        {
            if (levelSize < 1 || InterBlock.MaxDetailLevelSize < levelSize) throw new ArgumentOutOfRangeException("levelSize");

            var meshes = new BlockMesh[levelSize];

            var interBlocks = InterBlock.CreateInterBlock(block, levelSize);
            for (int i = 0; i < levelSize; i++) meshes[i] = CreateBlockMesh(interBlocks[i]);

            return meshes;
        }

        /// <summary>
        /// BlockMesh を生成します。
        /// </summary>
        /// <param name="block">InterBlock。</param>
        /// <returns>生成された BlockMesh。</returns>
        BlockMesh CreateBlockMesh(InterBlock block)
        {
            // BlockMesh を生成します。
            var mesh = new BlockMesh();

            // IBlockEffect を生成して登録します。
            foreach (var material in block.Materials)
            {
                var effect = CreateBlockEffect(material);
                mesh.InternalEffects.Add(effect);
            }

            // Element を分類します。
            var elementClassifier = ElementClassifier.Classify(block.Elements);

            // BlocklMeshPart を生成して登録します。
            var cubeSurfaceVertexSource = new CubeSurfaceVertexSource(block.ElementSize);
            foreach (var partList in elementClassifier.PartsMap.Values)
            {
                foreach (var part in partList)
                {
                    // BlocklMeshPart を生成します。
                    var meshPart = CreateBlockMeshPart(part, cubeSurfaceVertexSource, block.ElementSize);
                    // IBlockEffect への参照を設定します。
                    meshPart.Effect = mesh.InternalEffects[part.MaterialIndex];
                    // BlockMesh へ登録します。
                    mesh.InternalMeshParts.Add(meshPart);
                }
            }

            return mesh;
        }

        /// <summary>
        /// 指定の Material が持つ情報を設定した IBlockEffect を生成します。
        /// </summary>
        /// <param name="material">Material。</param>
        /// <returns>生成された IBlockEffect。</returns>
        IBlockEffect CreateBlockEffect(Material material)
        {
            // IBlockEffect の生成を IBlockEffectFactory へ委譲します。
            var effect = BlockEffectFactory.CreateBlockEffect();

            // Material を設定します。
            effect.DiffuseColor = material.DiffuseColor.ToColor().ToVector3();
            effect.EmissiveColor = material.EmissiveColor.ToColor().ToVector3();
            effect.SpecularColor = material.SpecularColor.ToColor().ToVector3();
            effect.SpecularPower = material.SpecularPower;

            return effect;
        }

        /// <summary>
        /// BlockMeshPart を生成します。
        /// </summary>
        /// <param name="part">Part。</param>
        /// <param name="cubeSurfaceVertexSource">立方体の面の頂点データを提供する VertexSource。</param>
        /// <returns>生成された BlockMeshPart。</returns>
        BlockMeshPart CreateBlockMeshPart(Part part, CubeSurfaceVertexSource cubeSurfaceVertexSource, float elementScale)
        {
            // BlockMeshPart 用 VertexSource に頂点データを詰めていきます。
            var meshPartVertexSource = new MeshPartVertexSource();

            // Block は最小位置を原点とするモデルであり、一方、立方体の VertexSource は立方体の中心が原点にあるため、
            // 立方体の最小位置を原点とするための移動行列を作成し、立方体の頂点データの変換に利用します。
            Matrix elementOriginTranslation = Matrix.CreateTranslation(new Vector3(elementScale * 0.5f));

            for (int i = 0; i < part.ResolvedElements.Count; i++)
            {
                var resolvedElement = part.ResolvedElements[i];

                // グリッド内位置へ移動させるための移動行列を作成します。
                var gridPosition = resolvedElement.Element.Position.ToVector3() * elementScale;
                Matrix gridTranslation = Matrix.CreateTranslation(gridPosition);

                // 立方体の最終的な移動行列を作成します。
                Matrix finalTranslation;
                Matrix.Multiply(ref elementOriginTranslation, ref gridTranslation, out finalTranslation);

                // 表示すべき面の頂点データを VertexSource へ設定します。
                for (int surfaceIndex = 0; surfaceIndex < 6; surfaceIndex++)
                {
                    if (resolvedElement.SurfaceVisible[surfaceIndex])
                    {
                        AddSurfaceVertices(meshPartVertexSource, cubeSurfaceVertexSource.Surfaces[surfaceIndex], ref finalTranslation);
                    }
                }
            }

            return BlockMeshPart.Create(GraphicsDevice, meshPartVertexSource.Vertices.ToArray(), meshPartVertexSource.Indices.ToArray());
        }

        /// <summary>
        /// 面の頂点データを BlockMeshPart の頂点データへ設定します。
        /// </summary>
        /// <param name="meshPartVertexSource">BlockMeshPart の頂点データを提供する VertexSource。</param>
        /// <param name="surfaceVertexSource">面の頂点データを提供する VertexSource。</param>
        /// <param name="transform">設定前に適用する変換行列。</param>
        void AddSurfaceVertices(MeshPartVertexSource meshPartVertexSource, SurfaceVertexSource surfaceVertexSource, ref Matrix transform)
        {
            var startIndex = meshPartVertexSource.Vertices.Count;
            var vertexSource = surfaceVertexSource;
            foreach (var index in vertexSource.Indices)
            {
                meshPartVertexSource.Indices.Add((ushort) (startIndex + index));
            }
            for (int i = 0; i < vertexSource.Vertices.Count; i++)
            {
                var cubeVertex = vertexSource.Vertices[i];
                Vector3 transformedVertexPosition;
                Vector3.Transform(ref cubeVertex.Position, ref transform, out transformedVertexPosition);
                meshPartVertexSource.Vertices.Add(new VertexPositionNormal(transformedVertexPosition, cubeVertex.Normal));
            }
        }
    }
}
