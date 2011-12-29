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
            topLeft = Content.Load<Texture2D>("WindowTopLeft");
            topRight = Content.Load<Texture2D>("WindowTopRight");
            bottomLeft = Content.Load<Texture2D>("WindowBottomLeft");
            bottomRight = Content.Load<Texture2D>("WindowBottomRight");
            left = Content.Load<Texture2D>("WindowLeft");
            right = Content.Load<Texture2D>("WindowRight");
            top = Content.Load<Texture2D>("WindowTop");
            bottom = Content.Load<Texture2D>("WindowBottom");
            fill = Content.Load<Texture2D>("WindowFill");

            base.LoadContent();
        }

        public override void Draw(Control control, Rectangle renderBounds, float totalOpacity)
        {
            var window = control as Controls.Window;
            if (window == null) return;

            var color = window.BackgroundColor * totalOpacity;
            var unit = Source.SpriteSize;

            // Top Lines
            SpriteBatch.Draw(topLeft, new Rectangle(renderBounds.X, renderBounds.Y, unit, unit), color);
            for (int x = unit; x < renderBounds.Width - unit; x += unit)
            {
                SpriteBatch.Draw(top, new Rectangle(renderBounds.X + x, renderBounds.Y, unit, unit), color);
            }
            SpriteBatch.Draw(topRight, new Rectangle(renderBounds.X + renderBounds.Width - unit, renderBounds.Y, unit, unit), color);

            // Middle Lines
            for (int y = unit; y < renderBounds.Height - unit; y += unit)
            {
                SpriteBatch.Draw(left, new Rectangle(renderBounds.X, renderBounds.Y + y, unit, unit), color);
                for (int x = unit; x < renderBounds.Width - unit; x += unit)
                {
                    SpriteBatch.Draw(fill, new Rectangle(renderBounds.X + x, renderBounds.Y + y, unit, unit), color);
                }
                SpriteBatch.Draw(right, new Rectangle(renderBounds.X + renderBounds.Width - unit, renderBounds.Y + y, unit, unit), color);
            }

            // Bottom LInes
            SpriteBatch.Draw(bottomLeft, new Rectangle(renderBounds.X, renderBounds.Y + renderBounds.Height - unit, unit, unit), color);
            for (int x = unit; x < renderBounds.Width - unit; x += unit)
            {
                SpriteBatch.Draw(bottom, new Rectangle(renderBounds.X + x, renderBounds.Y + renderBounds.Height - unit, unit, unit), color);
            }
            SpriteBatch.Draw(bottomRight, new Rectangle(renderBounds.X + renderBounds.Width - unit, renderBounds.Y + renderBounds.Height - unit, unit, unit), color);
        }
    }
}
