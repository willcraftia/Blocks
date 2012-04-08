#region Using

using System;
using System.Collections.Generic;
using System.Threading;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// InterBlockMesh のロード要求をキューで管理し、
    /// それらに Thread を割り当てて並列処理するクラスです。
    /// </summary>
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
            /// ロードする Block の名前。
            /// </summary>
            public string Name;

            /// <summary>
            /// 生成する LOD の数。
            /// </summary>
            public int LodCount;

            /// <summary>
            /// InterBlockMesh のロード完了で呼び出されるコールバック メソッド。
            /// </summary>
            public InterBlockMeshLoadQueueCallback Callback;

            /// <summary>
            /// InterBlockMesh をロードします。
            /// </summary>
            /// <returns>ロードされた InterBlockMesh。</returns>
            public InterBlockMesh Execute()
            {
                var block = Loader.LoadBlock(Name);
                return InterBlockMeshFactory.InterBlockMesh(block, LodCount);
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
            /// Thread で処理する Task。
            /// </summary>
            public Task Task;
        }

        #endregion

        #region ResultInQueue

        struct ResultInQueue
        {
            public InterBlockMeshLoadQueueCallback ResultCallback;

            public string Name;

            public InterBlockMesh InterBlockMesh;

            public Exception Exception;
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
        /// Item のリスト。
        /// キューとしては効率が悪いですが、取り消し要求を考慮してリストで管理します。
        /// </summary>
        List<Task> queue;

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
        public InterBlockMeshLoadQueue() : this(maxThreadCount) { }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="threadCount"></param>
        public InterBlockMeshLoadQueue(int threadCount)
        {
            if (threadCount < 1 || maxThreadCount < threadCount)
                throw new ArgumentOutOfRangeException("threadCount");
            this.threadCount = threadCount;

            queue = new List<Task>();
            freeThread = new Queue<TaskInThread>(threadCount);
            for (int i = 0; i < threadCount; i++) freeThread.Enqueue(new TaskInThread());
        }

        /// <summary>
        /// InterBlockMesh のロード要求をキューへ追加します。
        /// </summary>
        /// <param name="loader">Block のロードに使用する IBlockLoader。</param>
        /// <param name="name">ロードする Block の名前。</param>
        /// <param name="lodCount">生成する LOD の数。</param>
        /// <param name="callback">InterBlockMesh のロード完了で呼び出されるコールバック メソッド。</param>
        public void Load(IBlockLoader loader, string name, int lodCount, InterBlockMeshLoadQueueCallback callback)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            if (name == null) throw new ArgumentNullException("name");
            if (callback == null) throw new ArgumentNullException("callback");

            var item = new Task
            {
                Loader = loader,
                Name = name,
                LodCount = lodCount,
                Callback = callback
            };

            queue.Add(item);
        }

        //
        // MEMO
        //
        // threadCount = 2 でも、自分の環境ではほぼ取り消しのタイミングがない。
        // threadCount = 1 では主に発生。
        //

        /// <summary>
        /// 指定の名前についての InterBlockMesh のロード要求を取り消します。
        /// ただし、取り消しの対象は、またキューに存在するロード要求のみです。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public bool Cancel(string name)
        {
            for (int i = 0; i < queue.Count; i++)
            {
                if (queue[i].Name == name)
                {
                    queue.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// InterBlockMesh のロード要求をキューから取り出し、
        /// Thread を割り当てて実行します。
        /// Thread の割り当ては、コンストラクタで指定された threadCount を上限とします。
        /// また、メソッドの呼び出しごとに割り当てられる Thread は 1 つです。
        /// </summary>
        public void Update()
        {
            ProcessTask();
            ProcessResult();
        }

        void ProcessTask()
        {
            // 空き Thread を探して割り当てます。
            TaskInThread taskInThread;
            lock (queue)
            {
                // Task がないなら処理を終えます。
                if (queue.Count == 0) return;

                lock (freeThread)
                {
                    // 空き Thread がないなら処理を終えます。
                    if (freeThread.Count == 0) return;

                    // 空き Thread を確保します。
                    taskInThread = freeThread.Dequeue();
                }

                // 確保した Thread に Task を割り当てます。
                taskInThread.Task = queue[0];
                queue.RemoveAt(0);
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

            resultInQueue.ResultCallback(resultInQueue.Name, resultInQueue.InterBlockMesh);
        }

        /// <summary>
        /// ThreadPool から提供される Thread から呼び出されます。
        /// </summary>
        /// <param name="state">Task。</param>
        void WaitCallback(object state)
        {
            var taskInThread = state as TaskInThread;

            InterBlockMesh interBlockMesh = null;
            Exception exception = null;
            try
            {
                interBlockMesh = taskInThread.Task.Execute();
            }
            catch (Exception e)
            {
                exception = e;
            }

            var result = new ResultInQueue
            {
                ResultCallback = taskInThread.Task.Callback,
                Name = taskInThread.Task.Name,
                InterBlockMesh = interBlockMesh,
                Exception = exception
            };

            // 処理結果をキューへ入れます。
            lock (resultQueue) resultQueue.Enqueue(result);

            // 空き Thread としてマークします。
            lock (freeThread) freeThread.Enqueue(taskInThread);
        }
    }
}
