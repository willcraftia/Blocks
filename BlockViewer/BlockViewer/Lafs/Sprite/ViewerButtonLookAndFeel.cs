#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Lafs.Sprite;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Lafs.Sprite
{
    public sealed class ViewerButtonLookAndFeel : LookAndFeelBase
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            if (control.Focused)
            {
                var renderBounds = new Rect(control.RenderSize);
                drawContext.DrawRectangle(renderBounds, Color.Green * 0.5f);
            }
        }
    }
}
