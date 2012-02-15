#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Animations;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class StartScreen : Screen
    {
        public Texture2D CursorTexture { get; private set; }

        public StartScreen(Game game)
            : base(game)
        {
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            CursorTexture = Content.Load<Texture2D>("UI/Sprite/Cursor");

            var startMenuWindow = new StartMenuWindow(this);
            startMenuWindow.Show();

            var startEffectOverlay = new Overlay(this)
            {
                Opacity = 1,
                BackgroundColor = Color.Black
            };
            startEffectOverlay.Show();

            var startEffectOverlay_opacityAnimation = new PropertyLerpAnimation
            {
                Target = startEffectOverlay,
                PropertyName = "Opacity",
                From = 1,
                To = 0,
                BeginTime = TimeSpan.Zero,
                Duration = TimeSpan.FromSeconds(0.5d),
                Enabled = true
            };
            startEffectOverlay_opacityAnimation.Completed += (s, e) =>
            {
                startEffectOverlay.Close();
                startMenuWindow.Activate();
            };
            startEffectOverlay.Animations.Add(startEffectOverlay_opacityAnimation);

            base.LoadContent();
        }
    }
}
