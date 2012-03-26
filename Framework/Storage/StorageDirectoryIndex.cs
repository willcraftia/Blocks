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

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public StorageDirectoryIndex()
        {
            // MEMO
            //
            // XmlSerializer は List が存在する時に新規インスタンスを生成せず、
            // それをそのまま使用する模様。

            DirectoryNames = new List<string>();
            FileNames = new List<string>();
        }
    }
}
