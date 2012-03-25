#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens.Box
{
    public sealed class SaveSettingsTabItem : ContentControl
    {
        public event EventHandler YesSelected = delegate { };

        public event EventHandler NoSelected = delegate { };

        const string title = "Access Succeeded";

        const string message = "This application accessed your Box account successfully.\n\n" +
            "You can save this settings on your storage. " +
            "You will setup every time when use Box integration if don't save it.\n\n" +
            "Please select [Yes] button if you want to save this settings, otherwise [No].";

        Button noButton;

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

            var yesButton = ControlUtil.CreateDefaultDialogButton(Screen, "Yes");
            yesButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                YesSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(yesButton);

            noButton = ControlUtil.CreateDefaultDialogButton(Screen, "No");
            noButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                NoSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(noButton);
        }

        public void FocusToDefault()
        {
            noButton.Focus();
        }
    }
}
