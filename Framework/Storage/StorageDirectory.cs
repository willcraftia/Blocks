#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using Microsoft.Xna.Framework.Storage;

#endregion

namespace Willcraftia.Xna.Framework.Storage
{
    public sealed class StorageDirectory
    {
        static readonly XmlSerializer indexSerializer = new XmlSerializer(typeof(StorageDirectoryIndex));

        const string indexFileName = "index.xml";

        StorageContainer container;

        StorageDirectoryIndex index;

        string indexFilePath;

        public string Name { get; private set; }

        public string Path { get; private set; }

        StorageDirectory(StorageContainer container, string name, string path)
        {
            this.container = container;
            Name = name;
            Path = path;
        }

        internal static StorageDirectory GetRootDirectory(StorageContainer container)
        {
            if (container == null) throw new ArgumentNullException("container");

            // ルート ディレクトリのための StorageDirectory を初期化します。
            var directory = new StorageDirectory(container, null, null);
            if (container.FileExists(indexFileName))
            {
                directory.LoadIndexFile();
            }
            else
            {
                directory.CreateIndexFile();
            }
            return directory;
        }

        public IEnumerable<string> EnumerateDirectorieNames()
        {
            return index.DirectoryNames;
        }

        public IEnumerable<string> EnumerateFileNames()
        {
            return index.FileNames;
        }

        public bool DirectoryExists(string name)
        {
            return index.DirectoryNames.Contains(name);
        }

        public bool FileExists(string name)
        {
            return index.FileNames.Contains(name);
        }

        public StorageDirectory CreateDirectory(string name)
        {
            if (DirectoryExists(name))
                throw new InvalidOperationException(string.Format("The directory '{0}' already exists.", name));

            // サブ ディレクトリを作成します。
            var path = ResolveChildPath(name);
            container.CreateDirectory(path);

            // サブ ディレクトリの StorageDirectory を作成します。
            var directory = new StorageDirectory(container, name, path);
            directory.CreateIndexFile();

            // Index にサブ ディレクトリの情報を追加して保存します。
            index.DirectoryNames.Add(name);
            SaveIndexFile();

            return directory;
        }

        public StorageDirectory GetDirectory(string name)
        {
            if (!DirectoryExists(name))
                throw new InvalidOperationException(string.Format("The directory '{0}' does not exists.", name));

            var path = ResolveChildPath(name);
            var directory = new StorageDirectory(container, name, path);
            directory.LoadIndexFile();

            return directory;
        }

        public void DeleteDirectory(string name)
        {
            var path = ResolveChildPath(name);
            container.DeleteDirectory(path);

            index.DirectoryNames.Remove(name);
            SaveIndexFile();
        }

        public Stream CreateFile(string name)
        {
            return OpenFile(name, FileMode.OpenOrCreate);
        }

        public Stream OpenFile(string name, FileMode fileMode)
        {
            // どの FileMode であれ、Index に追加して保存します。
            if (!index.FileNames.Contains(name))
            {
                index.FileNames.Add(name);
                SaveIndexFile();
            }

            var path = ResolveChildPath(name);
            return container.OpenFile(path, fileMode);
        }

        public void DeleteFile(string name)
        {
            var path = ResolveChildPath(name);
            container.DeleteFile(path);

            index.FileNames.Remove(name);
            SaveIndexFile();
        }

        void CreateIndexFile()
        {
            indexFilePath = ResolveChildPath(indexFileName);
            index = new StorageDirectoryIndex();
            using (var stream = container.CreateFile(indexFilePath))
            {
                indexSerializer.Serialize(stream, index);
            }
        }

        void LoadIndexFile()
        {
            indexFilePath = ResolveChildPath(indexFileName);
            if (!container.FileExists(indexFilePath))
                throw new InvalidOperationException(string.Format("The file '{0}' does not exists.", indexFilePath));

            using (var stream = container.OpenFile(indexFilePath, FileMode.Open))
            {
                index = indexSerializer.Deserialize(stream) as StorageDirectoryIndex;
            }

            // デシリアライズでは要素が空の場合にインスタンスを生成しないため、
            // ここで生成して設定します。
            if (index.DirectoryNames == null) index.DirectoryNames = new List<string>();
            if (index.FileNames == null) index.FileNames = new List<string>();
        }

        void SaveIndexFile()
        {
            container.DeleteFile(indexFilePath);

            using (var stream = container.CreateFile(indexFilePath))
            {
                indexSerializer.Serialize(stream, index);
            }
        }

        string ResolveChildPath(string name)
        {
            return string.IsNullOrEmpty(Path) ? name : Path + "/" + name;
        }
    }
}
