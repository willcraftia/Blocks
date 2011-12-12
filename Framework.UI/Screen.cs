#region Using

using System;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class Screen : Control, IInputReceiver
    {
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

        public bool HasFocus(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            ensureControlContext(control);

            return focusedControl == control;
        }

        public void Focus(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            ensureControlContext(control);

            if (!control.Enabled || !control.Visible || !control.Focusable) return;

            focusedControl = control;
        }

        public void Defocus(Control control)
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
