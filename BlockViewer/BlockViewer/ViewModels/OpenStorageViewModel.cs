#region Using

using System;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Blocks.BlockViewer.Models;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels
{
    public sealed class OpenStorageViewModel
    {
        StorageModel storageModel;

        string[] fileNames = new string[0];

        Paging paging = new Paging();

        public int FileNameCount
        {
            get { return fileNames.Length; }
        }

        public int CurrentPageIndex
        {
            get { return paging.CurrentPageIndex; }
        }

        public int PageCount
        {
            get { return paging.PageCount; }
        }

        public string SelectedFileName { get; set; }

        public OpenStorageViewModel(StorageModel storageModel)
        {
            if (storageModel == null) throw new ArgumentNullException("storageModel");
            this.storageModel = storageModel;
        }

        public void Initialize(int itemCountPerPage)
        {
            SelectedFileName = null;

            paging.ItemCountPerPage = itemCountPerPage;

            fileNames = storageModel.GetBlockMeshFileNames();
            paging.ItemCount = fileNames.Length;
        }

        public void SetItemCountPerPage(int count)
        {
            paging.ItemCountPerPage = count;
        }

        public string GetFileName(int indexInPage)
        {
            if (indexInPage < 0 || paging.ItemCountPerPage < indexInPage)
                throw new ArgumentOutOfRangeException("indexInPage");

            int itemIndex = paging.GetItemIndex(indexInPage);
            return (0 <= itemIndex) ? fileNames[itemIndex] : null;
        }

        public void ForwardPage()
        {
            paging.Forward();
        }

        public void BackPage()
        {
            paging.Back();
        }

        //public string[] GetBlockMeshFileNames()
        //{
        //    return storageModel.GetBlockMeshFileNames();
        //}
    }
}
