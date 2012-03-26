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
        /// StorageContainer が選択された時に発生します。
        /// </summary>
        event EventHandler ContainerSelected;

        /// <summary>
        /// ルート ディレクトリを取得します。
        /// Select(string) の呼び出しが完了するまで値は null です。
        /// </summary>
        StorageDirectory RootDirectory { get; }

        /// <summary>
        /// StorageContainer を選択します。
        /// </summary>
        /// <param name="storageName">StorageContainer の名前。</param>
        void Select(string storageName);
    }
}
