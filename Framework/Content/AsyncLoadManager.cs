#region Using

using System;
using System.Threading.Tasks;

#endregion

namespace Willcraftia.Xna.Framework.Content
{
    public delegate void AsyncLoadCompleteCallback();

    public sealed class AsyncLoadManager
    {
        public void Execute(ILoadable loadable, AsyncLoadCompleteCallback callback)
        {
            var task = Task.Factory.StartNew(() => loadable.LoadContent());
            task.ContinueWith(t => callback());
        }
    }
}
