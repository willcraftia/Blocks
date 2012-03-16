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
    /// <param name="result">ロードされた InterBlockMesh。</param>
    public delegate void InterBlockMeshLoadTaskCallback(string name, InterBlockMesh result);

    /// <summary>
    /// InterBlockMesh を非同期にロードする構造体です。
    /// </summary>
    public struct InterBlockMeshLoadTask
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
        public InterBlockMeshLoadTaskCallback Callback;

        /// <summary>
        /// InterBlockMesh を非同期にロードします。
        /// </summary>
        /// <param name="task">InterBlockMeshLoadTask。</param>
        public static void Start(InterBlockMeshLoadTask task)
        {
            if (task.Loader == null) throw new InvalidOperationException("task.Loader is null.");
            if (task.Name == null) throw new InvalidOperationException("task.Name is null.");

            ThreadPool.QueueUserWorkItem(WaitCallback, task);
        }

        /// <summary>
        /// InterBlockMesh を非同期にロードします。
        /// </summary>
        /// <param name="loader">Block のロードに使用する IBlockLoader。</param>
        /// <param name="factory">InterBlockMesh の生成に使用する InterBlockMeshFactory。</param>
        /// <param name="name">ロードする Block の名前。</param>
        /// <param name="callback">InterBlockMesh のロード完了で呼び出されるコールバック メソッド。</param>
        public static void Start(IBlockLoader loader, InterBlockMeshFactory factory, string name, InterBlockMeshLoadTaskCallback callback)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            if (name == null) throw new ArgumentNullException("name");

            var task = new InterBlockMeshLoadTask
            {
                Loader = loader,
                Factory = factory,
                Name = name,
                Callback = callback
            };

            Start(task);
        }

        /// <summary>
        /// ThreadPool から提供される Thread から呼び出されます。
        /// </summary>
        /// <param name="state">InterBlockMeshLoadTask。</param>
        static void WaitCallback(object state)
        {
            var task = (InterBlockMeshLoadTask) state;
            task.LoadBlock();
        }

        /// <summary>
        /// InterBlockMesh をロードします。
        /// </summary>
        /// <returns>ロードされた InterBlockMesh。</returns>
        public InterBlockMesh LoadBlock()
        {
            if (Loader == null) throw new InvalidOperationException("Loader is null.");
            if (Name == null) throw new InvalidOperationException("Name is null.");

            var block = Loader.LoadBlock(Name);
            var interBlockMesh = Factory.Create(block);

            if (Callback != null) Callback(Name, interBlockMesh);

            return interBlockMesh;
        }
    }
}
