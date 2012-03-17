#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework
{
    public interface IUpdateQueueItem
    {
        TimeSpan Duration { get; }

        void Update(GameTime gameTime);
    }
}
