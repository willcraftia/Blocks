#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public class LabelLaf : DebugControlLafBase
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            var label = control as Controls.Label;
            if (label == null) return;

            var spriteBatch = drawContext.SpriteBatch;

            var foregroundColor = label.ForegroundColor * drawContext.Opacity;
            var bounds = drawContext.Bounds;
            var font = label.Font ?? label.Screen.Font;

            if (font != null && !string.IsNullOrEmpty(label.Text))
            {
                var padding = label.Padding;
                var paddedBounds = bounds;
                paddedBounds.X += (int) padding.Left;
                paddedBounds.Y += (int) padding.Top;
                paddedBounds.Width -= (int) (padding.Left + padding.Right);
                paddedBounds.Height -= (int) (padding.Top + padding.Bottom);

                var position = TextHelper.CalculateTextPosition(
                    paddedBounds, font, label.Text, label.FontStretch, label.TextHorizontalAlignment, label.TextVerticalAlignment);

                spriteBatch.DrawString(font, label.Text, position, foregroundColor, 0, Vector2.Zero, label.FontStretch, SpriteEffects.None, 0);
            }
        }
    }
}
