#region Using

using System;
using Willcraftia.Xna.Blocks.BlockViewer.Models;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels
{
    public sealed class OpenStorageViewModel
    {
        StorageModel storageModel;

        public string SelectedFileName { get; set; }

        public OpenStorageViewModel(StorageModel storageModel)
        {
            if (storageModel == null) throw new ArgumentNullException("storageModel");
            this.storageModel = storageModel;
        }

        public string[] GetBlockMeshFileNames()
        {
            return storageModel.GetBlockMeshFileNames();
        }
    }
}
