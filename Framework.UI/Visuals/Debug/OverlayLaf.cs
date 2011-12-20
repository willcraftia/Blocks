#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Visuals.Debug
{
    public class OverlayLaf : DebugControlLafBase
    {
        public override void Draw(Control control)
        {
            var overlay = control as Overlay;
            if (overlay == null) return;

            var bounds = overlay.GetAbsoluteBounds();

            // 常に半透明黒で覆うようにします。
            SpriteBatch.Draw(Source.UIContext.FillTexture, bounds, Color.Black * 0.5f);
        }
    }
}
