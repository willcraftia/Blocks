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

        public TextBlock Title { get; private set; }

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

            var stackPanel = new StackPanel(Screen)
            {
                Orientation = Orientation.Vertical
            };
            Content = stackPanel;

            Title = new TextBlock(Screen)
            {
                Text = Strings.ErrorTitle,
                Padding = new Thickness(4),
                ForegroundColor = Color.Yellow,
                BackgroundColor = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextHorizontalAlignment = HorizontalAlignment.Left,
                ShadowOffset = new Vector2(2)
            };
            stackPanel.Children.Add(Title);

            var separatorTexture = Screen.Content.Load<Texture2D>("UI/Separator");

            var separator0 = new Image(Screen)
            {
                Texture = separatorTexture,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 4)
            };
            stackPanel.Children.Add(separator0);

            messageContainer = new DialogMessageContainer(Screen);
            stackPanel.Children.Add(messageContainer);

            var separator1 = new Image(Screen)
            {
                Texture = separatorTexture,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 0, 0, 4)
            };
            stackPanel.Children.Add(separator1);

            var okButton = ControlUtil.CreateDefaultMenuButton(Screen, MessageBox.OKText);
            stackPanel.Children.Add(okButton);
            RegisterOKButton(okButton);

            okButton.Focus();
        }
    }
}
