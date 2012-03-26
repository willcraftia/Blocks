#region Using

using System;
using System.Collections.Generic;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Storage
{
    /// <summary>
    /// StorageContainer での Block の管理を担うサービスへのインタフェースです。
    /// </summary>
    public interface IStorageBlockService : IBlockLoader
    {
        void Save(string name, Block block, Description<Block> description);

        List<string> GetBlockNames();
    }
}
