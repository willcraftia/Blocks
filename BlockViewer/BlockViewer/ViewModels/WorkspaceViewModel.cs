#region Using

using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.Serialization;
using Willcraftia.Xna.Blocks.Content;
using Willcraftia.Xna.Blocks.Graphics;
using Willcraftia.Xna.Blocks.Serialization;
using Willcraftia.Xna.Blocks.BlockViewer.Models;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels
{
    public sealed class WorkspaceViewModel
    {
        Workspace workspace;

        public ViewerViewModel ViewerViewModel { get; private set; }

        public OpenStorageViewModel OpenStorageViewModel { get; private set; }

        public WorkspaceViewModel(Workspace workspace)
        {
            if (workspace == null) throw new ArgumentNullException("workspace");
            this.workspace = workspace;

            ViewerViewModel = new ViewerViewModel(workspace.Viewer)
            {
                ViewMovable = true,
                GridVisible = true
            };

            OpenStorageViewModel = new OpenStorageViewModel(workspace.StorageModel);
        }

        public ViewerViewModel CreateLodViewerViewModel(int levelOfDetail)
        {
            return new ViewerViewModel(workspace.Viewer)
            {
                LevelOfDetail = levelOfDetail
            };
        }

        // TODO: テスト用
        //public void StoreSampleBlockMesh()
        //{
        //    // テスト用にメモリ上で Block データを作ります。
        //    var block = CreateOctahedronLikeBlock();
        //    var serializer = new XmlSerializer<Block>();
        //    string blockData;
        //    using (var stream = new MemoryStream())
        //    {
        //        serializer.Serialize(stream, block);
        //        blockData = Encoding.ASCII.GetString(stream.ToArray());
        //    }

        //    // BlockMesh をロードします。
        //    using (var stream = blockData.ToMemoryStream())
        //    {
        //        LoadBlockMesh(stream);
        //    }
        //}

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
