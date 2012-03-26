#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;
using Willcraftia.Xna.Blocks.BlockViewer.Screens.Box;
using Willcraftia.Xna.Blocks.BlockViewer.ViewModels;

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

        ConfirmationDialog confirmInstallDialog;

        ConfirmationDialog confirmUploadDialog;

        InformationDialog installedDialog;

        InformationDialog uploadedDialog;

        BoxSetupWizardDialog boxSetupWizardDialog;

        BoxProgressDialog boxProgressDialog;

        StartMenuViewModel viewModel;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        public StartMenuWindow(Screen screen)
            : base(screen)
        {
            viewModel = new StartMenuViewModel(screen.Game);
            viewModel.UploadedDemoContents += new EventHandler(OnViewModelUploadedDemoContents);
            DataContext = viewModel;

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
            uploadDemoMeshesButton.Enabled = viewModel.BoxServiceEnabled;
            uploadDemoMeshesButton.Click += OnUploadDemoMeshesButtonClick;
            stackPanel.Children.Add(uploadDemoMeshesButton);

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
            viewModel.Update();
            
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

        void OnViewModelUploadedDemoContents(object sender, EventArgs e)
        {
            boxProgressDialog.Close();
            ShowUploadedDialog();
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
                        Text = "Are you sure you want to upload demo meshes to your Box ?",
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

        void OnConfirmUploadDialogClosed(object sender, EventArgs e)
        {
            if (confirmUploadDialog.Result == MessageBoxResult.OK)
            {
                viewModel.UploadDemoContentsAsync();

                if (boxProgressDialog == null)
                    boxProgressDialog = new BoxProgressDialog(Screen);
                boxProgressDialog.Message = "Uploading demo mesh files to your Box...";
                boxProgressDialog.Show();
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
                viewModel.InstallDemoContents();

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
