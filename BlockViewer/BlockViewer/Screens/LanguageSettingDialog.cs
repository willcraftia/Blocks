#region Using

using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class LanguageSettingDialog : Window
    {
        Button setDefaultButton;

        Button setJaButton;
        
        Button setEnButton;

        public LanguageSettingDialog(Screen screen)
            : base(screen)
        {
            SizeToContent = SizeToContent.WidthAndHeight;

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                Padding = new Thickness(8)
            };
            Content = stackPanel;

            setDefaultButton = new Button(screen)
            {
                Padding = new Thickness(8),
                Content = new TextBlock(screen)
                {
                    Text = Strings.DefaultButtonText
                }
            };
            stackPanel.Children.Add(setDefaultButton);
            setDefaultButton.Click += new RoutedEventHandler(OnButtonClick);
            setDefaultButton.GotFocus += new RoutedEventHandler(OnButtonGotFocus);
            setDefaultButton.LostFocus += new RoutedEventHandler(OnButtonLostFocus);
            setDefaultButton.Focus();

            setJaButton = new Button(screen)
            {
                Padding = new Thickness(8),
                Content = new TextBlock(screen)
                {
                    Text = Strings.JaButtonText
                }
            };
            stackPanel.Children.Add(setJaButton);
            setJaButton.Click += new RoutedEventHandler(OnButtonClick);
            setJaButton.GotFocus += new RoutedEventHandler(OnButtonGotFocus);
            setJaButton.LostFocus += new RoutedEventHandler(OnButtonLostFocus);

            setEnButton = new Button(screen)
            {
                Padding = new Thickness(8),
                Content = new TextBlock(screen)
                {
                    Text = Strings.EnButtonText
                }
            };
            stackPanel.Children.Add(setEnButton);
            setEnButton.Click += new RoutedEventHandler(OnButtonClick);
            setEnButton.GotFocus += new RoutedEventHandler(OnButtonGotFocus);
            setEnButton.LostFocus += new RoutedEventHandler(OnButtonLostFocus);
        }

        void OnButtonClick(Control sender, ref RoutedEventContext context)
        {
            var button = sender as Button;

            CultureInfo culture = null;

            if (button == setDefaultButton)
            {
                culture = CultureInfo.CurrentCulture;
            }
            else if (button == setJaButton)
            {
                culture = new CultureInfo("ja");
            }
            else if (button == setEnButton)
            {
                culture = new CultureInfo("en");
            }

            if (culture != null) Strings.Culture  = culture;

            var uiService = Screen.Game.Services.GetRequiredService<IUIService>();
            uiService.Show<ViewerStartScreen>();
        }

        void OnButtonGotFocus(Control sender, ref RoutedEventContext context)
        {
            (sender as Button).Content.ForegroundColor = Color.Yellow;
        }

        void OnButtonLostFocus(Control sender, ref RoutedEventContext context)
        {
            (sender as Button).Content.ForegroundColor = Color.White;
        }

        public override void Show()
        {
            var overlay = new Overlay(Screen)
            {
                Opacity = 0.5f,
                BackgroundColor = Color.Black
            };
            overlay.Owner = this;
            overlay.Show();

            base.Show();
        }
    }
}
