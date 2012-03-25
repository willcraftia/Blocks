#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Storage;

#endregion

namespace Willcraftia.Xna.Framework.Storage
{
    public sealed class StorageManager : IStorageService
    {
        StorageContainer container;

        public StorageDirectory RootDirectory { get; private set; }

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

        void EnsureStorageContainer()
        {
            if (container == null) throw new InvalidOperationException("StorageContainer is not selected.");
        }
    }
}
