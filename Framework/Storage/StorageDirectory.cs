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
    /// <summary>
    /// StorageContainer 内のサブ ディレクトリおよびファイルのリストを管理するクラスです。
    /// 
    /// StorageContainer には、特定のディレクトリについてのサブ ディレクトリや
    /// ファイルのリストを取得する方法がありません。
    /// Blocks では、様々なコンテンツをディレクトリ単位で StorageContainer へ保存するために、
    /// それら操作を簡単にするための StorageDirectory を用いたファイルの管理を利用します。
    /// 
    /// StorageDirectory によるディレクトリ管理は、
    /// 各ディレクトリに index.xml が存在することを仮定します。
    /// また、StorageContainer に対する全ての操作が StorageDirectory
    /// を経由することを仮定します。
    /// 
    /// StorageDirectory のメソッドを用いてサブ ディレクトリおよびファイルを作成すると、
    /// それらの情報は index.xml に書き込まれます。
    /// また、サブ ディレクトリおよびファイルの取得は、
    /// index.xml に保存された情報にのみ基づき行われます。
    /// つまり、StorageDirectory を用いずに StorageContainer を操作した場合、
    /// 実際のディレクトリおよびファイル構造と index.xml との間で不整合が発生します。
    /// 例えば、直接 StorageContainer を用いてファイルを作成した場合、
    /// その情報が index.xml に書き込まれることはありません。
    /// あるいは、Windows ならば、エクスプローラ上で直接ストレージ
    /// ディレクトリへファイルを配置した場合も同様です。
    /// 
    /// ゲーム開発のみを考えた場合、StorageContainer へ保存するファイルとして考えられるものは、
    /// システム設定あるいはゲームのセーブ データのみであり、
    /// このようなクラスによる独自のファイル管理は不要です。
    /// </summary>
    public sealed class StorageDirectory
    {
        /// <summary>
        /// StorageDirectoryIndex の XmlSerializer。
        /// </summary>
        static readonly XmlSerializer indexSerializer = new XmlSerializer(typeof(StorageDirectoryIndex));

        /// <summary>
        /// StorageDirectoryIndex のファイル名。
        /// </summary>
        const string indexFileName = "index.xml";

        /// <summary>
        /// StorageDirectoryIndex に変更が加えられた時に発生します。
        /// </summary>
        public event EventHandler IndexChanged = delegate { };

        /// <summary>
        /// StorageContainer。
        /// </summary>
        StorageContainer container;

        /// <summary>
        /// StorageDirectoryIndex。
        /// </summary>
        StorageDirectoryIndex index;

        /// <summary>
        /// ルート ディレクトリからの StorageDirectoryIndex の相対ファイル パス。
        /// </summary>
        string indexFilePath;

        /// <summary>
        /// ディレクトリ名を取得します。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// ルート ディレクトリからの相対ディレクトリ パスを取得します。
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="container">StorageContainer。</param>
        /// <param name="name">ディレクトリ名。</param>
        /// <param name="path">ルート ディレクトリからの相対ディレクトリ パス。</param>
        StorageDirectory(StorageContainer container, string name, string path)
        {
            this.container = container;
            Name = name;
            Path = path;
        }

        /// <summary>
        /// StorageContainer のルート ディレクトリの StorageDirectory を取得します。
        /// このメソッドは、ルート ディレクトリに StorageDirectoryIndex が存在しない場合、
        /// サブ ディレクトリおよびファイルが空の StorageDirectoryIndex を生成して保存します。
        /// </summary>
        /// <param name="container">StorageContainer。</param>
        /// <returns>ルート ディレクトリの StorageDirectory。</returns>
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

        /// <summary>
        /// サブ ディレクトリ名を列挙します。
        /// </summary>
        /// <returns>サブ ディレクトリ名の列挙。</returns>
        public IEnumerable<string> EnumerateDirectorieNames()
        {
            return index.DirectoryNames;
        }

        /// <summary>
        /// ファイル名を列挙します。
        /// </summary>
        /// <returns>ファイル名の列挙。</returns>
        public IEnumerable<string> EnumerateFileNames()
        {
            return index.FileNames;
        }

        /// <summary>
        /// 指定のサブ ディレクトリが存在するかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">サブ ディレクトリ名。</param>
        /// <returns>
        /// true (指定のサブ ディレクトリが存在する場合)、false (それ以外の場合)。
        /// </returns>
        public bool DirectoryExists(string name)
        {
            return index.DirectoryNames.Contains(name);
        }

        /// <summary>
        /// 指定のファイルが存在するかどうかを示す値を取得します。
        /// </summary>
        /// <param name="name">ファイル名。</param>
        /// <returns>
        /// true (指定のファイルが存在する場合)、false (それ以外の場合)。
        /// </returns>
        public bool FileExists(string name)
        {
            return index.FileNames.Contains(name);
        }

        /// <summary>
        /// サブ ディレクトリを作成します。
        /// 指定の名前のサブ ディレクトリが既に存在する場合は例外が発生します。
        /// </summary>
        /// <param name="name">サブ ディレクトリ名。</param>
        /// <returns>サブ ディレクトリの StorageDirectory。</returns>
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

            // todo
            IndexChanged(this, EventArgs.Empty);

            return directory;
        }

        /// <summary>
        /// サブ ディレクトリの StorageDirectory を取得します。
        /// </summary>
        /// <param name="name">サブ ディレクトリ名。</param>
        /// <returns>サブ ディレクトリの StorageDirectory。</returns>
        public StorageDirectory GetDirectory(string name)
        {
            if (!DirectoryExists(name))
                throw new InvalidOperationException(string.Format("The directory '{0}' does not exists.", name));

            var path = ResolveChildPath(name);
            var directory = new StorageDirectory(container, name, path);
            directory.LoadIndexFile();

            return directory;
        }

        /// <summary>
        /// サブ ディレクトリを削除します。
        /// </summary>
        /// <param name="name">サブ ディレクトリ名。</param>
        public void DeleteDirectory(string name)
        {
            var path = ResolveChildPath(name);
            container.DeleteDirectory(path);

            index.DirectoryNames.Remove(name);
            SaveIndexFile();
        }

        /// <summary>
        /// ファイルを作成します。
        /// </summary>
        /// <param name="name">ファイル名。</param>
        /// <returns>作成したファイルへの Stream。</returns>
        public Stream CreateFile(string name)
        {
            return OpenFile(name, FileMode.OpenOrCreate);
        }

        /// <summary>
        /// ファイルの Stream を開きます。
        /// </summary>
        /// <param name="name">ファイル名。</param>
        /// <param name="fileMode">FileMode。</param>
        /// <returns>ファイルの Stream。</returns>
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

        /// <summary>
        /// ファイルを削除します。
        /// </summary>
        /// <param name="name">ファイル名。</param>
        public void DeleteFile(string name)
        {
            var path = ResolveChildPath(name);
            container.DeleteFile(path);

            index.FileNames.Remove(name);
            SaveIndexFile();
        }

        /// <summary>
        /// index.xml を新規に作成します。
        /// </summary>
        void CreateIndexFile()
        {
            indexFilePath = ResolveChildPath(indexFileName);
            index = new StorageDirectoryIndex();
            using (var stream = container.CreateFile(indexFilePath))
            {
                indexSerializer.Serialize(stream, index);
            }
        }

        /// <summary>
        /// index.xml を読み込みます。
        /// </summary>
        void LoadIndexFile()
        {
            indexFilePath = ResolveChildPath(indexFileName);
            if (!container.FileExists(indexFilePath))
                throw new InvalidOperationException(string.Format("The file '{0}' does not exists.", indexFilePath));

            using (var stream = container.OpenFile(indexFilePath, FileMode.Open))
            {
                index = indexSerializer.Deserialize(stream) as StorageDirectoryIndex;
            }
        }

        /// <summary>
        /// index.xml を保存します。
        /// </summary>
        void SaveIndexFile()
        {
            container.DeleteFile(indexFilePath);

            using (var stream = container.CreateFile(indexFilePath))
            {
                indexSerializer.Serialize(stream, index);
            }

            IndexChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// 指定のサブ ディレクトリ名あるいはファイル名に対するパスを解決します。
        /// このパスは、StorageContainer のルート ディレクトリからの相対パスです。
        /// </summary>
        /// <param name="name">サブ ディレクトリ名あるいはファイル名。</param>
        /// <returns>StorageContainer のルート ディレクトリからの相対パス。</returns>
        string ResolveChildPath(string name)
        {
            return string.IsNullOrEmpty(Path) ? name : Path + "/" + name;
        }
    }
}
