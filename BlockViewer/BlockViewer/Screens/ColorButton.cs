#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class ColorButton : Button
    {
        Texture2D texture;

        public ColorButton(Screen screen)
            : base(screen)
        {
            texture = Texture2DHelper.CreateFillTexture(screen.GraphicsDevice);
        }

        public override void Draw(GameTime gameTime, IDrawContext drawContext)
        {
            drawContext.DrawTexture(new Rect(RenderSize), texture, ForegroundColor);

            base.Draw(gameTime, drawContext);
        }
    }
}
