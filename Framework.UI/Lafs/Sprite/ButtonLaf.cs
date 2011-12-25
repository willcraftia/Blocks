#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    public class ButtonLaf : SpriteControlLafBase
    {
        public override void Draw(Control control, Rectangle renderBounds)
        {
            var button = control as Controls.Button;
            if (button == null) return;

            if (button.MouseHovering)
            {
                // TODO: 色を汎用的に指定するにはどうしたらよいだろうか？
                SpriteBatch.Draw(Source.UIContext.FillTexture, renderBounds, Color.FromNonPremultiplied(255, 255, 255, 50));
            }

            if (!string.IsNullOrEmpty(button.Text))
            {
                var offset = Vector2.Zero;
                if (button.Pressed)
                {
                    offset.X += 2;
                    offset.Y += 2;
                }
                var font = button.Font ?? Source.Font;
                var position = TextHelper.CalculateTextPosition(
                    renderBounds, font, button.Text, button.TextHorizontalAlignment, button.TextVerticalAlignment) + offset;
                SpriteBatch.DrawString(font, button.Text, position, button.ForegroundColor);
            }
        }
    }
}
