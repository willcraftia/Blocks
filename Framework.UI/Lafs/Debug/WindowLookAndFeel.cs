#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public class WindowLookAndFeel : DefaultLookAndFeel
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            var renderBounds = new Rect(control.RenderSize);
            drawContext.DrawRectangle(renderBounds, Color.Black);

            base.Draw(control, drawContext);
        }
    }
}
