#region Using

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo
{
    public class PlainColorWindowAppearance : Appearance
    {
        public Color BackgroundColor = Color.Blue;

        public PlainColorWindowAppearance(GameServiceContainer container)
            : base(container)
        {
        }

        public override void Draw(Control control)
        {
            var bounds = control.GetAbsoluteBounds();
            var borderBounds = bounds;
            var canvasBounds = bounds;
            canvasBounds.X += 2;
            canvasBounds.Y += 2;
            canvasBounds.Width -= 4;
            canvasBounds.Height -= 4;

            SpriteBatch.Begin();
            SpriteBatch.Draw(UIService.FillTexture, borderBounds, Color.White);
            SpriteBatch.Draw(UIService.FillTexture, canvasBounds, BackgroundColor);
            SpriteBatch.End();
        }
    }
}
