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

        ErrorDialog errorDialog;

        StartMenuViewModel viewModel;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        public StartMenuWindow(Screen screen)
            : base(screen)
        {
            viewModel = new StartMenuViewModel(screen.Game);
            viewModel.RestoreBoxSessionAsyncCompleted += OnViewModelRestoreBoxSessionAsyncCompleted;
            viewModel.UploadDemoContentsAsyncCompleted += OnViewModelUploadDemoContentsAsyncCompleted;
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

            var installDemoBlocksButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.InstallDemoBlocksButton);
            installDemoBlocksButton.Click += OnInstallDemoBlocksButtonClick;
            stackPanel.Children.Add(installDemoBlocksButton);

            var uploadDemoBlocksButton = ControlUtil.CreateDefaultMenuButton(screen, Strings.UploadDemoBlocksButton);
            uploadDemoBlocksButton.Enabled = viewModel.BoxIntegrationEnabled;
            uploadDemoBlocksButton.Click += OnUploadDemoBlocksButtonClick;
            stackPanel.Children.Add(uploadDemoBlocksButton);

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

        void OnInstallDemoBlocksButtonClick(Control sender, ref RoutedEventContext context)
        {
            if (confirmInstallDialog == null)
            {
                confirmInstallDialog = new ConfirmationDialog(Screen)
                {
                    Message = new TextBlock(Screen)
                    {
                        Width = 320,
                        Text = Strings.InstallDemoBlocksConfirmation,
                        TextWrapping = TextWrapping.Wrap,
                        TextHorizontalAlignment = HorizontalAlignment.Left,
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    }
                };
                confirmInstallDialog.Closed += OnConfirmInstallDialogClosed;
            }

            confirmInstallDialog.Show();
        }

        void OnUploadDemoBlocksButtonClick(Control sender, ref RoutedEventContext context)
        {
            var boxIntegration = (Screen.Game as BlockViewerGame).BoxIntegration;
            
            // 保存されている設定からの BoxSession の復元を試みます。
            viewModel.RestoreBoxSettingsAsync();

            ShowProgressDialog(Strings.CheckingBoxSettingsMessage);
        }

        void OnViewModelRestoreBoxSessionAsyncCompleted(object sender, EventArgs e)
        {
            CloseProgressDialog();

            var result = viewModel.RestoreBoxSessionAsyncResult;
            if (result.Succeeded)
            {
                if (viewModel.BoxSessionEnabled && viewModel.HasValidFolderTree)
                {
                    // 復元できたならば、その BoxSession を用いて Upload を開始します。
                    ShowConfirmUploadDialog();
                }
                else
                {
                    // 設定が存在しない、あるいは、設定にあるフォルダ情報が無効な場合は、
                    // それらを設定するために BoxSetupWizardDialog を表示します。
                    if (boxSetupWizardDialog == null)
                    {
                        boxSetupWizardDialog = new BoxSetupWizardDialog(Screen);
                        boxSetupWizardDialog.Closed += OnBoxSetupWizardDialogClosed;
                    }
                    boxSetupWizardDialog.Show();
                }
            }
            else
            {
                ShowErrorDialog(Strings.BoxConnectionFailedMessage);
                Console.WriteLine(result.Exception.Message);
            }
        }

        void OnBoxSetupWizardDialogClosed(object sender, EventArgs e)
        {
            if (boxSetupWizardDialog.UploadSelected)
            {
                ShowConfirmUploadDialog();
            }
        }

        void OnViewModelUploadDemoContentsAsyncCompleted(object sender, EventArgs e)
        {
            CloseProgressDialog();

            var result = viewModel.UploadDemoContentsAsyncResult;
            if (result.Succeeded)
            {
                ShowUploadedDialog();
            }
            else
            {
                ShowErrorDialog(Strings.UploadDemoBlocksToBoxFailedMessage);
                Console.WriteLine(result.Exception.Message);
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
                        Text = Strings.UploadDemoBlocksConfirmation,
                        TextWrapping = TextWrapping.Wrap,
                        TextHorizontalAlignment = HorizontalAlignment.Left,
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
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

                ShowProgressDialog(Strings.UploadingDemoBlocksMessage);
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
                        Text = Strings.DemoBlocksUploadedInformation,
                        HorizontalAlignment = HorizontalAlignment.Left,
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
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
                            Text = Strings.DemoBlocksInstalledInformation,
                            HorizontalAlignment = HorizontalAlignment.Left,
                            ForegroundColor = Color.White,
                            BackgroundColor = Color.Black
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

        void ShowProgressDialog(string message)
        {
            if (boxProgressDialog == null) boxProgressDialog = new BoxProgressDialog(Screen);
            boxProgressDialog.Message = message;
            boxProgressDialog.Show();
        }

        void CloseProgressDialog()
        {
            boxProgressDialog.Close();
        }

        void ShowErrorDialog(string message)
        {
            if (errorDialog == null)
            {
                errorDialog = new ErrorDialog(Screen)
                {
                    Message = new TextBlock(Screen)
                    {
                        ForegroundColor = Color.White,
                        BackgroundColor = Color.Black
                    }
                };
            }
            (errorDialog.Message as TextBlock).Text = message;
            errorDialog.Show();
        }
    }
}
