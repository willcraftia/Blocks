#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens.Box
{
    public sealed class AttentionTabItem : ContentControl
    {
        public event EventHandler AgreeSelected = delegate { };

        public event EventHandler CancelSelected = delegate { };

        const string title = "Attention";

        const string message = "To use this function, you must setup Box integration on this application. " +
            "This setup will requires to access Box and your account on it via the Internet.\n\n" +
            "If you allow that, select [Agree] button after signed up for Box, otherwise [Cancel].";

        Button cancelButton;

        public AttentionTabItem(Screen screen)
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

            var agreeButton = ControlUtil.CreateDefaultDialogButton(Screen, "Agree");
            agreeButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                AgreeSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(agreeButton);

            cancelButton = ControlUtil.CreateDefaultDialogButton(Screen, "Cancel");
            cancelButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                CancelSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(cancelButton);
        }

        public void FocusToDefault()
        {
            cancelButton.Focus();
        }
    }
}
