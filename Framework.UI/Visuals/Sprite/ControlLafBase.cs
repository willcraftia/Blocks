#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Visuals.Sprite
{
    public abstract class ControlLafBase : IControlLaf, IDisposable
    {
        public SpriteControlLafSource Source { get; set; }

        protected ContentManager Content { get; private set; }

        protected GraphicsDevice GraphicsDevice { get; private set; }

        protected SpriteBatch SpriteBatch { get; private set; }

        protected ControlLafBase() { }

        public virtual void Initialize()
        {
            if (Source == null) throw new InvalidOperationException("Source is null.");
            if (Source.UIContext == null) throw new InvalidOperationException("Source is not bound to UIContext.");

            Content = Source.Content;
            GraphicsDevice = Source.UIContext.GraphicsDevice;
            SpriteBatch = Source.UIContext.SpriteBatch;

            LoadContent();
        }

        protected virtual void LoadContent() { }

        protected virtual void UnloadContent() { }

        public abstract void Draw(Control control);

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        bool disposed;

        ~ControlLafBase()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    UnloadContent();
                    Source = null;
                    Content = null;
                    GraphicsDevice = null;
                    SpriteBatch = null;
                }
                disposed = true;
            }
        }

        #endregion
    }
}
