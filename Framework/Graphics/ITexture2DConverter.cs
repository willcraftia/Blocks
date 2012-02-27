#region Using

using System;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    public interface ITexture2DConverter
    {
        Texture2D Convert(Texture2D texture);
    }
}
