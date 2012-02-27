#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo.Screens
{
    public sealed class MainMenuDemoScreen : Screen
    {
        #region MenuWindow

        class MenuWindow : Window
        {
            public MenuWindow(Screen screen)
                : base(screen)
            {
                BackgroundColor = Color.Blue;

                var stackPanel = new StackPanel(screen)
                {
                    Margin = new Thickness(8),
                    Orientation = Orientation.Vertical
                };
                Content = stackPanel;

                var newGameButton = new Button(screen)
                {
                    Padding = new Thickness(8),
                    Content = new TextBlock(screen)
                    {
                        Text = "NEW GAME (DUMMY)",
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    }
                };
                stackPanel.Children.Add(newGameButton);
                newGameButton.PreviewMouseEnter += new RoutedEventHandler(OnButtonMouseEnter);
                newGameButton.PreviewMouseLeave += new RoutedEventHandler(OnButtonMouseLeave);
                newGameButton.Focus();

                var textBox = new TextBox(screen)
                {
                    Width = unit * 8
                };
                stackPanel.Children.Add(textBox);

                var switchScreenButton = new Button(screen)
                {
                    Padding = new Thickness(8),
                    Content = new TextBlock(screen)
                    {
                        Text = "SWITCH SCREEN",
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    }
                };
                stackPanel.Children.Add(switchScreenButton);
                switchScreenButton.Click += new RoutedEventHandler(OnSwitchScreenButtonClick);
                switchScreenButton.PreviewMouseEnter += new RoutedEventHandler(OnButtonMouseEnter);
                switchScreenButton.PreviewMouseLeave += new RoutedEventHandler(OnButtonMouseLeave);

                var exitButton = new Button(screen)
                {
                    Padding = new Thickness(8),
                    Content = new TextBlock(screen)
                    {
                        Text = "EXIT",
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    }
                };
                stackPanel.Children.Add(exitButton);
                exitButton.Click += new RoutedEventHandler(OnExitButtonClick);
                exitButton.PreviewMouseEnter += new RoutedEventHandler(OnButtonMouseEnter);
                exitButton.PreviewMouseLeave += new RoutedEventHandler(OnButtonMouseLeave);
            }

            void OnButtonMouseEnter(Control sender, ref RoutedEventContext context)
            {
                var button = sender as Button;
                button.Content.ForegroundColor = Color.Yellow;
                context.Handled = true;
            }

            void OnButtonMouseLeave(Control sender, ref RoutedEventContext context)
            {
                var button = sender as Button;
                button.Content.ForegroundColor = Color.White;
                context.Handled = true;
            }

            void OnSwitchScreenButtonClick(Control sender, ref RoutedEventContext context)
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
                    uiService.Show("WindowDemoScreen");
                };
                Animations.Add(opacityAnimation);
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

        #endregion

        const int unit = 32;

        public MainMenuDemoScreen(Game game)
            : base(game)
        {
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            var menuWindow = new MenuWindow(this);
            menuWindow.Show();

            var startEffectOverlay = new Overlay(this)
            {
                Opacity = 1
            };
            startEffectOverlay.Show();

            var startEffectOverlay_opacityAnimation = new FloatLerpAnimation
            {
                Action = (current) => { startEffectOverlay.Opacity = current; },
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5d),
                Enabled = true
            };
            startEffectOverlay_opacityAnimation.Completed += (s, e) =>
            {
                startEffectOverlay.Close();
                menuWindow.Activate();
            };
            startEffectOverlay.Animations.Add(startEffectOverlay_opacityAnimation);

            base.LoadContent();
        }
    }
}
