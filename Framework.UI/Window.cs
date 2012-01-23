#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Window を表す Control です。
    /// </summary>
    public class Window : Control
    {
        /// <summary>
        /// Window が閉じる前に発生します。
        /// </summary>
        public event EventHandler Closing;

        /// <summary>
        /// Window が閉じた後に発生します。
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// Window がアクティブ化された時に発生します。
        /// </summary>
        public event EventHandler Activated;

        /// <summary>
        /// Window が非アクティブ化された時に発生します。
        /// </summary>
        public event EventHandler Deactivated;

        /// <summary>
        /// この Window を所有する Window。
        /// </summary>
        Window owner;

        /// <summary>
        /// true (Window がアクティブの場合)、false (それ以外の場合)。
        /// </summary>
        bool active;

        /// <summary>
        /// 非アクティブ化される時にフォーカスを持っていた子孫 Control の弱参照。
        /// </summary>
        WeakReference focusedControlWeakReference = new WeakReference(null);

        /// <summary>
        /// サイズをコンテンツのサイズに合わせて自動調整する方法を取得または設定します。
        /// </summary>
        public SizeToContent SizeToContent { get; set; }

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

                if (owner != null) owner.Closing -= new EventHandler(OnOwnerClosing);

                owner = value;

                if (owner != null) owner.Closing += new EventHandler(OnOwnerClosing);
            }
        }

        /// <summary>
        /// Window がアクティブ可動化を示す値を取得します。
        /// </summary>
        /// <value>
        /// true (Window がアクティブの場合)、false (それ以外の場合)。
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
            FocusNavigationMode = FocusNavigationMode.Cycle;
            SizeToContent = SizeToContent.Manual;
        }

        /// <summary>
        /// 指定の Control を管理している Window (直接の親にある Window) を取得します。
        /// Window を指定した場合には、それをそのまま返します。
        /// なお、Desktop へ直接追加した Control を指定した場合は、
        /// それら Control は Window で管理されないため null を返します。
        /// </summary>
        /// <param name="control">Control。</param>
        /// <returns>
        /// 指定の Control を管理している Window、あるいは、Window で管理されていない場合は null。
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
        /// </summary>
        public virtual void Show()
        {
            Screen.Desktop.ShowWindow(this);
        }

        /// <summary>
        /// Window を閉じます。
        /// </summary>
        public void Close()
        {
            // Closing イベントを発生させます。
            OnClosing();
            
            Screen.Desktop.CloseWindow(this);

            // Closed イベントを発生させます。
            OnClosed();
        }

        /// <summary>
        /// Window をアクティブ化します。
        /// </summary>
        public void Activate()
        {
            Screen.Desktop.ActivateWindow(this);
        }

        /// <summary>
        /// Window がアクティブ化された時に呼び出されます。
        /// Activated イベントを発生させます。
        /// </summary>
        protected void OnActivated()
        {
            // 非アクティブ化の際に記録した Control にフォーカスを設定します。
            var control = focusedControlWeakReference.Target as Control;
            if (control != null)
            {
                focusedControlWeakReference.Target = null;
                control.Focus();
            }
            else
            {
                FocusFirstFocusableDesendent();
            }

            if (Activated != null) Activated(this, EventArgs.Empty);
        }

        /// <summary>
        /// Window が非アクティブ化された時に呼び出されます。
        /// Deactivated イベントを発生させます。
        /// </summary>
        protected void OnDeactivated()
        {
            // それまでフォーカスが設定されていた子孫を弱参照で記録しておきます。
            if (this == GetWindow(Screen.FocusedControl))
                focusedControlWeakReference.Target = Screen.FocusedControl;

            if (Deactivated != null) Deactivated(this, EventArgs.Empty);
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
        /// SizeToContent に Width が設定された場合の測定を行います。
        /// </summary>
        /// <param name="availableSize">親が指定する利用可能なサイズ。</param>
        /// <returns>自身が希望するサイズ。</returns>
        protected virtual Size MeasureWidthToContent(Size availableSize)
        {
            var size = new Size();
            size.Width = availableSize.Width;
            size.Height = CalculateHeight(availableSize.Height);

            // 一旦、自分が希望するサイズで子の希望サイズを定めます。
            var finalSize = new Size(0, size.Height);
            foreach (var child in Children)
            {
                // 自身の希望サイズを測定したので、子が測定済かどうかによらず再測定します。
                child.Measure(size);
                var childMeasuredSize = child.MeasuredSize;
                var childMargin = child.Margin;
                finalSize.Width += childMeasuredSize.Width + childMargin.Left + childMargin.Right;
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
            size.Width = CalculateWidth(availableSize.Width);
            size.Height = availableSize.Height;

            // 一旦、自分が希望するサイズで子の希望サイズを定めます。
            var finalSize = new Size(size.Width, 0);
            foreach (var child in Children)
            {
                // 自身の希望サイズを測定したので、子が測定済かどうかによらず再測定します。
                child.Measure(size);
                var childMeasuredSize = child.MeasuredSize;
                var childMargin = child.Margin;
                finalSize.Height += childMeasuredSize.Height + childMargin.Top + childMargin.Bottom;
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
            foreach (var child in Children)
            {
                // 自身の希望サイズを測定したので、子が測定済かどうかによらず再測定します。
                child.Measure(size);
                var childMeasuredSize = child.MeasuredSize;
                var childMargin = child.Margin;
                finalSize.Width += childMeasuredSize.Width + childMargin.Left + childMargin.Right;
                finalSize.Height += childMeasuredSize.Height + childMargin.Top + childMargin.Bottom;
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
        /// この Window を所有する Window が閉じられる前に呼び出され、この Window を閉じます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnOwnerClosing(object sender, EventArgs e)
        {
            Close();
        }
    }
}
