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
            // 背景を BackgroundColor で塗り潰します。
            drawContext.SpriteBatch.Draw(Source.FillTexture, drawContext.Bounds, control.BackgroundColor * drawContext.Opacity);

            // 枠を White で描画します。
            // ただし、背景色が White の場合には区別が付くように Gray にします。
            {
                var borderColor = Color.White;
                if (control.BackgroundColor == Color.White) borderColor = Color.DimGray;

                Rectangle lineBounds;

                lineBounds = drawContext.Bounds;
                lineBounds.Height = 1;
                drawContext.SpriteBatch.Draw(Source.FillTexture, lineBounds, borderColor * drawContext.Opacity);

                lineBounds = drawContext.Bounds;
                lineBounds.Y += lineBounds.Height - 1;
                lineBounds.Height = 1;
                drawContext.SpriteBatch.Draw(Source.FillTexture, lineBounds, borderColor * drawContext.Opacity);

                lineBounds = drawContext.Bounds;
                lineBounds.Width = 1;
                drawContext.SpriteBatch.Draw(Source.FillTexture, lineBounds, borderColor * drawContext.Opacity);

                lineBounds = drawContext.Bounds;
                lineBounds.X += lineBounds.Width - 1;
                lineBounds.Width = 1;
                drawContext.SpriteBatch.Draw(Source.FillTexture, lineBounds, borderColor * drawContext.Opacity);
            }

            // フォーカスを持つ場合に枠の四隅に Magenta の矩形を描画します。
            if (control.Focused)
            {
                var focusColor = Color.Magenta;

                Rectangle focusBounds;
                int size = 8;

                focusBounds = drawContext.Bounds;
                focusBounds.Width = size;
                focusBounds.Height = size;
                drawContext.SpriteBatch.Draw(Source.FillTexture, focusBounds, focusColor * drawContext.Opacity);

                focusBounds = drawContext.Bounds;
                focusBounds.X += drawContext.Bounds.Width - size;
                focusBounds.Width = size;
                focusBounds.Height = size;
                drawContext.SpriteBatch.Draw(Source.FillTexture, focusBounds, focusColor * drawContext.Opacity);

                focusBounds = drawContext.Bounds;
                focusBounds.Y += drawContext.Bounds.Height - size;
                focusBounds.Width = size;
                focusBounds.Height = size;
                drawContext.SpriteBatch.Draw(Source.FillTexture, focusBounds, focusColor * drawContext.Opacity);

                focusBounds = drawContext.Bounds;
                focusBounds.X += drawContext.Bounds.Width - size;
                focusBounds.Y += drawContext.Bounds.Height - size;
                focusBounds.Width = size;
                focusBounds.Height = size;
                drawContext.SpriteBatch.Draw(Source.FillTexture, focusBounds, focusColor * drawContext.Opacity);
            }
        }
    }
}
