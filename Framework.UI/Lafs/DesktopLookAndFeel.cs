#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs
{
    /// <summary>
    /// Desktop の Look and Feel です。
    /// </summary>
    public class DesktopLookAndFeel : ILookAndFeel
    {
        public virtual void Draw(Control control, IDrawContext drawContext)
        {
            drawContext.DrawRectangle(new Rect(control.RenderSize), control.BackgroundColor);
        }
    }
}
