#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    /// <summary>
    /// Overlay 用の LaF です。
    /// </summary>
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
