#region Using

using System;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    /// <summary>
    /// BlockMeshFactory が IBlockEffect の生成を委譲するクラスのインタフェースです。
    /// </summary>
    /// <remarks>
    /// BlockMeshFactory が IBlockEffect を生成する際の Storategy として振る舞います。
    /// </remarks>
    public interface IBlockEffectFactory
    {
        /// <summary>
        /// IBlockEffect を生成します。
        /// </summary>
        /// <returns>生成された IBlockEffect。</returns>
        IBlockEffect CreateBlockEffect();
    }
}
