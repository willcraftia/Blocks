#region Using

using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs
{
    public class TextBlockLookAndFeel : ILookAndFeel
    {
        public virtual void Draw(Control control, IDrawContext drawContext)
        {
            var textBlock = control as TextBlock;
            if (textBlock == null) return;
            if (string.IsNullOrEmpty(textBlock.Text)) return;

            // TODO
            if (textBlock.TextWrapping == TextWrapping.Wrap)
            {
                DrawWrappedText(textBlock, drawContext);
                return;
            }

            var font = textBlock.Font ?? textBlock.Screen.Font;
            if (font == null) return;

            var clientBounds = new Rect(control.RenderSize);
            var outlineWidth = textBlock.TextOutlineWidth;

            var hAlign = textBlock.TextHorizontalAlignment;
            var vAlign = textBlock.TextVerticalAlignment;

            var shadowOffset = textBlock.ShadowOffset;
            if (shadowOffset.X != 0 || shadowOffset.Y != 0)
            {
                drawContext.DrawString(
                    new Rect(control.RenderSize), font, textBlock.Text, control.FontStretch,
                    hAlign, vAlign, control.BackgroundColor, control.Padding, shadowOffset);
            }

            if (0 < outlineWidth)
            {
                drawContext.DrawString(
                    clientBounds, font, textBlock.Text, control.FontStretch,
                    hAlign, vAlign, control.BackgroundColor, control.Padding,
                    new Vector2(-outlineWidth, -outlineWidth));
                drawContext.DrawString(
                    clientBounds, font, textBlock.Text, control.FontStretch,
                    hAlign, vAlign, control.BackgroundColor, control.Padding,
                    new Vector2(-outlineWidth, outlineWidth));
                drawContext.DrawString(
                    clientBounds, font, textBlock.Text, control.FontStretch,
                    hAlign, vAlign, control.BackgroundColor, control.Padding,
                    new Vector2(outlineWidth, -outlineWidth));
                drawContext.DrawString(
                    clientBounds, font, textBlock.Text, control.FontStretch,
                    hAlign, vAlign, control.BackgroundColor, control.Padding,
                    new Vector2(outlineWidth, outlineWidth));
            }

            drawContext.DrawString(
                new Rect(control.RenderSize), font, textBlock.Text, control.FontStretch,
                hAlign, vAlign, control.ForegroundColor, control.Padding);
        }

        StringBuilder builder = new StringBuilder();

        void DrawWrappedText(TextBlock textBlock, IDrawContext drawContext)
        {

            var font = textBlock.Font ?? textBlock.Screen.Font;
            if (font == null) return;

            var clientBounds = new Rect(textBlock.RenderSize);
            clientBounds.Height = textBlock.WrappedText.MaxMeasuredHeight;



            var outlineWidth = textBlock.TextOutlineWidth;
            var hAlign = textBlock.TextHorizontalAlignment;
            var vAlign = textBlock.TextVerticalAlignment;
            var shadowOffset = textBlock.ShadowOffset;



            var wrappedText = textBlock.WrappedText;
            for (int i = 0; i < wrappedText.LineCount; i++)
            {
                wrappedText.GetLine(i, builder);





                if (shadowOffset.X != 0 || shadowOffset.Y != 0)
                {
                    drawContext.DrawString(
                        clientBounds, font, builder, textBlock.FontStretch,
                        hAlign, vAlign, textBlock.BackgroundColor, textBlock.Padding, shadowOffset);
                }

                if (0 < outlineWidth)
                {
                    drawContext.DrawString(
                        clientBounds, font, builder, textBlock.FontStretch,
                        hAlign, vAlign, textBlock.BackgroundColor, textBlock.Padding,
                        new Vector2(-outlineWidth, -outlineWidth));
                    drawContext.DrawString(
                        clientBounds, font, builder, textBlock.FontStretch,
                        hAlign, vAlign, textBlock.BackgroundColor, textBlock.Padding,
                        new Vector2(-outlineWidth, outlineWidth));
                    drawContext.DrawString(
                        clientBounds, font, builder, textBlock.FontStretch,
                        hAlign, vAlign, textBlock.BackgroundColor, textBlock.Padding,
                        new Vector2(outlineWidth, -outlineWidth));
                    drawContext.DrawString(
                        clientBounds, font, builder, textBlock.FontStretch,
                        hAlign, vAlign, textBlock.BackgroundColor, textBlock.Padding,
                        new Vector2(outlineWidth, outlineWidth));
                }

                drawContext.DrawString(
                    clientBounds, font, builder, textBlock.FontStretch,
                    hAlign, vAlign, textBlock.ForegroundColor, textBlock.Padding);

                
                
                
                
                
                
                
                
                
                
                clientBounds.Y += textBlock.WrappedText.MaxMeasuredHeight;
                if (textBlock.RenderSize.Height <= clientBounds.Y) break;
            }
        }
    }
}
