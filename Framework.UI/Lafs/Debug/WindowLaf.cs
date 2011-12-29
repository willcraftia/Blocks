#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public class WindowLaf : DebugControlLafBase
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            var window = control as Controls.Window;
            if (window == null) return;

            // 枠のために白で塗り潰します。
            drawContext.SpriteBatch.Draw(drawContext.FillTexture, drawContext.Bounds, Color.White * drawContext.Opacity);

            // 少し小さくした領域を背景色で覆います。
            var inBounds = drawContext.Bounds;
            inBounds.X += 2;
            inBounds.Y += 2;
            inBounds.Width -= 4;
            inBounds.Height -= 4;
            drawContext.SpriteBatch.Draw(drawContext.FillTexture, inBounds, window.BackgroundColor * drawContext.Opacity);
        }
    }
}
