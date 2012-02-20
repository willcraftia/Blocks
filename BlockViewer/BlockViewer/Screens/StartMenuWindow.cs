#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework;
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
            Padding = new Thickness(16);

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical
            };
            Content = stackPanel;

            var cursor = (screen as StartScreen).CursorTexture;

            var startButton = new CustomButton(screen);
            startButton.TextBlock.Text = Strings.StartButtonText;
            startButton.Click += new RoutedEventHandler(OnStartButtonClick);
            stackPanel.Children.Add(startButton);

            var languageSettingButton = new CustomButton(screen);
            languageSettingButton.TextBlock.Text = Strings.LanguageSettingButtonText;
            languageSettingButton.Click += new RoutedEventHandler(OnLanguageSettingButtonClick);
            stackPanel.Children.Add(languageSettingButton);

            var exitButton = new CustomButton(screen);
            exitButton.TextBlock.Text = Strings.ExitButtonText;
            exitButton.Click += new RoutedEventHandler(OnExitButtonClick);
            stackPanel.Children.Add(exitButton);

            // デフォルト フォーカス。
            startButton.Focus();
        }

        void OnStartButtonClick(Control sender, ref RoutedEventContext context)
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
            opacityAnimation.Completed += (s, e) =>
            {
                var uiService = Screen.Game.Services.GetRequiredService<IUIService>();
                uiService.Show(ScreenNames.Main);
            };
            Animations.Add(opacityAnimation);
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
            opacityAnimation.Completed += (s, e) => Screen.Game.Exit();
            Animations.Add(opacityAnimation);
        }
    }
}
