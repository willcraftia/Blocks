#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    public class OverlayLaf : SpriteControlLafBase
    {
        public override void Draw(Control control, Rectangle renderBounds, float totalOpacity)
        {
            var overlay = control as Controls.Overlay;
            if (overlay == null) return;

            // 完全透明ならば描画をスキップします。
            if (overlay.BackgroundColor.A == 0 || totalOpacity == 0) return;

            SpriteBatch.Draw(Source.UIContext.FillTexture, renderBounds, overlay.BackgroundColor * totalOpacity);
        }
    }
}
