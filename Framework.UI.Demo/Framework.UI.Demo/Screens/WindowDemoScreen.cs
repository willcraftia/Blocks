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
        public WindowDemoScreen(Game game)
            : base(game)
        {
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            // 重いロードのテスト用にスリープさせてます。
            System.Threading.Thread.Sleep(2000);

            var cubeVertexSourceFactory = new Graphics.CubeVertexSourceFactory();

            // Unit size.
            int u = 32;

            var viewportBounds = GraphicsDevice.Viewport.TitleSafeArea;
            Desktop.BackgroundColor = Color.CornflowerBlue;
            Desktop.Margin = new Thickness(viewportBounds.Left, viewportBounds.Top, 0, 0);
            Desktop.Width = viewportBounds.Width;
            Desktop.Height = viewportBounds.Height;

            var screenOverlay = new Overlay()
            {
                Opacity = 1,
                BackgroundColor = Color.Black
            };
            {
                var animation = new PropertyLerpAnimation()
                {
                    Target = screenOverlay,
                    PropertyName = "Opacity",
                    From = 1,
                    To = 0,
                    BeginTime = TimeSpan.Zero,
                    Duration = TimeSpan.FromSeconds(0.5d),
                    Enabled = true
                };
                animation.Completed += delegate(object exitOverlayAnimationSender, EventArgs exitOverlayAnimationEvent)
                {
                    screenOverlay.Close();
                };
                Animations.Add(animation);
            }

            {
                var window = new Window()
                {
                    Width = u * 10,
                    Height = u * 10,
                    Margin = new Thickness(u, u, 0, 0),
                    BackgroundColor = Color.Red
                };
                {
                    var animation = new PropertyLerpAnimation()
                    {
                        Target = window,
                        PropertyName = "Width",
                        From = 0,
                        To = window.Width,
                        Repeat = Repeat.Forever,
                        BeginTime = TimeSpan.Zero,
                        Duration = TimeSpan.FromSeconds(2),
                        Enabled = true
                    };
                    Animations.Add(animation);
                }
                {
                    var animation = new PropertyLerpAnimation()
                    {
                        Target = window,
                        PropertyName = "Height",
                        From = 0,
                        To = window.Height,
                        Repeat = Repeat.Forever,
                        BeginTime = TimeSpan.Zero,
                        Duration = TimeSpan.FromSeconds(2),
                        Enabled = true
                    };
                    Animations.Add(animation);
                }
                window.Show(this);
            }

            {
                var window = new Window()
                {
                    Width = u * 10,
                    Height = u * 10,
                    Margin = new Thickness(u * 3, u * 3, 0, 0),
                    BackgroundColor = Color.Green
                };
                {
                    var animation = new PropertyLerpAnimation()
                    {
                        Target = window,
                        PropertyName = "Opacity",
                        From = 0,
                        To = 1,
                        Repeat = Repeat.Forever,
                        BeginTime = TimeSpan.Zero,
                        Duration = TimeSpan.FromSeconds(1),
                        AutoReversed = true,
                        Enabled = true
                    };
                    Animations.Add(animation);
                }
                window.Show(this);
            }

            {
                var window = new Window()
                {
                    Width = u * 15,
                    Height = u * 10,
                    Margin = new Thickness(u * 5, u * 5, 0, 0),
                    BackgroundColor = Color.Blue
                };
                {
                    var animation = new PropertyLerpAnimation()
                    {
                        Target = window,
                        PropertyName = "Width",
                        From = 0,
                        To = window.Width,
                        BeginTime = TimeSpan.Zero,
                        Duration = TimeSpan.FromSeconds(1),
                        Enabled = true
                    };
                    Animations.Add(animation);
                }
                {
                    var animation = new PropertyLerpAnimation()
                    {
                        Target = window,
                        PropertyName = "Height",
                        From = 0,
                        To = window.Height,
                        BeginTime = TimeSpan.Zero,
                        Duration = TimeSpan.FromSeconds(1),
                        Enabled = true
                    };
                    Animations.Add(animation);
                }

                var stackPanel = new StackPanel()
                {
                    Margin = new Thickness(8),
                    Orientation = Orientation.Horizontal
                };
                window.Children.Add(stackPanel);

                {
                    var button = new Button()
                    {
                        Text = "Open new dialog",
                        FontStretch = new Vector2(1.0f, 2.0f),
                        ForegroundColor = Color.White,
                        VerticalAlignment = VerticalAlignment.Center,
                        Margin = new Thickness(8),
                        Padding = new Thickness(8)
                    };
                    stackPanel.Children.Add(button);
                    button.Clicked += delegate(object s, EventArgs e)
                    {
                        var overlay = new Overlay()
                        {
                            Opacity = 0.5f,
                            BackgroundColor = Color.Black
                        };

                        var subWindow = new Window()
                        {
                            Width = u * 7,
                            SizeToContent = SizeToContent.Height,
                            Margin = new Thickness(u * 4, u * 6, 0, 0),
                            BackgroundColor = Color.Green
                        };
                        overlay.Children.Add(subWindow);
                        subWindow.Owner = overlay;

                        var subStackPanel = new StackPanel()
                        {
                            Margin = new Thickness(8),
                            Orientation = Orientation.Vertical
                        };
                        subWindow.Children.Add(subStackPanel);

                        {
                            var subButton = new Button()
                            {
                                Text = "Open new dialog",
                                TextHorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(8),
                                Padding = new Thickness(8),
                                ForegroundColor = Color.White,
                                Height = u,
                                HorizontalAlignment = HorizontalAlignment.Left
                            };
                            subStackPanel.Children.Add(subButton);
                            subButton.Clicked += delegate(object bs, EventArgs be)
                            {
                                var subOverlay = new Overlay()
                                {
                                    Opacity = 0.5f,
                                    BackgroundColor = Color.Black
                                };

                                var subSubWindow = new Window()
                                {
                                    SizeToContent = SizeToContent.WidthAndHeight,
                                    Margin = new Thickness(u * 1, u * 4, 0, 0),
                                    BackgroundColor = Color.Brown
                                };
                                subOverlay.Children.Add(subSubWindow);
                                subSubWindow.Owner = subOverlay;

                                {
                                    var subSubButton = new Button()
                                    {
                                        Text = "Close",
                                        TextHorizontalAlignment = HorizontalAlignment.Left,
                                        ForegroundColor = Color.White,
                                        Width = u * 2,
                                        Height = u,
                                        Margin = new Thickness(u)
                                    };
                                    subSubButton.Clicked += delegate(object subBs, EventArgs subBe)
                                    {
                                        subOverlay.Close();
                                    };
                                    subSubWindow.Children.Add(subSubButton);
                                }

                                subOverlay.Show(this);
                            };
                        }
                        {
                            var subButton = new Button()
                            {
                                Text = "Close",
                                TextHorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(8),
                                Padding = new Thickness(8),
                                ForegroundColor = Color.White,
                                Height = u,
                                HorizontalAlignment = HorizontalAlignment.Left
                            };
                            subStackPanel.Children.Add(subButton);
                            subButton.Clicked += delegate(object bs, EventArgs be)
                            {
                                overlay.Close();
                            };
                        }
                        {
                            var subButton = new Button()
                            {
                                Text = "Switch Screen",
                                TextHorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(8),
                                Padding = new Thickness(8),
                                ForegroundColor = Color.White,
                                Height = u,
                                HorizontalAlignment = HorizontalAlignment.Left
                            };
                            subStackPanel.Children.Add(subButton);
                            subButton.Clicked += delegate(object bs, EventArgs be)
                            {
                                var exitOverlay = new Overlay()
                                {
                                    Opacity = 0,
                                    BackgroundColor = Color.Black
                                };
                                {
                                    var animation = new PropertyLerpAnimation()
                                    {
                                        Target = exitOverlay,
                                        PropertyName = "Opacity",
                                        From = 0,
                                        To = 1,
                                        BeginTime = TimeSpan.Zero,
                                        Duration = TimeSpan.FromSeconds(0.5d),
                                        Enabled = true
                                    };
                                    animation.Completed += delegate(object exitOverlayAnimationSender, EventArgs exitOverlayAnimationEvent)
                                    {
                                        var uiService = Game.Services.GetRequiredService<IUIService>();
                                        uiService.Show("MainMenuDemoScreen");
                                    };
                                    Animations.Add(animation);
                                }
                                exitOverlay.Show(this);
                            };
                        }
                        {
                            var subButton = new Button()
                            {
                                Text = "Exit",
                                TextHorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(8),
                                Padding = new Thickness(8),
                                ForegroundColor = Color.White,
                                Height = u,
                                HorizontalAlignment = HorizontalAlignment.Left
                            };
                            subStackPanel.Children.Add(subButton);
                            subButton.Clicked += delegate(object bs, EventArgs be)
                            {
                                var exitOverlay = new Overlay()
                                {
                                    Opacity = 0,
                                    BackgroundColor = Color.Black
                                };
                                {
                                    var animation = new PropertyLerpAnimation()
                                    {
                                        Target = exitOverlay,
                                        PropertyName = "Opacity",
                                        From = 0,
                                        To = 1,
                                        BeginTime = TimeSpan.Zero,
                                        Duration = TimeSpan.FromSeconds(0.5d),
                                        Enabled = true
                                    };
                                    animation.Completed += delegate(object exitOverlayAnimationSender, EventArgs exitOverlayAnimationEvent)
                                    {
                                        Game.Exit();
                                    };
                                    Animations.Add(animation);
                                }
                                exitOverlay.Show(this);
                            };
                        }

                        overlay.Show(this);
                    };
                }
                {
                    var button = new CubeControl()
                    {
                        CubePrimitive = CreateCubePrimitive(),
                        Clipped = false,
                        ForegroundColor = Color.White,
                        Width = u * 7
                    };
                    stackPanel.Children.Add(button);

                    var rotateCubeTimelineAnimation = new RotateCubeAnimation()
                    {
                        CubeButton = button,
                        From = 0,
                        To = MathHelper.TwoPi,
                        BeginTime = TimeSpan.Zero,
                        Duration = TimeSpan.FromSeconds(4),
                        Repeat = Repeat.Forever
                    };
                    Animations.Add(rotateCubeTimelineAnimation);

                    button.MouseEntered += delegate(object s, EventArgs e)
                    {
                        button.Scale = 1.5f;
                        rotateCubeTimelineAnimation.Enabled = true;
                    };
                    button.MouseLeft += delegate(object s, EventArgs e)
                    {
                        button.Scale = 1;
                        rotateCubeTimelineAnimation.Enabled = false;
                    };
                }

                window.Show(this);
            }

            screenOverlay.Show(this);

            base.LoadContent();
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
            return Graphics.GeometricPrimitive.Create(GraphicsDevice, source);
        }
    }
}
