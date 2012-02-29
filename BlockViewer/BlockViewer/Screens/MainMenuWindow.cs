#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;
using Willcraftia.Xna.Blocks.BlockViewer.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class MainMenuWindow : Window
    {
        OpenStorageDialog openStorageDialog;

        Texture2D backgroundTexture;

        FloatLerpAnimation openAnimation;

        FloatLerpAnimation closeAnimation;

        public MainMenuWindow(Screen screen)
            : base(screen)
        {
            // 開く際に openAnimation で Width を設定するので 0 で初期化します。
            Width = 0;
            Height = 480;
            Padding = new Thickness(16);

            var stackPanel = new StackPanel(screen)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Center,
                Orientation = Orientation.Vertical
            };
            Content = stackPanel;

            var title = new TextBlock(screen)
            {
                Text = Strings.MainMenuTitle,
                Padding = new Thickness(4),
                ForegroundColor = Color.Yellow,
                BackgroundColor = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                TextHorizontalAlignment = HorizontalAlignment.Left,
                ShadowOffset = new Vector2(2)
            };
            stackPanel.Children.Add(title);

            var separator = ControlUtil.CreateDefaultSeparator(screen);
            stackPanel.Children.Add(separator);

            var loadButton = CreateMainMenuButton(screen, Strings.LoadFileButton);
            loadButton.Click += new RoutedEventHandler(OnLoadButtonClick);
            stackPanel.Children.Add(loadButton);

            var exitButton = CreateMainMenuButton(screen, Strings.ExitButton);
            exitButton.Click += new RoutedEventHandler(OnExitButtonClick);
            stackPanel.Children.Add(exitButton);

            var closeButton = CreateMainMenuButton(screen, Strings.CloseButton);
            closeButton.Click += new RoutedEventHandler(OnCloseButtonClick);
            stackPanel.Children.Add(closeButton);

            backgroundTexture = screen.Content.Load<Texture2D>("UI/MainMenuWindow");

            openAnimation = new FloatLerpAnimation
            {
                Action = (current) => { Width = current; },
                From = 0,
                To = 240,
                Duration = TimeSpan.FromSeconds(0.1f)
            };
            Animations.Add(openAnimation);

            // 閉じる場合には closeAnimation を実行し、その完了で完全に閉じます。
            closeAnimation = new FloatLerpAnimation
            {
                Action = (current) => { Width = current; },
                From = 240,
                To = 0,
                Duration = TimeSpan.FromSeconds(0.1f)
            };
            closeAnimation.Completed += (s, e) =>
            {
                base.Close();
                // Screen は最前面の Window をアクティブにするので、
                // 強制的に Desktop をアクティブにします。
                Screen.Desktop.Activate();
            };
            Animations.Add(closeAnimation);

            // デフォルト フォーカス。
            closeButton.Focus();
        }

        public override void Close()
        {
            // Close 処理はまだ呼び出さずに closeAnimation を実行します。
            closeAnimation.Enabled = true;
        }

        protected override void OnVisibleChanged()
        {
            // 表示されたら openAnimation を実行します。
            if (Visible) openAnimation.Enabled = true;

            base.OnVisibleChanged();
        }

        protected override void OnKeyDown(ref RoutedEventContext context)
        {
            base.OnKeyDown(ref context);

            if (Screen.KeyboardDevice.IsKeyPressed(Keys.Escape))
            {
                Close();

                context.Handled = true;
            }
        }

        void OnLoadButtonClick(Control sender, ref RoutedEventContext context)
        {
            if (openStorageDialog == null)
            {
                var mainViewModel = (DataContext as MainViewModel);
                openStorageDialog = new OpenStorageDialog(Screen, mainViewModel.OpenStorageViewModel);
                openStorageDialog.Closed += new EventHandler(OnOpenStorageDialogClosed);
            }
            openStorageDialog.Show();
        }

        void OnOpenStorageDialogClosed(object sender, EventArgs e)
        {
            var openStorageViewModel = openStorageDialog.DataContext as OpenStorageViewModel;
            if (string.IsNullOrEmpty(openStorageViewModel.SelectedFileName)) return;

            Close();

            // TODO: 少しでも負荷がかかると短い Animation が効果を表すことなく完了してしまう。
            (DataContext as MainViewModel).LoadBlockMeshFromStorage();
        }

        void OnExitButtonClick(Control sender, ref RoutedEventContext context)
        {
            var overlay = new Overlay(Screen);
            overlay.Show();

            var opacityAnimation = new FloatLerpAnimation
            {
                Action = (current) => { overlay.Opacity = current; },
                To = 1,
                Duration = TimeSpan.FromSeconds(0.5d),
                Enabled = true
            };
            opacityAnimation.Completed += new EventHandler(OnExitAnimationCompleted);
            Animations.Add(opacityAnimation);
        }

        void OnCloseButtonClick(Control sender, ref RoutedEventContext context)
        {
            Close();
        }

        void OnExitAnimationCompleted(object sender, EventArgs e)
        {
            var uiService = Screen.Game.Services.GetRequiredService<IUIService>();
            uiService.Show(ScreenNames.Start);
        }

        Button CreateMainMenuButton(Screen screen, String text)
        {
            float buttonHeight = 32;

            var button = new Button(screen)
            {
                Height = buttonHeight,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Padding = new Thickness(4),

                Content = new TextBlock(screen)
                {
                    Text = text,
                    ForegroundColor = Color.White,
                    BackgroundColor = Color.Black,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    TextHorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(16, 0, 0, 0),
                    ShadowOffset = new Vector2(2)
                }
            };

            ControlUtil.SetDefaultBehavior(button);

            return button;
        }
    }
}
