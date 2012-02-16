#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Serialization;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Blocks.Content;
using Willcraftia.Xna.Blocks.Serialization;
using Willcraftia.Xna.Blocks.BlockViewer.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class MainScreen : Screen
    {
        MainViewModel mainViewModel;

        BlockMeshView blockMeshView;

        MainMenuWindow mainMenuWindow;

        LodListWindow lodListWindow;

        public Texture2D CursorTexture { get; private set; }

        public MainScreen(Game game)
            : base(game)
        {
            Content.RootDirectory = "Content";

            // Data Context
            mainViewModel = new MainViewModel();
            DataContext = mainViewModel;
        }

        protected override void LoadContent()
        {
            CursorTexture = Content.Load<Texture2D>("UI/Sprite/Cursor");

            // TODO: テスト コード。
            mainViewModel.StoreSampleBlockMesh(GraphicsDevice);

            blockMeshView = new BlockMeshView(this, new BlockMeshViewModel(mainViewModel, 0));
            blockMeshView.Width = Desktop.Width;
            blockMeshView.Height = Desktop.Height;
            blockMeshView.Focusable = true;
            Desktop.Content = blockMeshView;

            mainMenuWindow = new MainMenuWindow(this);
            mainMenuWindow.HorizontalAlignment = HorizontalAlignment.Left;
            mainMenuWindow.VerticalAlignment = VerticalAlignment.Top;
            mainMenuWindow.Show();

            lodListWindow = new LodListWindow(this, mainViewModel);
            lodListWindow.HorizontalAlignment = HorizontalAlignment.Left;
            lodListWindow.VerticalAlignment = VerticalAlignment.Bottom;
            lodListWindow.Show();

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
                //startMenuWindow.Activate();
            };
            startEffectOverlay.Animations.Add(startEffectOverlay_opacityAnimation);

            Desktop.PreviewKeyDown += new RoutedEventHandler(OnDesktopPreviewKeyDown);

            // BlockMeshView にフォーカスを設定しておきます。
            blockMeshView.Focus();

            // Desktop をアクティブ化します。
            Desktop.Activate();

            base.LoadContent();
        }

        void OnDesktopPreviewKeyDown(Control sender, ref RoutedEventContext context)
        {
            if (KeyboardDevice.IsKeyPressed(Keys.Y))
            {
                if (!mainMenuWindow.Active) mainMenuWindow.Activate();
            }
            if (KeyboardDevice.IsKeyPressed(Keys.Escape))
            {
                if (mainMenuWindow.Active) Desktop.Activate();
            }
        }
    }
}
