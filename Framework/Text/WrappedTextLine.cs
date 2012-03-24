#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Text
{
    /// <summary>
    /// WrappedText.Wrap() の呼び出しで分割された行を表します。
    /// </summary>
    public struct WrappedTextLine
    {
        /// <summary>
        /// 元の文字列における行の文字列の開始インデックス。
        /// </summary>
        public int StartIndex;

        /// <summary>
        /// 行の文字数。
        /// </summary>
        public int Length;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="startIndex">元の文字列における行の文字列の開始インデックス。</param>
        /// <param name="length">行の文字数。</param>
        public WrappedTextLine(int startIndex, int length)
        {
            StartIndex = startIndex;
            Length = length;
        }
    }
}
