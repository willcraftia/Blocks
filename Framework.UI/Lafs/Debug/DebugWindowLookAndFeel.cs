#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public class DebugWindowLookAndFeel : ILookAndFeel
    {
        // I/F
        public virtual void Draw(Control control, IDrawContext drawContext)
        {
            var renderBounds = new Rect(control.RenderSize);
            drawContext.DrawRectangle(renderBounds, Color.Black);
        }
    }
}
