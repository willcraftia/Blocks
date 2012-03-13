#region Using

using System;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    public interface IBlockLoader
    {
        Block LoadBlock(string name);
    }
}
