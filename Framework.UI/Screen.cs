#region Using

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Screen を表す Control です。
    /// </summary>
    public class Screen : IInputReceiver
    {
        /// <summary>
        /// Desktop を表す Control です。
        /// </summary>
        public class DesktopControl : Control
        {
            internal DesktopControl() { }
        }

        /// <summary>
        /// フォーカスを得ている Control。
        /// </summary>
        Control focusedControl;

        /// <summary>
        /// GraphicsDevice。
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// デフォルトの SpriteFont を取得または設定します。
        /// </summary>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// Screen を表す Control を取得または設定します。
        /// </summary>
        public DesktopControl Desktop { get; private set; }

        /// <summary>
        /// Animation コレクションを取得します。
        /// </summary>
        public AnimationCollection Animations { get; private set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Screen(GraphicsDevice graphicsDevice)
        {
            if (graphicsDevice == null) throw new ArgumentNullException("graphicsDevice");
            GraphicsDevice = graphicsDevice;

            Animations = new AnimationCollection(this);
            Desktop = new DesktopControl()
            {
                Screen = this
            };
        }

        // I/F
        public void NotifyMouseMoved(int x, int y)
        {
            Desktop.ProcessMouseMoved(x, y);
        }

        // I/F
        public void NotifyMouseButtonPressed(MouseButtons button)
        {
            Desktop.ProcessMouseButtonPressed(button);
        }

        // I/F
        public void NotifyMouseButtonReleased(MouseButtons button)
        {
            Desktop.ProcessMouseButtonReleased(button);
        }

        // I/F
        public void NotifyMouseWheelRotated(int ticks)
        {
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
            if (control.Screen != this) throw new InvalidOperationException("Control is in another screen.");
        }
    }
}
