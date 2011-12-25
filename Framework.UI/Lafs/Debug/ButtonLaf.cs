#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public class ButtonLaf : DebugControlLafBase
    {
        public override void Draw(Control control, Rectangle renderBounds)
        {
            var button = control as Controls.Button;
            if (button == null) return;

            // 背景色で塗り潰します。
            //SpriteBatch.Draw(Source.UIContext.FillTexture, renderBounds, button.BackgroundColor * 1.0f);
            //var c = button.BackgroundColor;
            //SpriteBatch.Draw(Source.UIContext.FillTexture, renderBounds, Color.FromNonPremultiplied(c.R, c.G, c.B, 100));

            // 少し小さくした領域を半透明黒で覆います (ブレンドしつつ枠を作ります)。
            //var inBounds = renderBounds;
            //inBounds.X += 2;
            //inBounds.Y += 2;
            //inBounds.Width -= 4;
            //inBounds.Height -= 4;
            //SpriteBatch.Draw(Source.UIContext.FillTexture, inBounds, Color.Black * 0.8f);

            if (button.MouseHovering)
            {
                // TODO: 色を汎用的に指定するにはどうしたらよいだろうか？
                SpriteBatch.Draw(Source.UIContext.FillTexture, renderBounds, button.ForegroundColor * 0.5f);
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
