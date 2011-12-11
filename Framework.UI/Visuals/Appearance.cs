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
        public GraphicsDevice GraphicsDevice { get; private set; }

        public IUIService UIService { get; private set; }

        public SpriteBatch SpriteBatch { get; private set; }

        public Appearance(GameServiceContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            var graphicsDeviceService = container.GetRequiredService<IGraphicsDeviceService>();
            GraphicsDevice = graphicsDeviceService.GraphicsDevice;

            UIService = container.GetRequiredService<IUIService>();
            SpriteBatch = UIService.SpriteBatch;
        }

        public abstract void Draw(Control control);
    }
}
