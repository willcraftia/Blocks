#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.Graphics;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;
using Willcraftia.Xna.Blocks.BlockViewer.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class LodWindow : Window
    {
        WorkspaceViewModel WorkspaceViewModel
        {
            get { return DataContext as WorkspaceViewModel; }
        }

        public LodWindow(Screen screen)
            : base(screen)
        {
            // この Window はアクティブにできません。
            Activatable = false;

            ShadowOffset = new Vector2(4);
            Padding = new Thickness(8);

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical
            };
            Content = stackPanel;

            for (int i = 0; i < 4; i++)
            {
                var lodControl = CreateLodControl(i);
                stackPanel.Children.Add(lodControl);
            }
        }

        Control CreateLodControl(int levelOfDetail)
        {
            var stackPanel = new StackPanel(Screen)
            {
                Orientation = Orientation.Vertical,
                Padding = new Thickness(4)
            };

            var textBlock = new TextBlock(Screen)
            {
                Text = string.Format(Strings.LevelOfDetailLabel, levelOfDetail),
                FontStretch = new Vector2(0.7f),
                ForegroundColor = Color.White,
                HorizontalAlignment = HorizontalAlignment.Left,
                TextHorizontalAlignment = HorizontalAlignment.Left
            };
            stackPanel.Children.Add(textBlock);

            var viewModel = WorkspaceViewModel.CreateLodViewerViewModel(levelOfDetail);

            var meshView = new BlockMeshView(Screen)
            {
                Width = 32 * 2,
                Height = 32 * 2,
                DataContext = viewModel
            };
            stackPanel.Children.Add(meshView);

            return stackPanel;
        }
    }
}
