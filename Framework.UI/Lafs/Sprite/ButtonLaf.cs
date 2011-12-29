﻿#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    public class ButtonLaf : SpriteControlLafBase
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            var button = control as Controls.Button;
            if (button == null) return;

            var foregroundColor = button.ForegroundColor * drawContext.Opacity;
            var bounds = drawContext.Bounds;

            if (button.MouseHovering)
            {
                // TODO: 色を汎用的に指定するにはどうしたらよいだろうか？
                drawContext.SpriteBatch.Draw(drawContext.FillTexture, bounds, foregroundColor * 0.5f);
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
                    bounds, font, button.Text, button.TextHorizontalAlignment, button.TextVerticalAlignment) + offset;
                drawContext.SpriteBatch.DrawString(font, button.Text, position, foregroundColor);
            }
        }
    }
}
