#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo
{
    public sealed class MainMenuDemoScreen : Screen
    {
        public MainMenuDemoScreen(Game game) : base(game) { }

        protected override void LoadContent()
        {
            Content.RootDirectory = "Content";

            Font = Content.Load<SpriteFont>("Font/Default");

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

            var window = new Window()
            {
                Width = u * 10,
                Height = u * 4,
                BackgroundColor = Color.White
            };
            window.Margin = new Thickness((Desktop.Width - window.Width) * 0.5f, (Desktop.Height - window.Height) * 0.5f, 0, 0);

            {
                var stackPanel = new StackPanel()
                {
                    Margin = new Thickness(8),
                    Orientation = Orientation.Vertical
                };
                window.Children.Add(stackPanel);
                {
                    {
                        var button = new Button()
                        {
                            Text = "NEW GAME",
                            Height = u,
                            Padding = new Thickness(8)
                        };
                        stackPanel.Children.Add(button);
                    }
                    {
                        var button = new Button()
                        {
                            Text = "SWITCH SCREEN",
                            Height = u,
                            Padding = new Thickness(8)
                        };
                        stackPanel.Children.Add(button);
                        button.Clicked += delegate(object bs, EventArgs be)
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
                                    uiService.Show("WindowDemoScreen");
                                };
                                Animations.Add(animation);
                            }
                            exitOverlay.Show(this);
                        };
                    }
                    {
                        var button = new Button()
                        {
                            Text = "EXIT",
                            Height = u,
                            Padding = new Thickness(8)
                        };
                        stackPanel.Children.Add(button);
                        button.Clicked += delegate(object bs, EventArgs be)
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
                }
            }
            window.Show(this);

            screenOverlay.Show(this);

            base.LoadContent();
        }
    }
}
