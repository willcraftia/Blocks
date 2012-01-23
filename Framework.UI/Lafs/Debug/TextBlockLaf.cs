#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public class TextBlockLaf : DefaultControlLaf
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            base.Draw(control, drawContext);

            var textBlock = control as Controls.TextBlock;
            if (textBlock == null) return;

            var spriteBatch = drawContext.SpriteBatch;
            var foregroundColor = textBlock.ForegroundColor * drawContext.Opacity;
            var bounds = drawContext.Bounds;
            var font = textBlock.Font ?? textBlock.Screen.Font;

            if (font != null && !string.IsNullOrEmpty(textBlock.Text))
            {
                var padding = textBlock.Padding;
                var paddedBounds = bounds;
                paddedBounds.X += (int) padding.Left;
                paddedBounds.Y += (int) padding.Top;
                paddedBounds.Width -= (int) (padding.Left + padding.Right);
                paddedBounds.Height -= (int) (padding.Top + padding.Bottom);

                var position = TextHelper.CalculateTextPosition(
                    paddedBounds, font, textBlock.Text, textBlock.FontStretch,
                    textBlock.TextHorizontalAlignment, textBlock.TextVerticalAlignment);

                spriteBatch.DrawString(
                    font, textBlock.Text, position, foregroundColor, 0, Vector2.Zero,
                    textBlock.FontStretch, SpriteEffects.None, 0);
            }
        }
    }
}
