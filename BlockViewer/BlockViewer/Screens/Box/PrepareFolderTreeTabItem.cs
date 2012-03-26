#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens.Box
{
    public sealed class PrepareFolderTreeTabItem : ContentControl
    {
        public event EventHandler CreateSelected = delegate { };

        public event EventHandler CancelSelected = delegate { };

        const string title = "Prepare Folders for Blocks";

        const string message = "This application accessed your Box account successfully.\n\n" +
            "Next, the folder named 'Blocks Data' to upload Blocks's data is needed on your Box.\n\n" +
            "Please select [Create] button if you allow to create this folder on you Box, otherwise [Cancel].";

        Button defaultFocusedButton;

        public PrepareFolderTreeTabItem(Screen screen)
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

            var createButton = ControlUtil.CreateDefaultDialogButton(Screen, "Create");
            createButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                CreateSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(createButton);

            var cancelButton = ControlUtil.CreateDefaultDialogButton(Screen, "Cancel");
            cancelButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                CancelSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(cancelButton);

            defaultFocusedButton = createButton;
        }

        public void FocusToDefault()
        {
            defaultFocusedButton.Focus();
        }
    }
}
