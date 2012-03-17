#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Blocks.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    public delegate void BlockMeshLoadQueueItemCallback(string name, BlockMesh result);

    public struct BlockMeshLoadQueueItem : IUpdateQueueItem, IEquatable<BlockMeshLoadQueueItem>
    {
        public BlockMeshFactory BlockMeshFactory;

        public string Name;

        public InterBlockMesh InterBlockMesh;

        public BlockMeshLoadQueueItemCallback Callback;

        public TimeSpan Duration { get; set; }

        public void Update(GameTime gameTime)
        {
            var result = BlockMeshFactory.Create(InterBlockMesh);
            Callback(Name, result);
        }

        #region Equatable

        public static bool operator ==(BlockMeshLoadQueueItem o1, BlockMeshLoadQueueItem o2)
        {
            return o1.Equals(o2);
        }

        public static bool operator !=(BlockMeshLoadQueueItem o1, BlockMeshLoadQueueItem o2)
        {
            return !o1.Equals(o2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return Equals((BlockMeshLoadQueueItem) obj);
        }

        public bool Equals(BlockMeshLoadQueueItem other)
        {
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        #endregion
    }
}
