#region Using

using System;
using System.Text;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public class DebugTextBlockLookAndFeel : ILookAndFeel
    {
        /// <summary>
        /// TextBlock の TextWrapping プロパティが Wrap の場合に使用する作業用 StringBuilder。
        /// </summary>
        StringBuilder builder;

        // I/F
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

            // 文字色を白として描画します。
            drawContext.DrawString(
                new Rect(textBlock.RenderSize), font, textBlock.Text, textBlock.FontStretch,
                textBlock.TextHorizontalAlignment, textBlock.TextVerticalAlignment,
                Color.White, textBlock.Padding);
        }

        void DrawWrappedText(TextBlock textBlock, IDrawContext drawContext)
        {
            if (builder == null) builder = new StringBuilder();

            var font = textBlock.Font ?? textBlock.Screen.Font;
            if (font == null) return;

            var bounds = new Rect(textBlock.RenderSize);
            bounds.Height = textBlock.WrappedText.MaxMeasuredHeight;

            var wrappedText = textBlock.WrappedText;
            for (int i = 0; i < wrappedText.LineCount; i++)
            {
                // 行の文字列を取得します。
                wrappedText.GetLineText(i, builder);

                // 空行は文字列描画をスキップします。
                if (builder.Length != 0)
                {
                    // 文字を描画します。
                    drawContext.DrawString(bounds, font, builder, textBlock.FontStretch,
                        textBlock.TextHorizontalAlignment, textBlock.TextVerticalAlignment,
                        Color.White, textBlock.Padding);
                }

                // 描画領域を次の行の位置へ進めます。
                bounds.Y += textBlock.WrappedText.MaxMeasuredHeight;
                // 描画対象の領域を完全に越える行は描画しません。
                if (textBlock.RenderSize.Height <= bounds.Y) break;
            }
        }
    }
}
