#region Using

using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public interface IInputReceiver
    {
        void NotifyMouseMoved(int x, int y);

        void NotifyMouseButtonPressed(MouseButtons button);
        
        void NotifyMouseButtonReleased(MouseButtons button);
        
        void NotifyMouseWheelRotated(int ticks);
    }
}
