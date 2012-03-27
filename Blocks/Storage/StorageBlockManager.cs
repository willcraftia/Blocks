#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.Storage;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Storage
{
    /// <summary>
    /// IStorageBlockService の実装クラスです。
    /// </summary>
    public sealed class StorageBlockManager : IStorageBlockService
    {
        /// <summary>
        /// Block の ContentSerializer。
        /// </summary>
        ContentSerializer<Block> blockSerializer = new ContentSerializer<Block>();

        /// <summary>
        /// Description の ContentSerializer。
        /// </summary>
        ContentSerializer<Description> descriptionSerializer = new ContentSerializer<Description>();

        /// <summary>
        /// Block ディレクトリ名。
        /// </summary>
        const string directoryName = "Blocks";

        /// <summary>
        /// IStorageService。
        /// </summary>
        IStorageService storageService;

        /// <summary>
        /// BlockMeshes ディレクトリ。
        /// </summary>
        StorageDirectory directory;

        /// <summary>
        /// インスタンスを生成し、Game のサービスとして登録します。
        /// インスタンスの生成前に、Game のサービスとして IStorageService
        /// が登録されている必要があります。
        /// </summary>
        /// <param name="game">Game。</param>
        public StorageBlockManager(Game game)
        {
            // Game のサービスとして登録します。
            game.Services.AddService(typeof(IStorageBlockService), this);

            // IStorageService を取得します。
            storageService = game.Services.GetRequiredService<IStorageService>();
            storageService.ContainerSelected += OnStorageServiceContainerSelected;
            
            // 既に StorageContainer が選択済みの場合はディレクトリをロードします。
            if (storageService.RootDirectory != null) LoadDirectory();
        }

        // I/F
        public Block LoadBlock(string name)
        {
            EnsureDirectory();

            var blockFileName = Block.ResolveFileName(name);
            using (var stream = directory.OpenFile(blockFileName, FileMode.Open))
            {
                return blockSerializer.Deserialize(stream);
            }
        }

        // I/F
        public void Save(string name, Block block, Description description)
        {
            EnsureDirectory();

            var blockFileName = Block.ResolveFileName(name);
            var descriptionFileName = Description.ResolveFileName(name);

            using (var stream = directory.CreateFile(blockFileName))
            {
                blockSerializer.Serialize(stream, block);
            }

            using (var stream = directory.CreateFile(descriptionFileName))
            {
                descriptionSerializer.Serialize(stream, description);
            }
        }

        // I/F
        public List<string> GetBlockNames()
        {
            var blockNames = new List<string>();
            foreach (var fileName in directory.EnumerateFileNames())
            {
                if (fileName.EndsWith(Block.Extension))
                {
                    var name = fileName.Substring(0, fileName.LastIndexOf(Block.Extension));
                    blockNames.Add(name);
                }
            }
            return blockNames;
        }

        void OnStorageServiceContainerSelected(object sender, EventArgs e)
        {
            // ディレクトリをロードします。
            LoadDirectory();
        }

        /// <summary>
        /// BlockMeshes ディレクトリをロードします。
        /// </summary>
        void LoadDirectory()
        {
            var rootDirectory = storageService.RootDirectory;
            if (rootDirectory.DirectoryExists(directoryName))
            {
                directory = rootDirectory.GetDirectory(directoryName);
            }
            else
            {
                directory = rootDirectory.CreateDirectory(directoryName);
            }
        }

        void EnsureDirectory()
        {
            if (directory == null)
                throw new InvalidOperationException("BlockMeshes directory is unknown.");
        }
    }
}
