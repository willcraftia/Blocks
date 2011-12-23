﻿#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Visuals.Debug
{
    public class CanvasLaf : DebugControlLafBase
    {
        public override void Draw(Control control)
        {
            var canvas = control as Controls.Canvas;
            if (canvas == null) return;

            var bounds = canvas.GetAbsoluteBounds();
            SpriteBatch.Draw(Source.UIContext.FillTexture, bounds, canvas.BackgroundColor);
        }
    }
}
