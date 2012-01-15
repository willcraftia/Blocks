#region Using

using System;
using System.IO;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// Block を提供するリソースにアクセスするためのインタフェースです。
    /// </summary>
    public interface IBlockResource
    {
        /// <summary>
        /// Block を提供する Stream を取得します。
        /// Stream の Close は呼び出し側の責務です。
        /// </summary>
        /// <returns>Block を提供する Stream。</returns>
        Stream GetStream();
    }
}
