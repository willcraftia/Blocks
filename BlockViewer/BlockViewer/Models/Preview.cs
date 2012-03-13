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

        BlockMeshLoaderProxy blockMeshLoaderProxy = new BlockMeshLoaderProxy();

        public GraphicsDevice GraphicsDevice { get; private set; }

        public StorageModel StorageModel { get; private set; }

        public Preview(Workspace workspace)
        {
            if (workspace == null) throw new ArgumentNullException("workspace");
            this.workspace = workspace;

            GraphicsDevice = workspace.GraphicsDevice;

            blockMeshFactory = new BlockMeshFactory(GraphicsDevice, new BasicBlockEffectFactory(GraphicsDevice), 1);

            StorageModel = workspace.StorageModel;
            StorageModel.ContainerChanged += new EventHandler(OnStorageModelContainerChanged);
            if (StorageModel.Selected) InitializeBlockMeshLoader();
        }

        public Viewer CreateViewer()
        {
            // BlockMeshManager は StorageContainer の選択状態により構築・再構築されるため、
            // Proxy を Viewer に設定します。
            return new Viewer(workspace, blockMeshLoaderProxy);
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

            // Proxy の実体を更新します。
            blockMeshLoaderProxy.Subject = blockMeshManager;
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
                if (blockMeshManager != null) blockMeshManager.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
