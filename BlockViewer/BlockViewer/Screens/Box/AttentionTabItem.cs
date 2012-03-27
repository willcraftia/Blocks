#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens.Box
{
    public sealed class AttentionTabItem : ContentControl
    {
        public event EventHandler AgreeSelected = delegate { };

        public event EventHandler CancelSelected = delegate { };

        Button defaultFocusedButton;

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
                Text = Strings.BoxWizAttentionTitle,
                ForegroundColor = Color.Yellow,
                BackgroundColor = Color.Black,
                ShadowOffset = new Vector2(2)
            };
            stackPanel.Children.Add(titleTextBlock);

            var messageTextBlock = new TextBlock(Screen)
            {
                Text = Strings.BoxWizAttentionMessage,
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

            var agreeButton = ControlUtil.CreateDefaultDialogButton(Screen, Strings.AgreeButton);
            agreeButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                AgreeSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(agreeButton);

            var cancelButton = ControlUtil.CreateDefaultDialogButton(Screen, Strings.CancelButton);
            cancelButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                CancelSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(cancelButton);

            defaultFocusedButton = cancelButton;
        }

        public void FocusToDefault()
        {
            defaultFocusedButton.Focus();
        }
    }
}
