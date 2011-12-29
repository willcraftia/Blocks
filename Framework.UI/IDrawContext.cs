#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public interface IDrawContext
    {
        GraphicsDevice GraphicsDevice { get; }

        SpriteBatch SpriteBatch { get; }

        BasicEffect BasicEffect { get; }

        Rectangle Bounds { get; }

        float Opacity { get; }

        Texture2D FillTexture { get; }

        void Flush();
    }
}
