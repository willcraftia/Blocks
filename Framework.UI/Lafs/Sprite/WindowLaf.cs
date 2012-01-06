#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    public class WindowLaf : SpriteControlLafBase
    {
        SpriteSheet spriteSheet;

        protected override void LoadContent()
        {
            var content = Source.Content;

            //----------------------------------------------------------------
            // TODO test code
            //
            //System.Net.WebClient webClient = new System.Net.WebClient();
            //webClient.DownloadFile("http://blocks/Framework.UI.Demo/Framework.UI.DemoContent/UI/Sprite/WindowTopLeft.png", "Content/UI/Sprite/WindowTopLeft.png");

            //topLeft = Texture2D.FromStream(Source.GraphicsDevice, new System.IO.FileStream("Content/UI/Sprite/WindowTopLeft.png", System.IO.FileMode.Open));
            //
            //----------------------------------------------------------------

            var spriteSheetTexture = content.Load<Texture2D>("Window");
            spriteSheet = new SpriteSheet(spriteSheetTexture);

            var size = Source.SpriteSize;
            spriteSheet.SourceRectangles["TopLeft"] = new Rectangle(0, 0, size, size);
            spriteSheet.SourceRectangles["Top"] = new Rectangle(size, 0, size, size);
            spriteSheet.SourceRectangles["TopRight"] = new Rectangle(size * 2, 0, size, size);
            spriteSheet.SourceRectangles["Left"] = new Rectangle(0, size, size, size);
            spriteSheet.SourceRectangles["Fill"] = new Rectangle(size, size, size, size);
            spriteSheet.SourceRectangles["Right"] = new Rectangle(size * 2, size, size, size);
            spriteSheet.SourceRectangles["BottomLeft"] = new Rectangle(0, size * 2, size, size);
            spriteSheet.SourceRectangles["Bottom"] = new Rectangle(size, size * 2, size, size);
            spriteSheet.SourceRectangles["BottomRight"] = new Rectangle(size * 2, size * 2, size, size);

            base.LoadContent();
        }

        public override void Draw(Control control, IDrawContext drawContext)
        {
            var window = control as Controls.Window;
            if (window == null) return;

            var color = window.BackgroundColor * drawContext.Opacity;
            var bounds = drawContext.Bounds;
            var unit = Source.SpriteSize;
            var spriteBatch = drawContext.SpriteBatch;
            var texture = spriteSheet.Texture;
            Rectangle sourceRectangle;

            // Top Lines
            sourceRectangle = spriteSheet.SourceRectangles["TopLeft"];
            spriteBatch.Draw(texture, new Rectangle(bounds.X, bounds.Y, unit, unit), sourceRectangle, color);
            sourceRectangle = spriteSheet.SourceRectangles["Top"];
            for (int x = unit; x < bounds.Width - unit; x += unit)
            {
                spriteBatch.Draw(texture, new Rectangle(bounds.X + x, bounds.Y, unit, unit), sourceRectangle, color);
            }
            sourceRectangle = spriteSheet.SourceRectangles["TopRight"];
            spriteBatch.Draw(texture, new Rectangle(bounds.X + bounds.Width - unit, bounds.Y, unit, unit), sourceRectangle, color);

            // Middle Lines
            for (int y = unit; y < bounds.Height - unit; y += unit)
            {
                sourceRectangle = spriteSheet.SourceRectangles["Left"];
                spriteBatch.Draw(texture, new Rectangle(bounds.X, bounds.Y + y, unit, unit), sourceRectangle, color);
                sourceRectangle = spriteSheet.SourceRectangles["Fill"];
                for (int x = unit; x < bounds.Width - unit; x += unit)
                {
                    spriteBatch.Draw(texture, new Rectangle(bounds.X + x, bounds.Y + y, unit, unit), sourceRectangle, color);
                }
                sourceRectangle = spriteSheet.SourceRectangles["Right"];
                spriteBatch.Draw(texture, new Rectangle(bounds.X + bounds.Width - unit, bounds.Y + y, unit, unit), sourceRectangle, color);
            }

            // Bottom Lines
            sourceRectangle = spriteSheet.SourceRectangles["BottomLeft"];
            spriteBatch.Draw(texture, new Rectangle(bounds.X, bounds.Y + bounds.Height - unit, unit, unit), sourceRectangle, color);
            sourceRectangle = spriteSheet.SourceRectangles["Bottom"];
            for (int x = unit; x < bounds.Width - unit; x += unit)
            {
                spriteBatch.Draw(texture, new Rectangle(bounds.X + x, bounds.Y + bounds.Height - unit, unit, unit), sourceRectangle, color);
            }
            sourceRectangle = spriteSheet.SourceRectangles["BottomRight"];
            spriteBatch.Draw(texture, new Rectangle(bounds.X + bounds.Width - unit, bounds.Y + bounds.Height - unit, unit, unit), sourceRectangle, color);
        }
    }
}
