#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Visuals.Sprite
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

        public override void Draw(Control control)
        {
            var window = control as Controls.Window;
            if (window == null) return;

            var bounds = window.GetAbsoluteBounds();
            var color = window.BackgroundColor;
            var unit = Source.SpriteSize;

            // Top Lines
            SpriteBatch.Draw(topLeft, new Rectangle(bounds.X, bounds.Y, unit, unit), color);
            for (int x = unit; x < bounds.Width - unit; x += unit)
            {
                SpriteBatch.Draw(top, new Rectangle(bounds.X + x, bounds.Y, unit, unit), color);
            }
            SpriteBatch.Draw(topRight, new Rectangle(bounds.X + bounds.Width - unit, bounds.Y, unit, unit), color);

            // Middle Lines
            for (int y = unit; y < bounds.Height - unit; y += unit)
            {
                SpriteBatch.Draw(left, new Rectangle(bounds.X, bounds.Y + y, unit, unit), color);
                for (int x = unit; x < bounds.Width - unit; x += unit)
                {
                    SpriteBatch.Draw(fill, new Rectangle(bounds.X + x, bounds.Y + y, unit, unit), color);
                }
                SpriteBatch.Draw(right, new Rectangle(bounds.X + bounds.Width - unit, bounds.Y + y, unit, unit), color);
            }

            // Bottom LInes
            SpriteBatch.Draw(bottomLeft, new Rectangle(bounds.X, bounds.Y + bounds.Height - unit, unit, unit), color);
            for (int x = unit; x < bounds.Width - unit; x += unit)
            {
                SpriteBatch.Draw(bottom, new Rectangle(bounds.X + x, bounds.Y + bounds.Height - unit, unit, unit), color);
            }
            SpriteBatch.Draw(bottomRight, new Rectangle(bounds.X + bounds.Width - unit, bounds.Y + bounds.Height - unit, unit, unit), color);
        }
    }
}
