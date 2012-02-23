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
    public sealed class MessageBox : OverlayDialogBase
    {
        /// <summary>
        /// 配置するボタンを取得します。
        /// </summary>
        public MessageBoxButton Button { get; private set; }

        /// <summary>
        /// 表示時にフォーカスを設定するボタンを取得します。
        /// </summary>
        public MessageBoxResult DefaultResult { get; private set; }

        /// <summary>
        /// 文字列を表示する TextBlock を取得します。
        /// </summary>
        public TextBlock TextBlock { get; private set; }

        /// <summary>
        /// ボタンを配置している StackPanel を取得します。
        /// </summary>
        public StackPanel ButtonsPanel { get; private set; }

        /// <summary>
        /// OK ボタンを表す Button を取得します。
        /// OK ボタンを使用しない場合は null です。
        /// </summary>
        public Button OKButton { get; private set; }

        /// <summary>
        /// Cancel ボタンを表す Button を取得します。
        /// Cancel ボタンを使用しない場合は null です。
        /// </summary>
        public Button CancelButton { get; private set; }

        /// <summary>
        /// Yes ボタンを表す Button を取得します。
        /// Yes ボタンを使用しない場合は null です。
        /// </summary>
        public Button YesButton { get; private set; }

        /// <summary>
        /// No ボタンを表す Button を取得します。
        /// No ボタンを使用しない場合は null です。
        /// </summary>
        public Button NoButton { get; private set; }

        /// <summary>
        /// 選択された結果を取得します。
        /// </summary>
        public MessageBoxResult Result { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        /// <param name="button">配置するボタン。</param>
        /// <param name="defaultResult">フォーカスを設定するボタン。</param>
        public MessageBox(Screen screen, MessageBoxButton button, MessageBoxResult defaultResult)
            : base(screen)
        {
            Button = button;
            DefaultResult = defaultResult;
            Result = MessageBoxResult.None;

            var basePanel = new StackPanel(Screen)
            {
                Orientation = Orientation.Vertical
            };
            Content = basePanel;

            TextBlock = new TextBlock(Screen);
            basePanel.Children.Add(TextBlock);

            ButtonsPanel = new StackPanel(Screen);
            basePanel.Children.Add(ButtonsPanel);

            switch (Button)
            {
                case MessageBoxButton.OKCancel:
                    ArrangeOKButton();
                    ArrangeCancelButton();
                    break;
                case MessageBoxButton.YesNo:
                    ArrangeYesButton();
                    ArrangeNoButton();
                    break;
                case MessageBoxButton.YesNoCancel:
                    ArrangeYesButton();
                    ArrangeNoButton();
                    ArrangeCancelButton();
                    break;
                case MessageBoxButton.OK:
                default:
                    ArrangeOKButton();
                    break;
            }
        }

        public override void Show()
        {
            InitializeFocus();
            base.Show();
        }

        protected override void OnKeyDown(ref RoutedEventContext context)
        {
            base.OnKeyDown(ref context);

            if (Screen.KeyboardDevice.IsKeyPressed(Keys.Escape))
            {
                switch (Button)
                {
                    case MessageBoxButton.OK:
                        Result = MessageBoxResult.OK;
                        break;
                    case MessageBoxButton.OKCancel:
                        Result = MessageBoxResult.Cancel;
                        break;
                    case MessageBoxButton.YesNo:
                        Result = MessageBoxResult.None;
                        break;
                    case MessageBoxButton.YesNoCancel:
                        Result = MessageBoxResult.Cancel;
                        break;
                    default:
                        Result = MessageBoxResult.None;
                        break;
                }

                Close();
                context.Handled = true;
            }
        }

        /// <summary>
        /// OK ボタンがクリックされると呼び出されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="context"></param>
        void OnOKButtonClick(Control sender, ref RoutedEventContext context)
        {
            Result = MessageBoxResult.OK;
            Close();
        }

        /// <summary>
        /// Cancel ボタンがクリックされると呼び出されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="context"></param>
        void OnCancelButtonClick(Control sender, ref RoutedEventContext context)
        {
            Result = MessageBoxResult.Cancel;
            Close();
        }

        /// <summary>
        /// Yes ボタンがクリックされると呼び出されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="context"></param>
        void OnYesButtonClick(Control sender, ref RoutedEventContext context)
        {
            Result = MessageBoxResult.Yes;
            Close();
        }

        /// <summary>
        /// No ボタンがクリックされると呼び出されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="context"></param>
        void OnNoButtonClick(Control sender, ref RoutedEventContext context)
        {
            Result = MessageBoxResult.No;
            Close();
        }

        /// <summary>
        /// OK ボタンを配置します。
        /// </summary>
        void ArrangeOKButton()
        {
            OKButton = CreateButton(Strings.OK);
            OKButton.Click += new RoutedEventHandler(OnOKButtonClick);
            ButtonsPanel.Children.Add(OKButton);
        }

        /// <summary>
        /// Cancel ボタンを配置します。
        /// </summary>
        void ArrangeCancelButton()
        {
            CancelButton = CreateButton(Strings.Cancel);
            CancelButton.Click += new RoutedEventHandler(OnCancelButtonClick);
            ButtonsPanel.Children.Add(CancelButton);
        }

        /// <summary>
        /// Yes ボタンを配置します。
        /// </summary>
        void ArrangeYesButton()
        {
            YesButton = CreateButton(Strings.Yes);
            YesButton.Click += new RoutedEventHandler(OnYesButtonClick);
            ButtonsPanel.Children.Add(YesButton);
        }

        /// <summary>
        /// No ボタンを配置します。
        /// </summary>
        void ArrangeNoButton()
        {
            NoButton = CreateButton(Strings.No);
            NoButton.Click += new RoutedEventHandler(OnNoButtonClick);
            ButtonsPanel.Children.Add(NoButton);
        }

        /// <summary>
        /// DefaultResult プロパティに従ってフォーカスを設定します。
        /// </summary>
        void InitializeFocus()
        {
            switch (Button)
            {
                case MessageBoxButton.OKCancel:
                    if (DefaultResult == MessageBoxResult.OK)
                    {
                        OKButton.Focus();
                    }
                    else
                    {
                        CancelButton.Focus();
                    }
                    break;
                case MessageBoxButton.YesNo:
                    if (DefaultResult == MessageBoxResult.Yes)
                    {
                        YesButton.Focus();
                    }
                    else
                    {
                        NoButton.Focus();
                    }
                    break;
                case MessageBoxButton.YesNoCancel:
                    if (DefaultResult == MessageBoxResult.Yes)
                    {
                        YesButton.Focus();
                    }
                    else if (DefaultResult == MessageBoxResult.No)
                    {
                        NoButton.Focus();
                    }
                    else
                    {
                        CancelButton.Focus();
                    }
                    break;
                case MessageBoxButton.OK:
                default:
                    OKButton.Focus();
                    break;
            }
        }

        /// <summary>
        /// 指定の文字列を表示するボタンを生成します。
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        Button CreateButton(string text)
        {
            return new Button(Screen)
            {
                Content = new TextBlock(Screen)
                {
                    Text = text
                }
            };
        }
    }
}
