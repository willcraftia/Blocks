#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public class OverlayLaf : DebugControlLafBase
    {
        public override void Draw(Control control, Rectangle renderBounds, float totalOpacity)
        {
            var overlay = control as Controls.Overlay;
            if (overlay == null) return;

            // 常に半透明黒で覆うようにします。
            SpriteBatch.Draw(Source.UIContext.FillTexture, renderBounds, overlay.BackgroundColor * totalOpacity);
        }
    }
}
