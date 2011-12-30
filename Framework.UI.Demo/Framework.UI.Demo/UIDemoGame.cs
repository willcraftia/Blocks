#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Lafs;
using Willcraftia.Xna.Framework.UI.Lafs.Debug;
using Willcraftia.Xna.Framework.UI.Lafs.Sprite;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo
{
    public class UIDemoGame : Game
    {
        GraphicsDeviceManager graphics;

        InputManager inputManager;

        UIManager uiManager;

        DebugControlLafSource debugControlLafSource;

        SpriteControlLafSource spriteControlLafSource;

        Graphics.GeometricPrimitive cubePrimitive;

        public UIDemoGame()
        {
            graphics = new GraphicsDeviceManager(this);

            //
            // REFERENCE: http://creators.xna.com/ja-jp/education/bestpractices
            //            http://social.msdn.microsoft.com/Forums/ja-JP/xnagameja/thread/60a6d9d9-1ede-4657-a77c-9ff65edd563a
            //
            // Xbox360:
            // 640x480
            // 720x480
            // 1280x720 (720p)
            // 1980x1080
            // 
            graphics.PreferredBackBufferWidth = 720;
            graphics.PreferredBackBufferHeight = 480;
            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 720;
            // for recording settings
            //graphics.PreferredBackBufferWidth = 640;
            //graphics.PreferredBackBufferHeight = 480;
            //graphics.PreferredBackBufferWidth = 320;
            //graphics.PreferredBackBufferHeight = 240;

            graphics.PreferMultiSampling = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            inputManager = new InputManager(this);
            Components.Add(inputManager);

            uiManager = new UIManager(this);
            Components.Add(uiManager);

            debugControlLafSource = new DebugControlLafSource("Content/UI/Debug");

            spriteControlLafSource = new SpriteControlLafSource("Content/UI/Sprite");
            spriteControlLafSource.SpriteSize = 32;

            uiManager.ControlLafSource = debugControlLafSource;
            //uiManager.ControlLafSource = spriteControlLafSource;

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            //LoadSimpleGameDemoGui();
            LoadSimpleWindowDemoGui();
        }

        void LoadSimpleGameDemoGui()
        {
            // Unit size.
            int u = 32;

            var viewportBounds = GraphicsDevice.Viewport.Bounds;
            var screen = new Screen()
            {
                Margin = new Thickness(viewportBounds.Left, viewportBounds.Top, 0, 0),
                Width = viewportBounds.Width,
                Height = viewportBounds.Height
            };

            var window = new Window()
            {
                Width = u * 10,
                Height = u * 4
            };
            window.Margin = new Thickness((screen.Width - window.Width) * 0.5f, (screen.Height - window.Height) * 0.5f, 0, 0);

            {
                {
                    var button = new Button()
                    {
                        Width = u * 8,
                        Height = u,
                        Margin = new Thickness(u, u, 0, 0),
                        Text = "NEW GAME",
                        ForegroundColor = Color.White
                    };
                    window.Children.Add(button);
                }
                {
                    var button = new Button()
                    {
                        Width = u * 8,
                        Height = u,
                        Margin = new Thickness(u, u * 2, 0, 0),
                        Text = "EXIT",
                        ForegroundColor = Color.White
                    };
                    window.Children.Add(button);
                }
            }
            window.Show(screen);

            uiManager.Screen = screen;
        }

        void LoadSimpleWindowDemoGui()
        {
            var cubePrimitiveFactory = new Graphics.CubePrimitiveFactory();
            cubePrimitive = cubePrimitiveFactory.Create(GraphicsDevice);

            // Unit size.
            int u = 32;

            var viewportBounds = GraphicsDevice.Viewport.Bounds;
            var screen = new Screen()
            {
                Margin = new Thickness(viewportBounds.Left, viewportBounds.Top, 0, 0),
                Width = viewportBounds.Width,
                Height = viewportBounds.Height
            };

            {
                var window = new Window()
                {
                    Width = u * 10,
                    Height = u * 10,
                    Margin = new Thickness(u, u, 0, 0),
                    BackgroundColor = Color.Red
                };
                {
                    var animation = new Animations.PropertyLerpAnimation()
                    {
                        Target = window,
                        PropertyName = "Width",
                        From = 0,
                        To = window.Width,
                        Repeat = Animations.Repeat.Forever,
                        BeginTime = new TimeSpan(0, 0, 0),
                        Duration = new TimeSpan(0, 0, 3)
                    };
                    screen.Animations.Add(animation);
                    animation.Enabled = true;
                }
                {
                    var animation = new Animations.PropertyLerpAnimation()
                    {
                        Target = window,
                        PropertyName = "Height",
                        From = 0,
                        To = window.Height,
                        Repeat = Animations.Repeat.Forever,
                        BeginTime = new TimeSpan(0, 0, 0),
                        Duration = new TimeSpan(0, 0, 3),
                        Enabled = true
                    };
                    screen.Animations.Add(animation);
                }

                window.Show(screen);
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
                    var animation = new Animations.PropertyLerpAnimation()
                    {
                        Target = window,
                        PropertyName = "Opacity",
                        From = 0,
                        To = 1,
                        Repeat = Animations.Repeat.Forever,
                        BeginTime = new TimeSpan(0, 0, 0),
                        Duration = new TimeSpan(0, 0, 5),
                        AutoReversed = true,
                        Enabled = true
                    };
                    screen.Animations.Add(animation);
                }
                window.Show(screen);
            }

            {
                var window = new Window()
                {
                    Width = u * 10,
                    Height = u * 10,
                    Margin = new Thickness(u * 5, u * 5, 0, 0),
                    BackgroundColor = Color.Blue
                };
                {
                    var animation = new Animations.PropertyLerpAnimation()
                    {
                        Target = window,
                        PropertyName = "Width",
                        From = 0,
                        To = window.Width,
                        BeginTime = new TimeSpan(0, 0, 0),
                        Duration = new TimeSpan(0, 0, 1),
                        Enabled = true
                    };
                    screen.Animations.Add(animation);
                }
                {
                    var animation = new Animations.PropertyLerpAnimation()
                    {
                        Target = window,
                        PropertyName = "Height",
                        From = 0,
                        To = window.Height,
                        BeginTime = new TimeSpan(0, 0, 0),
                        Duration = new TimeSpan(0, 0, 1),
                        Enabled = true
                    };
                    screen.Animations.Add(animation);
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
                        ForegroundColor = Color.White,
                        VerticalAlignment = UI.VerticalAlignment.Center,
                        Margin = new Thickness(8),
                        Height = u * 3,
                        Width = u * 5
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
                            SizeToContent = Controls.SizeToContent.Height,
                            Margin = new Thickness(u * 4, u * 6, 0, 0),
                            BackgroundColor = Color.Green * 0.8f
                        };
                        overlay.Children.Add(subWindow);

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
                                    SizeToContent = Controls.SizeToContent.WidthAndHeight,
                                    Margin = new Thickness(u * 1, u * 4, 0, 0),
                                    BackgroundColor = Color.Brown * 0.8f
                                };
                                subOverlay.Children.Add(subSubWindow);

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

                                subOverlay.Show(screen);
                            };
                        }
                        {
                            var subButton = new Button()
                            {
                                Text = "Close",
                                TextHorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(8),
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
                                Text = "Exit",
                                TextHorizontalAlignment = HorizontalAlignment.Left,
                                Margin = new Thickness(8),
                                ForegroundColor = Color.White,
                                Height = u,
                                HorizontalAlignment = HorizontalAlignment.Left
                            };
                            subStackPanel.Children.Add(subButton);
                            subButton.Clicked += delegate(object bs, EventArgs be)
                            {
                                var exitOverlay = new Controls.Overlay()
                                {
                                    Opacity = 0,
                                    BackgroundColor = Color.Black
                                };
                                {
                                    var animation = new Animations.PropertyLerpAnimation()
                                    {
                                        Target = exitOverlay,
                                        PropertyName = "Opacity",
                                        From = 0,
                                        To = 1,
                                        BeginTime = new TimeSpan(0, 0, 0),
                                        Duration = new TimeSpan(0, 0, 2),
                                        Enabled = true
                                    };
                                    animation.Completed += delegate(object exitOverlayAnimationSender, EventArgs exitOverlayAnimationEvent)
                                    {
                                        Exit();
                                    };
                                    screen.Animations.Add(animation);
                                }
                                exitOverlay.Show(screen);
                            };
                        }

                        overlay.Show(screen);
                    };
                }
                {
                    var button = new CubeButton()
                    {
                        CubePrimitive = cubePrimitive,
                        Clipped = false,
                        ForegroundColor = Color.White,
                        Width = u * 5
                    };
                    stackPanel.Children.Add(button);

                    var rotateCubeTimelineAnimation = new RotateCubeAnimation()
                    {
                        CubeButton = button,
                        From = 0,
                        To = MathHelper.TwoPi,
                        BeginTime = TimeSpan.Zero,
                        Duration = new TimeSpan(0, 0, 4),
                        Repeat = Animations.Repeat.Forever
                    };
                    screen.Animations.Add(rotateCubeTimelineAnimation);

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

                window.Show(screen);
            }

            uiManager.Screen = screen;
        }

        protected override void UnloadContent()
        {
            debugControlLafSource.Dispose();
            spriteControlLafSource.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
