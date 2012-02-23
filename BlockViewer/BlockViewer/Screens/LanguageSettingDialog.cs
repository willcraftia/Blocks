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
            Padding = new Thickness(16);

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical
            };
            Content = stackPanel;

            setDefaultButton = new TextButton(screen);
            setDefaultButton.TextBlock.Text = Strings.DefaultButtonText;
            setDefaultButton.Click += new RoutedEventHandler(OnButtonClick);
            stackPanel.Children.Add(setDefaultButton);

            setJaButton = new TextButton(screen);
            setJaButton.TextBlock.Text = Strings.JaButtonText;
            setJaButton.Click += new RoutedEventHandler(OnButtonClick);
            stackPanel.Children.Add(setJaButton);

            setEnButton = new TextButton(screen);
            setEnButton.TextBlock.Text = Strings.EnButtonText;
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
