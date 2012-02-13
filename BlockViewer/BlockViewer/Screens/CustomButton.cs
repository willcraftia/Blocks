#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    // MEMO
    //
    // ちょっと効率のために OO 的な隠蔽の観点は無視。
    //

    public class CustomButton : Button
    {
        public StackPanel ContentStackPanel { get; private set; }

        public Image Cursor { get; private set; }

        public TextBlock TextBlock { get; private set; }

        public CustomButton(Screen screen)
            : base(screen)
        {
            Cursor = new Image(screen)
            {
                Visible = false
            };

            TextBlock = new TextBlock(screen)
            {
                ForegroundColor = Color.White,
                Margin = new Thickness(4, 0, 0, 0)
            };

            var content = new StackPanel(Screen)
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left,
                Padding = new Thickness(4)
            };

            content.Children.Add(Cursor);
            content.Children.Add(TextBlock);

            Content = content;

            HorizontalAlignment = HorizontalAlignment.Left;
        }

        protected override void OnGotFocus(ref RoutedEventContext context)
        {
            base.OnGotFocus(ref context);

            Cursor.Visible = true;
        }

        protected override void OnLostFocus(ref RoutedEventContext context)
        {
            base.OnLostFocus(ref context);

            Cursor.Visible = false;
        }
    }
}
