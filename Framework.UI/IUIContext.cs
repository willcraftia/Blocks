#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public interface IUIContext
    {
        GraphicsDevice GraphicsDevice { get; }

        ContentManager CreateContentManager();
    }
}
