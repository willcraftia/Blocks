﻿#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Serialization;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Lafs;
using Willcraftia.Xna.Blocks.Content;
using Willcraftia.Xna.Blocks.Serialization;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;
using Willcraftia.Xna.Blocks.BlockViewer.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class MainScreen : Screen
    {
        DefaultSpriteSheetSource spriteSheetSource;

        MainViewModel mainViewModel;

        ImageButton mainMenuButton;

        BlockMeshView blockMeshView;

        MainMenuWindow mainMenuWindow;

        LodListWindow lodListWindow;

        public MainScreen(Game game)
            : base(game)
        {
            Content.RootDirectory = "Content";

            // Data Context
            mainViewModel = new MainViewModel(game.GraphicsDevice);
            DataContext = mainViewModel;
        }

        protected override void LoadContent()
        {
            InitializeSpriteSheet();
            InitializeLookAndFeelSource();
            InitializeControls();

            base.LoadContent();
        }

        void InitializeSpriteSheet()
        {
            var windowTemplate = new WindowSpriteSheetTemplate(32, 32);
            var windowShadowConverter = new DecoloringTexture2DConverter(new Color(0, 0, 0, 0.5f));
            var windowTexture = Content.Load<Texture2D>("UI/SpriteSheet/Window");
            var windowShadowTexture = windowShadowConverter.Convert(windowTexture);
            var lodWindowTexture = Content.Load<Texture2D>("UI/SpriteSheet/LodWindow");

            spriteSheetSource = new DefaultSpriteSheetSource();
            spriteSheetSource.SpriteSheetMap["Window"] = new SpriteSheet(windowTemplate, windowTexture);
            spriteSheetSource.SpriteSheetMap["WindowShadow"] = new SpriteSheet(windowTemplate, windowShadowTexture);
            spriteSheetSource.SpriteSheetMap["LodWindow"] = new SpriteSheet(windowTemplate, lodWindowTexture);
        }

        void InitializeLookAndFeelSource()
        {
            var source = new DefaultLookAndFeelSource();

            source.LookAndFeelMap[typeof(Desktop)] = new DesktopLookAndFeel();
            source.LookAndFeelMap[typeof(Window)] = new SpriteSheetWindowLookAndFeel
            {
                WindowSpriteSheet = spriteSheetSource.GetSpriteSheet("Window"),
                WindowShadowSpriteSheet = spriteSheetSource.GetSpriteSheet("WindowShadow")
            };
            source.LookAndFeelMap[typeof(MainMenuWindow)] = new TextureBackgroundLookAndFeel
            {
                Texture = Content.Load<Texture2D>("UI/MainMenuWindow")
            };
            source.LookAndFeelMap[typeof(LodListWindow)] = new SpriteSheetWindowLookAndFeel
            {
                WindowSpriteSheet = spriteSheetSource.GetSpriteSheet("LodWindow")
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
            // TODO: テスト コード。
            mainViewModel.StoreSampleBlockMesh();

            var canvas = new Canvas(this);
            canvas.HorizontalAlignment = HorizontalAlignment.Stretch;
            canvas.VerticalAlignment = VerticalAlignment.Stretch;
            Desktop.Content = canvas;

            blockMeshView = new BlockMeshView(this, new BlockMeshViewModel(mainViewModel, 0))
            {
                Width = Desktop.Width,
                Height = Desktop.Height,
                Focusable = true,
                GridVisible = true,
                CameraMovable = true
            };
            canvas.Children.Add(blockMeshView);

            mainMenuButton = new ImageButton(this)
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            mainMenuButton.Image.Texture = Content.Load<Texture2D>("UI/MainMenuButton");
            mainMenuButton.TextBlock.Text = Strings.MainMenuButton;
            mainMenuButton.TextBlock.HorizontalAlignment = HorizontalAlignment.Right;
            mainMenuButton.TextBlock.Padding = new Thickness(4);
            mainMenuButton.TextBlock.ForegroundColor = Color.Yellow;
            mainMenuButton.TextBlock.BackgroundColor = Color.Black;
            mainMenuButton.TextBlock.ShadowOffset = new Vector2(2);
            mainMenuButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                mainMenuButton.Visible = false;
                mainMenuWindow.Show();
            };
            canvas.Children.Add(mainMenuButton);

            mainMenuWindow = new MainMenuWindow(this)
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            mainMenuWindow.VisibleChanged += (s, e) =>
            {
                mainMenuButton.Visible = !mainMenuWindow.Visible;
            };
            //mainMenuWindow.Show();

            lodListWindow = new LodListWindow(this, mainViewModel)
            {
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Bottom
            };
            lodListWindow.Show();

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
                // Desktop をアクティブにしておきます。
                Desktop.Activate();
            };
            startEffectOverlay.Animations.Add(startEffectOverlay_opacityAnimation);

            Root.PreviewKeyDown += new RoutedEventHandler(OnRootPreviewKeyDown);

            // BlockMeshView にフォーカスを設定しておきます。
            blockMeshView.Focus();

            // Desktop をアクティブ化します。
            Desktop.Activate();
        }

        void OnRootPreviewKeyDown(Control sender, ref RoutedEventContext context)
        {
            if (KeyboardDevice.IsKeyPressed(Keys.Y))
            {
                mainMenuWindow.Show();
            }
        }
    }
}
