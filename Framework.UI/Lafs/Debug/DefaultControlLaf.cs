#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

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
            var spriteBatch = drawContext.SpriteBatch;
            var renderBounds = drawContext.Bounds;

            // 背景を BackgroundColor で塗り潰します。
            spriteBatch.Draw(Source.FillTexture, renderBounds, control.BackgroundColor * drawContext.Opacity);

            // 枠を White で描画します。
            // ただし、背景色が White の場合には区別が付くように Gray にします。
            var borderColor = Color.White;
            if (control.BackgroundColor == Color.White) borderColor = Color.DimGray;
            borderColor *= drawContext.Opacity;

            Rectangle lineBounds;

            lineBounds = renderBounds;
            lineBounds.Height = 1;
            spriteBatch.Draw(Source.FillTexture, lineBounds, borderColor);

            lineBounds = renderBounds;
            lineBounds.Y += lineBounds.Height - 1;
            lineBounds.Height = 1;
            spriteBatch.Draw(Source.FillTexture, lineBounds, borderColor);

            lineBounds = renderBounds;
            lineBounds.Width = 1;
            spriteBatch.Draw(Source.FillTexture, lineBounds, borderColor);

            lineBounds = renderBounds;
            lineBounds.X += lineBounds.Width - 1;
            lineBounds.Width = 1;
            spriteBatch.Draw(Source.FillTexture, lineBounds, borderColor);

            // フォーカスを持つ場合に枠の四隅に矩形を描画します。
            if (control.Focused)
            {
                var focusColor = Color.Violet * drawContext.Opacity;

                Rectangle focusBounds;
                int size = 8;

                focusBounds = renderBounds;
                focusBounds.Width = size;
                focusBounds.Height = size;
                DrawRectangle(spriteBatch, focusBounds, focusColor, borderColor, 1);

                focusBounds = renderBounds;
                focusBounds.X += renderBounds.Width - size;
                focusBounds.Width = size;
                focusBounds.Height = size;
                DrawRectangle(spriteBatch, focusBounds, focusColor, borderColor, 1);

                focusBounds = renderBounds;
                focusBounds.Y += renderBounds.Height - size;
                focusBounds.Width = size;
                focusBounds.Height = size;
                DrawRectangle(spriteBatch, focusBounds, focusColor, borderColor, 1);

                focusBounds = renderBounds;
                focusBounds.X += renderBounds.Width - size;
                focusBounds.Y += renderBounds.Height - size;
                focusBounds.Width = size;
                focusBounds.Height = size;
                DrawRectangle(spriteBatch, focusBounds, focusColor, borderColor, 1);
            }

            // マウス オーバの場合に枠の四辺に矩形を描画します。 
            var mouseOverColor = (control.MouseDirectlyOver) ? Color.DodgerBlue : Color.DarkGoldenrod;
            mouseOverColor *= drawContext.Opacity;
            if (control.MouseOver)
            {
                Rectangle mouseOverBounds;
                int size = 8;

                mouseOverBounds = renderBounds;
                mouseOverBounds.X += (mouseOverBounds.Width - size) / 2;
                mouseOverBounds.Width = size;
                mouseOverBounds.Height = size;
                DrawRectangle(spriteBatch, mouseOverBounds, mouseOverColor, borderColor, 1);

                mouseOverBounds = renderBounds;
                mouseOverBounds.X += (mouseOverBounds.Width - size) / 2;
                mouseOverBounds.Y += renderBounds.Height - size;
                mouseOverBounds.Width = size;
                mouseOverBounds.Height = size;
                DrawRectangle(spriteBatch, mouseOverBounds, mouseOverColor, borderColor, 1);

                mouseOverBounds = renderBounds;
                mouseOverBounds.Y += (mouseOverBounds.Height - size) / 2;
                mouseOverBounds.Width = size;
                mouseOverBounds.Height = size;
                DrawRectangle(spriteBatch, mouseOverBounds, mouseOverColor, borderColor, 1);

                mouseOverBounds = renderBounds;
                mouseOverBounds.X += renderBounds.Width - size;
                mouseOverBounds.Y += (mouseOverBounds.Height - size) / 2;
                mouseOverBounds.Width = size;
                mouseOverBounds.Height = size;
                DrawRectangle(spriteBatch, mouseOverBounds, mouseOverColor, borderColor, 1);
            }
        }

        void DrawRectangle(SpriteBatch spriteBatch, Rectangle bounds, Color color, Color borderColor, int borderThickness)
        {
            spriteBatch.Draw(Source.FillTexture, bounds, borderColor);

            bounds.X += borderThickness;
            bounds.Y += borderThickness;
            bounds.Width -= borderThickness * 2;
            bounds.Height -= borderThickness * 2;
            spriteBatch.Draw(Source.FillTexture, bounds, color);
        }
    }
}
