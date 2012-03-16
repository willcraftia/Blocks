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
        InterBlockMeshFactory interBlockMeshFactory;

        BlockMeshFactory blockMeshFactory;

        InterBlockMeshLoadQueue interBlockMeshLoadQueue;

        public Game Game { get; private set; }

        public GraphicsDevice GraphicsDevice { get; private set; }

        public StorageModel StorageModel { get; private set; }

        public Viewer Viewer { get; private set; }

        public Preview Preview { get; private set; }

        public GridBlockMesh GridBlockMesh { get; private set; }

        public Workspace(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");
            Game = game;

            GraphicsDevice = game.GraphicsDevice;

            interBlockMeshFactory = new InterBlockMeshFactory(4);
            blockMeshFactory = new BlockMeshFactory(GraphicsDevice, new BasicBlockEffectFactory(GraphicsDevice));
            interBlockMeshLoadQueue = new InterBlockMeshLoadQueue();

            StorageModel = (game as BlockViewerGame).StorageModel;

            Viewer = new Viewer(this, interBlockMeshFactory);

            Preview = new Preview(this);

            GridBlockMesh = new GridBlockMesh(GraphicsDevice, 16, 0.1f, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            interBlockMeshLoadQueue.Update();
        }

        public void LoadInterBlockMeshAsync(InterBlockMeshFactory factory, string name, InterBlockMeshLoadQueueCallback callback)
        {
            if (StorageModel.BlockLoader == null)
                throw new InvalidOperationException("No block loader exists.");

            //asyncInterBlockMeshLoader.Load(StorageModel.BlockLoader, factory, name, callback);
            interBlockMeshLoadQueue.Load(StorageModel.BlockLoader, factory, name, callback);
        }

        public void CancelLoadInterBlockMeshAsync(string name)
        {
            var result = interBlockMeshLoadQueue.Cancel(name);
            Console.WriteLine("Cancel result: " + result);
        }

        public void LoadBlockMeshAsync(string name, InterBlockMesh interBlockMesh, AsyncBlockMeshLoadCallback callback)
        {
            var service = Game.Services.GetRequiredService<IAsyncBlockMeshLoadService>();
            service.Load(blockMeshFactory, name, interBlockMesh, callback);
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
            }

            disposed = true;
        }

        #endregion
    }
}
