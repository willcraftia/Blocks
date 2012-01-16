#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    /// <summary>
    /// マウス カーソルの移動の通知に利用する delegate。
    /// </summary>
    /// <param name="x">マウス カーソルの x 座標。</param>
    /// <param name="y">マウス カーソルの y 座標。</param>
    public delegate void MouseMoveDelegate(int x, int y);

    /// <summary>
    /// マウス ボタンの押下と解放の通知に利用する delegate。
    /// </summary>
    /// <param name="buttons">押下あるいは解放されたマウスのボタン (複数ボタンあり)。</param>
    public delegate void MouseButtonDelegate(MouseButtons buttons);

    /// <summary>
    /// マウス ホイールの回転の通知に利用する delegate。
    /// </summary>
    /// <param name="ticks">マウス ホイールの回転量。</param>
    public delegate void MouseWheelDelegate(int ticks);

    /// <summary>
    /// IMouse のデフォルト実装クラスです。
    /// </summary>
    public class MouseDevice : IInputDevice
    {
        /// <summary>
        /// マウス カーソルが移動した場合に発生します。
        /// </summary>
        public event MouseMoveDelegate MouseMoved;

        /// <summary>
        /// マウス ボタンが押下された場合に発生します。
        /// </summary>
        public event MouseButtonDelegate MouseButtonPressed;

        /// <summary>
        /// マウス ボタンの押下が解放された場合に発生します。
        /// </summary>
        public event MouseButtonDelegate MouseButtonReleased;

        /// <summary>
        /// マウス ホイールが回転された場合に発生します。
        /// </summary>
        public event MouseWheelDelegate MouseWheelRotated;

        /// <summary>
        /// 前回の Update メソッドにおける MouseState。
        /// </summary>
        MouseState previousMouseState = new MouseState();

        /// <summary>
        /// Update メソッドで得る MouseState。
        /// </summary>
        MouseState currentMouseState = new MouseState();

        // I/F
        public bool Enabled
        {
            get { return true; }
        }

        // I/F
        public string Name
        {
            get { return "Main Mouse"; }
        }

        // I/F
        public void Update()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            // マウス カーソルの位置が前回から移動したかどうか
            if (previousMouseState.X != currentMouseState.X || previousMouseState.Y != currentMouseState.Y)
            {
                // イベント発生
                RaiseMouseMoved(currentMouseState.X, currentMouseState.Y);
            }

            // ボタン押下状態の判定
            // 左ボタン判定
            if (previousMouseState.LeftButton == ButtonState.Released)
            {
                // 押されたかどうか
                if (currentMouseState.LeftButton == ButtonState.Pressed) RaiseMouseButtonPressed(MouseButtons.Left);
            }
            else
            {
                // 離されたかどうか
                if (currentMouseState.LeftButton == ButtonState.Released) RaiseMouseButtonReleased(MouseButtons.Left);
            }
            // 右ボタン判定
            if (previousMouseState.RightButton == ButtonState.Released)
            {
                // 押されたかどうか
                if (currentMouseState.RightButton == ButtonState.Pressed) RaiseMouseButtonPressed(MouseButtons.Right);
            }
            else
            {
                // 離されたかどうか
                if (currentMouseState.RightButton == ButtonState.Released) RaiseMouseButtonReleased(MouseButtons.Right);
            }
            // 中央ボタン判定
            if (previousMouseState.MiddleButton == ButtonState.Released)
            {
                // 押されたかどうか
                if (currentMouseState.MiddleButton == ButtonState.Pressed) RaiseMouseButtonPressed(MouseButtons.Middle);
            }
            else
            {
                // 離されたかどうか
                if (currentMouseState.MiddleButton == ButtonState.Released) RaiseMouseButtonReleased(MouseButtons.Middle);
            }

            // マウス ホイールの回転量から前回から変化したかどうか
            if (previousMouseState.ScrollWheelValue != currentMouseState.ScrollWheelValue)
            {
                // 前回からの移動量の通知でイベント発生
                RaiseMouseWheelRotated(currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue);
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
        /// MouseMoved イベントを発生させます。
        /// </summary>
        /// <param name="x">マウス カーソルの x 座標。</param>
        /// <param name="y">マウス カーソルの y 座標。</param>
        void RaiseMouseMoved(int x, int y)
        {
            if (MouseMoved != null) MouseMoved(x, y);
        }

        /// <summary>
        /// MouseButtonPressed イベントを発生させます。
        /// </summary>
        /// <param name="button">押下されているマウス ボタン。</param>
        void RaiseMouseButtonPressed(MouseButtons button)
        {
            if (MouseButtonPressed != null) MouseButtonPressed(button);
        }

        /// <summary>
        /// MouseButtonReleased イベントを発生させます。
        /// </summary>
        /// <param name="button">押下が解放されたマウス ボタン。</param>
        void RaiseMouseButtonReleased(MouseButtons button)
        {
            if (MouseButtonReleased != null) MouseButtonReleased(button);
        }

        /// <summary>
        /// MouseWheelRotated イベントを発生させます。
        /// </summary>
        /// <param name="ticks">ホイール回転量。</param>
        void RaiseMouseWheelRotated(int ticks)
        {
            if (MouseWheelRotated != null) MouseWheelRotated(ticks);
        }
    }
}
