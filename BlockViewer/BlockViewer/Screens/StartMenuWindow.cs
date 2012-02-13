#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

            var cursor = (screen as StartScreen).CursorTexture;

            var startButton = new CustomButton(screen)
            {
                Width = 200
            };
            startButton.Cursor.Texture = cursor;
            startButton.TextBlock.Text = Strings.StartButtonText;
            stackPanel.Children.Add(startButton);
            startButton.Focus();

            var languageSettingButton = new CustomButton(screen)
            {
                Width = 200
            };
            languageSettingButton.Cursor.Texture = cursor;
            languageSettingButton.TextBlock.Text = Strings.LanguageSettingButtonText;
            languageSettingButton.Click += new RoutedEventHandler(OnLanguageSettingButtonClick);
            stackPanel.Children.Add(languageSettingButton);

            var exitButton = new CustomButton(screen)
            {
                Width = 200
            };
            exitButton.Cursor.Texture = cursor;
            exitButton.TextBlock.Text = Strings.ExitButtonText;
            exitButton.Click += new RoutedEventHandler(OnExitButtonClick);
            stackPanel.Children.Add(exitButton);
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
