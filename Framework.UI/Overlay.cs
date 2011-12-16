#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class Overlay
    {
        public Control Container { get; private set; }

        public Overlay()
        {
            Container = new Control();
        }
    }
}
