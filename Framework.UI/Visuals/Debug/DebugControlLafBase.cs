﻿#region Using

using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Visuals.Debug
{
    public abstract class DebugControlLafBase : ControlLafBase
    {
        public DebugControlLafSource Source { get; set; }

        protected ContentManager Content { get; private set; }

        protected GraphicsDevice GraphicsDevice { get; private set; }

        protected SpriteBatch SpriteBatch { get; private set; }

        protected DebugControlLafBase() { }

        public override void Initialize()
        {
            if (Source == null) throw new InvalidOperationException("Source is null.");
            if (Source.UIContext == null) throw new InvalidOperationException("Source is not bound to UIContext.");

            Content = Source.Content;
            GraphicsDevice = Source.UIContext.GraphicsDevice;
            SpriteBatch = Source.UIContext.SpriteBatch;

            base.Initialize();
        }
    }
}