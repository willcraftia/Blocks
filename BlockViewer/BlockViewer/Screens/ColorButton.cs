#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class ColorButton : Button
    {
        public ColorButton(Screen screen) : base(screen) { }

        public override void Draw(GameTime gameTime, IDrawContext drawContext)
        {
            drawContext.DrawRectangle(new Rect(RenderSize), ForegroundColor);

            base.Draw(gameTime, drawContext);
        }
    }
}
