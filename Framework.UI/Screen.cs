#region Using

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Screen を表す Control です。
    /// </summary>
    public class Screen : Control, IInputReceiver
    {
        /// <summary>
        /// IUIContext。
        /// </summary>
        IUIContext uiContext;

        /// <summary>
        /// フォーカスを得ている Control。
        /// </summary>
        Control focusedControl;

        /// <summary>
        /// UIContext を取得します。
        /// </summary>
        public IUIContext UIContext
        {
            get { return uiContext; }
            internal set
            {
                if (uiContext == value) return;

                uiContext = value;
            }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Screen()
        {
            Screen = this;
        }

        // I/F
        public void NotifyMouseMoved(int x, int y)
        {
            ProcessMouseMoved(x, y);
        }

        // I/F
        public void NotifyMouseButtonPressed(MouseButtons button)
        {
            ProcessMouseButtonPressed(button);
        }

        // I/F
        public void NotifyMouseButtonReleased(MouseButtons button)
        {
            ProcessMouseButtonReleased(button);
        }

        // I/F
        public void NotifyMouseWheelRotated(int ticks)
        {
        }

        public override void Arrange()
        {
            // Screen は自分で自分に設定されたサイズを描画時のサイズとして設定します。
            ActualWidth = Width;
            ActualHeight = Height;
            
            base.Arrange();
        }

        /// <summary>
        /// 指定の Control がフォーカスを持つかどうかを判定します。
        /// </summary>
        /// <param name="control">フォーカスを持つかどうかを判定したい Control。</param>
        /// <returns>
        /// true (指定の Control がフォーカスを持つ場合)、false (それ以外の場合)。
        /// </returns>
        internal bool HasFocus(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            EnsureControlState(control);

            return focusedControl == control;
        }

        /// <summary>
        /// 指定の Control にフォーカスを与えます。
        /// </summary>
        /// <param name="control">フォーカスを与えたい Control。</param>
        internal void Focus(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            EnsureControlState(control);

            if (!control.Enabled || !control.Visible || !control.Focusable) return;

            focusedControl = control;
        }

        /// <summary>
        /// 指定の Control のフォーカスを解除します。
        /// </summary>
        /// <param name="control">フォーカスを解除したい Control。</param>
        internal void Defocus(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            EnsureControlState(control);

            if (HasFocus(control)) focusedControl = null;
        }

        /// <summary>
        /// 指定の Control がこの Screen で操作できる状態であるかどうかを保証します。
        /// </summary>
        /// <param name="control"></param>
        void EnsureControlState(Control control)
        {
            if (control.Screen != Screen) throw new InvalidOperationException("Control is in another screen.");
        }
    }
}
