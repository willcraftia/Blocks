#region Using

using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs
{
    /// <summary>
    /// TextBlock の ILookAndFeel です。
    /// </summary>
    public class TextBlockLookAndFeel : ILookAndFeel
    {
        /// <summary>
        /// TextBlock の TextWrapping プロパティが Wrap の場合に使用する作業用 StringBuilder。
        /// </summary>
        StringBuilder builder;

        public virtual void Draw(Control control, IDrawContext drawContext)
        {
            var textBlock = control as TextBlock;
            if (textBlock == null) return;
            if (string.IsNullOrEmpty(textBlock.Text)) return;

            if (textBlock.TextWrapping == TextWrapping.Wrap)
            {
                DrawWrappedText(textBlock, drawContext);
            }
            else
            {
                DrawText(textBlock, drawContext);
            }
        }

        void DrawText(TextBlock textBlock, IDrawContext drawContext)
        {
            var font = textBlock.Font ?? textBlock.Screen.Font;
            if (font == null) return;

            var text = textBlock.Text;
            var stretch = textBlock.FontStretch;
            var padding = textBlock.Padding;
            var fColor = textBlock.ForegroundColor;
            var bColor = textBlock.BackgroundColor;
            var bounds = new Rect(textBlock.RenderSize);
            var outlineWidth = textBlock.TextOutlineWidth;
            var hAlign = textBlock.TextHorizontalAlignment;
            var vAlign = textBlock.TextVerticalAlignment;

            // 影を描画します。
            var shadowOffset = textBlock.ShadowOffset;
            if (shadowOffset.X != 0 || shadowOffset.Y != 0)
            {
                drawContext.DrawString(bounds, font, text, stretch, hAlign, vAlign, bColor, padding,
                    shadowOffset);
            }

            // 文字枠を描画します。
            if (0 < outlineWidth)
            {
                drawContext.DrawString(bounds, font, text, stretch, hAlign, vAlign, bColor, padding,
                    new Vector2(-outlineWidth, -outlineWidth));
                drawContext.DrawString(bounds, font, text, stretch, hAlign, vAlign, bColor, padding,
                    new Vector2(-outlineWidth, outlineWidth));
                drawContext.DrawString(bounds, font, text, stretch, hAlign, vAlign, bColor, padding,
                    new Vector2(outlineWidth, -outlineWidth));
                drawContext.DrawString(bounds, font, text, stretch, hAlign, vAlign, bColor, padding,
                    new Vector2(outlineWidth, outlineWidth));
            }

            // 文字を描画します。
            drawContext.DrawString(bounds, font, text, stretch, hAlign, vAlign, fColor, padding);
        }

        void DrawWrappedText(TextBlock textBlock, IDrawContext drawContext)
        {
            if (builder == null) builder = new StringBuilder();

            var font = textBlock.Font ?? textBlock.Screen.Font;
            if (font == null) return;

            var stretch = textBlock.FontStretch;
            var padding = textBlock.Padding;
            var fColor = textBlock.ForegroundColor;
            var bColor = textBlock.BackgroundColor;
            var outlineWidth = textBlock.TextOutlineWidth;
            var hAlign = textBlock.TextHorizontalAlignment;
            var vAlign = textBlock.TextVerticalAlignment;
            var shadowOffset = textBlock.ShadowOffset;

            var bounds = new Rect(textBlock.RenderSize);
            bounds.Height = textBlock.WrappedText.MaxMeasuredHeight;

            var wrappedText = textBlock.WrappedText;
            for (int i = 0; i < wrappedText.LineCount; i++)
            {
                // 行の文字列を取得します。
                wrappedText.GetLineText(i, builder);

                // 影を描画します。
                if (shadowOffset.X != 0 || shadowOffset.Y != 0)
                {
                    drawContext.DrawString(bounds, font, builder, stretch, hAlign, vAlign, bColor, padding,
                        shadowOffset);
                }

                // 文字枠を描画します。
                if (0 < outlineWidth)
                {
                    drawContext.DrawString(bounds, font, builder, stretch, hAlign, vAlign, bColor, padding,
                        new Vector2(-outlineWidth, -outlineWidth));
                    drawContext.DrawString(bounds, font, builder, stretch, hAlign, vAlign, bColor, padding,
                        new Vector2(-outlineWidth, outlineWidth));
                    drawContext.DrawString(bounds, font, builder, stretch, hAlign, vAlign, bColor, padding,
                        new Vector2(outlineWidth, -outlineWidth));
                    drawContext.DrawString(bounds, font, builder, stretch, hAlign, vAlign, bColor, padding,
                        new Vector2(outlineWidth, outlineWidth));
                }

                // 文字を描画します。
                drawContext.DrawString(bounds, font, builder, stretch, hAlign, vAlign, fColor, padding);

                // 描画領域を次の行の位置へ進めます。
                bounds.Y += textBlock.WrappedText.MaxMeasuredHeight;
                // 描画対象の領域を完全に越える行は描画しません。
                if (textBlock.RenderSize.Height <= bounds.Y) break;
            }
        }
    }
}
