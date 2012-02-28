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
            Width = 240;
            ShadowOffset = new Vector2(4);
            Padding = new Thickness(16);

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            Content = stackPanel;

            var startButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.StartButton);
            startButton.Click += new RoutedEventHandler(OnStartButtonClick);
            stackPanel.Children.Add(startButton);

            var selectLanguageButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.SelectLanguageButton);
            selectLanguageButton.Click += new RoutedEventHandler(OnLanguageSettingButtonClick);
            stackPanel.Children.Add(selectLanguageButton);

            var exitButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.ExitButton);
            exitButton.Click += new RoutedEventHandler(OnExitButtonClick);
            stackPanel.Children.Add(exitButton);

            // デフォルト フォーカス。
            startButton.Focus();
        }

        void OnStartButtonClick(Control sender, ref RoutedEventContext context)
        {
            var overlay = new Overlay(Screen);
            overlay.Show();

            var opacityAnimation = new FloatLerpAnimation
            {
                Action = (current) => { overlay.Opacity = current; },
                To = 1,
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
            var dialog = new SelectLanguageDialog(Screen);
            dialog.Show();
        }

        void OnExitButtonClick(Control sender, ref RoutedEventContext context)
        {
            var overlay = new Overlay(Screen);
            overlay.Show();

            var opacityAnimation = new FloatLerpAnimation
            {
                Action = (current) => { overlay.Opacity = current; },
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5d),
                Enabled = true
            };
            opacityAnimation.Completed += (s, e) => Screen.Game.Exit();
            Animations.Add(opacityAnimation);
        }
    }
}
