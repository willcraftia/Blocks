#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework
{
    public sealed class UpdateQueue
    {
        List<IUpdateQueueItem> items;

        TimeSpan delay = TimeSpan.Zero;

        TimeSpan lastUpdateTime = TimeSpan.Zero;

        public UpdateQueue(int queueSize)
        {
            items = new List<IUpdateQueueItem>(queueSize);
        }

        public void Enqueue(IUpdateQueueItem item)
        {
            lock (this)
            {
                items.Add(item);
            }
        }

        public bool Cancel(IUpdateQueueItem item)
        {
            lock (this)
            {
                return items.Remove(item);
            }
        }

        public int Cancel(Predicate<IUpdateQueueItem> match)
        {
            lock (this)
            {
                return items.RemoveAll(match);
            }
        }

        public void Update(GameTime gameTime)
        {
            var time = gameTime.TotalGameTime;
            if (time < lastUpdateTime + delay) return;

            if (gameTime.IsRunningSlowly)
            {
                delay += TimeSpan.FromMilliseconds(1000);
                return;
            }

            IUpdateQueueItem item;

            lock (this)
            {
                if (items.Count == 0) return;

                item = items[0];
                items.RemoveAt(0);
            }

            item.Update(gameTime);

            delay = item.Duration;
            lastUpdateTime = time;
        }
    }
}
