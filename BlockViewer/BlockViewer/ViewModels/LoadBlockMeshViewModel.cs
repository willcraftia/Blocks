#region Using

using System;
using System.Collections.Generic;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Blocks.BlockViewer.Models;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels
{
    public sealed class LoadBlockMeshViewModel
    {
        Preview preview;

        List<string> fileNames;

        Paging paging = new Paging();

        List<ViewerViewModel> viewerViewModels = new List<ViewerViewModel>();

        public int CurrentPageIndex
        {
            get { return paging.CurrentPageIndex; }
        }

        public int PageCount
        {
            get { return paging.PageCount; }
        }

        public string SelectedFileName { get; set; }

        public LoadBlockMeshViewModel(Preview preview)
        {
            if (preview == null) throw new ArgumentNullException("preview");
            this.preview = preview;
        }

        public void Initialize(int itemCountPerPage)
        {
            SelectedFileName = null;

            paging.ItemCountPerPage = itemCountPerPage;

            fileNames = preview.Workspace.StorageBlockService.GetBlockNames();
            paging.ItemCount = fileNames.Count;

            while (viewerViewModels.Count < itemCountPerPage)
            {
                var viewerViewModel = new ViewerViewModel(preview.CreateViewer());
                viewerViewModels.Add(viewerViewModel);
            }

            InitializeViewerViewModels();
        }

        public void UnloadMeshes()
        {
            foreach (var viewerViewModel in viewerViewModels)
            {
                viewerViewModel.UnloadMesh();
            }
        }

        public ViewerViewModel GetViewerViewModel(int indexInPage)
        {
            return viewerViewModels[indexInPage];
        }

        public void ForwardPage()
        {
            paging.Forward();
            InitializeViewerViewModels();
        }

        public void BackPage()
        {
            paging.Back();
            InitializeViewerViewModels();
        }

        public void InitializeViewerViewModels()
        {
            for (int i = 0; i < paging.ItemCountPerPage; i++)
            {
                viewerViewModels[i].MeshName = GetFileName(i);
            }
        }

        string GetFileName(int indexInPage)
        {
            if (indexInPage < 0 || paging.ItemCountPerPage < indexInPage)
                throw new ArgumentOutOfRangeException("indexInPage");

            int itemIndex = paging.GetItemIndex(indexInPage);
            return (0 <= itemIndex) ? fileNames[itemIndex] : null;
        }
    }
}
