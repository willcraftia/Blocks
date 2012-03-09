#region Using

using System;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    /// <summary>
    /// ListBox の項目として用いる Control です。
    /// ListBox には ListBoxItem 以外の Control を追加することもできますが、
    /// ListBox の Items プロパティへ ListBoxItem 以外の Control を追加しようとした場合、
    /// ListBoxItem にラップされてから追加されます。
    /// </summary>
    public class ListBoxItem : ContentControl
    {
        #region Routing Events
        
        /// <summary>
        /// 選択された時に発生します。
        /// </summary>
        public static readonly string SelectedEvent = "Selected";

        /// <summary>
        /// 選択が解除された時に発生します。
        /// </summary>
        public static readonly string UnselectedEvent = "Unselected";

        #endregion

        #region Routing Event Handlers

        /// <summary>
        /// Selected イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler Selected
        {
            add { AddHandler(SelectedEvent, value); }
            remove { RemoveHandler(SelectedEvent, value); }
        }

        /// <summary>
        /// Unselected イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler Unselected
        {
            add { AddHandler(UnselectedEvent, value); }
            remove { RemoveHandler(UnselectedEvent, value); }
        }

        #endregion

        bool isSelected;

        /// <summary>
        /// 選択されているかどうかを示す値を取得または設定します。
        /// true を設定した場合、Selected イベントが発生します。
        /// false を設定した場合、Unselected イベントが発生します。
        /// </summary>
        public bool IsSelected
        {
            get { return isSelected; }
            set
            {
                if (isSelected == value) return;

                isSelected = value;

                if (isSelected)
                {
                    RaiseEvent(null, SelectedEvent);
                }
                else
                {
                    RaiseEvent(null, UnselectedEvent);
                }
            }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="screen">Screen。</param>
        public ListBoxItem(Screen screen)
            : base(screen)
        {
            Focusable = true;
            Selected += CreateRoutedEventHandler(OnSelected);
            Unselected += CreateRoutedEventHandler(OnUnselected);
        }

        /// <summary>
        /// Selected イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnSelected(ref RoutedEventContext context) { }

        /// <summary>
        /// Unselected イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnUnselected(ref RoutedEventContext context) { }

        /// <summary>
        /// 左マウス ボタンが押された場合、IsSelected プロパティの値を反転させます。
        /// </summary>
        /// <param name="context"></param>
        protected override void OnPreviewMouseDown(ref RoutedEventContext context)
        {
            if (Screen.MouseDevice.IsButtonPressed(MouseButtons.Left))
            {
                IsSelected = !IsSelected;
            }

            base.OnPreviewMouseDown(ref context);
        }

        /// <summary>
        /// Enter/Space キーが押された場合、IsSelected プロパティの値を反転させます。
        /// </summary>
        /// <param name="context"></param>
        protected override void OnPreviewKeyDown(ref RoutedEventContext context)
        {
            if (Screen.KeyboardDevice.IsKeyPressed(Keys.Enter) ||
                Screen.KeyboardDevice.IsKeyPressed(Keys.Space))
            {
                IsSelected = !IsSelected;
            }

            base.OnPreviewKeyDown(ref context);
        }
    }
}
