#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    /// <summary>
    /// デフォルトのデバッグ用 LaF です。
    /// </summary>
    public class DefaultControlLaf : DebugControlLafBase
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            // 枠のために白で塗り潰します。
            drawContext.SpriteBatch.Draw(Source.FillTexture, drawContext.Bounds, Color.White * drawContext.Opacity);

            // 少し小さくした領域を背景色で覆います。
            var inBounds = drawContext.Bounds;
            inBounds.X += 1;
            inBounds.Y += 1;
            inBounds.Width -= 2;
            inBounds.Height -= 2;
            drawContext.SpriteBatch.Draw(Source.FillTexture, inBounds, control.BackgroundColor * drawContext.Opacity);
        }
    }
}
