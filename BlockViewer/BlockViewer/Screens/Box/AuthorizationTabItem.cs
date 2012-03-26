#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens.Box
{
    public sealed class AuthorizationTabItem : ContentControl
    {
        public event EventHandler NextSelected = delegate { };

        public event EventHandler BackSelected = delegate { };

        const string title = "Authorization";

        const string message = "Now, this application obtained permission to use Box, but can not access your Box account yet. " +
            "To do it, this application must be authorized by you on Box website.\n\n" +
            "Please select [Launch web browser] button that will open your web browser and forward the authorization page on Box.";

        Button defaultFocusedButton;

        public AuthorizationTabItem(Screen screen)
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

            var launchWebBrowserButton = ControlUtil.CreateDefaultDialogButton(Screen, "Launch web browser");
            launchWebBrowserButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                NextSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(launchWebBrowserButton);

            var backButton = ControlUtil.CreateDefaultDialogButton(Screen, "Back");
            backButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                BackSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(backButton);

            defaultFocusedButton = launchWebBrowserButton;
        }

        public void FocusToDefault()
        {
            defaultFocusedButton.Focus();
        }
    }
}
