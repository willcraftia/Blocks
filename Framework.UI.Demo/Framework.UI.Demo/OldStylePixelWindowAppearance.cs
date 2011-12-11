#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Visuals;

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
        public int Unit = 16;

        public OldStylePixelWindowAppearance(GameServiceContainer services)
            : base(services)
        {
        }

        public override void Draw(Control control)
        {
            var bounds = control.GetAbsoluteBounds();

            var color = new Color(255, 255, 255, 1.0f);

            SpriteBatch.Begin();

            // Top Lines
            SpriteBatch.Draw(TopLeft, new Rectangle(bounds.X, bounds.Y, Unit, Unit), color);
            for (int x = Unit; x < bounds.Width - Unit; x += Unit)
            {
                SpriteBatch.Draw(Top, new Rectangle(bounds.X + x, bounds.Y, Unit, Unit), color);
            }
            SpriteBatch.Draw(TopRight, new Rectangle(bounds.X + bounds.Width - Unit, bounds.Y, Unit, Unit), color);

            // Middle Lines
            for (int y = Unit; y < bounds.Height - Unit; y += Unit)
            {
                SpriteBatch.Draw(Left, new Rectangle(bounds.X, bounds.Y + y, Unit, Unit), color);
                for (int x = Unit; x < bounds.Width - Unit; x += Unit)
                {
                    SpriteBatch.Draw(Fill, new Rectangle(bounds.X + x, bounds.Y + y, Unit, Unit), color);
                }
                SpriteBatch.Draw(Right, new Rectangle(bounds.X + bounds.Width - Unit, bounds.Y + y, Unit, Unit), color);
            }

            // Bottom LInes
            SpriteBatch.Draw(BottomLeft, new Rectangle(bounds.X, bounds.Y + bounds.Height - Unit, Unit, Unit), color);
            for (int x = Unit; x < bounds.Width - Unit; x += Unit)
            {
                SpriteBatch.Draw(Bottom, new Rectangle(bounds.X + x, bounds.Y + bounds.Height - Unit, Unit, Unit), color);
            }
            SpriteBatch.Draw(BottomRight, new Rectangle(bounds.X + bounds.Width - Unit, bounds.Y + bounds.Height - Unit, Unit, Unit), color);

            SpriteBatch.End();
        }
    }
}
