#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo
{
    public sealed class LongSleepingLoader : Content.ILoader<object>
    {
        public object Load()
        {
            System.Threading.Thread.Sleep(5000);
            return 1000;
        }
    }
}
