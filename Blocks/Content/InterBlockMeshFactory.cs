﻿#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Blocks.Graphics;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    using MeshPartVertexSource = VertexSource<VertexPositionNormal, ushort>;
    using SurfaceVertexSource = VertexSource<VertexPositionNormal, ushort>;

    /// <summary>
    /// 永続化用データ表現の Block から、
    /// BlockMesh の頂点情報を保持する InterBlockMesh を生成するクラスです。
    /// </summary>
    public sealed class InterBlockMeshFactory
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

                testPosition.Z = targetPosition.Z - 1;
                resolvedElement.SurfaceVisible[(int) CubeSurfaces.North] = !elements.Contains(testPosition);

                testPosition.Z = targetPosition.Z + 1;
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
            Dictionary<int, List<Part>> partListMap = new Dictionary<int, List<Part>>();

            /// <summary>
            /// 全ての Part を保持するリスト。
            /// </summary>
            public List<Part> Parts = new List<Part>();

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

                // Parts プロパティへ全ての Part を追加します。
                foreach (var partList in instance.partListMap.Values)
                {
                    foreach (var part in partList) instance.Parts.Add(part);
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
                if (!partListMap.TryGetValue(resolvedElement.Element.MaterialIndex, out partList))
                {
                    partList = new List<Part>();
                    partList.Add(new Part(resolvedElement.Element.MaterialIndex));
                    partListMap[resolvedElement.Element.MaterialIndex] = partList;
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
        /// 生成する InterBlockMesh で扱う LOD のサイズを取得します。
        /// </summary>
        public int LODSize { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="lodSize">LOD のサイズ。</param>
        public InterBlockMeshFactory(int lodSize)
        {
            if (lodSize < 1 || InterBlock.MaxDetailLevelSize < lodSize) throw new ArgumentOutOfRangeException("lodSize");
            LODSize = lodSize;
        }

        /// <summary>
        /// InterBlockMesh を生成します。
        /// </summary>
        /// <param name="block">Block。</param>
        /// <param name="lodSize">LOD のサイズ。</param>
        /// <returns>生成された InterBlockMesh。</returns>
        public InterBlockMesh Create(Block block)
        {
            // 中間データを作成します。
            var interBlocks = InterBlock.CreateInterBlock(block, LODSize);

            // 中間データから InterBlockMesh を作成します。
            return CreateInterBlockMesh(interBlocks);
        }

        /// <summary>
        /// InterBlockMesh を生成します。
        /// </summary>
        /// <param name="lodBlocks">各 LOD の InterBlock を要素とした配列。</param>
        /// <returns>生成された BlockMesh。</returns>
        InterBlockMesh CreateInterBlockMesh(InterBlock[] lodBlocks)
        {
            // InterBlockMesh を生成します。
            var mesh = new InterBlockMesh(lodBlocks.Length);

            // InterBlockEffect を生成して登録します。
            // LOD 間で Material は共有しているので、最大 LOD の Material から生成します。
            var effects = new InterBlockEffect[lodBlocks[0].Materials.Count];
            for (int i = 0; i < effects.Length; i++)
            {
                effects[i] = CreateInterBlockEffect(lodBlocks[0].Materials[i]);
            }
            mesh.SetEffectArray(effects);

            // 各 LOD ごとに InterBlockMeshPart を生成して InterBlockMesh へ設定します。
            for (int lod = 0; lod < lodBlocks.Length; lod++)
            {
                // Element を分類します。
                var elementClassifier = ElementClassifier.Classify(lodBlocks[lod].Elements);

                // Parts が空となる LOD の場合は、上位 LOD で十分に頂点数が少ないとみなし、
                // 上位の InterBlockMesh を設定します。
                // なお、最大 LOD の InterBlockMeshPart が空ではないことを仮定します。
                if (elementClassifier.Parts.Count == 0)
                {
                    mesh.InheritLODMeshParts(lod);
                    continue;
                }

                // InterBlockMeshPart を生成して登録します。
                var cubeSurfaceVertexSource = new CubeSurfaceVertexSource(lodBlocks[lod].ElementSize);

                var meshParts = new InterBlockMeshPart[elementClassifier.Parts.Count];
                for (int i = 0; i < meshParts.Length; i++)
                {
                    var part = elementClassifier.Parts[i];
                    // InterBlockMeshPart を生成します。
                    meshParts[i] = CreateBlockMeshPart(part, cubeSurfaceVertexSource, lodBlocks[lod].ElementSize);
                    // 参照する Effect のインデックスを設定します。
                    meshParts[i].EffectIndex = part.MaterialIndex;
                }

                // 生成した InterBlockMeshPart を指定の LOD で InterBlockMesh へ設定します。
                mesh.SetLODMeshPartArray(lod, meshParts);
            }

            return mesh;
        }

        /// <summary>
        /// 指定の Material が持つ情報を設定した InterBlockEffect を生成します。
        /// </summary>
        /// <param name="material">Material。</param>
        /// <returns>生成された InterBlockEffect。</returns>
        InterBlockEffect CreateInterBlockEffect(Material material)
        {
            return new InterBlockEffect
            {
                DiffuseColor = material.DiffuseColor.ToColor().ToVector3(),
                EmissiveColor = material.EmissiveColor.ToColor().ToVector3(),
                SpecularColor = material.SpecularColor.ToColor().ToVector3(),
                SpecularPower = material.SpecularPower
            };
        }

        /// <summary>
        /// InterBlockMeshPart を生成します。
        /// </summary>
        /// <param name="part">Part。</param>
        /// <param name="cubeSurfaceVertexSource">立方体の面の頂点データを提供する VertexSource。</param>
        /// <param name="elementSize">Element のサイズ。</param>
        /// <returns>生成された InterBlockMeshPart。</returns>
        InterBlockMeshPart CreateBlockMeshPart(Part part, CubeSurfaceVertexSource cubeSurfaceVertexSource, float elementSize)
        {
            var meshPart = new InterBlockMeshPart();

            // Block は最小位置を原点とするモデルであり、一方、立方体の VertexSource は立方体の中心が原点にあるため、
            // 立方体の最小位置を原点とするための移動行列を作成し、立方体の頂点データの変換に利用します。
            Matrix elementOriginTranslation = Matrix.CreateTranslation(new Vector3(elementSize * 0.5f));

            for (int i = 0; i < part.ResolvedElements.Count; i++)
            {
                var resolvedElement = part.ResolvedElements[i];

                // グリッド内位置へ移動させるための移動行列を作成します。
                var gridPosition = resolvedElement.Element.Position.ToVector3() * elementSize;
                Matrix gridTranslation = Matrix.CreateTranslation(gridPosition);

                // 立方体の最終的な移動行列を作成します。
                Matrix finalTranslation;
                Matrix.Multiply(ref elementOriginTranslation, ref gridTranslation, out finalTranslation);

                // 表示すべき面の頂点データを VertexSource へ設定します。
                for (int surfaceIndex = 0; surfaceIndex < 6; surfaceIndex++)
                {
                    if (resolvedElement.SurfaceVisible[surfaceIndex])
                        AddSurfaceVertices(meshPart, cubeSurfaceVertexSource.Surfaces[surfaceIndex], ref finalTranslation);
                }
            }

            // 頂点とインデックスを変更不能に設定します。
            meshPart.Freeze();

            return meshPart;
        }

        /// <summary>
        /// 面の頂点データを InterBlockMeshPart の頂点データへ設定します。
        /// </summary>
        /// <param name="meshPart">InterBlockMeshPart。</param>
        /// <param name="surfaceVertexSource">面の頂点データを提供する VertexSource。</param>
        /// <param name="transform">設定前に適用する変換行列。</param>
        void AddSurfaceVertices(InterBlockMeshPart meshPart, SurfaceVertexSource surfaceVertexSource, ref Matrix transform)
        {
            var startIndex = meshPart.VertexSource.Vertices.Count;
            var vertexSource = surfaceVertexSource;
            foreach (var index in vertexSource.Indices)
            {
                meshPart.VertexSource.Indices.Add((ushort) (startIndex + index));
            }
            for (int i = 0; i < vertexSource.Vertices.Count; i++)
            {
                var cubeVertex = vertexSource.Vertices[i];
                Vector3 transformedVertexPosition;
                Vector3.Transform(ref cubeVertex.Position, ref transform, out transformedVertexPosition);
                meshPart.VertexSource.Vertices.Add(new VertexPositionNormal(transformedVertexPosition, cubeVertex.Normal));
            }
        }
    }
}