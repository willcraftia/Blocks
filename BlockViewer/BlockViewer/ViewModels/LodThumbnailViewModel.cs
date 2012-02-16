#region Using

using System;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels
{
    public sealed class LodThumbnailViewModel
    {
        public MainViewModel MainViewModel { get; private set; }

        public int LevelOfDetail { get; set; }

        public LodThumbnailViewModel(MainViewModel mainViewModel, int levelOfDetail)
        {
            MainViewModel = mainViewModel;
            LevelOfDetail = levelOfDetail;
        }
    }
}
