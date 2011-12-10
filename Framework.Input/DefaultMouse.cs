#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    public class DefaultMouse : IMouse
    {
        public event MouseMoveDelegate MouseMoved;

        public event MouseButtonDelegate MouseButtonPressed;

        public event MouseButtonDelegate MouseButtonReleased;

        public event MouseWheelDelegate MouseWheelRotated;

        MouseState previousMouseState = new MouseState();

        MouseState currentMouseState = new MouseState();

        public bool Enabled
        {
            get { return true; }
        }

        public string Name
        {
            get { return "Main Mouse"; }
        }

        public void MoveTo(int x, int y)
        {
            Mouse.SetPosition(x, y);
        }

        public void Update()
        {
            currentMouseState = Mouse.GetState();

            // マウス カーソルの位置が前回から移動したかどうか
            if (previousMouseState.X != currentMouseState.X || previousMouseState.Y != currentMouseState.Y)
            {
                // イベント発生
                RaiseMouseMoved(currentMouseState.X, currentMouseState.Y);
            }

            // ボタン押下状態の判定
            MouseButtons pressedButtons = 0;
            MouseButtons releasedButtons = 0;
            // 左ボタン判定
            if (previousMouseState.LeftButton == ButtonState.Released)
            {
                // 押されたかどうか
                if (currentMouseState.LeftButton == ButtonState.Pressed) pressedButtons |= MouseButtons.Left;
            }
            else
            {
                // 離されたかどうか
                if (currentMouseState.LeftButton == ButtonState.Released) releasedButtons |= MouseButtons.Left;
            }
            // 右ボタン判定
            if (previousMouseState.RightButton == ButtonState.Released)
            {
                // 押されたかどうか
                if (currentMouseState.RightButton == ButtonState.Pressed) pressedButtons |= MouseButtons.Right;
            }
            else
            {
                // 離されたかどうか
                if (currentMouseState.RightButton == ButtonState.Released) releasedButtons |= MouseButtons.Right;
            }
            // 中央ボタン判定
            if (previousMouseState.MiddleButton == ButtonState.Released)
            {
                // 押されたかどうか
                if (currentMouseState.MiddleButton == ButtonState.Pressed) pressedButtons |= MouseButtons.Middle;
            }
            else
            {
                // 離されたかどうか
                if (currentMouseState.MiddleButton == ButtonState.Released) releasedButtons |= MouseButtons.Middle;
            }

            // いずれかのボタンが押されたならばイベント発生
            if (pressedButtons != 0) RaiseMouseButtonPressed(pressedButtons);
            // いずれかのボタンが離されたならばイベント発生
            if (releasedButtons != 0) RaiseMouseButtonReleased(releasedButtons);

            // マウス ホイールの回転量から前回から変化したかどうか
            if (previousMouseState.ScrollWheelValue != currentMouseState.ScrollWheelValue)
            {
                // 前回からの移動量の通知でイベント発生
                RaiseMouseWheelRotated(currentMouseState.ScrollWheelValue - previousMouseState.ScrollWheelValue);
            }
        }

        void RaiseMouseMoved(int x, int y)
        {
            if (MouseMoved != null) MouseMoved(x, y);
        }

        void RaiseMouseButtonPressed(MouseButtons buttons)
        {
            if (MouseButtonPressed != null) MouseButtonPressed(buttons);
        }

        void RaiseMouseButtonReleased(MouseButtons buttons)
        {
            if (MouseButtonReleased != null) MouseButtonReleased(buttons);
        }

        void RaiseMouseWheelRotated(int ticks)
        {
            if (MouseWheelRotated != null) MouseWheelRotated(ticks);
        }
    }
}
