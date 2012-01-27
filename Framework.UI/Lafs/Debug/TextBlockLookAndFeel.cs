#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public class TextBlockLookAndFeel : DefaultLookAndFeel
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            base.Draw(control, drawContext);

            var textBlock = control as Controls.TextBlock;
            if (textBlock == null) return;

            var font = textBlock.Font ?? textBlock.Screen.Font;

            if (font != null && !string.IsNullOrEmpty(textBlock.Text))
            {
                drawContext.DrawString(
                    new Rect(control.RenderSize), font, textBlock.Text, control.FontStretch,
                    control.HorizontalAlignment, control.VerticalAlignment, control.ForegroundColor, control.Padding);
            }
        }
    }
}
