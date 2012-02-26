#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    public class Image : Control
    {
        public Texture2D Texture { get; set; }

        public Image(Screen screen) : base(screen) { }

        protected override Size MeasureOverride(Size availableSize)
        {
            var controlSize = new Size(Width, Height);

            if (float.IsNaN(controlSize.Width))
                controlSize.Width = (Texture == null) ? CalculateWidth(0) : Texture.Width;

            if (float.IsNaN(controlSize.Height))
                controlSize.Height = (Texture == null) ? CalculateHeight(0) : Texture.Height;

            // 子は持たないことを前提として測定を終えます。
            return new Size
            {
                Width = controlSize.Width + Margin.Left + Margin.Right,
                Height = controlSize.Height + Margin.Top + Margin.Bottom
            };
        }

        public override void Draw(GameTime gameTime, IDrawContext drawContext)
        {
            base.Draw(gameTime, drawContext);

            if (Texture != null)
                drawContext.DrawTexture(new Rect(Vector2.Zero, RenderSize), Texture, BackgroundColor);
        }
    }
}
