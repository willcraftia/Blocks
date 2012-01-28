#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    /// <summary>
    /// マウスの状態を管理するクラスです。
    /// </summary>
    public class MouseDevice
    {
        /// <summary>
        /// 前回の Update メソッドで得られた MouseState。
        /// </summary>
        MouseState previousMouseState = new MouseState();

        /// <summary>
        /// Update メソッドで得られた MouseState。
        /// </summary>
        MouseState mouseState = new MouseState();

        /// <summary>
        /// Update メソッドで得られた MouseState を取得します。
        /// </summary>
        public MouseState MouseState
        {
            get { return mouseState; }
        }

        /// <summary>
        /// 前回の Update メソッドで得られた MouseState を取得します。
        /// </summary>
        public MouseState PreviousMouseState
        {
            get { return previousMouseState; }
        }

        /// <summary>
        /// カーソルが移動したかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (カーソルが移動した場合)、false (それ以外の場合)。
        /// </value>
        public bool MouseMoved { get; private set; }

        /// <summary>
        /// 押されたボタンを取得します。
        /// </summary>
        public MouseButtons PressedButtons { get; private set; }

        /// <summary>
        /// ボタンが押されたかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (ボタンが押された場合)、false (それ以外の場合)。
        /// </value>
        public bool ButtonPressed
        {
            get { return PressedButtons != 0; }
        }

        /// <summary>
        /// 離されたボタンを取得します。
        /// </summary>
        public MouseButtons ReleasedButtons { get; private set; }

        /// <summary>
        /// ボタンが離されたかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (ボタンが離された場合)、false (それ以外の場合)。
        /// </value>
        public bool ButtonReleased
        {
            get { return ReleasedButtons != 0; }
        }

        /// <summary>
        /// ホイールの回転量を取得します。
        /// </summary>
        public int WheelDelta { get; private set; }

        /// <summary>
        /// ホイールが回転したかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (ホイールが回転した場合)、false (それ以外の場合)。
        /// </value>
        public bool WheelScrolled
        {
            get { return WheelDelta != 0; }
        }

        /// <summary>
        /// 指定されたボタンが押されたかどうかを判定します。
        /// </summary>
        /// <param name="buttons">判定するボタン。</param>
        /// <returns>
        /// true (ボタンが押された場合)、false (それ以外の場合)。
        /// </returns>
        public bool IsButtonPressed(MouseButtons buttons)
        {
            return (PressedButtons & buttons) == buttons;
        }

        /// <summary>
        /// 指定されたボタンが離されてたかどうかを判定します。
        /// </summary>
        /// <param name="buttons">判定するボタン。</param>
        /// <returns>
        /// true (ボタンが離された場合)、false (それ以外の場合)。
        /// </returns>
        public bool IsButtonReleased(MouseButtons buttons)
        {
            return (ReleasedButtons & buttons) == buttons;
        }

        /// <summary>
        /// 状態を更新します。
        /// </summary>
        public void Update()
        {
            // 状態をリセットします。
            MouseMoved = false;
            PressedButtons = 0;
            ReleasedButtons = 0;

            // MouseState を取得します。
            previousMouseState = mouseState;
            mouseState = Mouse.GetState();

            // カーソルが移動したかどうかを判定します。
            MouseMoved = (previousMouseState.X != mouseState.X || previousMouseState.Y != mouseState.Y);

            // ボタンが押されたかどうか、および、離されたかどうかを判定します。
            // 左ボタン
            if (previousMouseState.LeftButton == ButtonState.Released)
            {
                if (mouseState.LeftButton == ButtonState.Pressed) PressedButtons |= MouseButtons.Left;
            }
            else
            {
                if (mouseState.LeftButton == ButtonState.Released) ReleasedButtons |= MouseButtons.Left;
            }
            // 右ボタン
            if (previousMouseState.RightButton == ButtonState.Released)
            {
                if (mouseState.RightButton == ButtonState.Pressed) PressedButtons |= MouseButtons.Right;
            }
            else
            {
                if (mouseState.RightButton == ButtonState.Released) ReleasedButtons |= MouseButtons.Right;
            }
            // 中央ボタン
            if (previousMouseState.MiddleButton == ButtonState.Released)
            {
                if (mouseState.MiddleButton == ButtonState.Pressed) PressedButtons |= MouseButtons.Middle;
            }
            else
            {
                if (mouseState.MiddleButton == ButtonState.Released) ReleasedButtons |= MouseButtons.Middle;
            }

            // ホイールの回転量を取得します。
            WheelDelta = mouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue;
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
    }
}
