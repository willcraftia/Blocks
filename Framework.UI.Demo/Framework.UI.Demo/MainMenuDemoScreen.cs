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

            var window = new Window()
            {
                Width = u * 10,
                Height = u * 4
            };
            window.Margin = new Thickness((Desktop.Width - window.Width) * 0.5f, (Desktop.Height - window.Height) * 0.5f, 0, 0);

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
            window.Show(this);

            base.LoadContent();
        }
    }
}
