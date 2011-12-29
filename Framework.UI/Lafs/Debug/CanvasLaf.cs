#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public class CanvasLaf : DebugControlLafBase
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            var canvas = control as Controls.Canvas;
            if (canvas == null) return;

            drawContext.SpriteBatch.Draw(drawContext.FillTexture, drawContext.Bounds, canvas.BackgroundColor * drawContext.Opacity);
        }
    }
}
