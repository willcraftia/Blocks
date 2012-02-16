#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;
using Willcraftia.Xna.Blocks.BlockViewer.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class LodView : ContentControl
    {
        public BlockMeshView BlockMeshView { get; private set; }

        public LodView(Screen screen, LodThumbnailViewModel viewModel)
            : base(screen)
        {
            DataContext = viewModel;

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                Padding = new Thickness(4)
            };
            Content = stackPanel;

            var textBlock = new TextBlock(screen)
            {
                Text = Strings.LevelOfDetailLabelText,
                ForegroundColor = Color.White
            };
            stackPanel.Children.Add(textBlock);

            BlockMeshView = new BlockMeshView(screen, new BlockMeshViewModel(viewModel.MainViewModel, viewModel.LevelOfDetail));
            BlockMeshView.Width = 32 * 2;
            BlockMeshView.Height = 32 * 2;
            stackPanel.Children.Add(BlockMeshView);
        }
    }
}
