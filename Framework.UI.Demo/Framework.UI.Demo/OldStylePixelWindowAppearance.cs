#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo
{
    public class OldStylePixelWindowAppearance : Appearance
    {
        public Texture2D TopLeft;
        public Texture2D TopRight;
        public Texture2D BottomLeft;
        public Texture2D BottomRight;
        public Texture2D Top;
        public Texture2D Bottom;
        public Texture2D Left;
        public Texture2D Right;
        public Texture2D Fill;

        public OldStylePixelWindowAppearance(GameServiceContainer services)
            : base(services)
        {
        }

        public override void Draw(Control control)
        {
            var bounds = control.GetAbsoluteBounds();

            var color = new Color(255, 255, 255, 0.8f);

            SpriteBatch.Begin();

            // Top Lines
            SpriteBatch.Draw(TopLeft, new Rectangle(bounds.X, bounds.Y, 32, 32), color);
            for (int x = 32; x < bounds.Width - 32; x += 32)
            {
                SpriteBatch.Draw(Top, new Rectangle(bounds.X + x, bounds.Y, 32, 32), color);
            }
            SpriteBatch.Draw(TopRight, new Rectangle(bounds.X + bounds.Width - 32, bounds.Y, 32, 32), color);

            // Middle Lines
            for (int y = 32; y < bounds.Height - 32; y += 32)
            {
                SpriteBatch.Draw(Left, new Rectangle(bounds.X, bounds.Y + y, 32, 32), color);
                for (int x = 32; x < bounds.Width - 32; x += 32)
                {
                    SpriteBatch.Draw(Fill, new Rectangle(bounds.X + x, bounds.Y + y, 32, 32), color);
                }
                SpriteBatch.Draw(Right, new Rectangle(bounds.X + bounds.Width - 32, bounds.Y + y, 32, 32), color);
            }

            // Bottom LInes
            SpriteBatch.Draw(BottomLeft, new Rectangle(bounds.X, bounds.Y + bounds.Height - 32, 32, 32), color);
            for (int x = 32; x < bounds.Width - 32; x += 32)
            {
                SpriteBatch.Draw(Bottom, new Rectangle(bounds.X + x, bounds.Y + bounds.Height - 32, 32, 32), color);
            }
            SpriteBatch.Draw(BottomRight, new Rectangle(bounds.X + bounds.Width - 32, bounds.Y + bounds.Height - 32, 32, 32), color);

            SpriteBatch.End();
        }
    }
}
