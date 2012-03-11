#region Using

using System;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    public struct BlockMeshLoadRequest : IEquatable<BlockMeshLoadRequest>
    {
        public string Name;

        public AsyncBlockMeshLoaderCallback Callback;

        #region Equatable

        public static bool operator ==(BlockMeshLoadRequest r1, BlockMeshLoadRequest r2)
        {
            return r1.Equals(r2);
        }

        public static bool operator !=(BlockMeshLoadRequest r1, BlockMeshLoadRequest r2)
        {
            return !r1.Equals(r2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return Equals((BlockMeshLoadRequest) obj);
        }

        public bool Equals(BlockMeshLoadRequest other)
        {
            return Name == other.Name && Callback == other.Callback;
        }

        public override int GetHashCode()
        {
            return (Name != null) ? Name.GetHashCode() : string.Empty.GetHashCode();
        }

        #endregion
    }
}
