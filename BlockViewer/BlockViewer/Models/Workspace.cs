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

        StorageBlockMeshLoader storageBlockMeshLoader;

        AsyncBlockMeshLoader asyncBlockMeshLoader;

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

            StorageModel = (game as BlockViewerGame).StorageModel;
            StorageModel.ContainerChanged += new EventHandler(OnStorageModelContainerChanged);

            blockMeshFactory = new BlockMeshFactory(GraphicsDevice, new BasicBlockEffectFactory(GraphicsDevice), 4);
            blockMeshManager = new BlockMeshManager(blockMeshFactory);

            if (StorageModel.Selected) InitializeBlockMeshLoaders();

            Viewer = new Viewer(this, asyncBlockMeshLoader);

            Preview = new Preview(this);

            GridBlockMesh = new GridBlockMesh(GraphicsDevice, 16, 0.1f, Color.White);
        }

        void OnStorageModelContainerChanged(object sender, EventArgs e)
        {
            InitializeBlockMeshLoaders();
        }

        void InitializeBlockMeshLoaders()
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

        ~Workspace()
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
                Preview.Dispose();
                GridBlockMesh.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
