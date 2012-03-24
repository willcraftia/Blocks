#region Using

using System;
using System.IO;
using System.Text;

#endregion

namespace Willcraftia.Xna.Framework.IO
{
    /// <summary>
    /// 文字列の拡張です。
    /// </summary>
    public static class StringExtension
    {
        /// <summary>
        /// 文字列の MemoryStream を生成します。
        /// 文字列は Encoding.UTF8 が仮定されます。
        /// </summary>
        /// <param name="text">文字列。</param>
        /// <returns>文字列の MemoryStream。</returns>
        public static Stream ToMemoryStream(this string text)
        {
            return ToMemoryStream(text, Encoding.UTF8);
        }

        /// <summary>
        /// 文字列の MemoryStream を生成します。
        /// </summary>
        /// <param name="text">文字列。</param>
        /// <param name="encoding">Encoding。</param>
        /// <returns>文字列の MemoryStream。</returns>
        public static Stream ToMemoryStream(this string text, Encoding encoding)
        {
            if (encoding == null) throw new ArgumentNullException("encoding");
            return new MemoryStream(encoding.GetBytes(text));
        }
    }
}
