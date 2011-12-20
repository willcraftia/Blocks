#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Visuals.Debug
{
    public class ButtonLaf : DebugControlLafBase
    {
        public override void Draw(Control control)
        {
            var button = control as Controls.Button;
            if (button == null) return;

            var bounds = button.GetAbsoluteBounds();

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
                var font = button.Font ?? Source.Font;
                var position = TextHelper.CalculateTextPosition(
                    bounds, font, button.Text, button.TextHorizontalAlignment, button.TextVerticalAlignment) + offset;
                // Button の Font プロパティを無視して白で描画します (Window を黒ベースにしているので、見やすさのため)。
                SpriteBatch.DrawString(font, button.Text, position, Color.White);
            }
        }
    }
}
