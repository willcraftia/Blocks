#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Lafs
{
    public sealed class ViewerButtonLookAndFeel : ILookAndFeel
    {
        public Texture2D FocusedButtonBackground { get; set; }

        public void Draw(Control control, IDrawContext drawContext)
        {
            if (control.Focused)
            {
                var renderBounds = new Rect(control.RenderSize);
                drawContext.DrawTexture(renderBounds, FocusedButtonBackground, Color.White);
            }
        }
    }
}
