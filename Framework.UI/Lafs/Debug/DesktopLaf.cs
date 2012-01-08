#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    /// <summary>
    /// Desktop 用の LaF です。
    /// </summary>
    public sealed class DesktopLaf : DebugControlLafBase
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            drawContext.SpriteBatch.Draw(Source.FillTexture, drawContext.Bounds, control.BackgroundColor * drawContext.Opacity);
        }
    }
}
