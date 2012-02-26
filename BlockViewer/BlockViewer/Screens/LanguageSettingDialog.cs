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
    public sealed class LanguageSettingDialog : OverlayDialogBase
    {
        TextButton setDefaultButton;

        TextButton setJaButton;

        TextButton setEnButton;

        public LanguageSettingDialog(Screen screen)
            : base(screen)
        {
            TitleContent = new TextBlock(screen)
            {
                Text = "Select Language",
                Margin = new Thickness(20, 4, 20, 4),
                Padding = new Thickness(4, 0, 4, 0),
                ForegroundColor = Color.Yellow,
                BackgroundColor = Color.Black,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                ShadowOffset = new Vector2(2)
            };

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(16, 4, 16, 16)
            };
            Content = stackPanel;

            setDefaultButton = new TextButton(screen);
            setDefaultButton.TextBlock.Text = Strings.DefaultButton;
            setDefaultButton.TextBlock.ForegroundColor = Color.White;
            setDefaultButton.HorizontalAlignment = HorizontalAlignment.Stretch;
            setDefaultButton.Padding = new Thickness(4);
            setDefaultButton.Click += new RoutedEventHandler(OnButtonClick);
            stackPanel.Children.Add(setDefaultButton);

            setJaButton = new TextButton(screen);
            setJaButton.TextBlock.Text = Strings.JaButton;
            setJaButton.TextBlock.ForegroundColor = Color.White;
            setJaButton.HorizontalAlignment = HorizontalAlignment.Stretch;
            setJaButton.Padding = new Thickness(4);
            setJaButton.Click += new RoutedEventHandler(OnButtonClick);
            stackPanel.Children.Add(setJaButton);

            setEnButton = new TextButton(screen);
            setEnButton.TextBlock.Text = Strings.EnButton;
            setEnButton.TextBlock.ForegroundColor = Color.White;
            setEnButton.HorizontalAlignment = HorizontalAlignment.Stretch;
            setEnButton.Padding = new Thickness(4);
            setEnButton.Click += new RoutedEventHandler(OnButtonClick);
            stackPanel.Children.Add(setEnButton);

            // デフォルト フォーカス。
            setDefaultButton.Focus();

            Overlay.Opacity = 0.5f;
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
    }
}
