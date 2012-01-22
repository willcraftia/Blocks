#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    /// <summary>
    /// マウス状態の変化からイベントを発生させるクラスです。
    /// </summary>
    /// <remarks>
    /// MouseDevice はイベント処理のためのクラスです。
    /// マウス状態を参照したい場合は、XNA の Mouse クラスから MouseState を取得して参照します。
    /// </remarks>
    public class MouseDevice
    {
        /// <summary>
        /// マウス カーソルが移動した場合に発生します。
        /// </summary>
        public event MouseEventHandler MouseMove;

        /// <summary>
        /// マウス ボタンが押下された場合に発生します。
        /// </summary>
        public event MouseButtonEventHandler MouseDown;

        /// <summary>
        /// マウス ボタン押下が解放された場合に発生します。
        /// </summary>
        public event MouseButtonEventHandler MouseUp;

        /// <summary>
        /// マウス ホイールが回転された場合に発生します。
        /// </summary>
        public event MouseWheelEventHandler MouseWheel;

        /// <summary>
        /// 前回の Update メソッドにおける MouseState。
        /// </summary>
        MouseState previousMouseState = new MouseState();

        /// <summary>
        /// Update メソッドで得る MouseState。
        /// </summary>
        MouseState mouseState = new MouseState();

        /// <summary>
        /// Update メソッドで得た MouseState を取得します。
        /// </summary>
        public MouseState MouseState
        {
            get { return mouseState; }
        }

        /// <summary>
        /// デバイスの状態を更新します。
        /// </summary>
        internal void Update()
        {
            previousMouseState = mouseState;
            mouseState = Mouse.GetState();

            // マウス カーソルの位置が前回から移動したかどうか
            if (previousMouseState.X != mouseState.X || previousMouseState.Y != mouseState.Y)
                OnMouseMove();

            // 押下状態のボタン
            MouseButtons downButtons = 0;
            MouseButtons upButtons = 0;

            // ボタン押下状態の判定
            // 左ボタン判定
            if (previousMouseState.LeftButton == ButtonState.Released)
            {
                // 押されたかどうか
                if (mouseState.LeftButton == ButtonState.Pressed) downButtons |= MouseButtons.Left;
            }
            else
            {
                // 離されたかどうか
                if (mouseState.LeftButton == ButtonState.Released) upButtons |= MouseButtons.Left;
            }
            // 右ボタン判定
            if (previousMouseState.RightButton == ButtonState.Released)
            {
                // 押されたかどうか
                if (mouseState.RightButton == ButtonState.Pressed) downButtons |= MouseButtons.Right;
            }
            else
            {
                // 離されたかどうか
                if (mouseState.RightButton == ButtonState.Released) upButtons |= MouseButtons.Right;
            }
            // 中央ボタン判定
            if (previousMouseState.MiddleButton == ButtonState.Released)
            {
                // 押されたかどうか
                if (mouseState.MiddleButton == ButtonState.Pressed) downButtons |= MouseButtons.Middle;
            }
            else
            {
                // 離されたかどうか
                if (mouseState.MiddleButton == ButtonState.Released) upButtons |= MouseButtons.Middle;
            }

            if (downButtons != 0) OnMouseDown(downButtons);
            if (upButtons != 0) OnMouseUp(upButtons);

            // マウス ホイールの回転量から前回から変化したかどうか
            if (previousMouseState.ScrollWheelValue != mouseState.ScrollWheelValue)
            {
                // 前回からの移動量の通知でイベント発生
                OnMouseWheel(mouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue);
            }
        }

        /// <summary>
        /// マウス カーソルを指定の座標へ移動させます。
        /// </summary>
        /// <param name="x">移動先の x 座標。</param>
        /// <param name="y">移動先の y 座標。</param>
        public void MoveTo(int x, int y)
        {
            Mouse.SetPosition(x, y);
        }

        /// <summary>
        /// マウスが移動した時に呼び出されます。
        /// MouseMove イベントを発生させます。
        /// </summary>
        protected void OnMouseMove()
        {
            if (MouseMove != null) MouseMove(this, this);
        }

        /// <summary>
        /// マウス ボタンが押下された時に呼び出されます。
        /// MouseDown イベントを発生させます。
        /// </summary>
        /// <param name="buttons">押下されているマウス ボタン。</param>
        protected void OnMouseDown(MouseButtons buttons)
        {
            if (MouseDown != null) MouseDown(this, this, buttons);
        }

        /// <summary>
        /// マウス ボタン押下が解放された時に呼び出されます。
        /// MouseUp イベントを発生させます。
        /// </summary>
        /// <param name="buttons">押下が解放されたマウス ボタン。</param>
        protected void OnMouseUp(MouseButtons buttons)
        {
            if (MouseUp != null) MouseUp(this, this, buttons);
        }

        /// <summary>
        /// マウス ホイールが回転した時に呼び出されます。
        /// MouseWheel イベントを発生させます。
        /// </summary>
        /// <param name="delta">ホイール回転量。</param>
        protected void OnMouseWheel(int delta)
        {
            if (MouseWheel != null) MouseWheel(this, this, delta);
        }
    }
}
