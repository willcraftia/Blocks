#region Using

using System;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    /// <summary>
    /// ViewStartScreen に表示するメニュー ウィンドウです。
    /// </summary>
    public sealed class StartMenuWindow : Window
    {
        SelectLanguageDialog selectLanguageDialog;

        Button changeLookAndFeelButton;

        ConfirmationDialog confirmationDialog;

        InformationDialog informationDialog;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        public StartMenuWindow(Screen screen)
            : base(screen)
        {
            Width = 320;
            ShadowOffset = new Vector2(4);
            Padding = new Thickness(16);

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                HorizontalAlignment = HorizontalAlignment.Stretch
            };
            Content = stackPanel;

            var startButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.StartButton);
            startButton.Click += OnStartButtonClick;
            stackPanel.Children.Add(startButton);

            var selectLanguageButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.SelectLanguageButton);
            selectLanguageButton.Click += OnLanguageSettingButtonClick;
            stackPanel.Children.Add(selectLanguageButton);

            var installDemoModelsButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.InstallDemoModelsButton);
            installDemoModelsButton.Click += OnInstallModelsButtonClick;
            stackPanel.Children.Add(installDemoModelsButton);

            changeLookAndFeelButton = ControlUtil.CreateDefaultMenuButton(screen, "Look & Feel [Debug]");
            changeLookAndFeelButton.Click += OnChangeLookAndFeelButtonClick;
            stackPanel.Children.Add(changeLookAndFeelButton);

            var exitButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.ExitButton);
            exitButton.Click += OnExitButtonClick;
            stackPanel.Children.Add(exitButton);

            // デフォルト フォーカス。
            startButton.Focus();
        }

        void OnStartButtonClick(Control sender, ref RoutedEventContext context)
        {
            var overlay = new FadeOverlay(Screen);
            overlay.OpacityAnimation.To = 1;
            overlay.OpacityAnimation.Duration = TimeSpan.FromSeconds(0.5d);
            overlay.OpacityAnimation.Completed += (s, e) => Screen.ShowScreen(ScreenNames.Main);
            overlay.Show();
        }

        void OnLanguageSettingButtonClick(Control sender, ref RoutedEventContext context)
        {
            if (selectLanguageDialog == null)
                selectLanguageDialog = new SelectLanguageDialog(Screen);

            selectLanguageDialog.Show();
        }

        void OnInstallModelsButtonClick(Control sender, ref RoutedEventContext context)
        {
            if (confirmationDialog == null)
            {
                confirmationDialog = new ConfirmationDialog(Screen)
                {
                    Message = new TextBlock(Screen)
                    {
                        Text = Strings.InstallDemoModelsConfirmation,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black,
                        ShadowOffset = new Vector2(2)
                    }
                };
                confirmationDialog.Closed += OnConfirmationDialogClosed;
            }

            confirmationDialog.Show();

            context.Handled = true;
        }

        void OnConfirmationDialogClosed(object sender, EventArgs e)
        {
            if (confirmationDialog.Result == MessageBoxResult.OK)
            {
                var showSelectorResult = StorageDevice.BeginShowSelector(null, null);
                showSelectorResult.AsyncWaitHandle.WaitOne();
                var storageDevice = StorageDevice.EndShowSelector(showSelectorResult);
                showSelectorResult.AsyncWaitHandle.Close();

                var openContainerResult = storageDevice.BeginOpenContainer("BlockViewer", null, null);
                openContainerResult.AsyncWaitHandle.WaitOne();
                var storageContainer = storageDevice.EndOpenContainer(openContainerResult);
                openContainerResult.AsyncWaitHandle.Close();

                InstallDemoModel(
                    "Content/DemoModels/OctahedronLikeBlock.xml",
                    "Model_OctahedronLikeBlock.xml",
                    storageContainer);

                for (int i = 0; i < 20; i++)
                {
                    var destinationFileName = string.Format("Model_Dummy_{0:d2}.xml", i);
                    InstallDemoModel(
                        "Content/DemoModels/SimpleBlock.xml",
                        destinationFileName,
                        storageContainer);
                }

                if (informationDialog == null)
                {
                    informationDialog = new InformationDialog(Screen)
                    {
                        Message = new TextBlock(Screen)
                        {
                            Text = Strings.DemoModelsInstalledInformation,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            ForegroundColor = Color.White,
                            BackgroundColor = Color.Black,
                            ShadowOffset = new Vector2(2)
                        }
                    };
                }
                informationDialog.Show();
            }
        }

        void InstallDemoModel(string sourceFileName, string destinationFileName, StorageContainer storageContainer)
        {
            using (var input = TitleContainer.OpenStream(sourceFileName))
            {
                using (var output = storageContainer.CreateFile(destinationFileName))
                {
                    input.CopyTo(output);
                }
            }
        }

        void OnChangeLookAndFeelButtonClick(Control sender, ref RoutedEventContext context)
        {
            var startScreen = Screen as StartScreen;
            if (startScreen.SelectedLookAndFeelSourceIndex == StartScreen.DefaultLookAndFeelIndex)
            {
                startScreen.SelectedLookAndFeelSourceIndex = StartScreen.DebugLookAndFeelIndex;
                (changeLookAndFeelButton.Content as TextBlock).Text = "Look & Feel [Default]";
            }
            else
            {
                startScreen.SelectedLookAndFeelSourceIndex = StartScreen.DefaultLookAndFeelIndex;
                (changeLookAndFeelButton.Content as TextBlock).Text = "Look & Feel [Debug]";
            }
        }

        void OnExitButtonClick(Control sender, ref RoutedEventContext context)
        {
            var overlay = new FadeOverlay(Screen);
            overlay.OpacityAnimation.To = 1;
            overlay.OpacityAnimation.Duration = TimeSpan.FromSeconds(0.5d);
            overlay.OpacityAnimation.Completed += (s, e) => Screen.Game.Exit();
            overlay.Show();
        }
    }
}
