#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Visuals;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo
{
    public class PlainColorButtonAppearance : Appearance
    {
        public Color BackgroundColor = Color.Gray;

        public Color HoverColor = Color.Red;

        public Color PressedColor = Color.Yellow;

        public PlainColorButtonAppearance(IUIContext uiContext)
            : base(uiContext)
        {
        }

        public override void Draw(Control control)
        {
            var button = control as Button;
            if (button == null) return;

            var bounds = control.GetAbsoluteBounds();
            var borderBounds = bounds;
            var canvasBounds = bounds;
            canvasBounds.X += 2;
            canvasBounds.Y += 2;
            canvasBounds.Width -= 4;
            canvasBounds.Height -= 4;

            Color bgColor = BackgroundColor;
            if (button.MouseHovering)
            {
                bgColor = HoverColor;
            }
            if (button.Pressed)
            {
                bgColor = PressedColor;
            }

            SpriteBatch.Begin();
            SpriteBatch.Draw(UIContext.FillTexture, borderBounds, Color.White);
            SpriteBatch.Draw(UIContext.FillTexture, canvasBounds, bgColor);
            SpriteBatch.End();
        }
    }
}
