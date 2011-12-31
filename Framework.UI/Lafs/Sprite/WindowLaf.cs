#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    public class WindowLaf : SpriteControlLafBase
    {
        Texture2D topLeft;
        Texture2D topRight;
        Texture2D bottomLeft;
        Texture2D bottomRight;
        Texture2D left;
        Texture2D right;
        Texture2D top;
        Texture2D bottom;
        Texture2D fill;

        protected override void LoadContent()
        {
            var content = Source.Content;

            topLeft = content.Load<Texture2D>("WindowTopLeft");
            topRight = content.Load<Texture2D>("WindowTopRight");
            bottomLeft = content.Load<Texture2D>("WindowBottomLeft");
            bottomRight = content.Load<Texture2D>("WindowBottomRight");
            left = content.Load<Texture2D>("WindowLeft");
            right = content.Load<Texture2D>("WindowRight");
            top = content.Load<Texture2D>("WindowTop");
            bottom = content.Load<Texture2D>("WindowBottom");
            fill = content.Load<Texture2D>("WindowFill");

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

            // Top Lines
            spriteBatch.Draw(topLeft, new Rectangle(bounds.X, bounds.Y, unit, unit), color);
            for (int x = unit; x < bounds.Width - unit; x += unit)
            {
                spriteBatch.Draw(top, new Rectangle(bounds.X + x, bounds.Y, unit, unit), color);
            }
            spriteBatch.Draw(topRight, new Rectangle(bounds.X + bounds.Width - unit, bounds.Y, unit, unit), color);

            // Middle Lines
            for (int y = unit; y < bounds.Height - unit; y += unit)
            {
                spriteBatch.Draw(left, new Rectangle(bounds.X, bounds.Y + y, unit, unit), color);
                for (int x = unit; x < bounds.Width - unit; x += unit)
                {
                    spriteBatch.Draw(fill, new Rectangle(bounds.X + x, bounds.Y + y, unit, unit), color);
                }
                spriteBatch.Draw(right, new Rectangle(bounds.X + bounds.Width - unit, bounds.Y + y, unit, unit), color);
            }

            // Bottom LInes
            spriteBatch.Draw(bottomLeft, new Rectangle(bounds.X, bounds.Y + bounds.Height - unit, unit, unit), color);
            for (int x = unit; x < bounds.Width - unit; x += unit)
            {
                spriteBatch.Draw(bottom, new Rectangle(bounds.X + x, bounds.Y + bounds.Height - unit, unit, unit), color);
            }
            spriteBatch.Draw(bottomRight, new Rectangle(bounds.X + bounds.Width - unit, bounds.Y + bounds.Height - unit, unit, unit), color);
        }
    }
}
