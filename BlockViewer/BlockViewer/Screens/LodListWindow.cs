#region Using

using System;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class LodListWindow : Window
    {
        LodView[] lodThumbnailViews = new LodView[4];

        public LodListWindow(Screen screen, MainViewModel mainViewModel)
            : base(screen)
        {
            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                Padding = new Thickness(8)
            };
            Content = stackPanel;

            for (int i = 0; i < lodThumbnailViews.Length; i++)
            {
                lodThumbnailViews[i] = new LodView(screen, new LodThumbnailViewModel(mainViewModel, i));
                stackPanel.Children.Add(lodThumbnailViews[i]);
            }
        }
    }
}
