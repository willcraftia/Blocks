#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs
{
    /// <summary>
    /// Control の文字列描画を助けるメソッドを定義したヘルパ クラスです。
    /// </summary>
    public static class TextHelper
    {
        /// <summary>
        /// 文字列の表示位置を計算します。
        /// </summary>
        /// <param name="bounds">文字列を表示するクライアント領域。</param>
        /// <param name="font">フォント。</param>
        /// <param name="text">文字列。</param>
        /// <param name="stretch">フォントの拡大縮小の度合い。</param>
        /// <param name="hAlign">文字列の水平方向についての配置方法。</param>
        /// <param name="vAlign">文字列の垂直方向についての配置方法。</param>
        /// <returns>算出された表示位置。</returns>
        public static Vector2 CalculateTextPosition(
            Rectangle bounds, SpriteFont font, string text, Vector2 stretch,
            HorizontalAlignment hAlign, VerticalAlignment vAlign)
        {
            if (font == null) throw new ArgumentNullException("font");
            if (text == null) throw new ArgumentNullException("text");

            Vector2 textSize = font.MeasureString(text);
            textSize.X *= stretch.X;
            textSize.Y *= stretch.Y;
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

        /// <summary>
        /// 文字列を描画します。
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch。</param>
        /// <param name="bounds">文字列を表示するクライアント領域。</param>
        /// <param name="font">フォント。</param>
        /// <param name="text">文字列。</param>
        /// <param name="stretch">フォントの拡大縮小の度合い。</param>
        /// <param name="hAlign">文字列の水平方向についての配置方法。</param>
        /// <param name="vAlign">文字列の垂直方向についての配置方法。</param>
        /// <param name="color">文字色。</param>
        /// <param name="padding">クライアント領域から文字列表示領域の間の余白。</param>
        /// <param name="offset">表示位置のオフセット。</param>
        public static void DrawString(
            SpriteBatch spriteBatch,
            Rectangle bounds, SpriteFont font, string text, Vector2 stretch,
            HorizontalAlignment hAlign, VerticalAlignment vAlign,
            Color color, Thickness padding, Vector2 offset)
        {
            var paddedBounds = bounds;
            paddedBounds.X += (int) padding.Left;
            paddedBounds.Y += (int) padding.Top;
            paddedBounds.Width -= (int) (padding.Left + padding.Right);
            paddedBounds.Height -= (int) (padding.Top + padding.Bottom);

            var position = TextHelper.CalculateTextPosition(paddedBounds, font, text, stretch, hAlign, vAlign) + offset;

            spriteBatch.DrawString(font, text, position, color, 0, Vector2.Zero, stretch, SpriteEffects.None, 0);
        }
    }
}
