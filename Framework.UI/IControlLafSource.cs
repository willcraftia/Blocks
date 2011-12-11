#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public interface IControlLafSource
    {
        IUIContext UIContext { get; set; }

        void LoadContent();

        void UnloadContent();

        IControlLaf GetControlLaf(Control control);
    }
}
