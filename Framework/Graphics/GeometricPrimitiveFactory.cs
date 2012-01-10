#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// GeometricPrimitive の生成を担う抽象クラスです。
    /// </summary>
    public abstract class GeometricPrimitiveFactory
    {
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
