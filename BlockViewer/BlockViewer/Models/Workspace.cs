#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Blocks.Content;
using Willcraftia.Xna.Blocks.Graphics;
using Willcraftia.Xna.Blocks.Storage;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Models
{
    public sealed class Workspace : IDisposable
    {
        InterBlockMeshLoadQueue interBlockMeshLoadQueue;

        BlockMeshLoadQueue blockMeshLoadQueue;

        public Game Game { get; private set; }

        public GraphicsDevice GraphicsDevice { get; private set; }

        public IStorageBlockService StorageBlockService { get; private set; }

        public Viewer Viewer { get; private set; }

        public Preview Preview { get; private set; }

        public GridBlockMesh GridBlockMesh { get; private set; }

        public BasicBlockEffect BasicBlockEffect { get; private set; }

        public Workspace(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");
            Game = game;

            GraphicsDevice = game.GraphicsDevice;

            interBlockMeshLoadQueue = new InterBlockMeshLoadQueue();
            blockMeshLoadQueue = new BlockMeshLoadQueue(100);

            StorageBlockService = game.Services.GetRequiredService<IStorageBlockService>();

            Viewer = new Viewer(this);

            Preview = new Preview(this);

            GridBlockMesh = new GridBlockMesh(GraphicsDevice, 16, 0.1f, Color.White);

            BasicBlockEffect = new BasicBlockEffect(GraphicsDevice);
            BasicBlockEffect.EnableDefaultLighting();
        }

        public void Update(GameTime gameTime)
        {
            interBlockMeshLoadQueue.Update();
            blockMeshLoadQueue.Update(gameTime);
        }

        public void LoadInterBlockMeshAsync(string name, int lodCount, InterBlockMeshLoadQueueCallback callback)
        {
            interBlockMeshLoadQueue.Load(StorageBlockService, name, lodCount, callback);
        }

        public void CancelLoadInterBlockMeshAsync(string name)
        {
            interBlockMeshLoadQueue.Cancel(name);
        }

        public BlockMesh LoadBlockMesh(InterBlockMesh interBlockMesh)
        {
            return blockMeshLoadQueue.Load(GraphicsDevice, interBlockMesh);
        }

        public void CancelLoadBlockMesh(BlockMesh blockMesh)
        {
            blockMeshLoadQueue.Cancel(blockMesh);
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool disposed;

        ~Workspace()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                Viewer.Dispose();
                Preview.Dispose();
                GridBlockMesh.Dispose();
                BasicBlockEffect.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
