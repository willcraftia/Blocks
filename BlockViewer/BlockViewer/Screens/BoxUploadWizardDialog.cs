#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Net.Box.Service;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class BoxUploadWizardDialog : OverlayDialogBase
    {
        public delegate string GetTicketDelegate();

        TabControl tabControl;

        Button cancelButton;

        Button step2NextButton;

        FloatLerpAnimation openAnimation;

        string ticket;

        bool hasTicket;

        GetTicketDelegate getTicketDelegate;

        public BoxUploadWizardDialog(Screen screen)
            : base(screen)
        {
            // 開く際に openAnimation で Width を設定するので 0 で初期化します。
            Width = 0;
            ShadowOffset = new Vector2(4);
            Padding = new Thickness(16);

            Overlay.Opacity = 0.5f;

            tabControl = new TabControl(screen)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            Content = tabControl;

            var step1Control = CreateStep1Control();
            tabControl.Items.Add(step1Control);
            tabControl.SelectedIndex = 0;

            var step2Control = CreateStep2Control();
            tabControl.Items.Add(step2Control);

            cancelButton.Focus();

            openAnimation = new FloatLerpAnimation
            {
                Action = (current) => { Width = current; },
                From = 0,
                To = 480,
                Duration = TimeSpan.FromSeconds(0.1f)
            };
            Animations.Add(openAnimation);
        }

        Control CreateStep1Control()
        {
            var stackPanel = new StackPanel(Screen)
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            var titleTextBlock = new TextBlock(Screen)
            {
                Text = "Step 1: Confirmation",
                ForegroundColor = Color.Yellow,
                BackgroundColor = Color.Black,
                ShadowOffset = new Vector2(2)
            };
            stackPanel.Children.Add(titleTextBlock);

            var messageTextBlock = new TextBlock(Screen)
            {
                Text =
                    "To use this function, you have to get your Box (http://www.box.com) account. " +
                    "This function requires to access it with REST API provided by Box.\n\n" +
                    "Select [Next] button if you allow our application to access your Box account, " +
                    "otherwise select [Cancel].",
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
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            stackPanel.Children.Add(buttonPanel);

            var nextButton = ControlUtil.CreateDefaultDialogButton(Screen, "Next");
            nextButton.Click += OnStep1NextButtonClick;
            buttonPanel.Children.Add(nextButton);

            cancelButton = ControlUtil.CreateDefaultDialogButton(Screen, "Cancel");
            cancelButton.Click += OnCancelButtonClick;
            buttonPanel.Children.Add(cancelButton);

            return stackPanel;
        }

        Control CreateStep2Control()
        {
            var stackPanel = new StackPanel(Screen)
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            var textBlock = new TextBlock(Screen)
            {
                Text = "Step 2: Getting the Box ticket for you...",
                ForegroundColor = Color.White,
                BackgroundColor = Color.Black
            };
            stackPanel.Children.Add(textBlock);

            var buttonPanel = new StackPanel(Screen)
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            stackPanel.Children.Add(buttonPanel);

            step2NextButton = ControlUtil.CreateDefaultDialogButton(Screen, "Next");
            step2NextButton.Enabled = false;
            step2NextButton.Click += OnStep1NextButtonClick;
            buttonPanel.Children.Add(step2NextButton);

            var backButton = ControlUtil.CreateDefaultDialogButton(Screen, "Back");
            backButton.Click += OnCancelButtonClick;
            buttonPanel.Children.Add(backButton);

            return stackPanel;
        }

        public override void Show()
        {
            openAnimation.Enabled = true;
            base.Show();
        }

        public override void Update(GameTime gameTime)
        {
            if (ticket != null)
            {
                step2NextButton.Enabled = true;
            }

            base.Update(gameTime);
        }

        void OnStep1NextButtonClick(Control sender, ref RoutedEventContext context)
        {
            tabControl.SelectedIndex = tabControl.SelectedIndex + 1;

            ticket = null;

            if (getTicketDelegate == null)
            {
                var boxService = Screen.Game.Services.GetRequiredService<IBoxService>();
                getTicketDelegate = new GetTicketDelegate(boxService.GetTicket);
            }

            getTicketDelegate.BeginInvoke(GetTicketAsyncCallback, null);
        }

        void OnCancelButtonClick(Control sender, ref RoutedEventContext context)
        {
            Close();
        }

        void GetTicketAsyncCallback(IAsyncResult asyncResult)
        {
            ticket = getTicketDelegate.EndInvoke(asyncResult);
        }
    }
}
