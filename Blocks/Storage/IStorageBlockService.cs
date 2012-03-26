#region Using

using System;
using System.IO;
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
        void SaveBlock(string name, Block block);

        void SaveBlock(string name, Stream stream);

        IEnumerable<string> EnumerateFileNames();
    }
}
