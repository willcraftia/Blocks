#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    /// <summary>
    /// BlockModel の Mesh を表すクラスです。
    /// </summary>
    public sealed class BlockModelMesh
    {
        /// <summary>
        /// Geometric Primitive (Cube Primitive を仮定) を取得します。
        /// </summary>
        public GeometricPrimitive GeometricPrimitive { get; internal set; }

        /// <summary>
        /// BlockModel に対する変換行列。
        /// </summary>
        public Matrix Transform { get; set; }

        /// <summary>
        /// 参照する BlockModelMaterial のインデックス。
        /// </summary>
        public int MaterialIndex { get; internal set; }

        /// <summary>
        /// インスタンスを生成します (内部処理用)。
        /// </summary>
        /// <param name="geometricPrimitive"></param>
        internal BlockModelMesh(GeometricPrimitive geometricPrimitive)
        {
            if (geometricPrimitive == null) throw new ArgumentNullException("geometricPrimitive");
            this.GeometricPrimitive = geometricPrimitive;

            Transform = Matrix.Identity;
        }

        /// <summary>
        /// 描画します。
        /// </summary>
        /// <param name="effect">Effect。</param>
        public void Draw(Effect effect)
        {
            GeometricPrimitive.Draw(effect);
        }
    }
}
