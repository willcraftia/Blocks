#region Using

using System;
using Willcraftia.Xna.Blocks.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// BlockMesh のロードを担うクラスへのインタフェースです。
    /// </summary>
    public interface IBlockMeshLoader
    {
        /// <summary>
        /// name が示す BlockMesh をロードします。
        /// name の仕様は、実装クラスに依存します。
        /// </summary>
        /// <param name="name">BlockMesh の name。</param>
        /// <returns>ロードされた BlockMesh。</returns>
        BlockMesh LoadBlockMesh(string name);
    }
}
