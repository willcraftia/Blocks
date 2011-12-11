﻿#region Using

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

        void RaiseMouseMoved(int x, int y)
        {
            if (MouseMoved != null) MouseMoved(x, y);
        }

        void RaiseMouseButtonPressed(MouseButtons button)
        {
            if (MouseButtonPressed != null) MouseButtonPressed(button);
        }

        void RaiseMouseButtonReleased(MouseButtons button)
        {
            if (MouseButtonReleased != null) MouseButtonReleased(button);
        }

        void RaiseMouseWheelRotated(int ticks)
        {
            if (MouseWheelRotated != null) MouseWheelRotated(ticks);
        }
    }
}