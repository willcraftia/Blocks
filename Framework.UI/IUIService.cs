#region Using

using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public interface IUIService
    {
        SpriteBatch SpriteBatch { get; }

        Texture2D FillTexture { get; }

        Screen Screen { get; set; }
    }
}
