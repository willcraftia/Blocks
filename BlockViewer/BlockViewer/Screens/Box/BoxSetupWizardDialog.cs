#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
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

        SaveSettingsTabItem saveSettingsTabItem;

        BoxProgressDialog boxProgressDialog;

        public BoxSetupWizardDialog(Screen screen)
            : base(screen)
        {
            viewModel = new BoxSetupViewModel(screen.Game);
            viewModel.GotTicket += OnViewModelGotTicket;
            viewModel.AccessSucceeded += OnViewModelAccessSucceeded;
            viewModel.SavedSettings += OnViewModelSavedSettings;
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

            saveSettingsTabItem = new SaveSettingsTabItem(Screen);
            saveSettingsTabItem.YesSelected += OnSaveSettingsTabItemYesSelected;
            saveSettingsTabItem.NoSelected += OnSaveSettingsTabItemNoSelected;
            tabControl.Items.Add(saveSettingsTabItem);

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
            openAnimation.Enabled = true;
            base.Show();
        }

        public override void Update(GameTime gameTime)
        {
            viewModel.Update();
            base.Update(gameTime);
        }

        void OnViewModelGotTicket(object sender, EventArgs e)
        {
            CloseProgressDialog();

            ShowAuthorizationTabItem();
        }

        void OnViewModelAccessSucceeded(object sender, EventArgs e)
        {
            CloseProgressDialog();

            ShowSaveSettingsTabItem();
        }

        void OnViewModelSavedSettings(object sender, EventArgs e)
        {
            CloseProgressDialog();
        }

        void OnAttentionTabItemCancelSelected(object sender, EventArgs e)
        {
            Close();
        }

        void OnAttentionTabItemAgreeSelected(object sender, EventArgs e)
        {
            viewModel.GetTicketAsync();

            ShowProgressDialog("Connecting to Box...");
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
            viewModel.AccessAccountAsync();

            ShowProgressDialog("Try to access to your Box account...");
        }

        void OnAccessTabItemBackSelected(object sender, EventArgs e)
        {
            ShowAuthorizationTabItem();
        }

        void OnSaveSettingsTabItemYesSelected(object sender, EventArgs e)
        {
            viewModel.SaveSettingsAsync();

            ShowProgressDialog("Saving your Box settings...");
        }

        void OnSaveSettingsTabItemNoSelected(object sender, EventArgs e)
        {
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

        void ShowSaveSettingsTabItem()
        {
            tabControl.SelectedIndex = 3;
            saveSettingsTabItem.FocusToDefault();
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
    }
}
