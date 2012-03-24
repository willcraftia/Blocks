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
using Willcraftia.Xna.Blocks.BlockViewer.Models;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;
using Willcraftia.Net.Box.Service;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    /// <summary>
    /// ViewStartScreen に表示するメニュー ウィンドウです。
    /// </summary>
    public sealed class StartMenuWindow : Window
    {
        StorageModel storageModel;

        SelectLanguageDialog selectLanguageDialog;

        Button changeLookAndFeelButton;

        ConfirmationDialog confirmationDialog;

        InformationDialog informationDialog;

        BoxUploadWizardDialog boxUploadWizardDialog;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        public StartMenuWindow(Screen screen)
            : base(screen)
        {
            storageModel = (screen.Game as BlockViewerGame).StorageModel;

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

            var installDemoMeshesButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.InstallDemoMeshesButton);
            installDemoMeshesButton.Click += OnInstallModelsButtonClick;
            stackPanel.Children.Add(installDemoMeshesButton);

            var uploadDemoMeshesButton = ControlUtil.CreateDefaultMenuButton(screen, "Upload Demo Meshes to Box");
            uploadDemoMeshesButton.Click += OnUploadDemoMeshesButtonClick;
            stackPanel.Children.Add(uploadDemoMeshesButton);
            if (screen.Game.Services.GetService<IBoxService>() == null)
                uploadDemoMeshesButton.Enabled = false;

            changeLookAndFeelButton = ControlUtil.CreateDefaultMenuButton(screen, "Look & Feel [Debug]");
            changeLookAndFeelButton.Click += OnChangeLookAndFeelButtonClick;
            stackPanel.Children.Add(changeLookAndFeelButton);

            var exitButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.ExitButton);
            exitButton.Click += OnExitButtonClick;
            stackPanel.Children.Add(exitButton);

            // デフォルト フォーカス。
            startButton.Focus();
        }

        void SelectStorageIfNeeded()
        {
            if (!storageModel.Selected) storageModel.SelectStorage();
        }

        void OnStartButtonClick(Control sender, ref RoutedEventContext context)
        {
            SelectStorageIfNeeded();

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
                        Width = 320,
                        Text = Strings.InstallDemoMeshesConfirmation,
                        TextWrapping = TextWrapping.Wrap,
                        TextHorizontalAlignment = HorizontalAlignment.Left,
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

        void OnUploadDemoMeshesButtonClick(Control sender, ref RoutedEventContext context)
        {
            if (boxUploadWizardDialog == null)
            {
                boxUploadWizardDialog = new BoxUploadWizardDialog(Screen);
            }
            boxUploadWizardDialog.Show();
        }

        void OnConfirmationDialogClosed(object sender, EventArgs e)
        {
            if (confirmationDialog.Result == MessageBoxResult.OK)
            {
                SelectStorageIfNeeded();

                InstallDemoModel(
                    "Content/DemoMeshes/SimpleBlock.blockmesh.xml",
                    "SimpleBlock.blockmesh.xml",
                    storageModel.Container);
                //InstallDemoModel(
                //    "Content/DemoModels/OctahedronLikeBlock.xml",
                //    "Model_OctahedronLikeBlock.xml",
                //    storageModel.Container);

                for (int i = 0; i < 20; i++)
                {
                    var destinationFileName = string.Format("Dummy_{0:d2}.blockmesh.xml", i);
                    InstallDemoModel(
                        "Content/DemoMeshes/OctahedronLikeBlock.blockmesh.xml",
                        destinationFileName,
                        storageModel.Container);
                    //InstallDemoModel(
                    //    "Content/DemoModels/SimpleBlock.xml",
                    //    destinationFileName,
                    //    storageModel.Container);
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
