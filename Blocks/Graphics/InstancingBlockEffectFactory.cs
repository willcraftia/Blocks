#region Using

using System;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    /// <summary>
    /// InstancingBlockEffect を生成する IBlockEffectFactory の実装クラスです。
    /// </summary>
    public sealed class InstancingBlockEffectFactory : IBlockEffectFactory
    {
        /// <summary>
        /// InstancingBlockEffect に設定する複製元となる Effect を取得または設定します。
        /// </summary>
        public Effect Effect { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="effect">
        /// InstancingBlockEffect に設定する複製元となる Effect を指定します。
        /// すなわち、この Effect はインスタンシングに対応した Effect でなければなりません。
        /// </param>
        public InstancingBlockEffectFactory(Effect effect)
        {
            if (effect == null) throw new ArgumentNullException("effect");
            Effect = effect;
        }

        // I/F
        public IBlockEffect CreateBlockEffect()
        {
            return new InstancingBlockEffect(Effect);
        }
    }
}
