﻿#region Using

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
            Children.Add(stackPanel);

            var startButton = new Button(screen)
            {
                Width = 200,
                Padding = new Thickness(8)
            };
            {
                stackPanel.Children.Add(startButton);

                var textBlock = new TextBlock(screen)
                {
                    Text = Strings.StartButtonText
                };
                startButton.Children.Add(textBlock);

                startButton.GotFocus += (s, e) => textBlock.ForegroundColor = Color.Yellow;
                startButton.LostFocus += (s, e) => textBlock.ForegroundColor = Color.White;
            }

            var languageSettingButton = new Button(screen)
            {
                Width = 200,
                Padding = new Thickness(8)
            };
            {
                stackPanel.Children.Add(languageSettingButton);

                var textBlock = new TextBlock(screen)
                {
                    Text = Strings.LanguageSettingButtonText
                };
                languageSettingButton.Children.Add(textBlock);

                languageSettingButton.Click += new EventHandler(OnLanguageSettingButtonClick);
                languageSettingButton.GotFocus += (s, e) => textBlock.ForegroundColor = Color.Yellow;
                languageSettingButton.LostFocus += (s, e) => textBlock.ForegroundColor = Color.White;
            }

            var exitButton = new Button(screen)
            {
                Width = 200,
                Padding = new Thickness(8)
            };
            {
                stackPanel.Children.Add(exitButton);

                var textBlock = new TextBlock(screen)
                {
                    Text = Strings.ExitButtonText
                };
                exitButton.Children.Add(textBlock);

                exitButton.Click += new EventHandler(OnExitButtonClick);
                exitButton.GotFocus += (s, e) => textBlock.ForegroundColor = Color.Yellow;
                exitButton.LostFocus += (s, e) => textBlock.ForegroundColor = Color.White;
            }
        }

        void OnLanguageSettingButtonClick(object sender, EventArgs e)
        {
            var dialog = new LanguageSettingDialog(Screen);
            dialog.BackgroundColor = Color.Black;
            dialog.Opacity = 0.5f;
            dialog.Show();
        }

        void OnExitButtonClick(object sender, EventArgs e)
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