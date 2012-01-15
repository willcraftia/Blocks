#region Using

using System;
using System.IO;
using System.Text;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// 文字列を Stream とする IBlockResource の実装クラスです。
    /// </summary>
    public sealed class StringBlockResource : IBlockResource
    {
        /// <summary>
        /// 文字列を取得します。
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Encoding を取得します。
        /// </summary>
        public Encoding Encoding { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// 文字列の Encoding は UTF-8 が仮定されます。
        /// </summary>
        /// <param name="text">Stream の元とする文字列。</param>
        public StringBlockResource(string text) : this(text, Encoding.UTF8) { }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="text">Stream の元とする文字列。</param>
        /// <param name="encoding">文字列の Encoding。</param>
        public StringBlockResource(string text, Encoding encoding)
        {
            if (text == null) throw new ArgumentNullException("text");
            if (encoding == null) throw new ArgumentNullException("encoding");
            Text = text;
            Encoding = encoding;
        }

        // I/F
        public Stream GetStream()
        {
            return new MemoryStream(Encoding.GetBytes(Text));
        }
    }
}
