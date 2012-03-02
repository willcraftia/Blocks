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
        Button changeModeButton;

        Button lodWindowButton;

        OpenStorageDialog openStorageDialog;

        LightWindow lightWindow;

        Texture2D backgroundTexture;

        FloatLerpAnimation openAnimation;

        FloatLerpAnimation closeAnimation;

        MainViewModel ViewModel
        {
            get { return DataContext as MainViewModel; }
        }

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

            var loadButton = CreateMainMenuButton(Strings.LoadFileButton);
            loadButton.Click += new RoutedEventHandler(OnLoadButtonClick);
            stackPanel.Children.Add(loadButton);

            changeModeButton = CreateMainMenuButton(Strings.LightModeButton);
            changeModeButton.Click += new RoutedEventHandler(OnChangeModeButtonClick);
            stackPanel.Children.Add(changeModeButton);

            lodWindowButton = CreateMainMenuButton(Strings.ShowLodWindowButton);
            lodWindowButton.Click += new RoutedEventHandler(OnLodWindowButtonClick);
            stackPanel.Children.Add(lodWindowButton);

            var exitButton = CreateMainMenuButton(Strings.ExitButton);
            exitButton.Click += new RoutedEventHandler(OnExitButtonClick);
            stackPanel.Children.Add(exitButton);

            var closeButton = CreateMainMenuButton(Strings.CloseButton);
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
                //Screen.Desktop.Activate();
            };
            Animations.Add(closeAnimation);

            // デフォルト フォーカス。
            closeButton.Focus();
        }

        public override void Show()
        {
            openAnimation.Enabled = true;

            base.Show();
        }

        public override void Close()
        {
            // Close 処理はまだ呼び出さずに closeAnimation を実行します。
            closeAnimation.Enabled = true;
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
                openStorageDialog = new OpenStorageDialog(Screen)
                {
                    DataContext = ViewModel.OpenStorageViewModel
                };
                openStorageDialog.Closed += new EventHandler(OnOpenStorageDialogClosed);
            }
            openStorageDialog.Show();
        }

        void OnOpenStorageDialogClosed(object sender, EventArgs e)
        {
            var openStorageViewModel = ViewModel.OpenStorageViewModel;
            if (string.IsNullOrEmpty(openStorageViewModel.SelectedFileName)) return;

            Close();

            // TODO: 少しでも負荷がかかると短い Animation が効果を表すことなく完了してしまう。
            ViewModel.LoadBlockMeshFromStorage();
        }

        void OnChangeModeButtonClick(Control sender, ref RoutedEventContext context)
        {
            if (ViewModel.BlockMeshViewModel.ViewMode == ViewMode.Camera)
            {
                if (lightWindow == null)
                {
                    lightWindow = new LightWindow(Screen)
                    {
                        DataContext = ViewModel.BlockMeshViewModel
                    };
                }
                lightWindow.Show();

                (changeModeButton.Content as TextBlock).Text = Strings.CameraModeButton;
            }
            else
            {
                if (lightWindow != null) lightWindow.Close();

                (changeModeButton.Content as TextBlock).Text = Strings.LightModeButton;
            }

            Close();
        }

        void OnLodWindowButtonClick(Control sender, ref RoutedEventContext context)
        {
            ViewModel.LodWindowVisible = !ViewModel.LodWindowVisible;

            var buttonText = (ViewModel.LodWindowVisible) ? Strings.CloseLodWindowButton : Strings.ShowLodWindowButton;
            (lodWindowButton.Content as TextBlock).Text = buttonText;

            Close();
        }

        void OnExitButtonClick(Control sender, ref RoutedEventContext context)
        {
            var overlay = new FadeOverlay(Screen);
            overlay.OpacityAnimation.To = 1;
            overlay.OpacityAnimation.Duration = TimeSpan.FromSeconds(1);
            overlay.OpacityAnimation.Completed += (s, e) => Screen.ShowScreen(ScreenNames.Start);
            overlay.Show();
        }

        void OnCloseButtonClick(Control sender, ref RoutedEventContext context)
        {
            Close();
        }

        Button CreateMainMenuButton(String text)
        {
            float buttonHeight = 32;

            var button = new Button(Screen)
            {
                Height = buttonHeight,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Padding = new Thickness(4),

                Content = new TextBlock(Screen)
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
