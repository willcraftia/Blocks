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
                Width = unit * 10;
                Height = unit * 4;
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
                opacityAnimation.Completed += (s, evt) =>
                {
                    var uiService = Screen.Game.Services.GetRequiredService<IUIService>();
                    uiService.Show("WindowDemoScreen");
                };
                Screen.Animations.Add(opacityAnimation);
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

        #endregion

        const int unit = 32;

        public MainMenuDemoScreen(Game game)
            : base(game)
        {
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            var viewportBounds = GraphicsDevice.Viewport.TitleSafeArea;
            Desktop.BackgroundColor = Color.CornflowerBlue;
            Desktop.Margin = new Thickness(viewportBounds.Left, viewportBounds.Top, 0, 0);
            Desktop.Width = viewportBounds.Width;
            Desktop.Height = viewportBounds.Height;

            var menuWindow = new MenuWindow(this);
            menuWindow.Show();

            var startEffectOverlay = new Overlay(this)
            {
                Opacity = 1,
                BackgroundColor = Color.Black
            };
            startEffectOverlay.Show();

            var startEffectOverlay_opacityAnimation = new PropertyLerpAnimation
            {
                Target = startEffectOverlay,
                PropertyName = "Opacity",
                From = 1,
                To = 0,
                BeginTime = TimeSpan.Zero,
                Duration = TimeSpan.FromSeconds(0.5d),
                Enabled = true
            };
            startEffectOverlay_opacityAnimation.Completed += (s, e) =>
            {
                startEffectOverlay.Close();
                menuWindow.Activate();
            };
            Animations.Add(startEffectOverlay_opacityAnimation);

            base.LoadContent();
        }
    }
}
