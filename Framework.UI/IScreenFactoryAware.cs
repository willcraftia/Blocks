#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public interface IScreenFactoryAware
    {
        IScreenFactory ScreenFactory { set; }
    }
}
