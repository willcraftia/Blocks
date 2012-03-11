#region Using

using System;
using System.IO;
using Microsoft.Xna.Framework.Storage;
using Willcraftia.Xna.Blocks.Content;
using Willcraftia.Xna.Blocks.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Storage
{
    /// <summary>
    /// StorageContainer から BlockMesh をロードする IBlockMeshLoader の実装です。
    /// </summary>
    public sealed class StorageBlockMeshLoader : IBlockMeshLoader
    {
        BlockMeshManager blockMeshManager;

        public StorageContainer StorageContainer { get; private set; }

        /// <summary>
        /// </summary>
        /// <param name="blockMeshManager"></param>
        /// <param name="storageContainer"></param>
        public StorageBlockMeshLoader(BlockMeshManager blockMeshManager, StorageContainer storageContainer)
        {
            if (blockMeshManager == null) throw new ArgumentNullException("blockMeshManager");
            if (storageContainer == null) throw new ArgumentNullException("storageContainer");
            this.blockMeshManager = blockMeshManager;
            StorageContainer = storageContainer;
        }

        /// <summary>
        /// name パラメータを StorageContainer 内の BlockMesh データのファイル名として、
        /// BlockMesh をロードします。
        /// </summary>
        /// <param name="name">StorageContainer 内の BlockMesh データのファイル名。</param>
        /// <returns></returns>
        public BlockMesh LoadBlockMesh(string name)
        {
            using (var stream = StorageContainer.OpenFile(name, FileMode.Open))
            {
                return blockMeshManager.Load(stream);
            }
        }
    }
}
