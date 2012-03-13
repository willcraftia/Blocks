#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Willcraftia.Xna.Blocks.Content;
using Willcraftia.Xna.Blocks.Graphics;
using Willcraftia.Xna.Blocks.Storage;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Models
{
    public sealed class Workspace : IDisposable
    {
        BlockMeshFactory blockMeshFactory;

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

            blockMeshFactory = new BlockMeshFactory(GraphicsDevice, new BasicBlockEffectFactory(GraphicsDevice), 4);

            StorageModel = (game as BlockViewerGame).StorageModel;

            Viewer = new Viewer(this, blockMeshFactory);

            Preview = new Preview(this);

            GridBlockMesh = new GridBlockMesh(GraphicsDevice, 16, 0.1f, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            Viewer.Update(gameTime);
            Preview.Update(gameTime);
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
