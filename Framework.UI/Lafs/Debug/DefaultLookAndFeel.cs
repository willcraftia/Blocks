#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    /// <summary>
    /// デバッグ用のデフォルトの Look & Feel です。
    /// </summary>
    public class DefaultLookAndFeel : LookAndFeelBase
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            var renderBounds = new Rect(control.RenderSize);

            // 背景を BackgroundColor で塗り潰します。
            drawContext.DrawRectangle(renderBounds, control.BackgroundColor);

            // 枠を White で描画します。
            // ただし、背景色が White の場合には区別が付くように Gray にします。
            var borderColor = Color.White;
            if (control.BackgroundColor == Color.White) borderColor = Color.DimGray;

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
            var mouseOverColor = (control.MouseDirectlyOver) ? Color.DodgerBlue : Color.DarkGoldenrod;
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
