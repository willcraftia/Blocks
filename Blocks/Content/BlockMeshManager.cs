#region Using

using System;
using System.Collections.Generic;
using Willcraftia.Xna.Blocks.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// ロードした BlockMesh の管理をともなう IBlockMeshLoader の実装クラスです。
    /// このクラスからロードした BlockMesh は、
    /// Unload(BlockMesh) あるいは Unload() の呼び出しで破棄する必要があります。
    /// </summary>
    public sealed class BlockMeshManager : IBlockMeshLoader, IDisposable
    {
        /// <summary>
        /// BlockMesh をロードする IBlockMeshLoader。
        /// </summary>
        IBlockMeshLoader blockMeshLoader;

        /// <summary>
        /// ロードした BlockMesh のリスト。
        /// </summary>
        List<BlockMesh> meshes = new List<BlockMesh>();

        /// <summary>
        /// 管理している BlockMesh の数を返します。
        /// </summary>
        public int BlockMeshCount
        {
            get { return meshes.Count; }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="blockMeshLoader">
        /// BlockMesh をロードする IBlockMeshLoader。
        /// </param>
        public BlockMeshManager(IBlockMeshLoader blockMeshLoader)
        {
            if (blockMeshLoader == null) throw new ArgumentNullException("blockMeshLoader");
            this.blockMeshLoader = blockMeshLoader;
        }

        /// <summary>
        /// name が示す BlockMesh をロードします。
        /// ロードした BlockMesh は BlockMeshManager の管理下に置かれます。
        /// </summary>
        /// <param name="name">BlockMesh を示す名前。</param>
        /// <returns>ロードされた BlockMesh。</returns>
        public BlockMesh LoadBlockMesh(string name)
        {
            var mesh = blockMeshLoader.LoadBlockMesh(name);
            meshes.Add(mesh);
            return mesh;
        }

        /// <summary>
        /// BlockMesh をアンロードします。
        /// </summary>
        /// <param name="mesh">アンロードする BlockMesh。</param>
        public void Unload(BlockMesh mesh)
        {
            if (!meshes.Remove(mesh))
                throw new InvalidOperationException("The specified BlockMesh is not managed.");
            mesh.Dispose();
        }

        /// <summary>
        /// ロードした全ての BlockMesh をアンロードします。
        /// </summary>
        public void Unload()
        {
            foreach (var mesh in meshes) mesh.Dispose();
            meshes.Clear();
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool disposed;

        ~BlockMeshManager()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing) Unload();

            disposed = true;
        }

        #endregion
    }
}
