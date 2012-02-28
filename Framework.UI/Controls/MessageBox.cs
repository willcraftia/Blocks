#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI.Resources;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    /// <summary>
    /// メッセージ ダイアログです。
    /// </summary>
    public class MessageBox : OverlayDialogBase
    {
        public static readonly string OKText = Strings.OK;
        public static readonly string CancelText = Strings.Cancel;
        public static readonly string YesText = Strings.Yes;
        public static readonly string NoText = Strings.No;

        /// <summary>
        /// Escape キーの押下で設定する Result プロパティを取得または設定します。
        /// デフォルトは MessageBoxResult.None です。
        /// </summary>
        public MessageBoxResult EscapeKeyDownResult { get; set; }

        /// <summary>
        /// 選択された結果を取得します。
        /// </summary>
        public MessageBoxResult Result { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        public MessageBox(Screen screen)
            : base(screen)
        {
            EscapeKeyDownResult = MessageBoxResult.None;
            Result = MessageBoxResult.None;
        }

        protected override void OnKeyDown(ref RoutedEventContext context)
        {
            base.OnKeyDown(ref context);

            if (Screen.KeyboardDevice.IsKeyPressed(Keys.Escape))
            {
                Result = EscapeKeyDownResult;

                Close();

                context.Handled = true;
            }
        }

        /// <summary>
        /// OK ボタンがクリックされると呼び出されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="context"></param>
        protected virtual void OnOKButtonClick(Control sender, ref RoutedEventContext context)
        {
            OnButtonClick(MessageBoxResult.OK);
        }

        /// <summary>
        /// Cancel ボタンがクリックされると呼び出されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="context"></param>
        protected virtual void OnCancelButtonClick(Control sender, ref RoutedEventContext context)
        {
            OnButtonClick(MessageBoxResult.Cancel);
        }

        /// <summary>
        /// Yes ボタンがクリックされると呼び出されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="context"></param>
        protected virtual void OnYesButtonClick(Control sender, ref RoutedEventContext context)
        {
            OnButtonClick(MessageBoxResult.Yes);
        }

        /// <summary>
        /// No ボタンがクリックされると呼び出されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="context"></param>
        protected virtual void OnNoButtonClick(Control sender, ref RoutedEventContext context)
        {
            OnButtonClick(MessageBoxResult.No);
        }

        protected void RegisterOKButton(Button button)
        {
            button.Click += new RoutedEventHandler(OnOKButtonClick);
        }

        protected void RegisterCancelButton(Button button)
        {
            button.Click += new RoutedEventHandler(OnCancelButtonClick);
        }

        protected void RegisterYesButton(Button button)
        {
            button.Click += new RoutedEventHandler(OnYesButtonClick);
        }

        protected void RegisterNoButton(Button button)
        {
            button.Click += new RoutedEventHandler(OnNoButtonClick);
        }

        void OnButtonClick(MessageBoxResult result)
        {
            Result = result;
            Close();
        }
    }
}
