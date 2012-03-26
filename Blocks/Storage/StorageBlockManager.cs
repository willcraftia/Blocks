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
        /// Block の XmlSerializer。
        /// </summary>
        XmlSerializer blockSerializer = new XmlSerializer(typeof(Block));

        /// <summary>
        /// BlockMeshes ディレクトリ名。
        /// </summary>
        const string directoryName = "BlockMeshes";

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

            using (var stream = directory.OpenFile(name, FileMode.Open))
            {
                return blockSerializer.Deserialize(stream) as Block;
            }
        }

        // I/F
        public void SaveBlock(string name, Block block)
        {
            EnsureDirectory();

            using (var stream = directory.CreateFile(name))
            {
                blockSerializer.Serialize(stream, block);
            }
        }

        // I/F
        public void SaveBlock(string name, Stream stream)
        {
            EnsureDirectory();

            using (var fileStream = directory.CreateFile(name))
            {
                stream.CopyTo(fileStream);
            }
        }

        // I/F
        public IEnumerable<string> EnumerateFileNames()
        {
            return directory.EnumerateFileNames();
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
