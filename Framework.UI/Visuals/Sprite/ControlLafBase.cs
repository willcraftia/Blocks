#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Visuals.Sprite
{
    public abstract class ControlLafBase : IControlLaf
    {
        public SpriteControlLafSource Source { get; private set; }

        protected GraphicsDevice GraphicsDevice { get; private set; }

        protected SpriteBatch SpriteBatch { get; private set; }

        protected ControlLafBase(SpriteControlLafSource source)
        {
            if (source == null) throw new ArgumentNullException("source");

            Source = source;
            GraphicsDevice = source.UIContext.GraphicsDevice;
            SpriteBatch = source.UIContext.SpriteBatch;
        }

        public abstract void Draw(Control control);
    }
}
