#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    public class PlayerMouse : IMouse
    {
        public event MouseMoveDelegate MouseMoved;

        public event MouseButtonDelegate MouseButtonPressed;

        public event MouseButtonDelegate MouseButtonReleased;

        public event MouseWheelDelegate MouseWheelRotated;

        public bool Enabled
        {
            get { return true; }
        }

        public string Name
        {
            get { return "Main Mouse"; }
        }

        public MouseState GetState()
        {
            return Mouse.GetState();
        }

        public void MoveTo(int x, int y)
        {
            Mouse.SetPosition(x, y);
        }

        public void Update()
        {
        }
    }
}
