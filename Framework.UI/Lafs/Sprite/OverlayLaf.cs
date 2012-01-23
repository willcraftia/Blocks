#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    /// <summary>
    /// Overlay 用の LaF です。
    /// </summary>
    public class OverlayLaf : SpriteControlLafBase
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            drawContext.DrawRectangle(new Rect(control.RenderSize), control.BackgroundColor);
        }
    }
}
