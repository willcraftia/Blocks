﻿#region Using

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
        bool screenLoadCompleted;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game">Game。</param>
        public DemoLoadingScreen(Game game)
            : base(game)
        {
            Content.RootDirectory = "Content";
        }

        public override void Update(GameTime gameTime)
        {
            // Screen ローディングの完了で新たな Control を追加しますが、
            // Control はスレッドセーフではない問題があります。
            // このため、非同期処理では screenLoadCompleted フラグを立て、
            // 同期処理となる Update で Control の追加を行います。
            // Control をスレッドセーフしない理由は、それによる負荷とデッドロックの問題を考慮した結果です。
            // また、非同期な Control の追加や削除の頻度は極めて少なく、
            // Screen ローディングでのみ必要とするであろうと考えています。
            lock (this)
            {
                if (screenLoadCompleted)
                {
                    var exitOverlay = new Overlay(this)
                    {
                        Opacity = 0,
                        BackgroundColor = Color.Black
                    };
                    {
                        var animation = new PropertyLerpAnimation
                        {
                            Target = exitOverlay,
                            PropertyName = "Opacity",
                            From = 0,
                            To = 1,
                            BeginTime = TimeSpan.Zero,
                            Duration = TimeSpan.FromSeconds(0.5d),
                            Enabled = true
                        };
                        animation.Completed += (exitOverlayAnimationSender, exitOverlayAnimationEvent) =>
                        {
                            // NextScreen を表示させます。
                            var uiService = Game.Services.GetRequiredService<IUIService>();
                            uiService.PrepareNextScreen(LoadedScreen);
                        };
                        Animations.Add(animation);
                    }
                    exitOverlay.Show();
                }
            }

            base.Update(gameTime);
        }

        protected override void LoadContent()
        {
            var viewportBounds = GraphicsDevice.Viewport.TitleSafeArea;
            Desktop.BackgroundColor = Color.Black;
            Desktop.Margin = new Thickness(viewportBounds.Left, viewportBounds.Top, 0, 0);
            Desktop.Width = viewportBounds.Width;
            Desktop.Height = viewportBounds.Height;

            var screenOverlay = new Overlay(this)
            {
                Opacity = 1,
                BackgroundColor = Color.Black
            };
            {
                var animation = new PropertyLerpAnimation
                {
                    Target = screenOverlay,
                    PropertyName = "Opacity",
                    From = 1,
                    To = 0,
                    BeginTime = TimeSpan.Zero,
                    Duration = TimeSpan.FromSeconds(0.5d),
                    Enabled = true
                };
                animation.Completed += (exitOverlayAnimationSender, exitOverlayAnimationEvent) => screenOverlay.Close();
                Animations.Add(animation);
            }

            var label = new Label(this)
            {
                Text = "NOW LOADING...",
                FontStretch = new Vector2(2),
                Width = 32 * 10,
                Height = 32 * 2,
                Padding = new Thickness(8)
            };
            label.Margin = new Thickness((Desktop.Width - label.Width) * 0.5f, (Desktop.Height - label.Height) * 0.5f, 0, 0);
            Desktop.Children.Add(label);

            // フラグだけを立てます。
            ScreenLoadCompleted += (s, e) =>
            {
                lock (this)
                {
                    screenLoadCompleted = true;
                }
            };

            screenOverlay.Show();

            base.LoadContent();
        }
    }
}
