#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    /// <summary>
    /// Button 用の LaF です。
    /// </summary>
    public class ButtonLaf : DefaultControlLaf
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            base.Draw(control, drawContext);

            var button = control as Controls.Button;
            if (button == null) return;

            var spriteBatch = drawContext.SpriteBatch;
            var opacity = drawContext.Opacity;
            var bounds = drawContext.Bounds;
            var foregroundColor = button.ForegroundColor * opacity;

            if (button.MouseHovering)
            {
                // TODO: 色を汎用的に指定するにはどうしたらよいだろうか？
                spriteBatch.Draw(Source.FillTexture, bounds, foregroundColor * 0.5f);
            }

            var font = button.Font ?? button.Screen.Font;

            if (font != null && !string.IsNullOrEmpty(button.Text))
            {
                var offset = Vector2.Zero;
                if (button.Pressed)
                {
                    offset.X += 2;
                    offset.Y += 2;
                }

                var padding = button.Padding;
                var paddedBounds = bounds;
                paddedBounds.X += (int) padding.Left;
                paddedBounds.Y += (int) padding.Top;
                paddedBounds.Width -= (int) (padding.Left + padding.Right);
                paddedBounds.Height -= (int) (padding.Top + padding.Bottom);

                var position = TextHelper.CalculateTextPosition(
                    paddedBounds, font, button.Text, button.FontStretch, button.TextHorizontalAlignment, button.TextVerticalAlignment) + offset;

                spriteBatch.DrawString(font, button.Text, position, foregroundColor, 0, Vector2.Zero, button.FontStretch, SpriteEffects.None, 0);
            }
        }
    }
}
