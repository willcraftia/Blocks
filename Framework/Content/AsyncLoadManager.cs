#region Using

using System;
using System.Threading.Tasks;

#endregion

namespace Willcraftia.Xna.Framework.Content
{
    public delegate void AsyncLoadCompleteCallback<TResult>(TResult result);

    public sealed class AsyncLoadManager
    {
        public void Execute<TResult>(ILoader<TResult> loader, AsyncLoadCompleteCallback<TResult> callback)
        {
            var task = Task.Factory.StartNew<TResult>(() => loader.Load());
            task.ContinueWith(t => callback(t.Result));
        }
    }
}
