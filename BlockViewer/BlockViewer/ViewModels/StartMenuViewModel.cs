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
        delegate void AsyncDelegate();

        Game game;

        IStorageBlockService storageBlockService;

        BoxIntegration boxIntegration;

        ContentSerializer<Block> blockSerializer = new ContentSerializer<Block>();

        ContentSerializer<Description> descriptionSerializer = new ContentSerializer<Description>();

        AsyncDelegate restoreSessionDelegate;

        AsyncDelegate uploadDemoContentsDelegate;

        public bool BoxIntegrationEnabled
        {
            get { return boxIntegration != null; }
        }

        public bool BoxSessionEnabled
        {
            get { return boxIntegration.BoxSessionEnabled; }
        }

        public bool HasValidFolderTree
        {
            get { return boxIntegration.HasValidFolderTree; }
        }

        public StartMenuViewModel(Game game)
        {
            this.game = game;

            storageBlockService = game.Services.GetRequiredService<IStorageBlockService>();
            boxIntegration = (game as BlockViewerGame).BoxIntegration;
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

        public void RestoreSessionAsync(AsyncWebRequestCallback callback)
        {
            if (restoreSessionDelegate == null)
                restoreSessionDelegate = new AsyncDelegate(RestoreSession);
            restoreSessionDelegate.BeginInvoke(RestoreSessionAsyncCallback, callback);
        }

        public void UploadDemoContentsAsync(AsyncWebRequestCallback callback)
        {
            if (uploadDemoContentsDelegate == null)
                uploadDemoContentsDelegate = new AsyncDelegate(UploadDemoContents);
            uploadDemoContentsDelegate.BeginInvoke(UploadDemoContentsAsyncCallback, callback);
        }

        void RestoreSessionAsyncCallback(IAsyncResult asyncResult)
        {
            HandleAsyncDelegate(restoreSessionDelegate, asyncResult);
        }

        void UploadDemoContentsAsyncCallback(IAsyncResult asyncResult)
        {
            HandleAsyncDelegate(uploadDemoContentsDelegate, asyncResult);
        }

        void RestoreSession()
        {
            boxIntegration.RestoreSession();
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

        void HandleAsyncDelegate(AsyncDelegate d, IAsyncResult asyncResult)
        {
            bool succeeded = true;
            Exception exception = null;
            try
            {
                d.EndInvoke(asyncResult);
            }
            catch (Exception e)
            {
                succeeded = false;
                exception = e;
            }

            var callback = asyncResult.AsyncState as AsyncWebRequestCallback;
            callback(succeeded, exception);
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
