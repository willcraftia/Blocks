#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Visuals.Sprite
{
    public class ButtonLaf : ControlLafBase
    {
        public ButtonLaf(SpriteControlLafSource source)
            : base(source)
        {
        }

        public override void Draw(Control control)
        {
            var button = control as Controls.Button;
            if (button == null) return;

            var bounds = button.GetAbsoluteBounds();

            SpriteBatch.Begin();

            if (button.MouseHovering)
            {
                // TODO: 色を汎用的に指定するにはどうしたらよいだろうか？
                SpriteBatch.Draw(Source.UIContext.FillTexture, bounds, Color.FromNonPremultiplied(255, 255, 255, 50));
            }

            if (!string.IsNullOrEmpty(button.Text))
            {
                var offset = Vector2.Zero;
                if (button.Pressed)
                {
                    offset.X += 2;
                    offset.Y += 2;
                }
                Source.DrawString(bounds, button.Font, button.FontColor, button.Text, button.TextHorizontalAlignment, button.TextVerticalAlignment, offset);
            }

            SpriteBatch.End();
        }
    }
}
