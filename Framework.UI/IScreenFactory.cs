#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public interface IScreenFactory
    {
        Screen CreateScreen(string screenName);
    }
}
