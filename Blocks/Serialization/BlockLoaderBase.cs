#region Using

using System;
using System.IO;
using Willcraftia.Xna.Framework.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    public abstract class BlockLoaderBase : IBlockLoader
    {
        static readonly XmlSerializer<Block> defaultSerializer = new XmlSerializer<Block>();

        protected ISerializer<Block> Serializer { get; private set; }

        protected BlockLoaderBase(ISerializer<Block> serializer)
        {
            Serializer = serializer ?? defaultSerializer;
        }

        public Block LoadBlock(string name)
        {
            using (var stream = OpenStream(name))
            {
                return Serializer.Deserialize(stream);
            }
        }

        protected abstract Stream OpenStream(string name);
    }
}
