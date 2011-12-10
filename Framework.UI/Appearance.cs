#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public abstract class Appearance
    {
        public IUIService UIService { get; private set; }

        public GraphicsDevice GraphicsDevice { get; private set; }

        public SpriteBatch SpriteBatch { get; private set; }

        public Appearance(GameServiceContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            UIService = container.GetService(typeof(IUIService)) as IUIService;
            if (UIService == null) throw new InvalidOperationException("IUIService not found.");

            var graphicsDeviceService = container.GetService(typeof(IGraphicsDeviceService)) as IGraphicsDeviceService;
            if (graphicsDeviceService == null) throw new InvalidOperationException("IGraphicsDeviceService not found.");

            GraphicsDevice = graphicsDeviceService.GraphicsDevice;
            SpriteBatch = UIService.SpriteBatch;
        }

        public abstract void Draw(Control control);
    }
}
