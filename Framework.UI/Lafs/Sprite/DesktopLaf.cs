#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    public sealed class DesktopLaf : SpriteControlLafBase
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            drawContext.SpriteBatch.Draw(drawContext.FillTexture, drawContext.Bounds, control.BackgroundColor * drawContext.Opacity);
        }
    }
}
