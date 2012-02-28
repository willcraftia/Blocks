#region Using

using System;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class ImageButton : Button
    {
        Canvas canvas;

        public Image Image { get; set; }

        public TextBlock TextBlock { get; set; }

        public ImageButton(Screen screen)
            : base(screen)
        {
            canvas = new Canvas(screen)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            Content = canvas;

            Image = new Image(screen);
            canvas.Children.Add(Image);

            TextBlock = new TextBlock(screen);
            canvas.Children.Add(TextBlock);
        }
    }
}
