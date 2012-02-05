﻿#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Window を表す Control です。
    /// </summary>
    public class Window : ContentControl
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
        /// 論理フォーカスが設定されている Control への弱参照。
        /// </summary>
        WeakReference focusedControl = new WeakReference(null);

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

                if (owner != null) owner.Closed -= new EventHandler(OnOwnerClosed);

                owner = value;

                if (owner != null) owner.Closed += new EventHandler(OnOwnerClosed);
            }
        }

        /// <summary>
        /// Window がアクティブどうかを示す値を取得します。
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

        public FocusNavigationMode FocusNavigationMode { get; set; }

        /// <summary>
        /// 論理フォーカス。
        /// </summary>
        internal Control LogicalFocusedControl
        {
            get { return focusedControl.Target as Control; }
            set
            {
                if (focusedControl.Target != null)
                {
                    (focusedControl.Target as Control).LogicalFocused = false;
                }

                focusedControl.Target = value;

                if (focusedControl.Target != null)
                {
                    (focusedControl.Target as Control).LogicalFocused = true;
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
            FocusNavigationMode = FocusNavigationMode.Cycle;
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
            Screen.ShowWindow(this);
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
        /// Window をアクティブ化します。
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

        protected override void OnPreviewKeyDown(ref RoutedEventContext context)
        {
            base.OnPreviewKeyDown(ref context);

            // フォーカス移動のキーを優先して処理します。
            bool focusMoved = false;
            if (Screen.KeyboardDevice.IsKeyPressed(Keys.Up))
            {
                MoveFocus(FocusNavigationDirection.Up);
                focusMoved = true;
            }
            else if (Screen.KeyboardDevice.IsKeyPressed(Keys.Down))
            {
                MoveFocus(FocusNavigationDirection.Down);
                focusMoved = true;
            }
            else if (Screen.KeyboardDevice.IsKeyPressed(Keys.Left))
            {
                MoveFocus(FocusNavigationDirection.Left);
                focusMoved = true;
            }
            else if (Screen.KeyboardDevice.IsKeyPressed(Keys.Right))
            {
                MoveFocus(FocusNavigationDirection.Right);
                focusMoved = true;
            }

            if (focusMoved)
            {
                context.Handled = true;
            }
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
        /// Window がアクティブ化された時に呼び出されます。
        /// Activated イベントを発生させます。
        /// </summary>
        protected void OnActivated()
        {
            if (Activated != null) Activated(this, EventArgs.Empty);
        }

        /// <summary>
        /// Window が非アクティブ化された時に呼び出されます。
        /// Deactivated イベントを発生させます。
        /// </summary>
        protected void OnDeactivated()
        {
            if (Deactivated != null) Deactivated(this, EventArgs.Empty);
        }

        /// <summary>
        /// 指定の方向にある Control へフォーカスを移動します。
        /// </summary>
        /// <param name="direction">フォーカス移動方向。</param>
        void MoveFocus(FocusNavigationDirection direction)
        {
            var candidate = GetFocusableControl(direction);
            if (candidate == null) return;

            // フォーカスを設定します。
            Screen.MoveFocusTo(candidate);
        }

        /// <summary>
        /// 指定の方向にあるフォーカス設定可能な Control を取得します。
        /// そのような Control が存在しない場合には null を返します。
        /// </summary>
        /// <param name="direction">フォーカス移動方向。</param>
        /// <returns>
        /// 指定の方向にあるフォーカス設定可能な Control。
        /// そのような Control が存在しない場合には null。
        /// </returns>
        Control GetFocusableControl(FocusNavigationDirection direction)
        {
            if (FocusNavigationMode == FocusNavigationMode.None) return null;

            var focusedControl = Screen.FocusedControl;
            var baseBounds = new Rect(focusedControl.PointToScreen(Point.Zero), focusedControl.RenderSize);
            float minDistance = float.PositiveInfinity;
            
            var candidate = GetFocusableControl(direction, ref baseBounds, ref minDistance);
            if (candidate != null) return candidate;

            if (FocusNavigationMode == FocusNavigationMode.Wrapped) return null;

            var windowBounds = new Rect(PointToScreen(Point.Zero), RenderSize);
            switch (direction)
            {
                case FocusNavigationDirection.Up:
                    baseBounds.Y = windowBounds.Bottom;
                    baseBounds.Height = 0;
                    break;
                case FocusNavigationDirection.Down:
                    baseBounds.Y = windowBounds.Top;
                    baseBounds.Height = 0;
                    break;
                case FocusNavigationDirection.Left:
                    baseBounds.X = windowBounds.Right;
                    baseBounds.Width = 0;
                    break;
                case FocusNavigationDirection.Right:
                    baseBounds.X = windowBounds.Left;
                    baseBounds.Width = 0;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            candidate = GetFocusableControl(direction, ref baseBounds, ref minDistance);

            return candidate;
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
