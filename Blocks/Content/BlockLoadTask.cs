#region Using

using System;
using System.Threading;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// Block のロード完了で呼び出されるコールバック メソッドを定義します。
    /// </summary>
    /// <param name="name">ロードされた Block の名前。</param>
    /// <param name="result">ロードされた Block。</param>
    public delegate void BlockLoadTaskCallback(string name, Block result);

    /// <summary>
    /// Block を非同期にロードする構造体です。
    /// </summary>
    public struct BlockLoadTask
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
        /// Block のロード完了で呼び出されるコールバック メソッド。
        /// </summary>
        public BlockLoadTaskCallback Callback;

        /// <summary>
        /// Block を非同期にロードします。
        /// </summary>
        /// <param name="task">BlockLoadTask。</param>
        public static void Start(BlockLoadTask task)
        {
            if (task.Loader == null) throw new InvalidOperationException("task.Loader is null.");
            if (task.Name == null) throw new InvalidOperationException("task.Name is null.");

            ThreadPool.QueueUserWorkItem(WaitCallback, task);
        }

        /// <summary>
        /// Block を非同期にロードします。
        /// </summary>
        /// <param name="loader">Block のロードに使用する IBlockLoader。</param>
        /// <param name="name">ロードする Block の名前。</param>
        /// <param name="callback">Block のロード完了で呼び出されるコールバック メソッド。</param>
        public static void Start(IBlockLoader loader, string name, BlockLoadTaskCallback callback)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            if (name == null) throw new ArgumentNullException("name");

            var task = new BlockLoadTask
            {
                Loader = loader,
                Name = name,
                Callback = callback
            };

            Start(task);
        }

        /// <summary>
        /// ThreadPool から提供される Thread から呼び出されます。
        /// </summary>
        /// <param name="state">BlockLoadTask。</param>
        static void WaitCallback(object state)
        {
            var task = (BlockLoadTask) state;
            task.LoadBlock();
        }

        /// <summary>
        /// Block をロードします。
        /// </summary>
        /// <returns>ロードされた Block。</returns>
        public Block LoadBlock()
        {
            if (Loader == null) throw new InvalidOperationException("Loader is null.");
            if (Name == null) throw new InvalidOperationException("Name is null.");

            var result = Loader.LoadBlock(Name);
            if (Callback != null) Callback(Name, result);

            return result;
        }
    }
}
