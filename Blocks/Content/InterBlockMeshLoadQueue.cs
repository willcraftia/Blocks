#region Using

using System;
using System.Collections.Generic;
using System.Threading;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// InterBlockMesh のロード完了で呼び出されるコールバック メソッドを定義します。
    /// </summary>
    /// <param name="name">ロードされた Block の名前。</param>
    /// <param name="result">ロードされた InterBlockMesh。</param>
    public delegate void InterBlockMeshLoadQueueCallback(string name, InterBlockMesh result);

    /// <summary>
    /// InterBlockMesh のロード要求をキューで管理し、
    /// それらに Thread を割り当てて並列処理するクラスです。
    /// </summary>
    public sealed class InterBlockMeshLoadQueue
    {
        #region Item

        /// <summary>
        /// Block のロードから InterBlockMesh の生成までの処理を表す構造体です。
        /// </summary>
        struct Item
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
            public void Execute()
            {
                var block = Loader.LoadBlock(Name);
                var interBlockMesh = InterBlockMeshFactory.InterBlockMesh(block, LodCount);
                Callback(Name, interBlockMesh);
            }
        }

        #endregion

        #region ItemInThread

        /// <summary>
        /// Thread で処理する Item を管理するクラスです。
        /// </summary>
        class ItemInThread
        {
            /// <summary>
            /// true (Thread に割り当てられている場合)、false (それ以外の場合)。
            /// </summary>
            public bool Busy;

            /// <summary>
            /// Thread で処理する Item。
            /// </summary>
            public Item Item;
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
        List<Item> queue;

        /// <summary>
        /// Thread に割り当てる ItemInThread の配列。
        /// </summary>
        ItemInThread[] itemInThreads;

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

            queue = new List<Item>();
            itemInThreads = new ItemInThread[threadCount];
            for (int i = 0; i < threadCount; i++) itemInThreads[i] = new ItemInThread();
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

            var item = new Item
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
            if (queue.Count == 0) return;

            var item = queue[0];
            queue.RemoveAt(0);

            ItemInThread itemInThread = null;
            lock (syncRoot)
            {
                for (int i = 0; i < threadCount; i++)
                {
                    if (!itemInThreads[i].Busy)
                    {
                        itemInThread = itemInThreads[i];
                        itemInThread.Busy = true;
                        itemInThread.Item = item;
                        break;
                    }
                }
            }

            if (itemInThread == null)
            {
                queue.Insert(0, item);
                return;
            }

            ThreadPool.QueueUserWorkItem(WaitCallback, itemInThread);
        }

        /// <summary>
        /// ThreadPool から提供される Thread から呼び出されます。
        /// </summary>
        /// <param name="state">Task。</param>
        void WaitCallback(object state)
        {
            var itemInThread = (ItemInThread) state;
            itemInThread.Item.Execute();

            lock (syncRoot)
            {
                itemInThread.Busy = false;
            }
        }
    }
}
