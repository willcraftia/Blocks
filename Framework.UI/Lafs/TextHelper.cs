#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs
{
    public static class TextHelper
    {
        public static Vector2 CalculateTextPosition(Rectangle bounds, SpriteFont font, string text, HorizontalAlignment hAlign, VerticalAlignment vAlign)
        {
            if (font == null) throw new ArgumentNullException("font");
            if (text == null) throw new ArgumentNullException("text");

            Vector2 textSize = font.MeasureString(text);
            float x;
            float y;

            switch (hAlign)
            {
                case HorizontalAlignment.Left:
                    {
                        x = bounds.Left;
                        break;
                    }
                case HorizontalAlignment.Right:
                    {
                        x = bounds.Right - textSize.X;
                        break;
                    }
                case HorizontalAlignment.Center:
                default:
                    {
                        x = (bounds.Width - textSize.X) / 2.0f + bounds.Left;
                        break;
                    }
            }

            switch (vAlign)
            {
                case VerticalAlignment.Top:
                    {
                        y = bounds.Top;
                        break;
                    }
                case VerticalAlignment.Bottom:
                    {
                        y = bounds.Bottom - font.LineSpacing;
                        break;
                    }
                case VerticalAlignment.Center:
                default:
                    {
                        y = (bounds.Height - font.LineSpacing) / 2.0f + bounds.Top;
                        break;
                    }
            }

            return new Vector2(x, y);
        }
    }
}
