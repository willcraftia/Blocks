#region Using

using System;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class Screen : Control, IInputReceiver
    {
        /// <summary>
        /// フォーカスを得ている Control を取得あるいは設定します。
        /// </summary>
        internal Control FocusedControl { get; set; }

        public Screen()
        {
            // Control としての自分の Screen に自身を設定しておきます。
            Screen = this;
        }

        // I/F
        public void NotifyMouseMoved(int x, int y)
        {
            ProcessMouseMoved(x, y);
        }

        // I/F
        public void NotifyMouseButtonPressed(MouseButtons button)
        {
            ProcessMouseButtonPressed(button);
        }

        // I/F
        public void NotifyMouseButtonReleased(MouseButtons button)
        {
            ProcessMouseButtonReleased(button);
        }

        // I/F
        public void NotifyMouseWheelRotated(int ticks)
        {
        }
    }
}
