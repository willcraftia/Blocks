#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    public class ButtonLaf : SpriteControlLafBase
    {
        public override void Draw(Control control, Rectangle renderBounds, float totalOpacity)
        {
            var button = control as Controls.Button;
            if (button == null) return;

            var foregroundColor = button.ForegroundColor * totalOpacity;

            if (button.MouseHovering)
            {
                // TODO: 色を汎用的に指定するにはどうしたらよいだろうか？
                SpriteBatch.Draw(Source.UIContext.FillTexture, renderBounds, foregroundColor * 0.5f);
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
                SpriteBatch.DrawString(font, button.Text, position, foregroundColor);
            }
        }
    }
}
