#region Using

using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Json;
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
        /// <summary>
        /// ロードした BlockMesh のリスト。
        /// </summary>
        List<BlockMesh> meshes = new List<BlockMesh>();

        /// <summary>
        /// JSON から Block を得るための DataContractJsonSerializer。
        /// </summary>
        DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(Block));

        /// <summary>
        /// BlockMeshFactory。
        /// </summary>
        BlockMeshFactory meshFactory;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="meshFactory">BlockMesh の生成を委譲する BlockMeshFactory。</param>
        public BlockMeshManager(BlockMeshFactory meshFactory)
        {
            if (meshFactory == null) throw new ArgumentNullException("meshFactory");
            this.meshFactory = meshFactory;
        }

        /// <summary>
        /// BlockMesh をロードします。
        /// </summary>
        /// <param name="resource">Block の JSON を提供する Stream。</param>
        /// <returns>ロードされた BlockMesh。</returns>
        public BlockMesh Load(Stream stream)
        {
            var block = serializer.ReadObject(stream) as Block;
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
