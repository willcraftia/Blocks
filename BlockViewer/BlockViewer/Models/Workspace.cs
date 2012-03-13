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

        BlockMeshManager blockMeshManager;

        BlockMeshLoaderProxy blockMeshLoaderProxy = new BlockMeshLoaderProxy();

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
            StorageModel.ContainerChanged += new EventHandler(OnStorageModelContainerChanged);
            if (StorageModel.Selected) InitializeBlockMeshLoader();

            // BlockMeshManager は StorageContainer の選択状態により構築・再構築されるため、
            // Proxy を Viewer に設定します。
            Viewer = new Viewer(this, blockMeshLoaderProxy);

            Preview = new Preview(this);

            GridBlockMesh = new GridBlockMesh(GraphicsDevice, 16, 0.1f, Color.White);
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

        ~Workspace()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                if (blockMeshManager != null) blockMeshManager.Dispose();
                Preview.Dispose();
                GridBlockMesh.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
