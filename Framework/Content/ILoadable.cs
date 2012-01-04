#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Content
{
    public interface ILoadable
    {
        void LoadContent();

        void UnloadContent();
    }
}
