#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public class CanvasLaf : DebugControlLafBase
    {
        public override void Draw(Control control, Rectangle renderBounds)
        {
            var canvas = control as Controls.Canvas;
            if (canvas == null) return;

            SpriteBatch.Draw(Source.UIContext.FillTexture, renderBounds, canvas.BackgroundColor);
        }
    }
}
