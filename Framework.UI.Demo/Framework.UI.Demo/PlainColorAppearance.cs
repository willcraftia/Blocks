#region Using

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo
{
    public class PlainColorAppearance : Appearance
    {
        public Color BackgroundColor { get; set; }

        public PlainColorAppearance(GameServiceContainer container, Color backgroundColor)
            : base(container)
        {
            BackgroundColor = backgroundColor;
        }

        public override void Draw(Control control)
        {
            SpriteBatch.Begin();
            SpriteBatch.Draw(AppearanceService.FillTexture, control.GetAbsoluteBounds(), BackgroundColor);
            SpriteBatch.End();
        }
    }
}
