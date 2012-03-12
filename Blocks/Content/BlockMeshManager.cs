#region Using

using System;
using System.IO;
using System.Collections.Generic;
using Willcraftia.Xna.Framework.Serialization;
using Willcraftia.Xna.Blocks.Graphics;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// BlockMesh の生成と破棄を管理するクラスです。
    /// </summary>
    public sealed class BlockMeshManager : IDisposable
    {
        static readonly XmlSerializer<Block> defaultSerializer = new XmlSerializer<Block>();

        /// <summary>
        /// ロードした BlockMesh のリスト。
        /// </summary>
        List<BlockMesh> meshes = new List<BlockMesh>();

        /// <summary>
        /// Stream から Block を得るための ISerializer。
        /// </summary>
        ISerializer<Block> serializer;

        /// <summary>
        /// BlockMeshFactory。
        /// </summary>
        BlockMeshFactory meshFactory;

        /// <summary>
        /// インスタンスを生成します。
        /// Stream から Block を得るための ISerializer には XmlSerializer が設定されます。
        /// </summary>
        /// <param name="meshFactory">BlockMesh の生成を委譲する BlockMeshFactory。</param>
        public BlockMeshManager(BlockMeshFactory meshFactory) : this(meshFactory, null) { }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="meshFactory">BlockMesh の生成を委譲する BlockMeshFactory。</param>
        /// <param name="serializer">Stream から Block を得るための ISerializer。</param>
        public BlockMeshManager(BlockMeshFactory meshFactory, ISerializer<Block> serializer)
        {
            if (meshFactory == null) throw new ArgumentNullException("meshFactory");
            this.meshFactory = meshFactory;

            this.serializer = serializer ?? defaultSerializer;
        }

        /// <summary>
        /// BlockMesh をロードします。
        /// </summary>
        /// <param name="resource">Block を提供する Stream。</param>
        /// <returns>ロードされた BlockMesh。</returns>
        public BlockMesh Load(Stream stream)
        {
            var block = serializer.Deserialize(stream);
            var mesh = meshFactory.CreateBlockMesh(block);
            meshes.Add(mesh);
            return mesh;
        }

        /// <summary>
        /// BlockMesh をアンロードします。
        /// </summary>
        /// <param name="mesh">アンロードする BlockMesh。</param>
        public void Unload(BlockMesh mesh)
        {
            if (!meshes.Remove(mesh)) throw new InvalidOperationException("BlockMesh is managed by another BlockMeshManager.");
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
