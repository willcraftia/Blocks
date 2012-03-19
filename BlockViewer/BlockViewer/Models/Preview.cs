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

        List<Viewer> viewers = new List<Viewer>();

        public GraphicsDevice GraphicsDevice { get; private set; }

        public StorageModel StorageModel { get; private set; }

        public Preview(Workspace workspace)
        {
            if (workspace == null) throw new ArgumentNullException("workspace");
            this.workspace = workspace;

            GraphicsDevice = workspace.GraphicsDevice;

            StorageModel = workspace.StorageModel;
        }

        public Viewer CreateViewer()
        {
            var viewer = new Viewer(workspace);
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
