#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Visuals;
using Willcraftia.Xna.Framework.UI.Visuals.Sprite;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo
{
    public class UIDemoGame : Game
    {
        GraphicsDeviceManager graphics;

        InputManager inputManager;

        UIManager uiManager;

        SpriteControlLafSource spriteControlLafSource;

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

            spriteControlLafSource = new SpriteControlLafSource("Content/UI/Sprite");
            spriteControlLafSource.SpriteSize = 32;
            uiManager.ControlLafSource = spriteControlLafSource;

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
                    button.FontColor = Color.White;
                    window.Children.Add(button);
                }
                {
                    var button = new Button();
                    button.Width = u * 8;
                    button.Height = u;
                    button.Margin = new Thickness(u, u * 2, 0, 0);
                    button.Text = "EXIT";
                    button.FontColor = Color.White;
                    window.Children.Add(button);
                }
            }

            uiManager.Screen = screen;
        }

        void LoadSimpleWindowDemoGui()
        {
            // Unit size.
            int u = 32;

            var viewportBounds = GraphicsDevice.Viewport.Bounds;
            var screen = new Screen();
            screen.Margin = new Thickness(viewportBounds.Left, viewportBounds.Top, 0, 0);
            screen.Width = viewportBounds.Width;
            screen.Height = viewportBounds.Height;

            var window_0 = new Window();
            {
                window_0.Width = u * 10;
                window_0.Height = u * 10;
                window_0.Margin = new Thickness(u, u, 0, 0);
                window_0.BackgroundColor = Color.White * 0.5f;
                window_0.Show(screen);
            }

            var window_1 = new Window();
            {
                window_1.Width = u * 10;
                window_1.Height = u * 10;
                window_1.Margin = new Thickness(u * 3, u * 3, 0, 0);
                window_1.BackgroundColor = Color.Yellow * 0.8f;
                window_1.Show(screen);
            }

            var window_2 = new Window();
            {
                window_2.Width = u * 10;
                window_2.Height = u * 10;
                window_2.Margin = new Thickness(u * 5, u * 5, 0, 0);
                window_2.Show(screen);
                {
                    var button = new Button();
                    button.Text = "Open new dialog";
                    button.FontColor = Color.White;
                    button.Width = u * 5;
                    button.Height = u;
                    button.Margin = new Thickness(u, u, 0, 0);
                    button.Clicked += delegate(object s, EventArgs e)
                    {
                        var overlay = new Overlay();
                        overlay.BackgroundColor = Color.Black * 0.5f;

                        var window = new Window();
                        window.Width = u * 7;
                        window.Height = u * 4;
                        window.Margin = new Thickness(u * 4, u * 6, 0, 0);
                        window.BackgroundColor = Color.Green * 0.8f;
                        {
                            var b = new Button();
                            b.Text = "Open new dialog";
                            b.TextHorizontalAlignment = HorizontalAlignment.Left;
                            b.FontColor = Color.White;
                            b.Width = u * 5;
                            b.Height = u;
                            b.Margin = new Thickness(u, u, 0, 0);
                            b.Clicked += delegate(object bs, EventArgs be)
                            {
                                var subOverlay = new Overlay();
                                subOverlay.BackgroundColor = Color.Black * 0.5f;

                                var subWindow = new Window();
                                subWindow.Width = u * 7;
                                subWindow.Height = u * 3;
                                subWindow.Margin = new Thickness(u * 1, u * 4, 0, 0);
                                subWindow.BackgroundColor = Color.Brown * 0.8f;
                                {
                                    var subB = new Button();
                                    subB.Text = "Close";
                                    subB.TextHorizontalAlignment = HorizontalAlignment.Left;
                                    subB.FontColor = Color.White;
                                    subB.Width = u * 2;
                                    subB.Height = u;
                                    subB.Margin = new Thickness(u, u, 0, 0);
                                    subB.Clicked += delegate(object subBs, EventArgs subBe)
                                    {
                                        subOverlay.Close();
                                    };
                                    subWindow.Children.Add(subB);
                                }
                                subOverlay.Children.Add(subWindow);
                                subOverlay.Show(screen);
                            };
                            window.Children.Add(b);
                        }
                        {
                            var b = new Button();
                            b.Text = "Close";
                            b.TextHorizontalAlignment = HorizontalAlignment.Left;
                            b.FontColor = Color.White;
                            b.Width = u * 2;
                            b.Height = u;
                            b.Margin = new Thickness(u, u * 2, 0, 0);
                            b.Clicked += delegate(object bs, EventArgs be)
                            {
                                overlay.Close();
                            };
                            window.Children.Add(b);
                        }
                        overlay.Children.Add(window);
                        overlay.Show(screen);
                    };
                    window_2.Children.Add(button);
                }
                {
                    var button = new Button();
                    button.Text = "Button #1";
                    button.FontColor = Color.White;
                    button.Width = u * 3;
                    button.Height = u;
                    button.Margin = new Thickness(u * 6, u, 0, 0);
                    window_2.Children.Add(button);
                }
            }

            //control_3.Visible = false;

            uiManager.Screen = screen;
        }

        protected override void UnloadContent()
        {
            spriteControlLafSource.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
