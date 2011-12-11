#region Using

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Visuals;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo
{
    public class PlainColorWindowAppearance : Appearance
    {
        public Color BackgroundColor = Color.Blue;

        public PlainColorWindowAppearance(IUIContext uiContext)
            : base(uiContext)
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
            SpriteBatch.Draw(UIContext.FillTexture, borderBounds, Color.White);
            SpriteBatch.Draw(UIContext.FillTexture, canvasBounds, BackgroundColor);
            SpriteBatch.End();
        }
    }
}
