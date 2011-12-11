#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Visuals
{
    public abstract class Appearance
    {
        public IUIContext UIContext { get; private set; }

        public GraphicsDevice GraphicsDevice { get; private set; }

        public SpriteBatch SpriteBatch { get; private set; }

        public Appearance(IUIContext uiContext)
        {
            if (uiContext == null) throw new ArgumentNullException("uiContext");

            UIContext = uiContext;
            GraphicsDevice = uiContext.GraphicsDevice;
            SpriteBatch = uiContext.SpriteBatch;
        }

        public abstract void Draw(Control control);
    }
}
