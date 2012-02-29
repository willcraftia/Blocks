#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public class DebugWindowLookAndFeel : DebugDefaultLookAndFeel
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            drawContext.DrawRectangle(new Rect(control.RenderSize), Color.Black);

            base.Draw(control, drawContext);
        }
    }
}
