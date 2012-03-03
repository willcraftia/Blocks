#region Using

using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.Serialization;
using Willcraftia.Xna.Blocks.Content;
using Willcraftia.Xna.Blocks.Graphics;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels
{
    public sealed class MainViewModel
    {
        /// <summary>
        /// BlockMeshManager。
        /// </summary>
        BlockMeshManager meshManager;

        /// <summary>
        /// LOD サイズ。
        /// </summary>
        int lodSize = 4;

        public GraphicsDevice GraphicsDevice { get; private set; }

        public StorageContainer StorageContainer { get; private set; }

        public BlockMesh BlockMesh { get; set; }

        public GridBlockMesh GridBlockMesh { get; private set; }

        // MEMO
        //
        // BasicEffect.EnableDefaultLighting() が呼び出されると Normal0 が要求されるため、
        // 念のため GridBlockMesh 専用の BasicEffect を使用します。
        //

        public BasicEffect GridBlockMeshEffect { get; private set; }

        public BlockMeshViewModel BlockMeshViewModel { get; private set; }

        public OpenStorageViewModel OpenStorageViewModel { get; private set; }

        public MainViewModel(GraphicsDevice graphicsDevice)
        {
            GraphicsDevice = graphicsDevice;

            GridBlockMesh = new GridBlockMesh(graphicsDevice, 16, 0.1f, Color.White);
            GridBlockMeshEffect = new BasicEffect(GraphicsDevice);
            GridBlockMeshEffect.VertexColorEnabled = true;

            InitializeStorageContainer();

            BlockMeshViewModel = new BlockMeshViewModel(this)
            {
                CameraMovable = true,
                GridVisible = true
            };
            OpenStorageViewModel = new OpenStorageViewModel(StorageContainer);
        }

        public void LoadBlockMeshFromStorage()
        {
            var fileName = OpenStorageViewModel.SelectedFileName;
            if (string.IsNullOrEmpty(fileName)) throw new InvalidOperationException("A file is not selected.");

            using (var stream = StorageContainer.OpenFile(fileName, FileMode.Open))
            {
                LoadBlockMesh(stream);
            }
        }

        // TODO: テスト用
        public void StoreSampleBlockMesh()
        {
            // テスト用にメモリ上で Block の JSON データを作ります。
            var block = CreateOctahedronLikeBlock();
            var blockJson = JsonHelper.ToJson<Block>(block);

            var meshFactory = new BlockMeshFactory(GraphicsDevice, new BasicBlockEffectFactory(GraphicsDevice), lodSize);
            meshManager = new BlockMeshManager(meshFactory);

            // BlockMesh をロードします。
            using (var stream = blockJson.ToMemoryStream())
            {
                LoadBlockMesh(stream);
            }
        }

        void InitializeStorageContainer()
        {
            var showSelectorResult = StorageDevice.BeginShowSelector(null, null);
            showSelectorResult.AsyncWaitHandle.WaitOne();
            var storageDevice = StorageDevice.EndShowSelector(showSelectorResult);
            showSelectorResult.AsyncWaitHandle.Close();

            var openContainerResult = storageDevice.BeginOpenContainer("BlockData", null, null);
            openContainerResult.AsyncWaitHandle.WaitOne();
            StorageContainer = storageDevice.EndOpenContainer(openContainerResult);
            openContainerResult.AsyncWaitHandle.Close();
        }

        void LoadBlockMesh(Stream stream)
        {
            BlockMesh = meshManager.Load(stream);
            foreach (var effect in BlockMesh.Effects) effect.EnableDefaultLighting();
        }

        /// <summary>
        /// 正八面体風のデータを定義する Block を作成します。
        /// </summary>
        /// <returns>作成された Block。</returns>
        Block CreateOctahedronLikeBlock()
        {
            var block = new Block();
            block.Materials = new List<Material>();
            block.Elements = new List<Element>();

            MaterialColor[] diffuses =
            {
                new MaterialColor(255, 255, 255),
                new MaterialColor(255,   0,   0),
                new MaterialColor(  0, 255,   0),
                new MaterialColor(  0,   0, 255),
                new MaterialColor(127, 127,   0),
                new MaterialColor(127,   0, 127),
                new MaterialColor(  0, 127, 127),
                new MaterialColor(  0,   0,   0),
            };
            Material[] materials = new Material[8];
            for (int i = 0; i < 8; i++)
            {
                materials[i] = new Material();
                materials[i].DiffuseColor = diffuses[i];
                block.Materials.Add(materials[i]);
            }

            int materialIndex;
            for (int x = -8; x < 8; x++)
            {
                for (int y = -8; y < 8; y++)
                {
                    for (int z = -8; z < 8; z++)
                    {
                        int testX = (x < 0) ? -x : x + 1;
                        int testY = (y < 0) ? -y : y + 1;
                        int testZ = (z < 0) ? -z : z + 1;

                        if (testX + testY + testZ <= 10)
                        {
                            materialIndex = 0;
                            if (x < 0) materialIndex |= 1;
                            if (y < 0) materialIndex |= 2;
                            if (z < 0) materialIndex |= 4;

                            var element = new Element();
                            element.Position = new Position(x, y, z);
                            element.MaterialIndex = materialIndex;
                            block.Elements.Add(element);
                        }
                    }
                }
            }

            return block;
        }
    }
}
