#region Uging

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public interface IControlLaf : IDisposable
    {
        void Draw(Control control, IDrawContext drawContext);
    }
}
