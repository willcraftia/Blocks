#region Using

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.Text
{
    /// <summary>
    /// 文字列の折り返しを管理します。
    /// </summary>
    public class WrappedText
    {
        /// <summary>
        /// 測定されたサイズ。
        /// </summary>
        Vector2 measuredSize;

        /// <summary>
        /// 元となる文字列。
        /// </summary>
        string text;

        /// <summary>
        /// SpriteFont。
        /// </summary>
        SpriteFont font;

        /// <summary>
        /// フォントの拡大縮小の度合い。
        /// </summary>
        Vector2 fontStretch;

        /// <summary>
        /// 文字列の表示を行うクライアントの幅。
        /// </summary>
        float clientWidth;

        /// <summary>
        /// 元の文字列の char 配列。
        /// </summary>
        char[] charArray;

        /// <summary>
        /// 折り返し作業用 StringBuilder。
        /// </summary>
        StringBuilder builder;

        /// <summary>
        /// WrappedTextLine のリスト。
        /// </summary>
        List<WrappedTextLine> lines = new List<WrappedTextLine>();

        /// <summary>
        /// 元となる文字列を取得または設定します。
        /// 以前と異なる値を設定した場合
        /// WrappingValid プロパティが false に設定されます。
        /// </summary>
        public string Text
        {
            get { return text; }
            set
            {
                if (text == value) return;

                text = value;

                charArray = (text != null) ? text.Replace("\r", "").ToCharArray() : null;
                WrappingValid = false;
            }
        }

        /// <summary>
        /// 測定の基準とする SpriteFont を取得または設定します。
        /// 以前と異なる値を設定した場合
        /// WrappingValid プロパティが false に設定されます。
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
            set
            {
                if (font == value) return;

                font = value;
                WrappingValid = false;
            }
        }

        /// <summary>
        /// フォントの拡大縮小の度合いを取得または設定します。
        /// 以前と異なる値を設定した場合
        /// WrappingValid プロパティが false に設定されます。
        /// </summary>
        public Vector2 FontStretch
        {
            get { return fontStretch; }
            set
            {
                if (value.X <= 0 || value.Y <= 0) throw new ArgumentOutOfRangeException("value");

                if (fontStretch == value) return;

                fontStretch = value;
                WrappingValid = false;
            }
        }

        /// <summary>
        /// 文字列の表示を行うクライアントの幅を取得または設定します。
        /// 以前と異なる値を設定した場合
        /// WrappingValid プロパティが false に設定されます。
        /// Wrap() は、このプロパティの範囲に収まるように Text プロパティの値を行へ分割します。
        /// </summary>
        public float ClientWidth
        {
            get { return clientWidth; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value");

                if (clientWidth == value) return;

                clientWidth = value;
                WrappingValid = false;
            }
        }

        /// <summary>
        /// Wrap() で測定されたサイズを取得します。
        /// これは、分割された全ての行を表示するために必要なサイズが返されます。
        /// 返されるサイズの幅は、ClientWidth プロパティの値を越えることはありません。
        /// 返されるサイズの高さは、LineCount * MaxMeasuredHeight になります。
        /// </summary>
        public Vector2 MeasuredSize
        {
            get { return measuredSize; }
        }

        /// <summary>
        /// Wrap() の結果として設定されるプロパティの値が有効かどうかを示す値を取得します。
        /// このプロパティは、Wrap() の呼び出しで true に設定されます。
        /// </summary>
        /// <value>
        /// true (MeasuredSize プロパティの値が有効な場合)、false (それ以外の場合)。
        /// </value>
        public bool WrappingValid { get; private set; }

        /// <summary>
        /// 最も高さの長い行の高さを取得します。
        /// </summary>
        public float MaxMeasuredHeight { get; private set; }

        /// <summary>
        /// 分割された行の数を返します。
        /// </summary>
        public int LineCount
        {
            get { return lines.Count; }
        }

        /// <summary>
        /// 分割された行の文字列を取得します。
        /// このメソッドでは、指定した StringBuilder の Length プロパティを 0 に設定してから、
        /// 指定の行の文字列を StringBuilder へ設定します。
        /// </summary>
        /// <param name="lineIndex">対象とする行のインデックス。</param>
        /// <param name="builder">文字列の取得先とする StringBuilder。</param>
        public void GetLineText(int lineIndex, StringBuilder builder)
        {
            var line = lines[lineIndex];
            builder.Length = 0;
            builder.Append(charArray, line.StartIndex, line.Length);
        }

        /// <summary>
        /// 文字列の折り返しを行います。
        /// このメソッドの呼び出しにより、WrappingValid プロパティが true に設定されます。
        /// このメソッドでは、LF (\n) を見つけた場合、LF を基準とした分割をまず行います。
        /// なお、このメソッドは CR (\r) には対応しておらず、
        /// Text プロパティへの設定において CR を除去した文字列を内部で管理しています。
        /// 続いて、このメソッドでは、空白文字を基準とした分割を行います。
        /// ここで、このメソッドが認識する空白文字は半角空白文字のみであり、
        /// 全角空白文字には対応しません。
        /// 最後に、基準にできる空白文字が見つからない場合は、
        /// ClientWidth プロパティが示す幅に収まるように強制的に分割します。
        /// </summary>
        public void Wrap()
        {
            if (WrappingValid) return;

            measuredSize = Vector2.Zero;
            MaxMeasuredHeight = 0;
            lines.Clear();

            // Font 不明、Text が空ならばサイズ 0 とします。
            if (font == null || charArray == null || charArray.Length == 0)
            {
                measuredSize = Vector2.Zero;
                WrappingValid = true;
                return;
            }

            // 文字数 1 ならば折り返せないため、そのまま測定して返します。
            // コンテナ幅が不明な場合も折り返せないため、そのまま測定して返します。
            if (text.Length == 1 || clientWidth <= 0 || float.IsNaN(clientWidth))
            {
                lines.Add(new WrappedTextLine(0, text.Length));
                measuredSize = font.MeasureString(text) * fontStretch;
                WrappingValid = true;
                return;
            }

            if (builder == null) builder = new StringBuilder();
            builder.Length = 0;

            int charIndex = 0;
            int startIndex = 0;
            int lastSpaceIndex = -1;
            Vector2 size = new Vector2();
            while (charIndex < charArray.Length)
            {
                var c = charArray[charIndex];

                // LF を見つけたら強制折り返しさせます。
                // LF ではない場合は文字を追加して測定し、改行するかどうかを判定します。
                if (c == '\n')
                {
                    lastSpaceIndex = -1;
                }
                else
                {
                    builder.Append(c);

                    size = font.MeasureString(builder) * fontStretch;
                    // clientWidth 未満ならばその行を継続します。
                    if (size.X <= clientWidth)
                    {
                        if (c == ' ') lastSpaceIndex = charIndex;
                        charIndex++;
                        continue;
                    }

                    if (0 <= lastSpaceIndex)
                    {
                        // スペースを見つけていたならば、その最後のスペースの位置まで戻します。
                        charIndex = lastSpaceIndex + 1;
                        builder.Length = charIndex - startIndex;
                        lastSpaceIndex = -1;
                    }
                    else
                    {
                        // スペースを見つけていないならば、1 文字前に戻します。
                        charIndex--;
                        
                        if (charIndex == 0)
                        {
                            // 改行できないほどに ClientWidth が小さいならば、
                            // 改行処理を中断します。
                            measuredSize = Vector2.Zero;
                            WrappingValid = true;
                            return;
                        }

                        // 対象文字についても 1 文字前に戻します。
                        builder.Length = builder.Length - 1;
                    }
                }

                // 行を測定します。
                // 改行のみが行われたなどの空行の場合は測定をスキップします。
                if (0 < builder.Length)
                {
                    size = font.MeasureString(builder) * fontStretch;
                    measuredSize.X = Math.Max(measuredSize.X, size.X);
                    MaxMeasuredHeight = Math.Max(MaxMeasuredHeight, size.Y);
                }
                // 折り返し範囲を記録します。
                lines.Add(new WrappedTextLine(startIndex, charIndex - startIndex));
                // LF で折り返した場合はインデックスを進めます。
                if (c == '\n') charIndex++;

                builder.Length = 0;
                startIndex = charIndex;
            }
            // 最後の 1 行を測定します。
            size = font.MeasureString(builder) * fontStretch;
            measuredSize.X = Math.Max(measuredSize.X, size.X);
            MaxMeasuredHeight = Math.Max(MaxMeasuredHeight, size.Y);
            // 折り返し範囲を記録します。
            var lastElement = new WrappedTextLine(startIndex, charIndex - startIndex);
            lines.Add(lastElement);

            // 最大の行の高さから行全体の高さを決定します。
            measuredSize.Y = lines.Count * MaxMeasuredHeight;

            WrappingValid = true;
        }
    }
}
