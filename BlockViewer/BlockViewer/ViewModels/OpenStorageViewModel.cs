#region Using

using System;
using Microsoft.Xna.Framework.Storage;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels
{
    public sealed class OpenStorageViewModel
    {
        StorageContainer storageContainer;

        public string SelectedFileName { get; set; }

        public OpenStorageViewModel(StorageContainer storageContainer)
        {
            this.storageContainer = storageContainer;
        }

        public string[] GetFileNames()
        {
            return storageContainer.GetFileNames("Model_*.xml");
        }
    }
}
