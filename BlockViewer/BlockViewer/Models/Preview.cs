#region Using

using System;
using System.Collections.Generic;
using Willcraftia.Xna.Blocks.Storage;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Models
{
    public sealed class Preview : IDisposable
    {
        List<Viewer> viewers = new List<Viewer>();

        public Workspace Workspace { get; private set; }

        public Preview(Workspace workspace)
        {
            if (workspace == null) throw new ArgumentNullException("workspace");
            Workspace = workspace;
        }

        public Viewer CreateViewer()
        {
            var viewer = new Viewer(Workspace);
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
