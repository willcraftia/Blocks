#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo.Screens
{
    /// <summary>
    /// デモ用の LoadingScreen です。
    /// </summary>
    public sealed class DemoLoadingScreen : LoadingScreen
    {
        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game">Game。</param>
        public DemoLoadingScreen(Game game)
            : base(game)
        {
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            var viewportBounds = GraphicsDevice.Viewport.TitleSafeArea;
            Desktop.BackgroundColor = Color.Black;
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

            var label = new Label()
            {
                Text = "NOW LOADING...",
                FontStretch = new Vector2(2),
                Width = 32 * 10,
                Height = 32 * 2,
                Padding = new Thickness(8)
            };
            label.Margin = new Thickness((Desktop.Width - label.Width) * 0.5f, (Desktop.Height - label.Height) * 0.5f, 0, 0);
            Desktop.Children.Add(label);

            NextScreenLoadCompleted += delegate(object s, EventArgs e)
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
                        // NextScreen を表示させます。
                        var uiService = Game.Services.GetRequiredService<IUIService>();
                        uiService.PrepareNextScreen(NextScreen);
                    };
                    Animations.Add(animation);
                }
                exitOverlay.Show(this);
            };

            screenOverlay.Show(this);

            base.LoadContent();
        }
    }
}
