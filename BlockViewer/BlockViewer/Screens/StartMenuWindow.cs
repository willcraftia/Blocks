#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Blocks.Storage;
using Willcraftia.Xna.Blocks.BlockViewer.Models;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;
using Willcraftia.Xna.Blocks.BlockViewer.Screens.Box;
using Willcraftia.Net.Box;
using Willcraftia.Net.Box.Service;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    /// <summary>
    /// ViewStartScreen に表示するメニュー ウィンドウです。
    /// </summary>
    public sealed class StartMenuWindow : Window
    {
        IStorageBlockService storageBlockService;

        SelectLanguageDialog selectLanguageDialog;

        Button changeLookAndFeelButton;

        ConfirmationDialog confirmInstallDialog;

        ConfirmationDialog confirmUploadDialog;

        InformationDialog installedDialog;

        InformationDialog uploadedDialog;

        BoxSetupWizardDialog boxSetupWizardDialog;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        public StartMenuWindow(Screen screen)
            : base(screen)
        {
            storageBlockService = screen.Game.Services.GetRequiredService<IStorageBlockService>();

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

        public override void Update(GameTime gameTime)
        {
            lock (showUploadedDialogSyncRoot)
            {
                if (showUploadedDialog)
                {
                    boxProgressDialog.Close();
                    ShowUploadedDialog();
                    showUploadedDialog = false;
                }
            }
            
            base.Update(gameTime);
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
            if (confirmInstallDialog == null)
            {
                confirmInstallDialog = new ConfirmationDialog(Screen)
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
                confirmInstallDialog.Closed += OnConfirmInstallDialogClosed;
            }

            confirmInstallDialog.Show();
        }

        void OnUploadDemoMeshesButtonClick(Control sender, ref RoutedEventContext context)
        {
            if (!(Screen.Game as BlockViewerGame).BoxIntegration.BoxSettingsInitialized)
            {
                if (boxSetupWizardDialog == null)
                {
                    boxSetupWizardDialog = new BoxSetupWizardDialog(Screen);
                    boxSetupWizardDialog.Closed += (s, c) => ShowConfirmUploadDialog();
                }
                boxSetupWizardDialog.Show();
            }
            else
            {
                ShowConfirmUploadDialog();
            }
        }

        void ShowConfirmUploadDialog()
        {
            if (confirmUploadDialog == null)
            {
                confirmUploadDialog = new ConfirmationDialog(Screen)
                {
                    Message = new TextBlock(Screen)
                    {
                        Width = 320,
                        Text = "Upload ?",
                        TextWrapping = TextWrapping.Wrap,
                        TextHorizontalAlignment = HorizontalAlignment.Left,
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black,
                        ShadowOffset = new Vector2(2)
                    }
                };
                confirmUploadDialog.Closed += new EventHandler(OnConfirmUploadDialogClosed);
            }

            confirmUploadDialog.Show();
        }

        // todo

        public delegate void UploadDelegate(IEnumerable<UploadFile> uploadFiles);

        UploadDelegate uploadDelegate;

        readonly object showUploadedDialogSyncRoot = new object();

        bool showUploadedDialog;

        BoxProgressDialog boxProgressDialog;

        void OnConfirmUploadDialogClosed(object sender, EventArgs e)
        {
            if (confirmUploadDialog.Result == MessageBoxResult.OK)
            {
                // todo
                var boxIntegration = (Screen.Game as BlockViewerGame).BoxIntegration;

                var memoryStream = new MemoryStream();

                string simpleBlockString;
                using (var input = TitleContainer.OpenStream("Content/DemoMeshes/SimpleBlock.blockmesh.xml"))
                {
                    input.CopyTo(memoryStream);
                    simpleBlockString = Encoding.ASCII.GetString(memoryStream.ToArray());
                }
                memoryStream.Position = 0;

                string octahedronLikeBlockString;
                using (var input = TitleContainer.OpenStream("Content/DemoMeshes/OctahedronLikeBlock.blockmesh.xml"))
                {
                    input.CopyTo(memoryStream);
                    octahedronLikeBlockString = Encoding.ASCII.GetString(memoryStream.ToArray());
                }

                memoryStream.Close();

                var uploadFiles = new List<UploadFile>();

                var simpleBlockFile = new UploadFile
                {
                    ContentType = "text/xml",
                    Name = "SimpleBlock.xml",
                    Content = simpleBlockString
                };
                uploadFiles.Add(simpleBlockFile);

                for (int i = 0; i < 20; i++)
                {
                    var destinationFileName = string.Format("Dummy_{0:d2}.xml", i);

                    var octahedronLikeBlockFile = new UploadFile
                    {
                        ContentType = "text/xml",
                        Name = destinationFileName,
                        Content = octahedronLikeBlockString
                    };
                    uploadFiles.Add(octahedronLikeBlockFile);
                }

                if (uploadDelegate == null)
                    uploadDelegate = new UploadDelegate(boxIntegration.Upload);
                uploadDelegate.BeginInvoke(uploadFiles, UploadAsyncCallback, null);

                if (boxProgressDialog == null)
                    boxProgressDialog = new BoxProgressDialog(Screen);
                boxProgressDialog.Message = "Uploading demo mesh files to your Box...";
                boxProgressDialog.Show();
            }
        }

        void UploadAsyncCallback(IAsyncResult asyncResult)
        {
            lock (showUploadedDialogSyncRoot)
            {
                uploadDelegate.EndInvoke(asyncResult);

                showUploadedDialog = true;
            }
        }

        void ShowUploadedDialog()
        {
            if (uploadedDialog == null)
            {
                uploadedDialog = new InformationDialog(Screen)
                {
                    Message = new TextBlock(Screen)
                    {
                        Text = "Uploaded.",
                        HorizontalAlignment = HorizontalAlignment.Left,
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black,
                        ShadowOffset = new Vector2(2)
                    }
                };
            }
            uploadedDialog.Show();
        }

        void OnConfirmInstallDialogClosed(object sender, EventArgs e)
        {
            if (confirmInstallDialog.Result == MessageBoxResult.OK)
            {
                InstallDemoMeshes("Content/DemoMeshes/SimpleBlock.blockmesh.xml", "SimpleBlock.xml");

                for (int i = 0; i < 20; i++)
                {
                    var destinationFileName = string.Format("Dummy_{0:d2}.xml", i);
                    InstallDemoMeshes(
                        "Content/DemoMeshes/OctahedronLikeBlock.blockmesh.xml", destinationFileName);
                }

                if (installedDialog == null)
                {
                    installedDialog = new InformationDialog(Screen)
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
                installedDialog.Show();
            }
        }

        void InstallDemoMeshes(string sourceFileName, string destinationFileName)
        {
            using (var stream = TitleContainer.OpenStream(sourceFileName))
            {
                storageBlockService.SaveBlock(destinationFileName, stream);
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
