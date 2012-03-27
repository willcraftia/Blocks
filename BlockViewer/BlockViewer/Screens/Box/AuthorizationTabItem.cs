#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens.Box
{
    public sealed class AuthorizationTabItem : ContentControl
    {
        public event EventHandler NextSelected = delegate { };

        public event EventHandler BackSelected = delegate { };

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
                Text = Strings.BoxWizAuthorizationTitle,
                ForegroundColor = Color.Yellow,
                BackgroundColor = Color.Black,
                ShadowOffset = new Vector2(2)
            };
            stackPanel.Children.Add(titleTextBlock);

            var messageTextBlock = new TextBlock(Screen)
            {
                Text = Strings.BoxWizAuthorizationMessage,
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

            var launchWebBrowserButton = ControlUtil.CreateDefaultDialogButton(Screen, Strings.LaunchWebBrowserButton);
            launchWebBrowserButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                NextSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(launchWebBrowserButton);

            var backButton = ControlUtil.CreateDefaultDialogButton(Screen, Strings.BackButton);
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
