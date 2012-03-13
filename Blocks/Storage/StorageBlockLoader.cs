#region Using

using System;
using System.IO;
using Microsoft.Xna.Framework.Storage;
using Willcraftia.Xna.Framework.Serialization;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Storage
{
    public sealed class StorageBlockLoader : BlockLoaderBase
    {
        StorageContainer storageContainer;

        public StorageBlockLoader(StorageContainer storageContainer, ISerializer<Block> serializer)
            : base(serializer)
        {
            if (storageContainer == null) throw new ArgumentNullException("storageContainer");
            this.storageContainer = storageContainer;
        }

        protected override Stream OpenStream(string name)
        {
            return storageContainer.OpenFile(name, FileMode.Open);
        }
    }
}
