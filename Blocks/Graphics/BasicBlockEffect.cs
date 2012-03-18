#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    /// <summary>
    /// IBlockEffect のインタフェースを備えた BasicEffect です。
    /// </summary>
    public sealed class BasicBlockEffect : BasicEffect, IBlockEffect
    {
        // I/F
        public EffectPass Pass
        {
            get { return CurrentTechnique.Passes[0]; }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice。</param>
        public BasicBlockEffect(GraphicsDevice graphicsDevice)
            : base(graphicsDevice)
        {
        }
    }
}
