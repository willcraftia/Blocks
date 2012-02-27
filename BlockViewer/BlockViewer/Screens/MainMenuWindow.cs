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
            TitleContent = ControlUtil.CreateDefaultTitle(screen, "Main Menu");

            var stackPanel = new StackPanel(screen)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Orientation = Orientation.Vertical,
                Margin = new Thickness(16, 4, 16, 16)
            };
            Content = stackPanel;

            var loadButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.LoadFileButton);
            loadButton.Click += new RoutedEventHandler(OnLoadButtonClick);
            stackPanel.Children.Add(loadButton);

            var exitButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.ExitButton);
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
                Screen.Desktop.Activate();
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
                openStorageDialog.Closed += new EventHandler(OnOpenStorageDialogClosed);
            }
            openStorageDialog.Show();
        }

        void OnOpenStorageDialogClosed(object sender, EventArgs e)
        {
            var openStorageViewModel = openStorageDialog.DataContext as OpenStorageViewModel;
            if (string.IsNullOrEmpty(openStorageViewModel.SelectedFileName)) return;

            (DataContext as MainViewModel).LoadBlockMeshFromStorage();

            // Desktop をアクティブにします。
            Screen.Desktop.Activate();
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

        void OnExitAnimationCompleted(object sender, EventArgs e)
        {
            var uiService = Screen.Game.Services.GetRequiredService<IUIService>();
            uiService.Show(ScreenNames.Start);
        }
    }
}
