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

            var screen = new Screen();
            screen.Bounds = GraphicsDevice.Viewport.Bounds;

            var window = new Window();
            {
                window.Bounds = new Rectangle(0, 0, u * 10, u * 4);
                window.Bounds.X = (int) ((screen.Bounds.Width - window.Bounds.Width) * 0.5f);
                window.Bounds.Y = (int) ((screen.Bounds.Height - window.Bounds.Height) * 0.5f);
                window.Show(screen);

                {
                    var button = new Button();
                    button.Bounds = new Rectangle(0, 0, u * 8, u);
                    button.Bounds.X = (int) ((window.Bounds.Width - button.Bounds.Width) * 0.5f);
                    button.Bounds.Y = u * 1;
                    button.Text = "NEW GAME";
                    button.FontColor = Color.White;
                    window.Children.Add(button);
                }
                {
                    var button = new Button();
                    button.Bounds = new Rectangle(0, 0, u * 8, u);
                    button.Bounds.X = (int) ((window.Bounds.Width - button.Bounds.Width) * 0.5f);
                    button.Bounds.Y = u * 2;
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

            var screen = new Screen();
            screen.Bounds = GraphicsDevice.Viewport.Bounds;

            var window_0 = new Window();
            {
                window_0.Bounds = new Rectangle(u, u, u * 10, u * 10);
                window_0.BackgroundColor = Color.White * 0.5f;
                window_0.Show(screen);
            }

            var window_1 = new Window();
            {
                window_1.Bounds = new Rectangle(u * 3, u * 3, u * 10, u * 10);
                window_1.BackgroundColor = Color.Yellow * 0.8f;
                window_1.Show(screen);
            }

            var window_2 = new Window();
            {
                window_2.Bounds = new Rectangle(u * 4, u * 4, u * 10, u * 10);
                window_2.Show(screen);

                {
                    var button = new Button();
                    button.Text = "Open new dialog";
                    button.FontColor = Color.White;
                    button.Bounds = new Rectangle(u, u, u * 5, u);
                    button.Clicked += delegate(object s, EventArgs e)
                    {
                        var overlay = new Overlay();
                        overlay.BackgroundColor = Color.White * 0.0f;

                        var window = new Window();
                        window.Bounds = new Rectangle(u * 4, u * 6, u * 7, u * 4);
                        window.BackgroundColor = Color.Green * 0.8f;
                        {
                            var b = new Button();
                            b.Text = "Open new dialog";
                            b.TextHorizontalAlignment = HorizontalAlignment.Left;
                            b.FontColor = Color.White;
                            b.Bounds = new Rectangle(u, u, u * 5, u);
                            b.Clicked += delegate(object bs, EventArgs be)
                            {
                                var subOverlay = new Overlay();
                                subOverlay.BackgroundColor = Color.Black * 0.5f;

                                var subWindow = new Window();
                                subWindow.Bounds = new Rectangle(u * 1, u * 4, u * 7, u * 3);
                                subWindow.BackgroundColor = Color.Brown * 0.8f;
                                {
                                    var subB = new Button();
                                    subB.Text = "Close";
                                    subB.TextHorizontalAlignment = HorizontalAlignment.Left;
                                    subB.FontColor = Color.White;
                                    subB.Bounds = new Rectangle(u, u * 1, u * 2, u);
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
                            b.Bounds = new Rectangle(u, u * 2, u * 2, u);
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
                    button.Bounds = new Rectangle(u * 6, u, u * 3, u);
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
