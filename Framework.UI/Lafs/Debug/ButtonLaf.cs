#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public class ButtonLaf : DebugControlLafBase
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            var button = control as Controls.Button;
            if (button == null) return;

            var spriteBatch = drawContext.SpriteBatch;

            var opacity = drawContext.Opacity;
            var bounds = drawContext.Bounds;

            // 枠のために白で塗り潰します。
            spriteBatch.Draw(drawContext.FillTexture, drawContext.Bounds, Color.White * drawContext.Opacity);

            // 背景色で塗り潰します。
            // 少し小さくした領域を背景色で覆います。
            var inBounds = drawContext.Bounds;
            inBounds.X += 2;
            inBounds.Y += 2;
            inBounds.Width -= 4;
            inBounds.Height -= 4;
            spriteBatch.Draw(drawContext.FillTexture, inBounds, null, button.BackgroundColor * opacity);

            var foregroundColor = button.ForegroundColor * opacity;

            if (button.MouseHovering)
            {
                // TODO: 色を汎用的に指定するにはどうしたらよいだろうか？
                spriteBatch.Draw(drawContext.FillTexture, bounds, foregroundColor * 0.5f);
            }

            if (!string.IsNullOrEmpty(button.Text))
            {
                var offset = Vector2.Zero;
                if (button.Pressed)
                {
                    offset.X += 2;
                    offset.Y += 2;
                }
                var font = button.Font ?? button.Screen.Font;
                var position = TextHelper.CalculateTextPosition(
                    bounds, font, button.Text, button.FontStretch, button.TextHorizontalAlignment, button.TextVerticalAlignment) + offset;
                spriteBatch.DrawString(font, button.Text, position, foregroundColor, 0, Vector2.Zero, button.FontStretch, SpriteEffects.None, 0);
            }
        }
    }
}
