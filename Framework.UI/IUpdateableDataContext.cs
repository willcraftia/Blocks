#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public interface IUpdateableDataContext
    {
        void Update(GameTime gameTime);
    }
}
