﻿#region Using

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
            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(16)
            };
            Content = stackPanel;

            var startButton = new TextButton(screen);
            startButton.TextBlock.Text = Strings.StartButton;
            startButton.TextBlock.ForegroundColor = Color.White;
            startButton.HorizontalAlignment = HorizontalAlignment.Stretch;
            startButton.Padding = new Thickness(4);
            startButton.Click += new RoutedEventHandler(OnStartButtonClick);
            stackPanel.Children.Add(startButton);

            var languageSettingButton = new TextButton(screen);
            languageSettingButton.TextBlock.Text = Strings.LanguageSettingButton;
            languageSettingButton.TextBlock.ForegroundColor = Color.White;
            languageSettingButton.HorizontalAlignment = HorizontalAlignment.Stretch;
            languageSettingButton.Padding = new Thickness(4);
            languageSettingButton.Click += new RoutedEventHandler(OnLanguageSettingButtonClick);
            stackPanel.Children.Add(languageSettingButton);

            var exitButton = new TextButton(screen);
            exitButton.TextBlock.Text = Strings.ExitButton;
            exitButton.TextBlock.ForegroundColor = Color.White;
            exitButton.HorizontalAlignment = HorizontalAlignment.Stretch;
            exitButton.Padding = new Thickness(4);
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
