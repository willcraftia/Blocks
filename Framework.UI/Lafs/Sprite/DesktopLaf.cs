#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    /// <summary>
    /// Desktop 用の LaF です。
    /// </summary>
    public sealed class DesktopLaf : SpriteControlLafBase
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            drawContext.SpriteBatch.Draw(Source.FillTexture, drawContext.Bounds, control.BackgroundColor * drawContext.Opacity);
        }
    }
}
