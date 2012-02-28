#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Lafs.Sprite;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Lafs.Sprite
{
    public sealed class ViewerButtonLookAndFeel : LookAndFeelBase
    {
        public Texture2D FocusedButtonBackground { get; set; }

        public override void Draw(Control control, IDrawContext drawContext)
        {
            if (control.Focused)
            {
                var renderBounds = new Rect(control.RenderSize);
                //drawContext.DrawRectangle(renderBounds, Color.Green * 0.5f);
                drawContext.DrawTexture(renderBounds, FocusedButtonBackground, Color.White);
            }
        }
    }
}
