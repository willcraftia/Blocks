#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    /// <summary>
    /// ViewStartScreen に表示するメニュー ウィンドウです。
    /// </summary>
    public sealed class StartMenuWindow : Window
    {
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        public StartMenuWindow(Screen screen)
            : base(screen)
        {
            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                Padding = new Thickness(8)
            };
            Content = stackPanel;

            var startButton = new Button(screen)
            {
                Width = 200,
                Padding = new Thickness(4),
                Content = new TextBlock(screen)
                {
                    Text = Strings.StartButtonText
                }
            };
            stackPanel.Children.Add(startButton);
            startButton.GotFocus += new RoutedEventHandler(OnButtonGotFocus);
            startButton.LostFocus += new RoutedEventHandler(OnButtonLostFocus);
            startButton.Focus();

            var languageSettingButton = new Button(screen)
            {
                Width = 200,
                Padding = new Thickness(4),
                Content = new TextBlock(screen)
                {
                    Text = Strings.LanguageSettingButtonText
                }
            };
            stackPanel.Children.Add(languageSettingButton);
            languageSettingButton.Click += new RoutedEventHandler(OnLanguageSettingButtonClick);
            languageSettingButton.GotFocus += new RoutedEventHandler(OnButtonGotFocus);
            languageSettingButton.LostFocus += new RoutedEventHandler(OnButtonLostFocus);

            var exitButton = new Button(screen)
            {
                Width = 200,
                Padding = new Thickness(4),
                Content = new TextBlock(screen)
                {
                    Text = Strings.ExitButtonText
                }
            };
            stackPanel.Children.Add(exitButton);
            exitButton.Click += new RoutedEventHandler(OnExitButtonClick);
            exitButton.GotFocus += new RoutedEventHandler(OnButtonGotFocus);
            exitButton.LostFocus += new RoutedEventHandler(OnButtonLostFocus);
        }

        void OnButtonGotFocus(Control sender, ref RoutedEventContext context)
        {
            (sender as Button).Content.ForegroundColor = Color.Yellow;
        }

        void OnButtonLostFocus(Control sender, ref RoutedEventContext context)
        {
            (sender as Button).Content.ForegroundColor = Color.White;
        }

        void OnLanguageSettingButtonClick(Control sender, ref RoutedEventContext context)
        {
            var dialog = new LanguageSettingDialog(Screen);
            dialog.Show();
        }

        void OnExitButtonClick(Control sender, ref RoutedEventContext context)
        {
            var overlay = new Overlay(Screen)
            {
                Opacity = 0,
                BackgroundColor = Color.Black
            };
            overlay.Show();

            var opacityAnimation = new PropertyLerpAnimation
            {
                Target = overlay,
                PropertyName = "Opacity",
                From = 0,
                To = 1,
                BeginTime = TimeSpan.Zero,
                Duration = TimeSpan.FromSeconds(0.5d),
                Enabled = true
            };
            opacityAnimation.Completed += (s, evt) => Screen.Game.Exit();
            Screen.Animations.Add(opacityAnimation);
        }
    }
}
