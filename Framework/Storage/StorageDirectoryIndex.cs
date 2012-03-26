#region Using

using System;
using System.Collections.Generic;

#endregion

namespace Willcraftia.Xna.Framework.Storage
{
    /// <summary>
    /// StorageDirectory の index.xml のオブジェクト表現です。
    /// </summary>
    public sealed class StorageDirectoryIndex
    {
        /// <summary>
        /// サブ ディレクトリ名のリストを取得または設定します。
        /// </summary>
        public List<string> DirectoryNames { get; set; }

        /// <summary>
        /// ファイル名のリストを取得または設定します。
        /// </summary>
        public List<string> FileNames { get; set; }
    }
}
