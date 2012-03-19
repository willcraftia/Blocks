#region Using

using System;
using System.Collections.Generic;
using System.Threading;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    public delegate void InterBlockMeshLoadQueueCallback(string name, InterBlockMesh result);

    public sealed class InterBlockMeshLoadQueue
    {
        #region Task

        /// <summary>
        /// Block のロードから InterBlockMesh の生成までの処理を表す構造体です。
        /// </summary>
        struct Task
        {
            /// <summary>
            /// Block のロードに使用する IBlockLoader。
            /// </summary>
            public IBlockLoader Loader;

            /// <summary>
            /// InterBlockMesh の生成に使用する InterBlockMeshFactory。
            /// </summary>
            public InterBlockMeshFactory Factory;

            /// <summary>
            /// ロードする Block の名前。
            /// </summary>
            public string Name;

            /// <summary>
            /// InterBlockMesh のロード完了で呼び出されるコールバック メソッド。
            /// </summary>
            public InterBlockMeshLoadQueueCallback Callback;

            /// <summary>
            /// InterBlockMesh をロードします。
            /// </summary>
            /// <returns>ロードされた InterBlockMesh。</returns>
            public void Execute()
            {
                var block = Loader.LoadBlock(Name);
                var interBlockMesh = Factory.Create(block);
                Callback(Name, interBlockMesh);
            }
        }

        #endregion

        #region TaskInThread

        /// <summary>
        /// Thread で処理する Task を管理するクラスです。
        /// </summary>
        class TaskInThread
        {
            /// <summary>
            /// true (Thread に割り当てられている場合)、false (それ以外の場合)。
            /// </summary>
            public bool Busy;

            /// <summary>
            /// Thread で処理する Task。
            /// </summary>
            public Task Task;
        }

        #endregion

        /// <summary>
        /// 利用できる Thread の上限。
        /// </summary>
        const int maxThreadCount = 5;

        /// <summary>
        /// 同期のためのオブジェクト。
        /// </summary>
        readonly object syncRoot = new object();

        /// <summary>
        /// 使用する Thread の上限。
        /// </summary>
        int threadCount;

        /// <summary>
        /// Task のリスト。
        /// キューとしては効率が悪いですが、取り消し要求を考慮してリストで管理します。
        /// </summary>
        List<Task> tasks;

        /// <summary>
        /// Thread に割り当てる TaskInThread の配列。
        /// </summary>
        TaskInThread[] taskInThreads;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public InterBlockMeshLoadQueue()
            : this(maxThreadCount)
        {
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="threadCount"></param>
        public InterBlockMeshLoadQueue(int threadCount)
        {
            if (threadCount < 1 || maxThreadCount < threadCount)
                throw new ArgumentOutOfRangeException("threadCount");
            this.threadCount = threadCount;

            tasks = new List<Task>();
            taskInThreads = new TaskInThread[threadCount];
            for (int i = 0; i < threadCount; i++) taskInThreads[i] = new TaskInThread();
        }

        public void Load(IBlockLoader loader, InterBlockMeshFactory factory, string name, InterBlockMeshLoadQueueCallback callback)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            if (factory == null) throw new ArgumentNullException("factory");
            if (name == null) throw new ArgumentNullException("name");
            if (callback == null) throw new ArgumentNullException("callback");

            var task = new Task
            {
                Loader = loader,
                Factory = factory,
                Name = name,
                Callback = callback
            };

            tasks.Add(task);
        }

        //
        // MEMO
        //
        // threadCount = 2 でも、自分の環境ではほぼ取り消しのタイミングがない。
        // threadCount = 1 では主に発生。
        //

        public bool Cancel(string name)
        {
            for (int i = 0; i < tasks.Count; i++)
            {
                if (tasks[i].Name == name)
                {
                    tasks.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public void Update()
        {
            if (tasks.Count == 0) return;

            // シンプルに処理するために、
            // 1 つの Task だけを取り出して Thread を割り当てます。

            var task = tasks[0];
            tasks.RemoveAt(0);

            TaskInThread taskInThread = null;
            lock (syncRoot)
            {
                for (int i = 0; i < threadCount; i++)
                {
                    if (!taskInThreads[i].Busy)
                    {
                        taskInThread = taskInThreads[i];
                        taskInThread.Busy = true;
                        taskInThread.Task = task;
                        break;
                    }
                }
            }

            if (taskInThread == null) return;
            ThreadPool.QueueUserWorkItem(WaitCallback, taskInThread);
        }

        /// <summary>
        /// ThreadPool から提供される Thread から呼び出されます。
        /// </summary>
        /// <param name="state">Task。</param>
        void WaitCallback(object state)
        {
            var taskInThread = (TaskInThread) state;
            taskInThread.Task.Execute();

            lock (syncRoot)
            {
                taskInThread.Busy = false;
            }
        }
    }
}
