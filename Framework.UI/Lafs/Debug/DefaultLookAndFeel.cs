#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    /// <summary>
    /// デバッグ用のデフォルトの Look and Feel です。
    /// </summary>
    public class DefaultLookAndFeel : ILookAndFeel
    {
        // I/F
        public virtual void Draw(Control control, IDrawContext drawContext)
        {
            var renderBounds = new Rect(control.RenderSize);

            // デフォルトは背景を透過にします。

            // 枠を白で描画します。
            var borderColor = Color.White;

            Rect lineBounds;

            lineBounds = renderBounds;
            lineBounds.Height = 1;
            drawContext.DrawRectangle(lineBounds, borderColor);

            lineBounds = renderBounds;
            lineBounds.Y += lineBounds.Height - 1;
            lineBounds.Height = 1;
            drawContext.DrawRectangle(lineBounds, borderColor);

            lineBounds = renderBounds;
            lineBounds.Width = 1;
            drawContext.DrawRectangle(lineBounds, borderColor);

            lineBounds = renderBounds;
            lineBounds.X += lineBounds.Width - 1;
            lineBounds.Width = 1;
            drawContext.DrawRectangle(lineBounds, borderColor);

            // マウス オーバの場合に枠の四辺に矩形を描画します。 
            var mouseOverColor = (control.MouseDirectlyOver) ? Color.Red : Color.Blue;
            if (control.MouseOver)
            {
                Rect mouseOverBounds;
                int size = 8;

                mouseOverBounds = renderBounds;
                mouseOverBounds.X += (mouseOverBounds.Width - size) / 2;
                mouseOverBounds.Width = size;
                mouseOverBounds.Height = size;
                DrawRectangle(drawContext, mouseOverBounds, mouseOverColor, borderColor, 1);

                mouseOverBounds = renderBounds;
                mouseOverBounds.X += (mouseOverBounds.Width - size) / 2;
                mouseOverBounds.Y += renderBounds.Height - size;
                mouseOverBounds.Width = size;
                mouseOverBounds.Height = size;
                DrawRectangle(drawContext, mouseOverBounds, mouseOverColor, borderColor, 1);

                mouseOverBounds = renderBounds;
                mouseOverBounds.Y += (mouseOverBounds.Height - size) / 2;
                mouseOverBounds.Width = size;
                mouseOverBounds.Height = size;
                DrawRectangle(drawContext, mouseOverBounds, mouseOverColor, borderColor, 1);

                mouseOverBounds = renderBounds;
                mouseOverBounds.X += renderBounds.Width - size;
                mouseOverBounds.Y += (mouseOverBounds.Height - size) / 2;
                mouseOverBounds.Width = size;
                mouseOverBounds.Height = size;
                DrawRectangle(drawContext, mouseOverBounds, mouseOverColor, borderColor, 1);
            }

            // フォーカスを持つ場合に枠の四隅に矩形を描画します。
            var focusColor = (control.Focused) ? Color.Red : Color.Blue;
            if (control.Focused || control.LogicalFocused)
            {
                Rect focusBounds;
                int size = 8;

                focusBounds = renderBounds;
                focusBounds.Width = size;
                focusBounds.Height = size;
                DrawRectangle(drawContext, focusBounds, focusColor, borderColor, 1);

                focusBounds = renderBounds;
                focusBounds.X += renderBounds.Width - size;
                focusBounds.Width = size;
                focusBounds.Height = size;
                DrawRectangle(drawContext, focusBounds, focusColor, borderColor, 1);

                focusBounds = renderBounds;
                focusBounds.Y += renderBounds.Height - size;
                focusBounds.Width = size;
                focusBounds.Height = size;
                DrawRectangle(drawContext, focusBounds, focusColor, borderColor, 1);

                focusBounds = renderBounds;
                focusBounds.X += renderBounds.Width - size;
                focusBounds.Y += renderBounds.Height - size;
                focusBounds.Width = size;
                focusBounds.Height = size;
                DrawRectangle(drawContext, focusBounds, focusColor, borderColor, 1);
            }
        }

        void DrawRectangle(IDrawContext drawContext, Rect bounds, Color color, Color borderColor, int borderThickness)
        {
            drawContext.DrawRectangle(bounds, borderColor);

            bounds.X += borderThickness;
            bounds.Y += borderThickness;
            bounds.Width -= borderThickness * 2;
            bounds.Height -= borderThickness * 2;
            drawContext.DrawRectangle(bounds, color);
        }
    }
}
