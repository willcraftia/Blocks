#region Using

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    public class WrappedText
    {
        Vector2 measuredSize;

        float maxMeasuredHeight;

        bool measuredSizeValid;

        string text;

        SpriteFont font;

        Vector2 fontStretch;

        float containerWidth;

        char[] charArray;

        StringBuilder builder;

        List<WrappedTextElement> elements = new List<WrappedTextElement>();

        public string Text
        {
            get { return text; }
            set
            {
                if (text == value) return;

                text = value;
                charArray = (text != null) ? text.ToCharArray() : null;
                measuredSizeValid = false;
            }
        }

        public SpriteFont Font
        {
            get { return font; }
            set
            {
                if (font == value) return;

                font = value;
                measuredSizeValid = false;
            }
        }

        public Vector2 FontStretch
        {
            get { return fontStretch; }
            set
            {
                if (value.X <= 0 || value.Y <= 0) throw new ArgumentOutOfRangeException("value");

                fontStretch = value;
                measuredSizeValid = false;
            }
        }

        public float ContainerWidth
        {
            get { return containerWidth; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value");

                if (containerWidth == value) return;

                containerWidth = value;
                measuredSizeValid = false;
            }
        }

        public Vector2 MeasuredSize
        {
            get { return measuredSize; }
        }

        public float MaxMeasuredHeight
        {
            get { return maxMeasuredHeight; }
        }

        public int LineCount
        {
            get { return elements.Count; }
        }

        public void GetLine(int lineIndex, StringBuilder builder)
        {
            var element = elements[lineIndex];
            builder.Length = 0;
            builder.Append(charArray, element.StartIndex, element.Length);
        }

        public void Wrap()
        {
            if (measuredSizeValid) return;

            measuredSize = Vector2.Zero;
            maxMeasuredHeight = 0;
            elements.Clear();

            // Font 不明、Text が空ならばサイズ 0 とします。
            if (font == null || charArray == null || charArray.Length == 0)
            {
                measuredSize = Vector2.Zero;
                measuredSizeValid = true;
                return;
            }

            // 文字数 1 ならば折り返せないため、そのまま測定して返します。
            // コンテナ幅が不明な場合も折り返せないため、そのまま測定して返します。
            if (text.Length == 1 || float.IsNaN(containerWidth))
            {
                var element = new WrappedTextElement
                {
                    StartIndex = 0,
                    Length = text.Length,
                    Size = font.MeasureString(text) * fontStretch
                };
                elements.Add(element);
                measuredSize = element.Size;
                measuredSizeValid = true;
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
                builder.Append(c);

                size = font.MeasureString(builder) * fontStretch;
                if (size.X <= containerWidth)
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
                }

                // 折り返しまでを再測定します。
                size = font.MeasureString(builder) * fontStretch;
                measuredSize.X = Math.Max(measuredSize.X, size.X);
                measuredSize.Y += size.Y;
                maxMeasuredHeight = Math.Max(maxMeasuredHeight, size.Y);
                // 折り返し範囲を記録します。
                var element = new WrappedTextElement(startIndex, charIndex - startIndex, size);
                elements.Add(element);

                builder.Length = 0;
                startIndex = charIndex;
            }
            // 最後の 1 行を測定します。
            size = font.MeasureString(builder) * fontStretch;
            measuredSize.X = Math.Max(measuredSize.X, size.X);
            measuredSize.Y += size.Y;
            maxMeasuredHeight = Math.Max(maxMeasuredHeight, size.Y);
            // 折り返し範囲を記録します。
            var lastElement = new WrappedTextElement(startIndex, charIndex - startIndex, size);
            elements.Add(lastElement);

            measuredSizeValid = true;
        }
    }
}
