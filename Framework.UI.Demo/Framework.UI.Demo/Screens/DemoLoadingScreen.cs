#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Lafs;

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

        protected override void Update(GameTime gameTime)
        {
            // LoadedScreen プロパティが null ではなければ、
            // 非同期な Screen のロードが完了しています。
            if (LoadedScreen != null)
            {
                var exitOverlay = new Overlay(this);

                var animation = new FloatLerpAnimation
                {
                    Action = (current) => { exitOverlay.Opacity = current; },
                    To = 1,
                    Duration = TimeSpan.FromSeconds(0.5d),
                    Enabled = true
                };
                animation.Completed += OnExitAnimationCompleted;
                exitOverlay.Animations.Add(animation);

                exitOverlay.Show();
            }

            base.Update(gameTime);
        }

        void OnExitAnimationCompleted(object sender, EventArgs e)
        {
            // NextScreen を表示させます。
            var uiService = Game.Services.GetRequiredService<IUIService>();
            uiService.Show(LoadedScreen);
        }

        protected override void LoadContent()
        {
            InitializeLookAndFeelSource();
            InitializeControls();

            base.LoadContent();
        }

        void InitializeLookAndFeelSource()
        {
            var source = new DefaultLookAndFeelSource();

            source.LookAndFeelMap[typeof(Desktop)] = new DesktopLookAndFeel();
            source.LookAndFeelMap[typeof(TextBlock)] = new TextBlockLookAndFeel();
            source.LookAndFeelMap[typeof(Overlay)] = new OverlayLookAndFeel();

            LookAndFeelSource = source;
        }

        void InitializeControls()
        {
            Desktop.BackgroundColor = Color.Black;

            var screenOverlay = new Overlay(this)
            {
                Opacity = 1
            };
            var screenOverlayOpacityAnimation = new FloatLerpAnimation
            {
                Action = (current) => { screenOverlay.Opacity = current; },
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5d),
                Enabled = true
            };
            screenOverlayOpacityAnimation.Completed += (s, e) => screenOverlay.Close();
            screenOverlay.Animations.Add(screenOverlayOpacityAnimation);

            var nowLoadingTextBlock = new TextBlock(this)
            {
                Text = "NOW LOADING...",
                ForegroundColor = Color.White,
                BackgroundColor = Color.Black,
                FontStretch = new Vector2(2),
                Padding = new Thickness(8)
            };
            Desktop.Content = nowLoadingTextBlock;

            screenOverlay.Show();
        }
    }
}
