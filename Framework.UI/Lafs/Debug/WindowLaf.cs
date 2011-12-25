#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public class WindowLaf : DebugControlLafBase
    {
        public override void Draw(Control control, Rectangle renderBounds)
        {
            var window = control as Controls.Window;
            if (window == null) return;

            // 背景色で塗り潰します。
            SpriteBatch.Draw(Source.UIContext.FillTexture, renderBounds, window.BackgroundColor);

            // 少し小さくした領域を半透明黒で覆います (ブレンドしつつ枠を作ります)。
            var inBounds = renderBounds;
            inBounds.X += 2;
            inBounds.Y += 2;
            inBounds.Width -= 4;
            inBounds.Height -= 4;
            SpriteBatch.Draw(Source.UIContext.FillTexture, inBounds, Color.Black * 0.8f);
        }
    }
}
