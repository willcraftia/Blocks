#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Storage;

#endregion

namespace Willcraftia.Xna.Framework.Storage
{
    /// <summary>
    /// IStorageService の実装クラスです。
    /// </summary>
    public sealed class StorageManager : IStorageService
    {
        /// <summary>
        /// StorageContainer。
        /// </summary>
        StorageContainer container;

        /// <summary>
        /// ルート ディレクトリの StorageDirectory。
        /// </summary>
        StorageDirectory rootDirectory;

        // I/F
        public StorageDirectory RootDirectory { get; private set; }

        // I/F
        public void Select(string storageName)
        {
            RootDirectory = null;

            var showSelectorResult = StorageDevice.BeginShowSelector(null, null);
            showSelectorResult.AsyncWaitHandle.WaitOne();

            var storageDevice = StorageDevice.EndShowSelector(showSelectorResult);
            showSelectorResult.AsyncWaitHandle.Close();

            var openContainerResult = storageDevice.BeginOpenContainer(storageName, null, null);
            openContainerResult.AsyncWaitHandle.WaitOne();

            container = storageDevice.EndOpenContainer(openContainerResult);
            openContainerResult.AsyncWaitHandle.Close();

            RootDirectory = StorageDirectory.GetRootDirectory(container);
        }
    }
}
