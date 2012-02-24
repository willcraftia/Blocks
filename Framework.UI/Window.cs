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
        /// サイズをコンテンツのサイズに合わせて自動調整する方法を取得または設定します。
        /// </summary>
        public SizeToContent SizeToContent { get; set; }

        public float Left
        {
            get { return Margin.Left; }
            set
            {
                var m = Margin;
                m.Left = value;
                Margin = m;
            }
        }

        public float Top
        {
            get { return Margin.Top; }
            set
            {
                var m = Margin;
                m.Top = value;
                Margin = m;
            }
        }

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

                if (owner != null) owner.Closed -= new EventHandler(OnOwnerClosed);

                owner = value;

                if (owner != null) owner.Closed += new EventHandler(OnOwnerClosed);
            }
        }

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
            SizeToContent = SizeToContent.Manual;
            FocusScope = new FocusScope(this);
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
        public void Close()
        {
            // Closing イベントを発生させます。
            OnClosing();

            Screen.CloseWindow(this);

            // Closed イベントを発生させます。
            OnClosed();
        }

        /// <summary>
        /// アクティブ化します。
        /// Active プロパティおよび Visible プロパティが true に設定されます。
        /// </summary>
        public void Activate()
        {
            Screen.ActivateWindow(this);
        }

        /// <summary>
        /// SizeToContent に Width が設定された場合の測定を行います。
        /// </summary>
        /// <param name="availableSize">親が指定する利用可能なサイズ。</param>
        /// <returns>自身が希望するサイズ。</returns>
        protected virtual Size MeasureWidthToContent(Size availableSize)
        {
            var size = new Size();
            size.Width = availableSize.Width;
            size.Height = !float.IsNaN(Height) ? Height : CalculateHeight(availableSize.Height);

            // 一旦、自分が希望するサイズで子の希望サイズを定めます。
            var finalSize = new Size(0, size.Height);
            for (int i = 0; i < ChildrenCount; i++)
            {
                var child = GetChild(i);
                // 自身の希望サイズを測定したので、子が測定済かどうかによらず再測定します。
                child.Measure(size);

                finalSize.Width += child.MeasuredSize.Width + child.Margin.Left + child.Margin.Right;
            }

            return finalSize;
        }

        /// <summary>
        /// SizeToContent に Height が設定された場合の測定を行います。
        /// </summary>
        /// <param name="availableSize">親が指定する利用可能なサイズ。</param>
        /// <returns>自身が希望するサイズ。</returns>
        protected virtual Size MeasureHeightToContent(Size availableSize)
        {
            var size = new Size();
            size.Width = !float.IsNaN(Width) ? Width : CalculateWidth(availableSize.Width);
            size.Height = availableSize.Height;

            // 一旦、自分が希望するサイズで子の希望サイズを定めます。
            var finalSize = new Size(size.Width, 0);
            for (int i = 0; i < ChildrenCount; i++)
            {
                var child = GetChild(i);
                // 自身の希望サイズを測定したので、子が測定済かどうかによらず再測定します。
                child.Measure(size);

                finalSize.Height += child.MeasuredSize.Height + child.Margin.Top + child.Margin.Bottom;
            }

            return finalSize;
        }

        /// <summary>
        /// SizeToContent に WidthAndHeight が設定された場合の測定を行います。
        /// </summary>
        /// <param name="availableSize">親が指定する利用可能なサイズ。</param>
        /// <returns>自身が希望するサイズ。</returns>
        protected virtual Size MeasureWidthAndHeightToContent(Size availableSize)
        {
            var size = availableSize;

            // 一旦、自分が希望するサイズで子の希望サイズを定めます。
            var finalSize = new Size(0, 0);
            for (int i = 0; i < ChildrenCount; i++)
            {
                var child = GetChild(i);
                // 自身の希望サイズを測定したので、子が測定済かどうかによらず再測定します。
                child.Measure(size);

                finalSize.Width += child.MeasuredSize.Width + child.Margin.Left + child.Margin.Right;
                finalSize.Height += child.MeasuredSize.Height + child.Margin.Top + child.Margin.Bottom;
            }

            return finalSize;
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

        protected override Size MeasureOverride(Size availableSize)
        {
            switch (SizeToContent)
            {
                case SizeToContent.Width:
                    return MeasureWidthToContent(availableSize);
                case SizeToContent.Height:
                    return MeasureHeightToContent(availableSize);
                case SizeToContent.WidthAndHeight:
                    return MeasureWidthAndHeightToContent(availableSize);
                case SizeToContent.Manual:
                default:
                    return base.MeasureOverride(availableSize);
            }
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
