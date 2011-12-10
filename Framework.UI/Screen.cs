#region Using

using System;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class Screen : Control, IInputReceiver
    {
        // I/F
        public void NotifyMouseMoved(int x, int y)
        {
            ProcessMouseMoved(x, y);
        }

        // I/F
        public void NotifyMouseButtonPressed(MouseButtons buttons)
        {
        }

        // I/F
        public void NotifyMouseButtonReleased(MouseButtons buttons)
        {
        }

        // I/F
        public void NotifyMouseWheelRotated(int ticks)
        {
        }
    }
}
