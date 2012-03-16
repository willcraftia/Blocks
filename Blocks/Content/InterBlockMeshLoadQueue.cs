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

        const int maxThreadCount = 5;

        readonly object syncRoot = new object();

        int threadCount;

        int threadUseCount;

        // キューとしては効率が悪いですが、取り消し要求を考慮してリストで管理します。
        List<Task> tasks;

        public InterBlockMeshLoadQueue(int threadCount)
        {
            if (threadCount < 1 || maxThreadCount < threadCount) throw new ArgumentOutOfRangeException("threadCount");
            this.threadCount = threadCount;

            tasks = new List<Task>();
        }

        public void Load(IBlockLoader loader, InterBlockMeshFactory factory, string name, InterBlockMeshLoadQueueCallback callback)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            if (factory == null) throw new ArgumentNullException("factory");
            if (name == null) throw new ArgumentNullException("name");

            var task = new Task
            {
                Loader = loader,
                Factory = factory,
                Name = name,
                Callback = callback
            };

            tasks.Add(task);
        }

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

            lock (syncRoot)
            {
                if (threadUseCount < maxThreadCount)
                {
                    var task = tasks[0];
                    tasks.RemoveAt(0);

                    threadUseCount++;
                    ThreadPool.QueueUserWorkItem(WaitCallback, task);

                    Console.WriteLine("threadUseCount++: " + threadUseCount);
                }
            }
        }

        /// <summary>
        /// ThreadPool から提供される Thread から呼び出されます。
        /// </summary>
        /// <param name="state">Task。</param>
        void WaitCallback(object state)
        {
            var task = (Task) state;
            task.Execute();

            lock (syncRoot)
            {
                threadUseCount--;

                Console.WriteLine("threadUseCount--: " + threadUseCount);
            }
        }
    }
}
