#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Visuals.Sprite
{
    public class OverlayLaf : ControlLafBase
    {
        public override void Draw(Control control)
        {
            var overlay = control as Overlay;
            if (overlay == null) return;

            // 完全透明ならば描画をスキップします。
            if (overlay.BackgroundColor.A == 0.0f) return;

            var bounds = overlay.GetAbsoluteBounds();

            SpriteBatch.Begin();
            SpriteBatch.Draw(Source.UIContext.FillTexture, bounds, overlay.BackgroundColor);
            SpriteBatch.End();
        }
    }
}
