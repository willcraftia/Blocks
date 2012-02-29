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
using Willcraftia.Xna.Blocks.BlockViewer.Resources;
using Willcraftia.Xna.Blocks.BlockViewer.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class MainScreen : Screen
    {
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

            base.LoadContent();
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
