#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo
{
    public sealed class LongSleepingLoader : Content.ILoadable
    {
        public void LoadContent()
        {
            System.Threading.Thread.Sleep(5000);
        }

        public void UnloadContent()
        {
        }
    }
}
