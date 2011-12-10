﻿#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// GUI コントロールの基底クラスです。
    /// </summary>
    public class Control
    {
        /// <summary>
        /// Screen。
        /// </summary>
        Screen screen;

        /// <summary>
        /// 親 Control。
        /// </summary>
        Control parent;

        Control mouseOverControl;

        /// <summary>
        /// true (Control が有効な場合)、false (それ以外の場合)。
        /// </summary>
        bool enabled;

        /// <summary>
        /// 親 Control を取得または設定します。
        /// </summary>
        public Control Parent
        {
            get { return parent; }
            internal set
            {
                if (parent == value) return;

                parent = value;

                if (parent != null)
                {
                    // 親 Control の Screen に一致させます。
                    if (Screen != parent.Screen) Screen = parent.Screen;
                }
                else
                {
                    // 親を失ったので Screen から切り離します。
                    Screen = null;
                }
            }
        }

        /// <summary>
        /// Screen を取得または設定します。
        /// </summary>
        public Screen Screen
        {
            get { return screen; }
            internal set
            {
                if (screen == value) return;

                screen = value;

                if (screen == null)
                {
                    // フォーカスが設定されていたならば解除します。
                    if (screen.FocusedControl == this) screen.FocusedControl = null;
                }

                // 子に Screen の状態を伝播させます。
                foreach (var child in Children)
                {
                    child.Screen = screen;
                }
            }
        }

        /// <summary>
        /// 子 Control のコレクションを取得します。
        /// </summary>
        public ControlCollection Children { get; private set; }

        /// <summary>
        /// 矩形サイズ (矩形座標は親 Control の矩形座標からの相対位置)。
        /// </summary>
        public Rectangle Bounds;

        /// <summary>
        /// Control が有効かどうかを取得または設定します。
        /// </summary>
        /// <value>true (Control が有効な場合)、false (それ以外の場合)。</value>
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled == value) return;

                enabled = value;

                // フォーカスを持った状態から無効にしたならば、Screen のフォーカス状態を解除します。
                if (!enabled && Focused) Screen.FocusedControl = null;
            }
        }

        /// <summary>
        /// Control が表示されるかどうかを取得または設定します。
        /// </summary>
        /// <value>true (Control が表示される場合)、false (それ以外の場合)。</value>
        public bool Visible { get; set; }

        /// <summary>
        /// Control がフォーカスを得られるかどうかを取得または設定します。
        /// </summary>
        /// <value>true (Control がフォーカスを得られる場合)、false (それ以外の場合)。</value>
        public bool Focusable { get; set; }

        /// <summary>
        /// Control がフォーカスを得ているかどうかを取得します。
        /// </summary>
        /// <value>true (Control がフォーカスを得ている場合)、false (それ以外の場合)。</value>
        public bool Focused
        {
            get { return Screen != null && Screen.FocusedControl == this; }
        }

        public Appearance Appearance { get; set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Control()
        {
            Children = new ControlCollection(this);
        }

        /// <summary>
        /// スクリーン上の絶対座標による矩形サイズを取得します。
        /// </summary>
        /// <returns>スクリーン上の絶対座標。</returns>
        public Rectangle GetAbsoluteBounds()
        {
            if (parent == null) return Bounds;

            var parentAbsoluteBounds = parent.GetAbsoluteBounds();

            var absoluteBounds = Bounds;

            absoluteBounds.X += parentAbsoluteBounds.X;
            absoluteBounds.Y += parentAbsoluteBounds.Y;

            return absoluteBounds;
        }

        internal void ProcessMouseMoved(int x, int y)
        {
            // x と y は親を基準としたカーソルの相対座標です。

            // 自分を基準としたカーソルの相対座標を算出します。
            int localX = x - Bounds.X;
            int localY = y - Bounds.Y;

            for (int i = Children.Count - 1; 0 <= i; i--)
            {
                var child = Children[i];
                if (child.Bounds.Contains(localX, localY))
                {
                    // 子をマウス オーバ状態にします。
                    switchMouseOverControl(child);
                    // 子にカーソル移動処理を転送します。
                    child.ProcessMouseMoved(localX, localY);
                    return;
                }
            }

            // マウス オーバ状態にできる子がいないならば、自分をマウス オーバ状態にします。
            switchMouseOverControl(this);
            OnMouseMoved(localX, localY);
        }

        internal void ProcessMouseLeft()
        {
            if (mouseOverControl == null) return;

            if (mouseOverControl != this)
            {
                // マウス オーバ状態の子へ処理を転送します。
                mouseOverControl.ProcessMouseLeft();
            }
            else
            {
                // 自分がマウス オーバ状態なのでイベント ハンドラを呼びます。
                OnMouseLeft();
            }

            // マウス オーバ状態を解除します。
            mouseOverControl = null;
        }

        internal bool ProcessMouseButtonPressed(MouseButtons button)
        {
            // 子がマウス オーバ状態ならば処理を転送します。
            if (mouseOverControl != this) return mouseOverControl.ProcessMouseButtonPressed(button);

            // フォーカス設定可能ならば、Screen に現在フォーカスを得ている Control として設定します。
            if (Enabled && Focusable) Screen.FocusedControl = this;

            // マウス ボタンが押されたことを通知します。
            OnMouseButtonPressed(button);
            return true;
        }

        internal void ProcessMouseButtonReleased(MouseButtons button)
        {
            // 子がマウス オーバ状態ならば処理を転送します。
            if (mouseOverControl != this)
            {
                mouseOverControl.ProcessMouseButtonReleased(button);
            }
            else
            {
                // マウス ボタンが離されたことを通知します。
                OnMouseButtonReleased(button);
            }
        }

        /// <summary>
        /// マウス カーソルが移動した時に呼び出されます。
        /// </summary>
        /// <param name="x">この Control の矩形位置を基準としたカーソルの X 座標。</param>
        /// <param name="y">この Control の矩形位置を基準としたカーソルの Y 座標。</param>
        protected virtual void OnMouseMoved(int x, int y) { }

        /// <summary>
        /// マウス カーソルがこの Control に入った時 (この Control がマウス オーバ状態になった時) に呼び出されます。
        /// </summary>
        protected virtual void OnMouseEntered() { }

        /// <summary>
        /// マウス カーソルがこの Control から出た時 (この Control のマウス オーバ状態が解除された時) に呼び出されます。
        /// </summary>
        protected virtual void OnMouseLeft() { }

        /// <summary>
        /// マウス ボタンがこの Control で押された時に呼び出されます。
        /// </summary>
        /// <param name="button"></param>
        protected virtual void OnMouseButtonPressed(MouseButtons button) { }

        /// <summary>
        /// マウス ボタンがこの Control で離された時に呼び出されます。
        /// </summary>
        /// <param name="button"></param>
        protected virtual void OnMouseButtonReleased(MouseButtons button) { }

        /// <summary>
        /// マウス オーバ状態の Control を新しい Control へ切り替えます。
        /// </summary>
        /// <param name="newControl"></param>
        void switchMouseOverControl(Control newControl)
        {
            if (mouseOverControl == newControl) return;

            // これまでマウス オーバ状態にあった Control に変更を通知します。
            if (mouseOverControl != null) mouseOverControl.ProcessMouseLeft();

            // 新たにマウス オーバ状態となった Control を設定し、変更を通知します。
            mouseOverControl = newControl;
            mouseOverControl.OnMouseEntered();
        }
    }
}
