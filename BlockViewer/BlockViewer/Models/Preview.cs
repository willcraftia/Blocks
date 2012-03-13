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
        }

        public Viewer CreateViewer()
        {
            return new Viewer(workspace, asyncBlockMeshLoader);
        }

        void OnStorageModelContainerChanged(object sender, EventArgs e)
        {
            InitializeBlockMeshLoader();
        }

        void InitializeBlockMeshLoader()
        {
            var blockMeshLoader = new DefaultBlockMeshLoader(StorageModel.BlockLoader, blockMeshFactory);

            if (blockMeshManager != null) blockMeshManager.Dispose();
            blockMeshManager = new BlockMeshManager(blockMeshLoader);

            if (asyncBlockMeshLoader != null) asyncBlockMeshLoader.Stop();
            asyncBlockMeshLoader = new AsyncBlockMeshLoader(blockMeshManager, 50);
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
                if (blockMeshManager != null) blockMeshManager.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
