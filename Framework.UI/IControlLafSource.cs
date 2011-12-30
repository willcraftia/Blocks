#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public interface IControlLafSource : IDisposable
    {
        void Initialize();

        IControlLaf GetControlLaf(Control control);
    }
}
