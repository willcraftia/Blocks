#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens.Box
{
    public sealed class AccessTabItem : ContentControl
    {
        public event EventHandler NextSelected = delegate { };

        public event EventHandler BackSelected = delegate { };

        const string title = "Access Your Box Account";

        const string message = "If you authorized this application on Box website, please select [Authorized] button " +
            "that will try to access your Box account from this application.";

        Button backButton;

        public AccessTabItem(Screen screen)
            : base(screen)
        {
            var stackPanel = new StackPanel(Screen)
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            Content = stackPanel;

            var titleTextBlock = new TextBlock(Screen)
            {
                Text = title,
                ForegroundColor = Color.Yellow,
                BackgroundColor = Color.Black,
                ShadowOffset = new Vector2(2)
            };
            stackPanel.Children.Add(titleTextBlock);

            var messageTextBlock = new TextBlock(Screen)
            {
                Text = message,
                TextWrapping = TextWrapping.Wrap,
                TextHorizontalAlignment = HorizontalAlignment.Left,
                ForegroundColor = Color.White,
                BackgroundColor = Color.Black
            };
            stackPanel.Children.Add(messageTextBlock);

            var separator = ControlUtil.CreateDefaultSeparator(Screen);
            stackPanel.Children.Add(separator);

            var buttonPanel = new StackPanel(Screen)
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            stackPanel.Children.Add(buttonPanel);

            var nextButton = ControlUtil.CreateDefaultDialogButton(Screen, "Authorized.");
            nextButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                NextSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(nextButton);

            backButton = ControlUtil.CreateDefaultDialogButton(Screen, "Back");
            backButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                BackSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(backButton);
        }

        public void FocusToDefault()
        {
            backButton.Focus();
        }
    }
}
