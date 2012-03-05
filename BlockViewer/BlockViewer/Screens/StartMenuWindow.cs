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
        SelectLanguageDialog selectLanguageDialog;

        Button changeLookAndFeelButton;

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

            changeLookAndFeelButton = ControlUtil.CreateDefaultMenuButton(screen, "Look & Feel [Debug]");
            changeLookAndFeelButton.Click += new RoutedEventHandler(OnChangeLookAndFeelButtonClick);
            stackPanel.Children.Add(changeLookAndFeelButton);

            var exitButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.ExitButton);
            exitButton.Click += new RoutedEventHandler(OnExitButtonClick);
            stackPanel.Children.Add(exitButton);

            // デフォルト フォーカス。
            startButton.Focus();
        }

        void OnStartButtonClick(Control sender, ref RoutedEventContext context)
        {
            var overlay = new FadeOverlay(Screen);
            overlay.OpacityAnimation.To = 1;
            overlay.OpacityAnimation.Duration = TimeSpan.FromSeconds(0.5d);
            overlay.OpacityAnimation.Completed += (s, e) => Screen.ShowScreen(ScreenNames.Main);
            overlay.Show();
        }

        void OnLanguageSettingButtonClick(Control sender, ref RoutedEventContext context)
        {
            if (selectLanguageDialog == null)
                selectLanguageDialog = new SelectLanguageDialog(Screen);

            selectLanguageDialog.Show();
        }

        void OnChangeLookAndFeelButtonClick(Control sender, ref RoutedEventContext context)
        {
            var startScreen = Screen as StartScreen;
            if (startScreen.SelectedLookAndFeelSourceIndex == StartScreen.DefaultLookAndFeelIndex)
            {
                startScreen.SelectedLookAndFeelSourceIndex = StartScreen.DebugLookAndFeelIndex;
                (changeLookAndFeelButton.Content as TextBlock).Text = "Look & Feel [Default]";
            }
            else
            {
                startScreen.SelectedLookAndFeelSourceIndex = StartScreen.DefaultLookAndFeelIndex;
                (changeLookAndFeelButton.Content as TextBlock).Text = "Look & Feel [Debug]";
            }
        }

        void OnExitButtonClick(Control sender, ref RoutedEventContext context)
        {
            var overlay = new FadeOverlay(Screen);
            overlay.OpacityAnimation.To = 1;
            overlay.OpacityAnimation.Duration = TimeSpan.FromSeconds(0.5d);
            overlay.OpacityAnimation.Completed += (s, e) => Screen.Game.Exit();
            overlay.Show();
        }
    }
}
