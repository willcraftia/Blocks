#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Blocks.Serialization;
using Willcraftia.Xna.Blocks.Storage;
using Willcraftia.Xna.Blocks.BlockViewer.Models;
using Willcraftia.Xna.Blocks.BlockViewer.Models.Box;
using Willcraftia.Net.Box;
using Willcraftia.Net.Box.Service;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels
{
    public sealed class StartMenuViewModel
    {
        public event EventHandler UploadedDemoContents = delegate { };

        delegate void UploadDemoContentsDelegate();

        Game game;

        IStorageBlockService storageBlockService;

        ContentSerializer<Block> blockSerializer = new ContentSerializer<Block>();

        ContentSerializer<Description> descriptionSerializer = new ContentSerializer<Description>();

        UploadDemoContentsDelegate uploadDemoContentsDelegate;

        readonly object uploadSyncRoot = new object();

        bool fireUploadedDemoContents;

        public bool BoxServiceEnabled
        {
            get { return game.Services.GetService<IBoxService>() != null; }
        }

        public StartMenuViewModel(Game game)
        {
            this.game = game;

            storageBlockService = game.Services.GetRequiredService<IStorageBlockService>();
        }

        public void Update()
        {
            lock (uploadSyncRoot)
            {
                if (fireUploadedDemoContents)
                {
                    UploadedDemoContents(this, EventArgs.Empty);
                    fireUploadedDemoContents = false;
                }
            }
        }

        public void InstallDemoContents()
        {
            var block = LoadDemoBlock("SimpleBlock");
            var descrption = LoadDemoDescription("SimpleBlock");
            storageBlockService.Save("SimpleBlock", block, descrption);

            block = LoadDemoBlock("OctahedronLikeBlock");
            descrption = LoadDemoDescription("OctahedronLikeBlock");
            for (int i = 0; i < 20; i++)
                storageBlockService.Save(string.Format("Dummy_{0:d2}", i), block, descrption);
        }

        public void UploadDemoContentsAsync()
        {
            if (uploadDemoContentsDelegate == null)
                uploadDemoContentsDelegate = new UploadDemoContentsDelegate(UploadDemoContents);
            uploadDemoContentsDelegate.BeginInvoke(UploadDemoContentsAsyncCallback, null);
        }

        void UploadDemoContentsAsyncCallback(IAsyncResult asyncResult)
        {
            uploadDemoContentsDelegate.EndInvoke(asyncResult);

            lock (uploadSyncRoot)
            {
                fireUploadedDemoContents = true;
            }
        }

        void UploadDemoContents()
        {
            var uploadFiles = new List<UploadFile>();

            var block = LoadDemoBlock("SimpleBlock");
            var descrption = LoadDemoDescription("SimpleBlock");
            uploadFiles.Add(CreateUploadBlock("SimpleBlock", block));
            uploadFiles.Add(CreateUploadDescription("SimpleBlock", descrption));

            block = LoadDemoBlock("OctahedronLikeBlock");
            descrption = LoadDemoDescription("OctahedronLikeBlock");
            for (int i = 0; i < 10; i++)
            {
                var name = string.Format("Dummy_{0:d2}", i);
                uploadFiles.Add(CreateUploadBlock(name, block));
                uploadFiles.Add(CreateUploadDescription(name, descrption));
            }

            var boxIntegration = (game as BlockViewerGame).BoxIntegration;
            boxIntegration.Upload(uploadFiles);
        }

        Block LoadDemoBlock(string name)
        {
            var fileName = Block.ResolveFileName(name);
            var path = Path.Combine("Content/DemoBlocks", fileName);

            using (var stream = TitleContainer.OpenStream(path))
            {
                return blockSerializer.Deserialize(stream);
            }
        }

        Description LoadDemoDescription(string name)
        {
            var fileName = Description.ResolveFileName(name);
            var path = Path.Combine("Content/DemoBlocks", fileName);

            using (var stream = TitleContainer.OpenStream(path))
            {
                return descriptionSerializer.Deserialize(stream);
            }
        }

        UploadFile CreateUploadBlock(string name, Block block)
        {
            using (var stream = new MemoryStream())
            {
                blockSerializer.Serialize(stream, block);

                return new UploadFile
                {
                    ContentType = "text/xml",
                    Name = Block.ResolveFileName(name),
                    Content = Encoding.UTF8.GetString(stream.ToArray())
                };
            }
        }

        UploadFile CreateUploadDescription(string name, Description description)
        {
            using (var stream = new MemoryStream())
            {
                descriptionSerializer.Serialize(stream, description);

                return new UploadFile
                {
                    ContentType = "text/xml",
                    Name = Description.ResolveFileName(name),
                    Content = Encoding.UTF8.GetString(stream.ToArray())
                };
            }
        }
    }
}
