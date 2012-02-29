#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class ErrorDialog : MessageBox
    {
        DialogMessageContainer messageContainer;

        public Control Message
        {
            get { return messageContainer.Content; }
            set { messageContainer.Content = value; }
        }

        public ErrorDialog(Screen screen)
            : base(screen)
        {
            ShadowOffset = new Vector2(4);
            Padding = new Thickness(16);

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            Content = stackPanel;

            messageContainer = new DialogMessageContainer(screen)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            stackPanel.Children.Add(messageContainer);

            var separator = ControlUtil.CreateDefaultSeparator(screen);
            stackPanel.Children.Add(separator);

            var okButton = ControlUtil.CreateDefaultDialogButton(screen, Strings.OKButton);
            stackPanel.Children.Add(okButton);
            RegisterOKButton(okButton);

            okButton.Focus();
        }
    }
}
