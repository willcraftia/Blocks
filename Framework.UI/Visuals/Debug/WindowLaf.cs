#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Visuals.Debug
{
    public class WindowLaf : DebugControlLafBase
    {
        public override void Draw(Control control)
        {
            var window = control as Controls.Window;
            if (window == null) return;

            var bounds = window.GetAbsoluteBounds();

            // 背景色で塗り潰します。
            SpriteBatch.Draw(Source.UIContext.FillTexture, bounds, window.BackgroundColor);

            // 少し小さくした領域を半透明黒で覆います (ブレンドしつつ枠を作ります)。
            var inBounds = bounds;
            inBounds.X += 2;
            inBounds.Y += 2;
            inBounds.Width -= 4;
            inBounds.Height -= 4;
            SpriteBatch.Draw(Source.UIContext.FillTexture, inBounds, Color.Black * 0.8f);
        }
    }
}
