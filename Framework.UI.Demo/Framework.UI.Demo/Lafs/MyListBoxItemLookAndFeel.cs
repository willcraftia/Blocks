#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo.Lafs
{
    public sealed class MyListBoxItemLookAndFeel : ILookAndFeel
    {
        public void Draw(Control control, IDrawContext drawContext)
        {
            if (control.Focused)
                drawContext.DrawRectangle(new Rect(control.RenderSize), Color.Navy * 0.5f);

            if (control.MouseOver)
                drawContext.DrawRectangle(new Rect(control.RenderSize), Color.DarkGreen * 0.5f);

            if ((control as ListBoxItem).IsSelected)
                drawContext.DrawRectangle(new Rect(control.RenderSize), Color.Yellow * 0.5f);
        }
    }
}
