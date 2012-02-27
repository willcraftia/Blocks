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
            // TODO: テスト コード。
            mainViewModel.StoreSampleBlockMesh();

            blockMeshView = new BlockMeshView(this, new BlockMeshViewModel(mainViewModel, 0));
            blockMeshView.Width = Desktop.Width;
            blockMeshView.Height = Desktop.Height;
            blockMeshView.Focusable = true;
            blockMeshView.GridVisible = true;
            blockMeshView.CameraMovable = true;
            Desktop.Content = blockMeshView;

            mainMenuWindow = new MainMenuWindow(this);
            mainMenuWindow.HorizontalAlignment = HorizontalAlignment.Right;
            mainMenuWindow.VerticalAlignment = VerticalAlignment.Top;
            mainMenuWindow.Show();

            lodListWindow = new LodListWindow(this, mainViewModel);
            lodListWindow.HorizontalAlignment = HorizontalAlignment.Left;
            lodListWindow.VerticalAlignment = VerticalAlignment.Bottom;
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

            base.LoadContent();
        }

        void OnRootPreviewKeyDown(Control sender, ref RoutedEventContext context)
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
