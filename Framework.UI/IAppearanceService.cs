#region Using

using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public interface IAppearanceService
    {
        SpriteBatch SpriteBatch { get; }

        Texture2D FillTexture { get; }

        Screen Screen { get; set; }
    }
}
