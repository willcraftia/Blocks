#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.Threading;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;
using Willcraftia.Xna.Blocks.BlockViewer.ViewModels.Box;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens.Box
{
    public sealed class BoxSetupWizardDialog : OverlayDialogBase
    {
        BoxSetupViewModel viewModel;

        FloatLerpAnimation openAnimation;

        TabControl tabControl;

        AttentionTabItem attentionTabItem;

        AuthorizationTabItem authorizationTabItem;

        AccessTabItem accessTabItem;

        PrepareFolderTreeTabItem prepareFolderTreeTabItem;

        SaveSettingsTabItem saveSettingsTabItem;

        FinishTabItem finishTabItem;

        BoxProgressDialog boxProgressDialog;

        ErrorDialog errorDialog;

        public bool UploadSelected { get; private set; }

        public BoxSetupWizardDialog(Screen screen)
            : base(screen)
        {
            viewModel = new BoxSetupViewModel(screen.Game);
            DataContext = viewModel;

            // 開く際に openAnimation で Width を設定するので 0 で初期化します。
            Width = 0;
            ShadowOffset = new Vector2(4);
            Padding = new Thickness(16);
            Overlay.Opacity = 0.5f;

            tabControl = new TabControl(screen)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch
            };
            Content = tabControl;

            attentionTabItem = new AttentionTabItem(Screen);
            attentionTabItem.FocusToDefault();
            attentionTabItem.AgreeSelected += OnAttentionTabItemAgreeSelected;
            attentionTabItem.CancelSelected += OnAttentionTabItemCancelSelected;
            tabControl.Items.Add(attentionTabItem);
            tabControl.SelectedIndex = 0;

            authorizationTabItem = new AuthorizationTabItem(Screen);
            authorizationTabItem.NextSelected += OnAuthorizationTabItemNextSelected;
            authorizationTabItem.BackSelected += OnAuthorizationTabItemBackSelected;
            tabControl.Items.Add(authorizationTabItem);

            accessTabItem = new AccessTabItem(Screen);
            accessTabItem.NextSelected += OnAccessTabItemNextSelected;
            accessTabItem.BackSelected += OnAccessTabItemBackSelected;
            tabControl.Items.Add(accessTabItem);

            prepareFolderTreeTabItem = new PrepareFolderTreeTabItem(Screen);
            prepareFolderTreeTabItem.CreateSelected += OnPrepareFolderTreeTabItemCreateSelected;
            prepareFolderTreeTabItem.CancelSelected += OnPrepareFolderTreeTabItemCancelSelected;
            tabControl.Items.Add(prepareFolderTreeTabItem);

            saveSettingsTabItem = new SaveSettingsTabItem(Screen);
            saveSettingsTabItem.YesSelected += OnSaveSettingsTabItemYesSelected;
            saveSettingsTabItem.NoSelected += OnSaveSettingsTabItemNoSelected;
            tabControl.Items.Add(saveSettingsTabItem);

            finishTabItem = new FinishTabItem(Screen);
            finishTabItem.UploadSelected += OnFinishTabItemUploadSelected;
            finishTabItem.CancelSelected += OnFinishTabItemCancelSelected;
            tabControl.Items.Add(finishTabItem);

            openAnimation = new FloatLerpAnimation
            {
                Action = (current) => { Width = current; },
                From = 0,
                To = 480,
                Duration = TimeSpan.FromSeconds(0.1f)
            };
            Animations.Add(openAnimation);
        }

        public override void Show()
        {
            UploadSelected = false;
            openAnimation.Enabled = true;
            base.Show();
        }

        void OnAttentionTabItemCancelSelected(object sender, EventArgs e)
        {
            Close();
        }

        void OnAttentionTabItemAgreeSelected(object sender, EventArgs e)
        {
            viewModel.GetTicketAsync(GetTicketCallback);

            ShowProgressDialog(Strings.BoxWizConnectingMessage);
        }

        void GetTicketCallback(AsyncTaskResult result)
        {
            CloseProgressDialog();

            try
            {
                result.CheckException();

                ShowAuthorizationTabItem();
            }
            catch (Exception e)
            {
                HandleWebException(e);
            }
        }

        void OnAuthorizationTabItemNextSelected(object sender, EventArgs e)
        {
            viewModel.LauchAuthorizationPageOnBrowser();

            ShowAccessBoxAccountTabItem();
        }

        void OnAuthorizationTabItemBackSelected(object sender, EventArgs e)
        {
            ShowAttentionTabItem();
        }

        void OnAccessTabItemNextSelected(object sender, EventArgs e)
        {
            viewModel.AccessAccountAsync(AccessAccountCallback);

            ShowProgressDialog(Strings.BoxWizTryAccessAccountMessage);
        }

        void AccessAccountCallback(AsyncTaskResult result)
        {
            CloseProgressDialog();

            try
            {
                result.CheckException();

                ShowCreateFolderTabItem();
            }
            catch (Exception e)
            {
                HandleWebException(e);
            }
        }

        void OnAccessTabItemBackSelected(object sender, EventArgs e)
        {
            ShowAuthorizationTabItem();
        }

        void OnPrepareFolderTreeTabItemCreateSelected(object sender, EventArgs e)
        {
            viewModel.PrepareFolderTreeAsync(PrepareFolderTreeCallback);

            ShowProgressDialog(Strings.BoxWizTryPrepareFoldersMessage);
        }

        void PrepareFolderTreeCallback(AsyncTaskResult result)
        {
            CloseProgressDialog();

            try
            {
                result.CheckException();

                ShowSaveSettingsTabItem();
            }
            catch (Exception e)
            {
                HandleWebException(e);
            }
        }

        void OnPrepareFolderTreeTabItemCancelSelected(object sender, EventArgs e)
        {
            Close();
        }

        void OnSaveSettingsTabItemYesSelected(object sender, EventArgs e)
        {
            viewModel.SaveSettingsAsync(SaveSettingsCallback);

            ShowProgressDialog(Strings.BoxWizSavingSettingsMessage);
        }

        void SaveSettingsCallback(AsyncTaskResult result)
        {
            CloseProgressDialog();

            try
            {
                result.CheckException();

                ShowFinishTabItem();
            }
            catch (Exception e)
            {
                HandleWebException(e);
            }
        }

        void OnSaveSettingsTabItemNoSelected(object sender, EventArgs e)
        {
            ShowFinishTabItem();
        }

        void OnFinishTabItemUploadSelected(object sender, EventArgs e)
        {
            UploadSelected = true;

            Close();
        }

        void OnFinishTabItemCancelSelected(object sender, EventArgs e)
        {
            Close();
        }

        void ShowAttentionTabItem()
        {
            tabControl.SelectedIndex = 0;
            attentionTabItem.FocusToDefault();
        }

        void ShowAuthorizationTabItem()
        {
            tabControl.SelectedIndex = 1;
            authorizationTabItem.FocusToDefault();
        }

        void ShowAccessBoxAccountTabItem()
        {
            tabControl.SelectedIndex = 2;
            accessTabItem.FocusToDefault();
        }

        void ShowCreateFolderTabItem()
        {
            tabControl.SelectedIndex = 3;
            prepareFolderTreeTabItem.FocusToDefault();
        }

        void ShowSaveSettingsTabItem()
        {
            tabControl.SelectedIndex = 4;
            saveSettingsTabItem.FocusToDefault();
        }

        void ShowFinishTabItem()
        {
            tabControl.SelectedIndex = 5;
            finishTabItem.FocusToDefault();
        }

        void ShowProgressDialog(string message)
        {
            if (boxProgressDialog == null)
                boxProgressDialog = new BoxProgressDialog(Screen);
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

        void HandleWebException(Exception exception)
        {
            ShowErrorDialog(Strings.BoxConnectionFailedMessage);
            Console.WriteLine(exception.Message);
        }
    }
}
