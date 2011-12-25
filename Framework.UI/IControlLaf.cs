#region Uging

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public interface IControlLaf : IDisposable
    {
        void Initialize();

        void Draw(Control control, Rectangle renderBounds);
    }
}
