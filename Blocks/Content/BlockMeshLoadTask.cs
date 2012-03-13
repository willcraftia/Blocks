#region Using

using System;
using System.Threading;
using Willcraftia.Xna.Blocks.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// BlockMesh のロード完了で呼び出されるコールバック メソッドを定義します。
    /// </summary>
    /// <param name="name">ロードされた BlockMesh の名前。</param>
    /// <param name="result">ロードされた BlockMesh。</param>
    public delegate void BlockMeshLoadTaskCallback(string name, BlockMesh result);

    /// <summary>
    /// BlockMesh を非同期にロードする構造体です。
    /// </summary>
    public struct BlockMeshLoadTask
    {
        /// <summary>
        /// BlockMesh のロードに使用する IBlockMeshLoader。
        /// </summary>
        public IBlockMeshLoader Loader;

        /// <summary>
        /// ロードする BlockMesh の名前。
        /// </summary>
        public string Name;

        /// <summary>
        /// BlockMesh のロード完了で呼び出されるコールバック メソッド。
        /// </summary>
        public BlockMeshLoadTaskCallback Callback;

        /// <summary>
        /// BlockMesh を非同期にロードします。
        /// </summary>
        /// <param name="task">BlockMeshLoadTask。</param>
        public static void Start(BlockMeshLoadTask task)
        {
            if (task.Loader == null) throw new InvalidOperationException("task.Loader is null.");
            if (task.Name == null) throw new InvalidOperationException("task.Name is null.");

            ThreadPool.QueueUserWorkItem(WaitCallback, task);
        }

        /// <summary>
        /// BlockMesh を非同期にロードします。
        /// </summary>
        /// <param name="loader">BlockMesh のロードに使用する IBlockMeshLoader。</param>
        /// <param name="name">ロードする BlockMesh の名前。</param>
        /// <param name="callback">BlockMesh のロード完了で呼び出されるコールバック メソッド。</param>
        public static void Start(IBlockMeshLoader loader, string name, BlockMeshLoadTaskCallback callback)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            if (name == null) throw new ArgumentNullException("name");

            var task = new BlockMeshLoadTask
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
        /// <param name="state">BlockMeshLoadTask。</param>
        static void WaitCallback(object state)
        {
            var task = (BlockMeshLoadTask) state;
            task.LoadBlockMesh();
        }

        /// <summary>
        /// BlockMesh をロードします。
        /// </summary>
        /// <returns>ロードされた BlockMesh。</returns>
        public BlockMesh LoadBlockMesh()
        {
            if (Loader == null) throw new InvalidOperationException("Loader is null.");
            if (Name == null) throw new InvalidOperationException("Name is null.");

            var result = Loader.LoadBlockMesh(Name);
            if (Callback != null) Callback(Name, result);

            return result;
        }
    }
}
