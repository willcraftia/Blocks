#region Using

using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Threading
{
    /// <summary>
    /// IAsyncTaskService の実装クラスです。
    /// </summary>
    public sealed class AsyncTaskManager : GameComponent, IAsyncTaskService
    {
        AsyncTaskQueue queue;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game"></param>
        public AsyncTaskManager(Game game) : this(game, AsyncTaskQueue.MaxThreadCount) { }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game">Game。</param>
        /// <param name="threadCount">使用する Thread 数の上限。</param>
        public AsyncTaskManager(Game game, int threadCount)
            : base(game)
        {
            queue = new AsyncTaskQueue(threadCount);

            // サービスとして登録します。
            game.Services.AddService(typeof(IAsyncTaskService), this);
        }

        public void Enqueue(WaitCallback waitCallback, object state, AsyncTaskResultCallback resultCallback)
        {
            queue.Enqueue(waitCallback, state, resultCallback);
        }

        public override void Update(GameTime gameTime)
        {
            queue.Update();

            base.Update(gameTime);
        }
    }
}
