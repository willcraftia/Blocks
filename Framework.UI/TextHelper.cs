#region Using

using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Control の文字列描画を助けるメソッドを定義したヘルパ クラスです。
    /// </summary>
    public static class TextHelper
    {
        /// <summary>
        /// 文字列の表示位置を計算します。
        /// </summary>
        /// <param name="clientBounds">文字列を表示するクライアント領域。</param>
        /// <param name="font">フォント。</param>
        /// <param name="text">文字列。</param>
        /// <param name="stretch">フォントの拡大縮小の度合い。</param>
        /// <param name="hAlign">文字列の水平方向についての配置方法。</param>
        /// <param name="vAlign">文字列の垂直方向についての配置方法。</param>
        /// <returns>算出された表示位置。</returns>
        public static Vector2 CalculateTextPosition(
            Rectangle clientBounds, SpriteFont font, string text, Vector2 stretch,
            HorizontalAlignment hAlign, VerticalAlignment vAlign)
        {
            if (font == null) throw new ArgumentNullException("font");
            if (text == null) throw new ArgumentNullException("text");

            Vector2 textSize = font.MeasureString(text) * stretch;
            int lineSpacing = (int) ((float) font.LineSpacing * stretch.Y);

            float x;
            float y;

            switch (hAlign)
            {
                case HorizontalAlignment.Left:
                    x = clientBounds.Left;
                    break;
                case HorizontalAlignment.Right:
                    x = clientBounds.Right - textSize.X;
                    break;
                case HorizontalAlignment.Center:
                default:
                    x = (clientBounds.Width - textSize.X) / 2 + clientBounds.Left;
                    break;
            }

            switch (vAlign)
            {
                case VerticalAlignment.Top:
                    y = clientBounds.Top;
                    break;
                case VerticalAlignment.Bottom:
                    y = clientBounds.Bottom - lineSpacing;
                    break;
                case VerticalAlignment.Center:
                default:
                    y = (clientBounds.Height - lineSpacing) / 2 + clientBounds.Top;
                    break;
            }

            return new Vector2(x, y);
        }

        /// <summary>
        /// 文字列の表示位置を計算します。
        /// </summary>
        /// <param name="clientBounds">文字列を表示するクライアント領域。</param>
        /// <param name="font">フォント。</param>
        /// <param name="text">文字列。</param>
        /// <param name="stretch">フォントの拡大縮小の度合い。</param>
        /// <param name="hAlign">文字列の水平方向についての配置方法。</param>
        /// <param name="vAlign">文字列の垂直方向についての配置方法。</param>
        /// <returns>算出された表示位置。</returns>
        public static Vector2 CalculateTextPosition(
            Rectangle clientBounds, SpriteFont font, StringBuilder text, Vector2 stretch,
            HorizontalAlignment hAlign, VerticalAlignment vAlign)
        {
            if (font == null) throw new ArgumentNullException("font");
            if (text == null) throw new ArgumentNullException("text");

            Vector2 textSize = font.MeasureString(text) * stretch;
            int lineSpacing = (int) ((float) font.LineSpacing * stretch.Y);

            float x;
            float y;

            switch (hAlign)
            {
                case HorizontalAlignment.Left:
                    x = clientBounds.Left;
                    break;
                case HorizontalAlignment.Right:
                    x = clientBounds.Right - textSize.X;
                    break;
                case HorizontalAlignment.Center:
                default:
                    x = (clientBounds.Width - textSize.X) / 2 + clientBounds.Left;
                    break;
            }

            switch (vAlign)
            {
                case VerticalAlignment.Top:
                    y = clientBounds.Top;
                    break;
                case VerticalAlignment.Bottom:
                    y = clientBounds.Bottom - lineSpacing;
                    break;
                case VerticalAlignment.Center:
                default:
                    y = (clientBounds.Height - lineSpacing) / 2 + clientBounds.Top;
                    break;
            }

            return new Vector2(x, y);
        }

        /// <summary>
        /// 文字列を描画します。
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch。</param>
        /// <param name="clientBounds">文字列を表示するクライアント領域。</param>
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
            Rectangle clientBounds, SpriteFont font, string text, Vector2 stretch,
            HorizontalAlignment hAlign, VerticalAlignment vAlign,
            Color color, Thickness padding, Vector2 offset)
        {
            var paddedBounds = clientBounds;
            paddedBounds.X += (int) padding.Left;
            paddedBounds.Y += (int) padding.Top;
            paddedBounds.Width -= (int) (padding.Left + padding.Right);
            paddedBounds.Height -= (int) (padding.Top + padding.Bottom);

            var position = TextHelper.CalculateTextPosition(paddedBounds, font, text, stretch, hAlign, vAlign) + offset;

            spriteBatch.DrawString(font, text, position, color, 0, Vector2.Zero, stretch, SpriteEffects.None, 0);
        }

        /// <summary>
        /// 文字列を描画します。
        /// </summary>
        /// <param name="spriteBatch">SpriteBatch。</param>
        /// <param name="clientBounds">文字列を表示するクライアント領域。</param>
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
            Rectangle clientBounds, SpriteFont font, StringBuilder text, Vector2 stretch,
            HorizontalAlignment hAlign, VerticalAlignment vAlign,
            Color color, Thickness padding, Vector2 offset)
        {
            var paddedBounds = clientBounds;
            paddedBounds.X += (int) padding.Left;
            paddedBounds.Y += (int) padding.Top;
            paddedBounds.Width -= (int) (padding.Left + padding.Right);
            paddedBounds.Height -= (int) (padding.Top + padding.Bottom);

            var position = TextHelper.CalculateTextPosition(paddedBounds, font, text, stretch, hAlign, vAlign) + offset;

            spriteBatch.DrawString(font, text, position, color, 0, Vector2.Zero, stretch, SpriteEffects.None, 0);
        }
    }
}
