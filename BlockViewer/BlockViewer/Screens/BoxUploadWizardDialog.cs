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

        public delegate BoxSession GetAuthTokenDelegate(string ticket);

        IBoxService boxService;

        BoxSession boxSession;

        TabControl tabControl;

        Button cancelButton;

        Button step2NextButton;

        Button step2BackButton;

        Button step3NextButton;

        Button step3BackButton;

        FloatLerpAnimation openAnimation;

        string ticket;

        GetTicketDelegate getTicketDelegate;

        GetAuthTokenDelegate getAuthTokenDelegate;

        BoxProgressDialog boxProgressDialog;

        bool closingBoxProgressDialog;

        bool forwardStep4;

        readonly object syncRoot = new object();

        public BoxUploadWizardDialog(Screen screen)
            : base(screen)
        {
            boxService = Screen.Game.Services.GetRequiredService<IBoxService>();

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

            var step3Control = CreateStep3Control();
            tabControl.Items.Add(step3Control);

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
                Text = "Step 1: About Box Integration",
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
                    "Please select [Next] button if you allow our application to access your Box account, " +
                    "otherwise select [Cancel] button.",
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

            var titleTextBlock = new TextBlock(Screen)
            {
                Text = "Step 2: The Authorization",
                ForegroundColor = Color.Yellow,
                BackgroundColor = Color.Black,
                ShadowOffset = new Vector2(2)
            };
            stackPanel.Children.Add(titleTextBlock);

            var messageTextBlock = new TextBlock(Screen)
            {
                Text =
                    "Now, this application was allowed to use Box REST API. " +
                    "To access your Box account from this application, you need to authorize it on Box.\n\n" +
                    "Please select [Launch web browser] button to open the authorization page in your web browser.",
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

            step2NextButton = ControlUtil.CreateDefaultDialogButton(Screen, "Launch web browser");
            step2NextButton.Enabled = false;
            step2NextButton.Click += OnStep2NextButtonClick;
            buttonPanel.Children.Add(step2NextButton);

            step2BackButton = ControlUtil.CreateDefaultDialogButton(Screen, "Back");
            step2BackButton.Click += OnStep2BackButtonClick;
            buttonPanel.Children.Add(step2BackButton);

            return stackPanel;
        }

        Control CreateStep3Control()
        {
            var stackPanel = new StackPanel(Screen)
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };

            var titleTextBlock = new TextBlock(Screen)
            {
                Text = "Step 3: The Confirmation",
                ForegroundColor = Color.Yellow,
                BackgroundColor = Color.Black,
                ShadowOffset = new Vector2(2)
            };
            stackPanel.Children.Add(titleTextBlock);

            var messageTextBlock = new TextBlock(Screen)
            {
                Text =
                    "Now, this application will be able to access your Box account if you authorized it on Box.\n\n" +
                    "Please select [Yes, authorized.] button if you authorized this application on Box.",
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

            step3NextButton = ControlUtil.CreateDefaultDialogButton(Screen, "Yes, authorized.");
            step3NextButton.Click += OnStep3NextButtonClick;
            buttonPanel.Children.Add(step3NextButton);

            step3BackButton = ControlUtil.CreateDefaultDialogButton(Screen, "Back");
            step3BackButton.Click += new RoutedEventHandler(OnStep3BackButtonClick);
            buttonPanel.Children.Add(step3BackButton);

            return stackPanel;
        }

        public override void Show()
        {
            openAnimation.Enabled = true;
            base.Show();
        }

        public override void Update(GameTime gameTime)
        {
            lock (syncRoot)
            {
                if (ticket != null)
                {
                    step2NextButton.Enabled = true;
                }
                if (closingBoxProgressDialog)
                {
                    CloseProgressDialog();
                    closingBoxProgressDialog = false;
                }
                if (forwardStep4)
                {
                    tabControl.SelectedIndex = tabControl.SelectedIndex + 1;
                    forwardStep4 = false;
                }
            }

            base.Update(gameTime);
        }

        void OnStep1NextButtonClick(Control sender, ref RoutedEventContext context)
        {
            tabControl.SelectedIndex = tabControl.SelectedIndex + 1;
            step2BackButton.Focus();

            ticket = null;

            if (getTicketDelegate == null)
                getTicketDelegate = new GetTicketDelegate(boxService.GetTicket);
            getTicketDelegate.BeginInvoke(GetTicketAsyncCallback, null);

            ShowProgressDialog("Getting your Box ticket...");
        }

        void OnCancelButtonClick(Control sender, ref RoutedEventContext context)
        {
            Close();
        }

        void OnStep2NextButtonClick(Control sender, ref RoutedEventContext context)
        {
            boxService.RedirectUserAuth(ticket);

            tabControl.SelectedIndex = tabControl.SelectedIndex + 1;
        }

        void OnStep2BackButtonClick(Control sender, ref RoutedEventContext context)
        {
            tabControl.SelectedIndex = tabControl.SelectedIndex - 1;
            cancelButton.Focus();
        }

        void OnStep3NextButtonClick(Control sender, ref RoutedEventContext context)
        {
            if (getAuthTokenDelegate == null)
                getAuthTokenDelegate = new GetAuthTokenDelegate(boxService.GetAuthToken);
            getAuthTokenDelegate.BeginInvoke(ticket, GetAuthTokenAsyncCallback, null);

            ShowProgressDialog("Getting your authorized token...");
        }

        void OnStep3BackButtonClick(Control sender, ref RoutedEventContext context)
        {
            tabControl.SelectedIndex = tabControl.SelectedIndex - 1;
            step2BackButton.Focus();
        }

        void GetTicketAsyncCallback(IAsyncResult asyncResult)
        {
            lock (syncRoot)
            {
                ticket = getTicketDelegate.EndInvoke(asyncResult);
                closingBoxProgressDialog = true;
            }
        }

        void GetAuthTokenAsyncCallback(IAsyncResult asyncResult)
        {
            lock (syncRoot)
            {
                boxSession = getAuthTokenDelegate.EndInvoke(asyncResult);
                closingBoxProgressDialog = true;
            }
        }

        void ShowProgressDialog(string message)
        {
            if (boxProgressDialog == null)
            {
                boxProgressDialog = new BoxProgressDialog(Screen);
            }
            boxProgressDialog.Message = message;
            boxProgressDialog.Show();
        }

        void CloseProgressDialog()
        {
            boxProgressDialog.Close();
        }
    }
}
