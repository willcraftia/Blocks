#region Using

using System;
using System.Collections.ObjectModel;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    /// <summary>
    /// Level of Detail ごとの BlockMeshPart を管理するクラスです。
    /// </summary>
    public sealed class BlockMeshLod : IDisposable
    {
        /// <summary>
        /// 全ての BlockMeshPart のロードが完了した時に発生します。
        /// </summary>
        public event EventHandler Loaded = delegate { };

        ReadOnlyCollection<BlockMeshPart> meshParts;

        public int LevelOfDetail { get; private set; }

        public bool IsLoaded { get; private set; }

        public ReadOnlyCollection<BlockMeshPart> MeshParts
        {
            get { return meshParts; }
        }

        internal BlockMeshLod(int levelOfDetail)
        {
            LevelOfDetail = levelOfDetail;
        }

        internal void AllocateMeshParts(GraphicsDevice graphicsDevice, int count)
        {
            var array = new BlockMeshPart[count];
            for (int i = 0; i < count; i++)
            {
                array[i] = new BlockMeshPart(graphicsDevice);
                array[i].Loaded += OnMeshPartLoaded;
            }
            
            meshParts = new ReadOnlyCollection<BlockMeshPart>(array);
        }

        void OnMeshPartLoaded(object sender, EventArgs e)
        {
            foreach (var meshPart in meshParts)
            {
                if (!meshPart.IsLoaded) return;
            }

            IsLoaded = true;
            Loaded(this, EventArgs.Empty);
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool disposed;

        ~BlockMeshLod()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                foreach (var meshPart in meshParts) meshPart.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
