#region Using

using System;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class LanguageSettingDialog : Window
    {
        CustomButton setDefaultButton;

        CustomButton setJaButton;

        CustomButton setEnButton;

        public LanguageSettingDialog(Screen screen)
            : base(screen)
        {
            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                Padding = new Thickness(8)
            };
            Content = stackPanel;

            var cursor = (screen as StartScreen).CursorTexture;

            setDefaultButton = new CustomButton(screen);
            setDefaultButton.Cursor.Texture = cursor;
            setDefaultButton.TextBlock.Text = Strings.DefaultButtonText;
            setDefaultButton.Click += new RoutedEventHandler(OnButtonClick);
            stackPanel.Children.Add(setDefaultButton);

            setJaButton = new CustomButton(screen);
            setJaButton.Cursor.Texture = cursor;
            setJaButton.TextBlock.Text = Strings.JaButtonText;
            setJaButton.Click += new RoutedEventHandler(OnButtonClick);
            stackPanel.Children.Add(setJaButton);

            setEnButton = new CustomButton(screen);
            setEnButton.Cursor.Texture = cursor;
            setEnButton.TextBlock.Text = Strings.EnButtonText;
            setEnButton.Click += new RoutedEventHandler(OnButtonClick);
            stackPanel.Children.Add(setEnButton);

            // デフォルト フォーカス。
            setDefaultButton.Focus();
        }

        void OnButtonClick(Control sender, ref RoutedEventContext context)
        {
            var button = sender as Button;

            CultureInfo culture = null;

            if (button == setDefaultButton)
            {
                culture = CultureInfo.CurrentCulture;
            }
            else if (button == setJaButton)
            {
                culture = new CultureInfo("ja");
            }
            else if (button == setEnButton)
            {
                culture = new CultureInfo("en");
            }

            if (culture != null) Strings.Culture  = culture;

            var uiService = Screen.Game.Services.GetRequiredService<IUIService>();
            uiService.Show(ScreenNames.Start);
        }

        public override void Show()
        {
            var overlay = new Overlay(Screen)
            {
                Opacity = 0.5f,
                BackgroundColor = Color.Black
            };
            overlay.Owner = this;
            overlay.Show();

            base.Show();
        }
    }
}
