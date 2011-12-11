#region Using

using System;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public interface IUIContext
    {
        GraphicsDevice GraphicsDevice { get; }

        SpriteBatch SpriteBatch { get; }

        Texture2D FillTexture { get; }

        Screen Screen { get; }

        void Bind(Control control);

        void Unbind(Control control);

        bool HasFocus(Control control);

        void Focus(Control control);

        void Defocus(Control control);
    }
}
