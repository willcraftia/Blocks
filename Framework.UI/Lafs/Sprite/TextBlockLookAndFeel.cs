#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    public class TextBlockLookAndFeel : LookAndFeelBase
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            var textBlock = control as Controls.TextBlock;
            if (textBlock == null) return;
            if (string.IsNullOrEmpty(textBlock.Text)) return;

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
    }
}
