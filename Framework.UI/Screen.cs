#region Using

using System;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class Screen : Control, IInputReceiver
    {
        /// <summary>
        /// NotifyMouseMoved で受けたマウス カーソルの X 座標。
        /// </summary>
        int mouseX;

        /// <summary>
        /// NotifyMouseMoved で受けたマウス カーソルの Y 座標。
        /// </summary>
        int mouseY;

        /// <summary>
        /// フォーカスを得ている Control。
        /// </summary>
        Control focusedControl;

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Screen() { }

        // I/F
        public void NotifyMouseMoved(int x, int y)
        {
            this.mouseX = x;
            this.mouseY = y;
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

        /// <summary>
        /// 新たな Window が表示されたことを通知します。
        /// </summary>
        internal void NotifyWindowShown()
        {
            // NotifyMouseMoved で記録しておいたマウス カーソル位置で状態の再計算を試みます。
            // これは、新規 Window の表示によるマウス オーバ状態の変化に対応するためです。
            ProcessMouseMoved(mouseX, mouseY);
        }

        internal bool HasFocus(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            ensureControlContext(control);

            return focusedControl == control;
        }

        internal void Focus(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            ensureControlContext(control);

            if (!control.Enabled || !control.Visible || !control.Focusable) return;

            focusedControl = control;
        }

        internal void Defocus(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            ensureControlContext(control);

            if (HasFocus(control)) focusedControl = null;
        }

        void ensureControlContext(Control control)
        {
            if (control.UIContext != UIContext) throw new InvalidOperationException("Control is in another context.");
        }
    }
}
