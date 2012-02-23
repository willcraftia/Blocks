#region Using

using System;
using Microsoft.Xna.Framework;
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

        public MainMenuWindow(Screen screen)
            : base(screen)
        {
            Padding = new Thickness(16);

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical
            };
            Content = stackPanel;

            var loadButton = new CustomButton(screen);
            loadButton.TextBlock.Text = "Load";
            loadButton.Click += new RoutedEventHandler(OnLoadButtonClick);
            stackPanel.Children.Add(loadButton);

            var exitButton = new CustomButton(screen);
            exitButton.TextBlock.Text = Strings.ExitButtonText;
            exitButton.Click += new RoutedEventHandler(OnExitButtonClick);
            stackPanel.Children.Add(exitButton);

            // デフォルト フォーカス。
            loadButton.Focus();
        }

        protected override void OnKeyDown(ref RoutedEventContext context)
        {
            base.OnKeyDown(ref context);

            if (Screen.KeyboardDevice.IsKeyPressed(Keys.Escape))
            {
                // Desktop をアクティブにすることで自身を非アクティブ化します。
                Screen.Root.Desktop.Activate();
                context.Handled = true;
            }
        }

        void OnLoadButtonClick(Control sender, ref RoutedEventContext context)
        {
            if (openStorageDialog == null)
            {
                var mainViewModel = (DataContext as MainViewModel);
                openStorageDialog = new OpenStorageDialog(Screen, mainViewModel.OpenStorageViewModel);
                openStorageDialog.HorizontalAlignment = HorizontalAlignment.Right;
                openStorageDialog.VerticalAlignment = VerticalAlignment.Top;
            }
            openStorageDialog.Show();
        }

        void OnExitButtonClick(Control sender, ref RoutedEventContext context)
        {
            var overlay = new Overlay(Screen)
            {
                Opacity = 0,
                BackgroundColor = Color.Black
            };
            overlay.Show();

            var opacityAnimation = new PropertyLerpAnimation
            {
                Target = overlay,
                PropertyName = "Opacity",
                From = 0,
                To = 1,
                BeginTime = TimeSpan.Zero,
                Duration = TimeSpan.FromSeconds(0.5d),
                Enabled = true
            };
            opacityAnimation.Completed += new EventHandler(OnExitAnimationCompleted);
            Animations.Add(opacityAnimation);
        }

        void OnExitAnimationCompleted(object sender, EventArgs e)
        {
            var uiService = Screen.Game.Services.GetRequiredService<IUIService>();
            uiService.Show(ScreenNames.Start);
        }
    }
}
