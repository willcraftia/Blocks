#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public sealed class DesktopLaf : DebugControlLafBase
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            drawContext.SpriteBatch.Draw(Source.FillTexture, drawContext.Bounds, control.BackgroundColor * drawContext.Opacity);
        }
    }
}
