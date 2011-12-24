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
            var screen = new Screen();
            screen.Margin = new Thickness(viewportBounds.Left, viewportBounds.Top, 0, 0);
            screen.Width = viewportBounds.Width;
            screen.Height = viewportBounds.Height;

            var window = new Window();
            {
                window.Width = u * 10;
                window.Height = u * 4;
                window.Margin = new Thickness((screen.Width - window.Width) * 0.5f, (screen.Height - window.Height) * 0.5f, 0, 0);
                window.Show(screen);
                {
                    var button = new Button();
                    button.Width = u * 8;
                    button.Height = u;
                    button.Margin = new Thickness(u, u, 0, 0);
                    button.Text = "NEW GAME";
                    button.ForegroundColor = Color.White;
                    window.Children.Add(button);
                }
                {
                    var button = new Button();
                    button.Width = u * 8;
                    button.Height = u;
                    button.Margin = new Thickness(u, u * 2, 0, 0);
                    button.Text = "EXIT";
                    button.ForegroundColor = Color.White;
                    window.Children.Add(button);
                }
            }

            uiManager.Screen = screen;
        }

        void LoadSimpleWindowDemoGui()
        {
            var cubePrimitiveFactory = new Graphics.CubePrimitiveFactory();
            cubePrimitive = cubePrimitiveFactory.Create(GraphicsDevice);

            // Unit size.
            int u = 32;

            var viewportBounds = GraphicsDevice.Viewport.Bounds;
            var screen = new Screen();
            screen.Margin = new Thickness(viewportBounds.Left, viewportBounds.Top, 0, 0);
            screen.Width = viewportBounds.Width;
            screen.Height = viewportBounds.Height;

            {
                var window = new Window();
                window.Width = u * 10;
                window.Height = u * 10;
                window.Margin = new Thickness(u, u, 0, 0);
                window.BackgroundColor = Color.Red;
                window.Show(screen);
            }

            {
                var window = new Window();
                window.Width = u * 10;
                window.Height = u * 10;
                window.Margin = new Thickness(u * 3, u * 3, 0, 0);
                window.BackgroundColor = Color.Green;
                window.Show(screen);
            }

            {
                var window = new Window();
                window.Width = u * 10;
                window.Height = u * 10;
                window.Margin = new Thickness(u * 5, u * 5, 0, 0);
                window.BackgroundColor = Color.Blue;

                var stackPanel = new StackPanel();
                stackPanel.Margin = new Thickness(8, 8, 8, 8);
                stackPanel.Orientation = Orientation.Horizontal;

                {
                    var button = new Button();
                    button.Text = "Open new dialog";
                    button.ForegroundColor = Color.White;
                    button.Width = u * 5;
                    button.Height = u;
                    //button.Margin = new Thickness(u, u, 0, 0);
                    button.Clicked += delegate(object s, EventArgs e)
                    {
                        var overlay = new Overlay();
                        overlay.BackgroundColor = Color.Black * 0.5f;

                        var subWindow = new Window();
                        subWindow.Width = u * 7;
                        subWindow.Height = u * 4;
                        subWindow.Margin = new Thickness(u * 4, u * 6, 0, 0);
                        subWindow.BackgroundColor = Color.Green * 0.8f;

                        var subStackPanel = new StackPanel();
                        subStackPanel.Margin = new Thickness(8, 8, 8, 8);
                        subStackPanel.Orientation = Orientation.Vertical;
                        {
                            var subButton = new Button();
                            subButton.Text = "Open new dialog";
                            subButton.TextHorizontalAlignment = HorizontalAlignment.Left;
                            subButton.ForegroundColor = Color.White;
                            subButton.Width = u * 5;
                            subButton.Height = u;
                            subButton.HorizontalAlignment = HorizontalAlignment.Left;
                            //subButton.Margin = new Thickness(u, u, 0, 0);
                            subButton.Clicked += delegate(object bs, EventArgs be)
                            {
                                var subOverlay = new Overlay();
                                subOverlay.BackgroundColor = Color.Black * 0.5f;

                                var subSubWindow = new Window();
                                subSubWindow.Width = u * 7;
                                subSubWindow.Height = u * 3;
                                subSubWindow.Margin = new Thickness(u * 1, u * 4, 0, 0);
                                subSubWindow.BackgroundColor = Color.Brown * 0.8f;
                                {
                                    var subSubButton = new Button();
                                    subSubButton.Text = "Close";
                                    subSubButton.TextHorizontalAlignment = HorizontalAlignment.Left;
                                    subSubButton.ForegroundColor = Color.White;
                                    subSubButton.Width = u * 2;
                                    subSubButton.Height = u;
                                    subSubButton.Margin = new Thickness(u, u, 0, 0);
                                    subSubButton.Clicked += delegate(object subBs, EventArgs subBe)
                                    {
                                        subOverlay.Close();
                                    };
                                    subSubWindow.Children.Add(subSubButton);
                                }
                                subOverlay.Children.Add(subSubWindow);
                                subOverlay.Show(screen);
                            };
                            subStackPanel.Children.Add(subButton);
                            //subWindow.Children.Add(subButton);
                        }
                        {
                            var subButton = new Button();
                            subButton.Text = "Close";
                            subButton.TextHorizontalAlignment = HorizontalAlignment.Left;
                            subButton.ForegroundColor = Color.White;
                            subButton.Width = u * 2;
                            subButton.Height = u;
                            subButton.HorizontalAlignment = HorizontalAlignment.Left;
                            //subButton.Margin = new Thickness(u, u * 2, 0, 0);
                            subButton.Clicked += delegate(object bs, EventArgs be)
                            {
                                overlay.Close();
                            };
                            subStackPanel.Children.Add(subButton);
                            //subWindow.Children.Add(b);
                        }
                        subWindow.Children.Add(subStackPanel);
                        overlay.Children.Add(subWindow);
                        overlay.Show(screen);
                    };
                    stackPanel.Children.Add(button);
                    //window.Children.Add(button);
                }
                {
                    var button = new CubeButton();
                    button.CubePrimitive = cubePrimitive;
                    button.Clipped = false;
                    //button.Text = "Just Button #1";
                    button.ForegroundColor = Color.White;
                    button.Width = u * 5;
                    button.Height = u;
                    //button.Margin = new Thickness(u * 6, u, 0, 0);
                    stackPanel.Children.Add(button);
                    //window.Children.Add(button);

                    var rotateCubeTimelineAnimation = new RotateCubeAnimation();
                    rotateCubeTimelineAnimation.From = 0;
                    rotateCubeTimelineAnimation.To = MathHelper.TwoPi;
                    rotateCubeTimelineAnimation.BeginTime = TimeSpan.Zero;
                    rotateCubeTimelineAnimation.Duration = new TimeSpan(0, 0, 4);
                    rotateCubeTimelineAnimation.Repeat = Animations.Repeat.Forever;
                    button.Animations.Add(rotateCubeTimelineAnimation);
                    rotateCubeTimelineAnimation.Enabled = true;

                    button.MouseEntered += delegate(object s, EventArgs e)
                    {
                        button.Scale = 1.5f;
                    };
                    button.MouseLeft += delegate(object s, EventArgs e)
                    {
                        button.Scale = 1;
                    };
                }
                window.Children.Add(stackPanel);
                window.Show(screen);
            }

            uiManager.Screen = screen;
        }

        protected override void UnloadContent()
        {
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
