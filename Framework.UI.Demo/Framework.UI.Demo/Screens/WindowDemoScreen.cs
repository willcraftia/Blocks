#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo.Screens
{
    public sealed class WindowDemoScreen : Screen
    {
        #region FirstWindow

        class FirstWindow : Window
        {
            public FirstWindow(Screen screen)
                : base(screen)
            {
                Width = unit * 10;
                Height = unit * 10;
                HorizontalAlignment = HorizontalAlignment.Left;
                VerticalAlignment = VerticalAlignment.Top;
                Margin = new Thickness(unit, unit, 0, 0);
                BackgroundColor = Color.Red;

                var firstWindow_widthAnimation = new PropertyLerpAnimation
                {
                    Target = this,
                    PropertyName = "Width",
                    From = 0,
                    To = Width,
                    Repeat = Repeat.Forever,
                    BeginTime = TimeSpan.Zero,
                    Duration = TimeSpan.FromSeconds(2),
                    Enabled = true
                };
                Animations.Add(firstWindow_widthAnimation);

                var firstWindow_HeightAnimation = new PropertyLerpAnimation
                {
                    Target = this,
                    PropertyName = "Height",
                    From = 0,
                    To = Height,
                    Repeat = Repeat.Forever,
                    BeginTime = TimeSpan.Zero,
                    Duration = TimeSpan.FromSeconds(2),
                    Enabled = true
                };
                Animations.Add(firstWindow_HeightAnimation);
            }
        }

        #endregion

        #region SecondWindow

        class SecondWindow : Window
        {
            public SecondWindow(Screen screen)
                : base(screen)
            {
                Width = unit * 10;
                Height = unit * 10;
                HorizontalAlignment = HorizontalAlignment.Right;
                VerticalAlignment = VerticalAlignment.Bottom;
                Margin = new Thickness(0, 0, unit, unit);
                BackgroundColor = Color.Green;

                var opacityAnimation = new PropertyLerpAnimation
                {
                    Target = this,
                    PropertyName = "Opacity",
                    From = 0,
                    To = 1,
                    Repeat = Repeat.Forever,
                    BeginTime = TimeSpan.Zero,
                    Duration = TimeSpan.FromSeconds(2),
                    AutoReversed = true,
                    Enabled = true
                };
                Animations.Add(opacityAnimation);
            }
        }

        #endregion

        #region ThirdWindow

        class ThirdWindow : Window
        {
            RotateCubeAnimation rotateCubeAnimation;

            public ThirdWindow(Screen screen)
                : base(screen)
            {
                Width = unit * 15;
                Height = unit * 10;
                BackgroundColor = Color.Blue;

                var stackPanel = new StackPanel(screen)
                {
                    Margin = new Thickness(8),
                    Orientation = Orientation.Horizontal
                };
                Content = stackPanel;

                var openNewDialogButton = new Button(screen)
                {
                    Margin = new Thickness(8),
                    Padding = new Thickness(8),
                    Content = new TextBlock(screen)
                    {
                        Text = "Open new dialog",
                        FontStretch = new Vector2(1.0f, 3.0f),
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    }
                };
                stackPanel.Children.Add(openNewDialogButton);
                openNewDialogButton.Click += new RoutedEventHandler(OnOpenNewDialogButtonClick);
                openNewDialogButton.PreviewMouseEnter += new RoutedEventHandler(OnButtonMouseEnter);
                openNewDialogButton.PreviewMouseLeave += new RoutedEventHandler(OnButtonMouseLeave);
                openNewDialogButton.Focus();

                var cubeControl = new CubeControl(screen)
                {
                    Margin = new Thickness(8),
                    CubePrimitive = CreateCubePrimitive(),
                    ForegroundColor = Color.White,
                    Width = unit * 7,
                    Height = unit * 7
                };
                stackPanel.Children.Add(cubeControl);

                rotateCubeAnimation = new RotateCubeAnimation
                {
                    CubeButton = cubeControl,
                    From = 0,
                    To = MathHelper.TwoPi,
                    BeginTime = TimeSpan.Zero,
                    Duration = TimeSpan.FromSeconds(4),
                    Repeat = Repeat.Forever
                };
                Animations.Add(rotateCubeAnimation);
                cubeControl.MouseEnter += new RoutedEventHandler(OnCubeControlMouseEnter);
                cubeControl.MouseLeave += new RoutedEventHandler(OnCubeControlMouseLeave);

                var thirdWindow_widthAnimation = new PropertyLerpAnimation
                {
                    Target = this,
                    PropertyName = "Width",
                    From = 0,
                    To = Width,
                    BeginTime = TimeSpan.Zero,
                    Duration = TimeSpan.FromSeconds(1),
                    Enabled = true
                };
                Animations.Add(thirdWindow_widthAnimation);

                var thirdWindow_heightAnimation = new PropertyLerpAnimation
                {
                    Target = this,
                    PropertyName = "Height",
                    From = 0,
                    To = Height,
                    BeginTime = TimeSpan.Zero,
                    Duration = TimeSpan.FromSeconds(1),
                    Enabled = true
                };
                thirdWindow_heightAnimation.Completed += (s, e) => cubeControl.CubeVisible = true;
                Animations.Add(thirdWindow_heightAnimation);
            }

            void OnOpenNewDialogButtonClick(Control sender, ref RoutedEventContext context)
            {
                var firstDialog = new FirstDialog(Screen);
                firstDialog.Show();
            }

            void OnCubeControlMouseEnter(Control sender, ref RoutedEventContext context)
            {
                var cubeControl = sender as CubeControl;
                cubeControl.Scale = 1.5f;

                rotateCubeAnimation.Enabled = true;
            }

            void OnCubeControlMouseLeave(Control sender, ref RoutedEventContext context)
            {
                var cubeControl = sender as CubeControl;
                cubeControl.Scale = 1;

                rotateCubeAnimation.Enabled = false;
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

            /// <summary>
            /// 立方体の GeometricPrimitive を生成します。
            /// </summary>
            /// <returns>生成された立方体の GeometricPrimitive。</returns>
            Graphics.GeometricPrimitive CreateCubePrimitive()
            {
                //var cubeVertexSourceFactory = new Graphics.CubeVertexSourceFactory();
                var cubeVertexSourceFactory = new Graphics.ColoredCubeVertexSourceFactory();
                cubeVertexSourceFactory.TopSurfaceColor = Color.Green;
                cubeVertexSourceFactory.BottomSurfaceColor = Color.GreenYellow;
                cubeVertexSourceFactory.NorthSurfaceColor = Color.Blue;
                cubeVertexSourceFactory.SouthSurfaceColor = Color.BlueViolet;
                cubeVertexSourceFactory.EastSurfaceColor = Color.Red;
                cubeVertexSourceFactory.WestSurfaceColor = Color.OrangeRed;

                var source = cubeVertexSourceFactory.CreateVertexSource();
                return Graphics.GeometricPrimitive.Create(Screen.GraphicsDevice, source);
            }
        }

        #endregion

        #region FirstDialog

        class FirstDialog : Window
        {
            public FirstDialog(Screen screen)
                : base(screen)
            {
                Width = unit * 7;
                SizeToContent = SizeToContent.WidthAndHeight;
                BackgroundColor = Color.Green;

                var stackPanel = new StackPanel(screen)
                {
                    Margin = new Thickness(8),
                    Orientation = Orientation.Horizontal
                };
                Content = stackPanel;

                var openNewDialogButton = new Button(screen)
                {
                    Margin = new Thickness(8),
                    Padding = new Thickness(8),
                    Height = unit,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Content = new TextBlock(screen)
                    {
                        Text = "Open new dialog",
                        TextHorizontalAlignment = HorizontalAlignment.Left,
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    }
                };
                stackPanel.Children.Add(openNewDialogButton);
                openNewDialogButton.Click += new RoutedEventHandler(OnOpenNewDialogButtonClick);
                openNewDialogButton.PreviewMouseEnter += new RoutedEventHandler(OnButtonMouseEnter);
                openNewDialogButton.PreviewMouseLeave += new RoutedEventHandler(OnButtonMouseLeave);
                openNewDialogButton.Focus();

                var closeButton = new Button(screen)
                {
                    Margin = new Thickness(8),
                    Padding = new Thickness(8),
                    Height = unit,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Content = new TextBlock(screen)
                    {
                        Text = "Close",
                        TextHorizontalAlignment = HorizontalAlignment.Left,
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    }
                };
                stackPanel.Children.Add(closeButton);
                closeButton.Click += new RoutedEventHandler(OnCloseButtonClick);
                closeButton.PreviewMouseEnter += new RoutedEventHandler(OnButtonMouseEnter);
                closeButton.PreviewMouseLeave += new RoutedEventHandler(OnButtonMouseLeave);

                var switchScreenButton = new Button(screen)
                {
                    Margin = new Thickness(8),
                    Padding = new Thickness(8),
                    Height = unit,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Content = new TextBlock(screen)
                    {
                        Text = "Switch Screen",
                        TextHorizontalAlignment = HorizontalAlignment.Left,
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
                    Margin = new Thickness(8),
                    Padding = new Thickness(8),
                    Height = unit,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Content = new TextBlock(screen)
                    {
                        Text = "Exit",
                        TextHorizontalAlignment = HorizontalAlignment.Left,
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    }
                };
                stackPanel.Children.Add(exitButton);
                exitButton.Click += new RoutedEventHandler(OnExitButtonClick);
                exitButton.PreviewMouseEnter += new RoutedEventHandler(OnButtonMouseEnter);
                exitButton.PreviewMouseLeave += new RoutedEventHandler(OnButtonMouseLeave);
            }

            void OnCloseButtonClick(Control sender, ref RoutedEventContext context)
            {
                Close();
            }

            void OnOpenNewDialogButtonClick(Control sender, ref RoutedEventContext context)
            {
                var secondDialog = new SecondDialog(Screen);
                secondDialog.Show();
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
                Animations.Add(opacityAnimation);
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
                    uiService.Show("MainMenuDemoScreen");
                };
                Animations.Add(opacityAnimation);
            }

            public override void Show()
            {
                var overlay = new Overlay(Screen)
                {
                    Opacity = 0.5f,
                    BackgroundColor = Color.Black
                };
                overlay.Owner = this;
                overlay.Show();

                base.Show();
            }
        }

        #endregion

        #region SecondDialog

        class SecondDialog : Window
        {
            public SecondDialog(Screen screen)
                : base(screen)
            {
                SizeToContent = SizeToContent.WidthAndHeight;
                BackgroundColor = Color.Brown;

                var closeButton = new Button(screen)
                {
                    Width = unit * 2,
                    Height = unit,
                    Margin = new Thickness(unit),
                    Content = new TextBlock(screen)
                    {
                        Text = "Close",
                        TextHorizontalAlignment = HorizontalAlignment.Left,
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    }
                };
                Content = closeButton;
                closeButton.Click += new RoutedEventHandler(OnCloseButtonClick);
                closeButton.PreviewMouseEnter += new RoutedEventHandler(OnButtonMouseEnter);
                closeButton.PreviewMouseLeave += new RoutedEventHandler(OnButtonMouseLeave);
                closeButton.Focus();
            }

            void OnCloseButtonClick(Control sender, ref RoutedEventContext context)
            {
                Close();
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

            public override void Show()
            {
                var overlay = new Overlay(Screen)
                {
                    Opacity = 0.7f,
                    BackgroundColor = Color.Black
                };
                overlay.Owner = this;
                overlay.Show();

                base.Show();
            }
        }

        #endregion

        const int unit = 32;

        public WindowDemoScreen(Game game)
            : base(game)
        {
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            // 重いロードのテスト用にスリープさせてます。
            System.Threading.Thread.Sleep(2000);

            var firstWindow = new FirstWindow(this);
            firstWindow.Show();

            var secondWindow = new SecondWindow(this);
            secondWindow.Show();

            var thirdWindow = new ThirdWindow(this);
            thirdWindow.Show();

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
                thirdWindow.Activate();
            };
            startEffectOverlay.Animations.Add(startEffectOverlay_opacityAnimation);

            base.LoadContent();
        }
    }
}
