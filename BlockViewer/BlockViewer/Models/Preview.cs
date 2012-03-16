#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Blocks.Content;
using Willcraftia.Xna.Blocks.Storage;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Models
{
    public sealed class Preview : IDisposable
    {
        Workspace workspace;

        InterBlockMeshFactory interBlockMeshFactory;

        BlockMeshFactory blockMeshFactory;

        List<Viewer> viewers = new List<Viewer>();

        public GraphicsDevice GraphicsDevice { get; private set; }

        public StorageModel StorageModel { get; private set; }

        public Preview(Workspace workspace)
        {
            if (workspace == null) throw new ArgumentNullException("workspace");
            this.workspace = workspace;

            GraphicsDevice = workspace.GraphicsDevice;

            interBlockMeshFactory = new InterBlockMeshFactory(1);
            blockMeshFactory = new BlockMeshFactory(GraphicsDevice, new BasicBlockEffectFactory(GraphicsDevice), 1);

            StorageModel = workspace.StorageModel;
        }

        public void Update(GameTime gameTime)
        {
            foreach (var viewer in viewers) viewer.Update(gameTime);
        }

        public Viewer CreateViewer()
        {
            var viewer = new Viewer(workspace, interBlockMeshFactory, blockMeshFactory);
            viewers.Add(viewer);
            return viewer;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool disposed;

        ~Preview()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                foreach (var viewer in viewers) viewer.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
