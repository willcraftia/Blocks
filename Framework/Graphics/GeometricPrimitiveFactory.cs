#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// GeometricPrimitive の生成を担う抽象ファクトリです。
    /// </summary>
    public abstract class GeometricPrimitiveFactory
    {
        /// <summary>
        /// 頂点バッファを作成するための一時データを管理するクラスです。
        /// </summary>
        protected class VertexSource : IDisposable
        {
            /// <summary>
            /// 頂点の位置のリスト。
            /// </summary>
            internal List<VertexPositionNormal> Vertices = new List<VertexPositionNormal>();

            /// <summary>
            /// 頂点のインデックスのリスト。
            /// </summary>
            internal List<ushort> Indices = new List<ushort>();

            /// <summary>
            /// 追加する頂点のインデックス。
            /// </summary>
            public int CurrentVertex
            {
                get { return Vertices.Count; }
            }

            // I/F
            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }

            /// <summary>
            /// 頂点を追加します。
            /// </summary>
            /// <param name="position">頂点の位置。</param>
            /// <param name="normal">頂点の法線。</param>
            public void AddVertex(Vector3 position, Vector3 normal)
            {
                Vertices.Add(new VertexPositionNormal(position, normal));
            }

            /// <summary>
            /// インデックスを追加します。
            /// </summary>
            /// <param name="index">インデックス。</param>
            public void AddIndex(int index)
            {
                Indices.Add((ushort) index);
            }
        }

        /// <summary>
        /// GeometricPrimitive を生成します。
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice。</param>
        /// <returns>生成された GeometricPrimitive。</returns>
        public GeometricPrimitive Create(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null) throw new ArgumentNullException("graphicsDevice");

            // 頂点データ作成用の一時データは、using を用いてそれら作成後に破棄するようにします。
            using (var builder = new VertexSource())
            {
                // サブクラスの実装で VertexSource に頂点データを詰めてもらいます。
                Initialize(builder);

                // サブクラスの実装が VertexSource に詰めた頂点データで GeometricPrimitive を構築します。
                var vertices = builder.Vertices.ToArray();
                var indices = builder.Indices.ToArray();
                return new GeometricPrimitive(graphicsDevice, vertices, indices);
            }
        }

        /// <summary>
        /// GeometricPrimitive の構築に用いる頂点データを VertexSource へ設定します。
        /// </summary>
        /// <param name="source"></param>
        protected abstract void Initialize(VertexSource source);
    }
}
