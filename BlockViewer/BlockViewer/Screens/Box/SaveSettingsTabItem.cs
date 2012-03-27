#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens.Box
{
    public sealed class SaveSettingsTabItem : ContentControl
    {
        public event EventHandler YesSelected = delegate { };

        public event EventHandler NoSelected = delegate { };

        Button defaultFocusedButton;

        public SaveSettingsTabItem(Screen screen)
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
                Text = Strings.BoxWizSaveSettingsTitle,
                ForegroundColor = Color.Yellow,
                BackgroundColor = Color.Black,
                ShadowOffset = new Vector2(2)
            };
            stackPanel.Children.Add(titleTextBlock);

            var messageTextBlock = new TextBlock(Screen)
            {
                Text = Strings.BoxWizSaveSettingsMessage,
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

            var yesButton = ControlUtil.CreateDefaultDialogButton(Screen, Strings.YesButton);
            yesButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                YesSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(yesButton);

            var noButton = ControlUtil.CreateDefaultDialogButton(Screen, Strings.NoButton);
            noButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                NoSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(noButton);

            defaultFocusedButton = yesButton;
        }

        public void FocusToDefault()
        {
            defaultFocusedButton.Focus();
        }
    }
}
