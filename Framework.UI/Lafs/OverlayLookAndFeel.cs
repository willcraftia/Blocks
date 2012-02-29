#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs
{
    /// <summary>
    /// Overlay の Look and Feel です。
    /// </summary>
    public class OverlayLookAndFeel : ILookAndFeel
    {
        public virtual void Draw(Control control, IDrawContext drawContext)
        {
            drawContext.DrawRectangle(new Rect(control.RenderSize), control.BackgroundColor);
        }
    }
}
