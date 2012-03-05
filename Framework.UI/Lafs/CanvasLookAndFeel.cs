#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs
{
    public sealed class CanvasLookAndFeel : ILookAndFeel
    {
        public void Draw(Control control, IDrawContext drawContext)
        {
            drawContext.DrawRectangle(new Rect(control.RenderSize), control.BackgroundColor);
        }
    }
}
