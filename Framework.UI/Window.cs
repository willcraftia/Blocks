#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Window を表す Control です。
    /// </summary>
    public class Window : ContentControl, IFocusScopeControl
    {
        /// <summary>
        /// Window が閉じる前に発生します。
        /// </summary>
        public event EventHandler Closing = delegate { };

        /// <summary>
        /// Window が閉じた後に発生します。
        /// </summary>
        public event EventHandler Closed = delegate { };

        /// <summary>
        /// アクティブ化された時に発生します。
        /// </summary>
        public event EventHandler Activated = delegate { };

        /// <summary>
        /// 非アクティブ化された時に発生します。
        /// </summary>
        public event EventHandler Deactivated = delegate { };

        /// <summary>
        /// この Window を所有する Window。
        /// </summary>
        Window owner;

        /// <summary>
        /// true (アクティブ化されている場合)、false (それ以外の場合)。
        /// </summary>
        bool active;

        // I/F
        public FocusScope FocusScope { get; private set; }

        /// <summary>
        /// この Window を所有する Window を取得あるいは設定します。
        /// </summary>
        /// <remarks>
        /// Window は、Owner で設定した Window が閉じられると自動的に閉じます。
        /// </remarks>
        public Window Owner
        {
            get { return owner; }
            set
            {
                if (owner == value) return;

                if (owner != null) owner.Closed -= OnOwnerClosed;

                owner = value;

                if (owner != null) owner.Closed += OnOwnerClosed;
            }
        }

        /// <summary>
        /// アクティブにできるかどうかを示す値を取得または設定します。
        /// </summary>
        public bool Activatable { get; set; }

        /// <summary>
        /// アクティブ化されているかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (アクティブ化されている場合)、false (それ以外の場合)。
        /// </value>
        public bool Active
        {
            get { return active; }
            internal set
            {
                if (active == value) return;

                active = value;

                if (active)
                {
                    OnActivated();
                }
                else
                {
                    OnDeactivated();
                }
            }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="screen">Screen。</param>
        public Window(Screen screen)
            : base(screen)
        {
            FocusScope = new FocusScope(this);
            Activatable = true;

            // Window は表示されるまで Visible = false です。
            Visible = false;
        }

        /// <summary>
        /// 指定の Control の親 Window を取得します。
        /// Window を指定した場合には、それをそのまま返します。
        /// </summary>
        /// <param name="control">Control。</param>
        /// <returns>
        /// 指定の Control を管理している親  Window。
        /// Window に配置されていない Control の場合には null。
        /// </returns>
        public static Window GetWindow(Control control)
        {
            var window = control as Window;
            if (window != null) return window;

            if (control.Parent == null) return null;

            return GetWindow(control.Parent);
        }

        /// <summary>
        /// Window を表示します。
        /// 新たに追加する Window の場合には最前面に Window が追加されます。
        /// 既に存在する Window の場合には最前面へ移動されます。
        /// Active プロパティおよび Visible プロパティが true に設定されます。
        /// </summary>
        public virtual void Show()
        {
            Screen.ShowWindow(this);
        }

        /// <summary>
        /// Window を非表示にします。
        /// Active プロパティおよび Visible プロパティが false に設定されます。
        /// </summary>
        public virtual void Hide()
        {
            Screen.HideWindow(this);
        }

        /// <summary>
        /// Window を閉じます。
        /// </summary>
        public virtual void Close()
        {
            // Closing イベントを発生させます。
            OnClosing();

            Screen.CloseWindow(this);

            // Closed イベントを発生させます。
            OnClosed();
        }

        /// <summary>
        /// アクティブにします。
        /// Activatable = false あるいは Visible = false の場合はアクティブにできません。
        /// </summary>
        public void Activate()
        {
            Screen.ActivateWindow(this);
        }

        /// <summary>
        /// Window を閉じる直前に呼び出されます。
        /// Closing イベントを発生させます。
        /// </summary>
        protected virtual void OnClosing()
        {
            if (Closing != null) Closing(this, EventArgs.Empty);
        }

        /// <summary>
        /// Window を閉じた直後に呼び出されます。
        /// Closed イベントを発生させます。
        /// </summary>
        protected virtual void OnClosed()
        {
            if (Closed != null) Closed(this, EventArgs.Empty);
        }

        /// <summary>
        /// アクティブ化された時に呼び出されます。
        /// Activated イベントを発生させます。
        /// </summary>
        protected virtual void OnActivated()
        {
            if (Activated != null) Activated(this, EventArgs.Empty);
        }

        /// <summary>
        /// 非アクティブ化された時に呼び出されます。
        /// Deactivated イベントを発生させます。
        /// </summary>
        protected virtual void OnDeactivated()
        {
            if (Deactivated != null) Deactivated(this, EventArgs.Empty);
        }

        protected override void OnKeyDown(ref RoutedEventContext context)
        {
            base.OnKeyDown(ref context);

            if (context.Handled) return;

            // フォーカス移動を処理します。
            MoveFocus();
        }

        /// <summary>
        /// 押されたキーに応じてフォーカスの移動を試みます。
        /// </summary>
        void MoveFocus()
        {
            if (Screen.FocusedControl == null) return;

            var focusScope = FocusScope.GetFocusScope(Screen.FocusedControl);
            if (focusScope == null) return;

            if (Screen.KeyboardDevice.IsKeyPressed(Keys.Up))
            {
                focusScope.MoveFocus(FocusNavigationDirection.Up);
            }
            else if (Screen.KeyboardDevice.IsKeyPressed(Keys.Down))
            {
                focusScope.MoveFocus(FocusNavigationDirection.Down);
            }
            else if (Screen.KeyboardDevice.IsKeyPressed(Keys.Left))
            {
                focusScope.MoveFocus(FocusNavigationDirection.Left);
            }
            else if (Screen.KeyboardDevice.IsKeyPressed(Keys.Right))
            {
                focusScope.MoveFocus(FocusNavigationDirection.Right);
            }
        }

        /// <summary>
        /// この Window を所有する Window が閉じられた時に呼び出され、この Window を閉じます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnOwnerClosed(object sender, EventArgs e)
        {
            Close();
        }
    }
}
