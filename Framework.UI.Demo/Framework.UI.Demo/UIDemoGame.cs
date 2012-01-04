#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI.Animations;
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

        Content.AsyncLoadManager asyncLoadManager = new Content.AsyncLoadManager();

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

            debugControlLafSource = new DebugControlLafSource(Services, "Content/UI/Debug");

            spriteControlLafSource = new SpriteControlLafSource(Services, "Content/UI/Sprite");
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

            asyncLoadManager.Execute(new LongSleepingLoader(), LongSleepingLoaderCompleteCallback);
        }

        void LongSleepingLoaderCompleteCallback()
        {
            Console.WriteLine("LongSleepingLoader completed.");
        }

        void LoadSimpleGameDemoGui()
        {
            // Unit size.
            int u = 32;

            var viewportBounds = GraphicsDevice.Viewport.TitleSafeArea;
            var screen = new Screen(GraphicsDevice);
            screen.Font = Content.Load<SpriteFont>("Font/Default");
            screen.Desktop.BackgroundColor = Color.CornflowerBlue;
            screen.Desktop.Margin = new Thickness(viewportBounds.Left, viewportBounds.Top, 0, 0);
            screen.Desktop.Width = viewportBounds.Width;
            screen.Desktop.Height = viewportBounds.Height;

            var window = new Window(screen)
            {
                Width = u * 10,
                Height = u * 4
            };
            window.Margin = new Thickness((screen.Desktop.Width - window.Width) * 0.5f, (screen.Desktop.Height - window.Height) * 0.5f, 0, 0);

            {
                {
                    var button = new Button(screen)
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
                    var button = new Button(screen)
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
            window.Show();

            uiManager.Screen = screen;
        }

        void LoadSimpleWindowDemoGui()
        {
            var cubePrimitiveFactory = new Graphics.CubePrimitiveFactory();
            cubePrimitive = cubePrimitiveFactory.Create(GraphicsDevice);

            // Unit size.
            int u = 32;

            var viewportBounds = GraphicsDevice.Viewport.TitleSafeArea;
            var screen = new Screen(GraphicsDevice);
            screen.Font = Content.Load<SpriteFont>("Font/Default");
            screen.Desktop.BackgroundColor = Color.CornflowerBlue;
            screen.Desktop.Margin = new Thickness(viewportBounds.Left, viewportBounds.Top, 0, 0);
            screen.Desktop.Width = viewportBounds.Width;
            screen.Desktop.Height = viewportBounds.Height;

            {
                var window = new Window(screen)
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
                    };
                    screen.Animations.Add(animation);
                    animation.Enabled = true;
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
                    screen.Animations.Add(animation);
                }
                window.Show();
            }

            {
                var window = new Window(screen)
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
                    screen.Animations.Add(animation);
                }
                window.Show();
            }

            {
                var window = new Window(screen)
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
                    screen.Animations.Add(animation);
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
                    screen.Animations.Add(animation);
                }

                var stackPanel = new StackPanel(screen)
                {
                    Margin = new Thickness(8),
                    Orientation = Orientation.Horizontal
                };
                window.Children.Add(stackPanel);

                {
                    var button = new Button(screen)
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
                        var overlay = new Overlay(screen)
                        {
                            Opacity = 0.5f,
                            BackgroundColor = Color.Black
                        };

                        var subWindow = new Window(screen)
                        {
                            Width = u * 7,
                            SizeToContent = Controls.SizeToContent.Height,
                            Margin = new Thickness(u * 4, u * 6, 0, 0),
                            BackgroundColor = Color.Green
                        };
                        overlay.Children.Add(subWindow);

                        var subStackPanel = new StackPanel(screen)
                        {
                            Margin = new Thickness(8),
                            Orientation = Orientation.Vertical
                        };
                        subWindow.Children.Add(subStackPanel);

                        {
                            var subButton = new Button(screen)
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
                                var subOverlay = new Overlay(screen)
                                {
                                    Opacity = 0.5f,
                                    BackgroundColor = Color.Black
                                };

                                var subSubWindow = new Window(screen)
                                {
                                    SizeToContent = Controls.SizeToContent.WidthAndHeight,
                                    Margin = new Thickness(u * 1, u * 4, 0, 0),
                                    BackgroundColor = Color.Brown
                                };
                                subOverlay.Children.Add(subSubWindow);

                                {
                                    var subSubButton = new Button(screen)
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
                            var subButton = new Button(screen)
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
                            var subButton = new Button(screen)
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
                                var exitOverlay = new Controls.Overlay(screen)
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
                    var button = new CubeButton(screen)
                    {
                        CubePrimitive = cubePrimitive,
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

                window.Show();
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
