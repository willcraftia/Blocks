#region Using

using System;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Blocks.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// BasicBlockEffect を生成する IBlockEffectFactory の実装クラスです。
    /// </summary>
    public sealed class BasicBlockEffectFactory : IBlockEffectFactory
    {
        /// <summary>
        /// GraphicsDevice を取得します。
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="graphicsDevice">GraphicsDevice。</param>
        public BasicBlockEffectFactory(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null) throw new ArgumentNullException("graphicsDevice");
            GraphicsDevice = graphicsDevice;
        }

        // I/F
        public IBlockEffect CreateBlockEffect()
        {
            return new BasicBlockEffect(GraphicsDevice);
        }
    }
}
