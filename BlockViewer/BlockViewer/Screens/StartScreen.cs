﻿#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Lafs;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class StartScreen : Screen
    {
        DefaultSpriteSheetSource spriteSheetSource;

        public StartScreen(Game game)
            : base(game)
        {
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            InitializeSpriteSheet();
            InitializeLookAndFeelSource();
            InitializeControls();

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            if (spriteSheetSource != null) spriteSheetSource.Dispose();

            base.UnloadContent();
        }

        void InitializeSpriteSheet()
        {
            var windowTemplate = new WindowSpriteSheetTemplate(32, 32);
            var windowShadowConverter = new DecoloringTexture2DConverter(new Color(0, 0, 0, 0.5f));
            spriteSheetSource = new DefaultSpriteSheetSource(Game);
            spriteSheetSource.Content.RootDirectory = "Content/UI/SpriteSheet";
            spriteSheetSource.DefinitionMap["Window"] = new SpriteSheetDefinition(windowTemplate, "Window");
            spriteSheetSource.DefinitionMap["WindowShadow"] = new SpriteSheetDefinition(windowTemplate, "Window", windowShadowConverter);

            spriteSheetSource.Initialize();
        }

        void InitializeLookAndFeelSource()
        {
            var source = new DefaultLookAndFeelSource(Game);

            source.LookAndFeelMap[typeof(Desktop)] = new DesktopLookAndFeel();
            source.LookAndFeelMap[typeof(Window)] = new SpriteSheetWindowLookAndFeel
            {
                SpriteSheetSource = spriteSheetSource
            };
            source.LookAndFeelMap[typeof(MainMenuWindow)] = new TextureBackgroundLookAndFeel
            {
                Texture = Content.Load<Texture2D>("UI/MainMenuWindow")
            };
            source.LookAndFeelMap[typeof(TextBlock)] = new TextBlockLookAndFeel();
            source.LookAndFeelMap[typeof(Overlay)] = new OverlayLookAndFeel();
            source.LookAndFeelMap[typeof(Button)] = new ButtonLookAndFeel
            {
                FocusTexture = Content.Load<Texture2D>("UI/Focus")
            };

            LookAndFeelSource = source;
        }

        void InitializeControls()
        {
            var startMenuWindow = new StartMenuWindow(this);
            startMenuWindow.Show();

            var startEffectOverlay = new Overlay(this)
            {
                Opacity = 1
            };
            startEffectOverlay.Show();

            var startEffectOverlay_opacityAnimation = new FloatLerpAnimation
            {
                Action = (current) => { startEffectOverlay.Opacity = current; },
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.5d),
                Enabled = true
            };
            startEffectOverlay_opacityAnimation.Completed += (s, e) =>
            {
                startEffectOverlay.Close();
                startMenuWindow.Activate();
            };
            startEffectOverlay.Animations.Add(startEffectOverlay_opacityAnimation);
        }
    }
}
