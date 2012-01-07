#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public class OverlayLaf : DebugControlLafBase
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            var overlay = control as Controls.Overlay;
            if (overlay == null) return;

            drawContext.SpriteBatch.Draw(Source.FillTexture, drawContext.Bounds, overlay.BackgroundColor * drawContext.Opacity);
        }
    }
}
