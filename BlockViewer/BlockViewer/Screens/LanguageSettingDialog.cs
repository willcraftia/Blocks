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
        Button setDefaultButton;

        Button setJaButton;

        Button setEnButton;

        public LanguageSettingDialog(Screen screen)
            : base(screen)
        {
            TitleContent = ControlUtil.CreateDefaultTitle(screen, "Select Language");
            ShadowOffset = new Vector2(4);

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                Width = 280,
                Margin = new Thickness(16, 4, 16, 16)
            };
            Content = stackPanel;

            setDefaultButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.DefaultButton);
            setDefaultButton.Click += new RoutedEventHandler(OnButtonClick);
            stackPanel.Children.Add(setDefaultButton);

            setJaButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.JaButton);
            setJaButton.Click += new RoutedEventHandler(OnButtonClick);
            stackPanel.Children.Add(setJaButton);

            setEnButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.EnButton);
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
