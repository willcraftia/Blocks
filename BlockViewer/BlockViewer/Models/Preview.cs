#region Using

using System;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Blocks.Content;
using Willcraftia.Xna.Blocks.Storage;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Models
{
    public sealed class Preview : IDisposable
    {
        Workspace workspace;

        BlockMeshFactory blockMeshFactory;

        BlockMeshManager blockMeshManager;

        StorageBlockMeshLoader storageBlockMeshLoader;

        AsyncBlockMeshLoader asyncBlockMeshLoader;

        public GraphicsDevice GraphicsDevice { get; private set; }

        public StorageModel StorageModel { get; private set; }

        public Preview(Workspace workspace)
        {
            if (workspace == null) throw new ArgumentNullException("workspace");
            this.workspace = workspace;

            GraphicsDevice = workspace.GraphicsDevice;

            StorageModel = workspace.StorageModel;
            StorageModel.ContainerChanged += new EventHandler(OnStorageModelContainerChanged);

            blockMeshFactory = new BlockMeshFactory(GraphicsDevice, new BasicBlockEffectFactory(GraphicsDevice), 1);
            blockMeshManager = new BlockMeshManager(blockMeshFactory);
        }

        public Viewer CreateViewer()
        {
            return new Viewer(workspace, asyncBlockMeshLoader);
        }

        void OnStorageModelContainerChanged(object sender, EventArgs e)
        {
            storageBlockMeshLoader = new StorageBlockMeshLoader(blockMeshManager, StorageModel.Container);

            if (asyncBlockMeshLoader != null) asyncBlockMeshLoader.Stop();
            asyncBlockMeshLoader = new AsyncBlockMeshLoader(storageBlockMeshLoader, 1);
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

            // AsyncBlockMeshLoader の Thread を終了させます。
            // Dipose() とデストラクタのいずれからでも終了させます。
            if (asyncBlockMeshLoader != null) asyncBlockMeshLoader.Stop();

            if (disposing)
            {
                blockMeshManager.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
