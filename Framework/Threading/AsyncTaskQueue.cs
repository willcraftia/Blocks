#region Using

using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Threading
{
    public sealed class AsyncTaskQueue
    {
        #region Task

        /// <summary>
        /// キューに入れられる非同期処理と結果のコールバック処理の一式を表します。
        /// </summary>
        struct Task
        {
            /// <summary>
            /// 非同期に処理するメソッド。
            /// </summary>
            public WaitCallback WaitCallback;

            /// <summary>
            /// WaitCallback に渡され、
            /// AsyncTaskResultCallback が受け取る AsyncTaskResult に設定されるオブジェクト。
            /// </summary>
            public object State;

            /// <summary>
            /// WaitCallback の処理完了を受け取るコールバック メソッド。
            /// </summary>
            public AsyncTaskResultCallback ResultCallback;
        }

        #endregion

        #region TaskInThread

        /// <summary>
        /// Thread への AsyncTask の割り当て状態を管理します。
        /// </summary>
        class TaskInThread
        {
            /// <summary>
            /// Thread で処理する AsyncTask。
            /// </summary>
            public Task Task;
        }

        #endregion

        #region ResultInQueue

        struct ResultInQueue
        {
            public AsyncTaskResultCallback ResultCallback;

            public object State;

            public Exception Exception;
        }

        #endregion

        /// <summary>
        /// 利用できる Thread 数の上限。
        /// </summary>
        public const int MaxThreadCount = 5;

        /// <summary>
        /// 使用する Thread 数の上限。
        /// </summary>
        int threadCount;

        /// <summary>
        /// AsyncTask の Queue。
        /// </summary>
        Queue<Task> taskQueue = new Queue<Task>();

        /// <summary>
        /// ResultInQueue の Queue。
        /// </summary>
        Queue<ResultInQueue> resultQueue = new Queue<ResultInQueue>();

        /// <summary>
        /// 空き Thread の Queue。
        /// </summary>
        Queue<TaskInThread> freeThread;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game"></param>
        public AsyncTaskQueue() : this(MaxThreadCount) { }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game">Game。</param>
        /// <param name="threadCount">使用する Thread 数の上限。</param>
        public AsyncTaskQueue(int threadCount)
        {
            if (threadCount < 1 || MaxThreadCount < threadCount)
                throw new ArgumentOutOfRangeException("threadCount");
            this.threadCount = threadCount;

            freeThread = new Queue<TaskInThread>(threadCount);
            for (int i = 0; i < threadCount; i++) freeThread.Enqueue(new TaskInThread());
        }

        public void Enqueue(WaitCallback waitCallback, object state, AsyncTaskResultCallback resultCallback)
        {
            var task = new Task
            {
                WaitCallback = waitCallback,
                State = state,
                ResultCallback = resultCallback
            };
            lock (taskQueue) taskQueue.Enqueue(task);
        }

        public void Update()
        {
            ProcessTask();
            ProcessResult();
        }

        /// <summary>
        /// キューにある AsyncTask を取り出し、Thread への割り当てを試みます。
        /// </summary>
        void ProcessTask()
        {
            // 空き Thread を探して割り当てます。
            TaskInThread taskInThread;
            lock (taskQueue)
            {
                // Task がないなら処理を終えます。
                if (taskQueue.Count == 0) return;

                lock (freeThread)
                {
                    // 空き Thread がないなら処理を終えます。
                    if (freeThread.Count == 0) return;

                    // 空き Thread を確保します。
                    taskInThread = freeThread.Dequeue();
                }

                // 確保した Thread に Task を割り当てます。
                taskInThread.Task = taskQueue.Dequeue();
            }

            // ThreadPool で実際の Thread を割り当ててもらいます。
            ThreadPool.QueueUserWorkItem(WaitCallback, taskInThread);
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
                State = resultInQueue.State,
                Exception = resultInQueue.Exception
            };

            resultInQueue.ResultCallback(result);
        }

        void WaitCallback(object state)
        {
            var taskInThread = state as TaskInThread;

            Exception exception = null;
            try
            {
                taskInThread.Task.WaitCallback(taskInThread.Task.State);
            }
            catch (Exception e)
            {
                exception = e;
            }

            var result = new ResultInQueue
            {
                ResultCallback = taskInThread.Task.ResultCallback,
                State = taskInThread.Task.State,
                Exception = exception
            };

            // 処理結果をキューへ入れます。
            lock (resultQueue) resultQueue.Enqueue(result);

            // 空き Thread としてマークします。
            lock (freeThread) freeThread.Enqueue(taskInThread);
        }
    }
}
