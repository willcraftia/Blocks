#region Using

using System;
using Willcraftia.Xna.Blocks.Graphics;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    public delegate void AsyncBlockMeshLoadCallback(string name, BlockMesh mesh);

    public interface IAsyncBlockMeshLoadService
    {
        void Load(BlockMeshFactory factory, string name, InterBlockMesh interMesh, AsyncBlockMeshLoadCallback callback);
    }
}
