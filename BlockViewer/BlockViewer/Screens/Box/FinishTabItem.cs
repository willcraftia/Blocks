#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens.Box
{
    public sealed class FinishTabItem : ContentControl
    {
        public event EventHandler UploadSelected = delegate { };

        public event EventHandler CancelSelected = delegate { };

        const string title = "Box Setup Finished";

        const string message = "All settings to use Box integration are finished.\n\n" +
            "Please select [Upload] button if you want to upload demo Blocks meshes in your Box, otherwise [Cancel].";

        Button defaultFocusedButton;

        public FinishTabItem(Screen screen)
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

            var uploadButton = ControlUtil.CreateDefaultDialogButton(Screen, "Upload");
            uploadButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                UploadSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(uploadButton);

            var cancelButton = ControlUtil.CreateDefaultDialogButton(Screen, "Cancel");
            cancelButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                CancelSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(cancelButton);

            defaultFocusedButton = uploadButton;
        }

        public void FocusToDefault()
        {
            defaultFocusedButton.Focus();
        }
    }
}
