#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    /// <summary>
    /// Desktop の Look & Feel です。
    /// </summary>
    public sealed class DesktopLookAndFeel : LookAndFeelBase
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            drawContext.DrawRectangle(new Rect(control.RenderSize), control.BackgroundColor);
        }
    }
}
