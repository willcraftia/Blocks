#region Using

using System;
using System.Collections.Generic;
using Willcraftia.Xna.Blocks.Graphics;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// ロードした BlockMesh の管理をともなう IBlockMeshLoader の実装クラスです。
    /// このクラスは、Block のロードから BlockMesh の生成までを一括で行う場合に用います。
    /// このクラスからロードした BlockMesh は、
    /// Unload(BlockMesh) あるいは Unload() の呼び出しで破棄する必要があります。
    /// </summary>
    public sealed class BlockMeshManager : IDisposable
    {
        /// <summary>
        /// Block をロードするための IBlockLoader。
        /// </summary>
        IBlockLoader blockLoader;

        /// <summary>
        /// 取得した Block から BlockMesh を生成するための BlockMeshFactory。
        /// </summary>
        BlockMeshFactory blockMeshFactory;

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
        /// <param name="blockLoader">
        /// Block をロードするための IBlockLoader。
        /// </param>
        /// <param name="blockMeshFactory">
        /// 取得した Block から BlockMesh を生成するための BlockMeshFactory。
        /// </param>
        public BlockMeshManager(IBlockLoader blockLoader, BlockMeshFactory blockMeshFactory)
        {
            if (blockLoader == null) throw new ArgumentNullException("blockLoader");
            if (blockMeshFactory == null) throw new ArgumentNullException("blockMeshFactory");
            this.blockLoader = blockLoader;
            this.blockMeshFactory = blockMeshFactory;
        }

        /// <summary>
        /// name が示す BlockMesh をロードします。
        /// ロードした BlockMesh は BlockMeshManager の管理下に置かれます。
        /// </summary>
        /// <param name="name">BlockMesh を示す名前。</param>
        /// <param name="lodCount">LOD の数。</param>
        /// <returns>ロードされた BlockMesh。</returns>
        public BlockMesh Load(string name, int lodCount)
        {
            var block = blockLoader.LoadBlock(name);
            var interMesh = InterBlockMeshFactory.InterBlockMesh(block, lodCount);
            var mesh = blockMeshFactory.Create(interMesh);

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
