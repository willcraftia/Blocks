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
    // todo
    public sealed class AsyncMethodResult
    {
        public bool Succeeded { get; set; }

        public Exception Exception { get; set; }
    }

    public sealed class StartMenuViewModel
    {
        public event EventHandler RestoreBoxSessionAsyncCompleted = delegate { };

        public event EventHandler UploadDemoContentsAsyncCompleted = delegate { };

        delegate bool RestoreBoxSessionDelegate();

        delegate void UploadDemoContentsDelegate();

        Game game;

        IStorageBlockService storageBlockService;

        BoxIntegration boxIntegration;

        ContentSerializer<Block> blockSerializer = new ContentSerializer<Block>();

        ContentSerializer<Description> descriptionSerializer = new ContentSerializer<Description>();

        RestoreBoxSessionDelegate restoreBoxSessionDelegate;

        UploadDemoContentsDelegate uploadDemoContentsDelegate;

        readonly object syncRoot = new object();

        bool fireRestoreBoxSessionAsyncCompleted;

        bool fireUploadDemoContentsAsyncCompleted;

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

        public AsyncMethodResult RestoreBoxSessionAsyncResult { get; private set; }

        public AsyncMethodResult UploadDemoContentsAsyncResult { get; private set; }

        public StartMenuViewModel(Game game)
        {
            this.game = game;

            storageBlockService = game.Services.GetRequiredService<IStorageBlockService>();
            boxIntegration = (game as BlockViewerGame).BoxIntegration;
            RestoreBoxSessionAsyncResult = new AsyncMethodResult();
            UploadDemoContentsAsyncResult = new AsyncMethodResult();
        }

        public void Update()
        {
            lock (syncRoot)
            {
                if (fireRestoreBoxSessionAsyncCompleted)
                {
                    RestoreBoxSessionAsyncCompleted(this, EventArgs.Empty);
                    fireRestoreBoxSessionAsyncCompleted = false;
                }
                if (fireUploadDemoContentsAsyncCompleted)
                {
                    UploadDemoContentsAsyncCompleted(this, EventArgs.Empty);
                    fireUploadDemoContentsAsyncCompleted = false;
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

        public void RestoreBoxSettingsAsync()
        {
            if (restoreBoxSessionDelegate == null)
                restoreBoxSessionDelegate = new RestoreBoxSessionDelegate(boxIntegration.RestoreSession);
            restoreBoxSessionDelegate.BeginInvoke(RestoreBoxSessionCallback, null);
        }

        public void UploadDemoContentsAsync()
        {
            if (uploadDemoContentsDelegate == null)
                uploadDemoContentsDelegate = new UploadDemoContentsDelegate(UploadDemoContents);
            uploadDemoContentsDelegate.BeginInvoke(UploadDemoContentsAsyncCallback, null);
        }

        void RestoreBoxSessionCallback(IAsyncResult asyncResult)
        {
            bool succeeded = true;
            Exception exception = null;
            try
            {
                restoreBoxSessionDelegate.EndInvoke(asyncResult);
            }
            catch (Exception e)
            {
                succeeded = false;
                exception = e;
            }

            lock (syncRoot)
            {
                RestoreBoxSessionAsyncResult.Succeeded = succeeded;
                RestoreBoxSessionAsyncResult.Exception = exception;
                fireRestoreBoxSessionAsyncCompleted = true;
            }
        }

        void UploadDemoContentsAsyncCallback(IAsyncResult asyncResult)
        {
            bool succeeded = true;
            Exception exception = null;
            try
            {
                uploadDemoContentsDelegate.EndInvoke(asyncResult);
            }
            catch (Exception e)
            {
                succeeded = false;
                exception = e;
            }

            lock (syncRoot)
            {
                UploadDemoContentsAsyncResult.Succeeded = succeeded;
                UploadDemoContentsAsyncResult.Exception = exception;
                fireUploadDemoContentsAsyncCompleted = true;
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
