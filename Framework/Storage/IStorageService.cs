#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Storage
{
    /// <summary>
    /// StorageDirectory により StorageContainer を管理するサービスへのインタフェースです。
    /// </summary>
    public interface IStorageService
    {
        /// <summary>
        /// ルート ディレクトリを取得します。
        /// Select(string) の呼び出しが完了するまで値は null です。
        /// </summary>
        StorageDirectory RootDirectory { get; }

        /// <summary>
        /// ストレージを選択します。
        /// </summary>
        /// <param name="storageName">ストレージの名前。</param>
        void Select(string storageName);
    }
}
