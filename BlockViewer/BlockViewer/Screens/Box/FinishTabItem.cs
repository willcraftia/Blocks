#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens.Box
{
    public sealed class FinishTabItem : ContentControl
    {
        public event EventHandler UploadSelected = delegate { };

        public event EventHandler CancelSelected = delegate { };

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
                Text = Strings.BoxWizFinishTitle,
                ForegroundColor = Color.Yellow,
                BackgroundColor = Color.Black,
                ShadowOffset = new Vector2(2)
            };
            stackPanel.Children.Add(titleTextBlock);

            var messageTextBlock = new TextBlock(Screen)
            {
                Text = Strings.BoxWizFinishMessage,
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

            var uploadButton = ControlUtil.CreateDefaultDialogButton(Screen, Strings.UploadButton);
            uploadButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                UploadSelected(this, EventArgs.Empty);
            };
            buttonPanel.Children.Add(uploadButton);

            var cancelButton = ControlUtil.CreateDefaultDialogButton(Screen, Strings.CancelButton);
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
