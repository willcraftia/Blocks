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
        #region TaskInThread

        /// <summary>
        /// Thread への AsyncTask の割り当て状態を管理します。
        /// </summary>
        class TaskInThread
        {
            /// <summary>
            /// true (Thread に割り当てられている場合)、false (それ以外の場合)。
            /// </summary>
            public bool Busy;

            /// <summary>
            /// Thread で処理する AsyncTask。
            /// </summary>
            public AsyncTask Task;
        }

        #endregion

        #region ResultInQueue

        struct ResultInQueue
        {
            public AsyncTaskCallback Callback;

            public Exception Exception;
        }

        #endregion

        /// <summary>
        /// 利用できる Thread 数の上限。
        /// </summary>
        const int maxThreadCount = 5;

        /// <summary>
        /// 使用する Thread 数の上限。
        /// </summary>
        int threadCount;

        /// <summary>
        /// Thread に割り当てる TaskInThread の配列。
        /// </summary>
        TaskInThread[] taskInThreads;

        /// <summary>
        /// AsyncTask の Queue。
        /// </summary>
        Queue<AsyncTask> taskQueue = new Queue<AsyncTask>();

        /// <summary>
        /// ResultInQueue の Queue。
        /// </summary>
        Queue<ResultInQueue> resultQueue = new Queue<ResultInQueue>();

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game"></param>
        public AsyncTaskManager(Game game) : this(game, maxThreadCount) { }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game">Game。</param>
        /// <param name="threadCount">使用する Thread 数の上限。</param>
        public AsyncTaskManager(Game game, int threadCount)
            : base(game)
        {
            if (threadCount < 1 || maxThreadCount < threadCount)
                throw new ArgumentOutOfRangeException("threadCount");
            this.threadCount = threadCount;

            taskInThreads = new TaskInThread[threadCount];
            for (int i = 0; i < threadCount; i++) taskInThreads[i] = new TaskInThread();

            // サービスとして登録します。
            game.Services.AddService(typeof(IAsyncTaskService), this);
        }

        // I/F
        public void Enqueue(AsyncTask task)
        {
            lock (taskQueue) taskQueue.Enqueue(task);
        }

        public override void Update(GameTime gameTime)
        {
            ProcessTask();
            ProcessResult();

            base.Update(gameTime);
        }

        /// <summary>
        /// キューにある AsyncTask を取り出し、Thread への割り当てを試みます。
        /// </summary>
        void ProcessTask()
        {
            AsyncTask task;
            lock (taskQueue)
            {
                // キューが空ならば何もしません。
                if (taskQueue.Count == 0) return;

                // まだ Thread を確保できるかどうかわからないので、
                // Peek で取得します。
                task = taskQueue.Peek();
            }

            // 空き Thread を探して割り当てます。
            TaskInThread taskInThread = null;
            lock (taskInThreads)
            {
                for (int i = 0; i < threadCount; i++)
                {
                    if (!taskInThreads[i].Busy)
                    {
                        // 空き Thread が見つかったので割り当てます。
                        taskInThread = taskInThreads[i];
                        taskInThread.Busy = true;
                        taskInThread.Task = task;
                        break;
                    }
                }
            }

            if (taskInThread != null)
            {
                // Thread に割り当てられたのでキューから取り除きます。
                lock (taskQueue) taskQueue.Dequeue();

                // ThreadPool で実際の Thread 割り当てを行ってもらいます。
                ThreadPool.QueueUserWorkItem(WaitCallback, taskInThread);
            }
        }

        /// <summary>
        /// キューにある ResultInQueue を取り出し、コールバックします。
        /// </summary>
        void ProcessResult()
        {
            ResultInQueue resultInQueue;
            lock (resultQueue)
            {
                if (resultQueue.Count == 0) return;

                resultInQueue = resultQueue.Dequeue();
            }

            var result = new AsyncTaskResult
            {
                Exception = resultInQueue.Exception
            };

            resultInQueue.Callback(result);
        }

        void WaitCallback(object state)
        {
            var taskInThread = state as TaskInThread;

            Exception exception = null;
            try
            {
                taskInThread.Task.Action();
            }
            catch (Exception e)
            {
                exception = e;
            }

            var result = new ResultInQueue
            {
                Callback = taskInThread.Task.Callback,
                Exception = exception
            };

            lock (resultQueue) resultQueue.Enqueue(result);

            // Atomic だから lock いらないよね？
            taskInThread.Busy = false;
        }
    }
}
