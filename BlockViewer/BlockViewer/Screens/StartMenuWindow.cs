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
                Orientation = Orientation.Vertical
            };
            Children.Add(stackPanel);

            var startButton = new Button(screen)
            {
                Width = 200,
                Padding = new Thickness(8)
            };
            stackPanel.Children.Add(startButton);

            var startTextBlock = new TextBlock(screen)
            {
                Text = Strings.StartButtonText
            };
            startButton.Children.Add(startTextBlock);

            startButton.GotFocus += (s, e) => startTextBlock.ForegroundColor = Color.Yellow;
            startButton.LostFocus += (s, e) => startTextBlock.ForegroundColor = Color.White;

            var exitButton = new Button(screen)
            {
                Width = 200,
                Padding = new Thickness(8)
            };
            stackPanel.Children.Add(exitButton);

            var exitTextBlock = new TextBlock(screen)
            {
                Text = Strings.ExitButtonText
            };
            exitButton.Children.Add(exitTextBlock);

            exitButton.Click += new EventHandler(OnExitButtonClick);
            exitButton.GotFocus += (s, e) => exitTextBlock.ForegroundColor = Color.Yellow;
            exitButton.LostFocus += (s, e) => exitTextBlock.ForegroundColor = Color.White;
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
