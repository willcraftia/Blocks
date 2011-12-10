#region Using

using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public interface IInputReceiver
    {
        void NotifyMouseMoved(int x, int y);

        void NotifyMouseButtonPressed(MouseButtons buttons);
        
        void NotifyMouseButtonReleased(MouseButtons buttons);
        
        void NotifyMouseWheelRotated(int ticks);
    }
}
