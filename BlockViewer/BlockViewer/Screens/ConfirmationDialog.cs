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
    public sealed class ConfirmationDialog : MessageBox
    {
        DialogMessageContainer messageContainer;

        Button cancelButton;

        public TextBlock Title { get; private set; }

        public Control Message
        {
            get { return messageContainer.Content; }
            set { messageContainer.Content = value; }
        }

        public ConfirmationDialog(Screen screen)
            : base(screen)
        {
            ShadowOffset = new Vector2(4);
            Padding = new Thickness(16);

            var stackPanel = new StackPanel(Screen)
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            Content = stackPanel;

            Title = new TextBlock(Screen)
            {
                Text = Strings.ConfirmationTitle,
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
                Margin = new Thickness(0, 0, 0, 8)
            };
            stackPanel.Children.Add(separator0);

            messageContainer = new DialogMessageContainer(Screen);
            stackPanel.Children.Add(messageContainer);

            var separator1 = new Image(Screen)
            {
                Texture = separatorTexture,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(0, 8, 0, 4)
            };
            stackPanel.Children.Add(separator1);

            var okButton = CreateButton(Screen, MessageBox.OKText);
            stackPanel.Children.Add(okButton);
            RegisterOKButton(okButton);

            cancelButton = CreateButton(Screen, MessageBox.CancelText);
            stackPanel.Children.Add(cancelButton);
            RegisterCancelButton(cancelButton);
        }

        public override void Show()
        {
            // 常に Cancel にフォーカスを再設定します。
            cancelButton.Focus();

            base.Show();
        }

        //
        // MEMO:
        // MainMenuWindow からコピーしました。
        //
        Button CreateButton(Screen screen, String text)
        {
            float buttonHeight = 32;

            var button = new Button(screen)
            {
                Height = buttonHeight,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Padding = new Thickness(4),

                Content = new TextBlock(screen)
                {
                    Text = text,
                    ForegroundColor = Color.White,
                    BackgroundColor = Color.Black,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    TextHorizontalAlignment = HorizontalAlignment.Left,
                    //Margin = new Thickness(16, 0, 0, 0),
                    ShadowOffset = new Vector2(2)
                }
            };

            ControlUtil.SetDefaultBehavior(button);

            return button;
        }
    }
}
