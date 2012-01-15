#region Using

using System;
using System.IO;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// コンストラクタで指定する Stream をそのまま Stream として返す IBlockResource の実装クラスです。
    /// </summary>
    public sealed class DirectBlockResource : IBlockResource
    {
        /// <summary>
        /// コンストラクタで指定された Stream。
        /// </summary>
        Stream stream;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="stream">Stream。</param>
        public DirectBlockResource(Stream stream)
        {
            if (stream == null) throw new ArgumentNullException("stream");
            this.stream = stream;
        }

        // I/F
        public Stream GetStream()
        {
            return stream;
        }
    }
}
