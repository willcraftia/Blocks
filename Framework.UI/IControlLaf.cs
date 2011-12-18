#region Uging

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public interface IControlLaf : IDisposable
    {
        void Initialize();

        void Draw(Control control);
    }
}
