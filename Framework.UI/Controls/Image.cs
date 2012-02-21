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
            var size = new Size();

            if (float.IsNaN(Width))
            {
                if (Texture == null)
                {
                    size.Width = CalculateWidth(availableSize.Width);
                }
                else
                {
                    // Texture が設定されているならば Texture の幅で希望します。
                    size.Width = Texture.Width;
                }
            }
            else
            {
                // 幅が設定されているならばそのまま希望します。
                size.Width = Width;
            }

            if (float.IsNaN(Height))
            {
                if (Texture == null)
                {
                    size.Height = CalculateHeight(availableSize.Height);
                }
                else
                {
                    // Texture が設定されているならば Texture の高さで希望します。
                    size.Height = Texture.Height;
                }
            }
            else
            {
                // 高さが設定されているならばそのまま希望します。
                size.Height = Height;
            }

            // 子は持たないことを前提として測定を終えます。
            return size;
        }

        public override void Draw(GameTime gameTime, IDrawContext drawContext)
        {
            base.Draw(gameTime, drawContext);

            if (Texture != null)
                drawContext.DrawTexture(new Rect(Vector2.Zero, RenderSize), Texture, BackgroundColor);
        }
    }
}
