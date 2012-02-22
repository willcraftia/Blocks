#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI.Resources;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    public sealed class MessageBox : Window
    {
        StackPanel buttonsPanel;

        public MessageBoxButton Button { get; private set; }

        public MessageBoxResult DefaultResult { get; private set; }

        public float OverlayOpacity { get; set; }

        public Color OverlayColor { get; set; }

        public TextBlock TextBlock { get; private set; }

        public Button OKButton { get; private set; }

        public Button CancelButton { get; private set; }

        public Button YesButton { get; private set; }

        public Button NoButton { get; private set; }

        public MessageBoxResult Result { get; private set; }

        public MessageBox(Screen screen, MessageBoxButton button, MessageBoxResult defaultResult)
            : base(screen)
        {
            Button = button;
            DefaultResult = defaultResult;
            OverlayOpacity = 0;
            OverlayColor = Color.Black;
            Result = MessageBoxResult.None;

            var basePanel = new StackPanel(Screen)
            {
                Orientation = Orientation.Vertical
            };
            Content = basePanel;

            TextBlock = new TextBlock(Screen);
            basePanel.Children.Add(TextBlock);

            buttonsPanel = new StackPanel(Screen);
            basePanel.Children.Add(buttonsPanel);

            switch (Button)
            {
                case MessageBoxButton.OKCancel:
                    ArrangeOKButton();
                    ArrangeCancelButton();
                    if (DefaultResult == MessageBoxResult.OK)
                    {
                        OKButton.Focus();
                    }
                    else
                    {
                        CancelButton.Focus();
                    }
                    break;
                case MessageBoxButton.YesNo:
                    ArrangeYesButton();
                    ArrangeNoButton();
                    if (DefaultResult == MessageBoxResult.Yes)
                    {
                        YesButton.Focus();
                    }
                    else
                    {
                        NoButton.Focus();
                    }
                    break;
                case MessageBoxButton.YesNoCancel:
                    ArrangeYesButton();
                    ArrangeNoButton();
                    ArrangeCancelButton();
                    if (DefaultResult == MessageBoxResult.Yes)
                    {
                        YesButton.Focus();
                    }
                    else if (DefaultResult == MessageBoxResult.No)
                    {
                        NoButton.Focus();
                    }
                    else
                    {
                        CancelButton.Focus();
                    }
                    break;
                case MessageBoxButton.OK:
                default:
                    ArrangeOKButton();
                    OKButton.Focus();
                    break;
            }
        }

        public override void Show()
        {
            var overlay = new Overlay(Screen)
            {
                Opacity = OverlayOpacity,
                BackgroundColor = OverlayColor
            };
            overlay.Owner = this;
            overlay.Show();

            base.Show();
        }

        protected override void OnKeyDown(ref RoutedEventContext context)
        {
            base.OnKeyDown(ref context);

            if (Screen.KeyboardDevice.IsKeyPressed(Keys.Escape))
            {
                switch (Button)
                {
                    case MessageBoxButton.OK:
                        Result = MessageBoxResult.OK;
                        break;
                    case MessageBoxButton.OKCancel:
                        Result = MessageBoxResult.Cancel;
                        break;
                    case MessageBoxButton.YesNo:
                        Result = MessageBoxResult.None;
                        break;
                    case MessageBoxButton.YesNoCancel:
                        Result = MessageBoxResult.Cancel;
                        break;
                    default:
                        Result = MessageBoxResult.None;
                        break;
                }

                Close();
                context.Handled = true;
            }
        }

        void OnOKButtonClick(Control sender, ref RoutedEventContext context)
        {
            Result = MessageBoxResult.OK;
            Close();
        }

        void OnCancelButtonClick(Control sender, ref RoutedEventContext context)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }

        void OnYesButtonClick(Control sender, ref RoutedEventContext context)
        {
            Result = MessageBoxResult.Yes;
            Close();
        }

        void OnNoButtonClick(Control sender, ref RoutedEventContext context)
        {
            Result = MessageBoxResult.No;
            Close();
        }

        void ArrangeOKButton()
        {
            OKButton = CreateButton(Strings.OK);
            OKButton.Click += new RoutedEventHandler(OnOKButtonClick);
            buttonsPanel.Children.Add(OKButton);
        }

        void ArrangeCancelButton()
        {
            CancelButton = CreateButton(Strings.Cancel);
            CancelButton.Click += new RoutedEventHandler(OnCancelButtonClick);
            buttonsPanel.Children.Add(CancelButton);
        }

        void ArrangeYesButton()
        {
            YesButton = CreateButton(Strings.Yes);
            YesButton.Click += new RoutedEventHandler(OnYesButtonClick);
            buttonsPanel.Children.Add(YesButton);
        }

        void ArrangeNoButton()
        {
            NoButton = CreateButton(Strings.No);
            NoButton.Click += new RoutedEventHandler(OnNoButtonClick);
            buttonsPanel.Children.Add(NoButton);
        }

        Button CreateButton(string text)
        {
            return new Button(Screen)
            {
                Content = new TextBlock(Screen)
                {
                    Text = text
                }
            };
        }
    }
}
