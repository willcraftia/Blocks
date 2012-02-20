#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class MainMenuWindow : Window
    {
        CustomButton fileButton;

        FileMenuWindow fileMenuWindow;

        public MainMenuWindow(Screen screen)
            : base(screen)
        {
            Padding = new Thickness(16);

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical
            };
            Content = stackPanel;

            fileButton = new CustomButton(screen);
            fileButton.TextBlock.Text = "File";
            fileButton.Click += new RoutedEventHandler(OnFileButtonClick);
            stackPanel.Children.Add(fileButton);

            var exitButton = new CustomButton(screen);
            exitButton.TextBlock.Text = Strings.ExitButtonText;
            exitButton.Click += new RoutedEventHandler(OnExitButtonClick);
            stackPanel.Children.Add(exitButton);

            // デフォルト フォーカス。
            exitButton.Focus();
        }

        void OnFileButtonClick(Control sender, ref RoutedEventContext context)
        {
            if (fileMenuWindow == null)
            {
                fileMenuWindow = new FileMenuWindow(Screen);
                fileMenuWindow.Owner = this;
            }
            fileMenuWindow.HorizontalAlignment = HorizontalAlignment.Left;
            fileMenuWindow.VerticalAlignment = VerticalAlignment.Top;

            var ownerPosition = PointToScreen(RenderOffset);
            var menuPosition = fileButton.PointToScreen(fileButton.RenderOffset);

            fileMenuWindow.Left = menuPosition.X;
            fileMenuWindow.Top = ownerPosition.Y + 32;
            fileMenuWindow.Show();
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
