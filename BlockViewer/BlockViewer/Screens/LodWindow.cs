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
    public sealed class LodWindow : Window
    {
        Control[] controls = new Control[4];

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

            var mainViewModel = DataContext as MainViewModel;
            for (int i = 0; i < controls.Length; i++)
            {
                controls[i] = CreateLodControl(mainViewModel, i);
                stackPanel.Children.Add(controls[i]);
            }
        }

        Control CreateLodControl(MainViewModel mainViewModel, int levelOfDetail)
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

            var viewModel = new BlockMeshViewModel(mainViewModel)
            {
                LevelOfDetail = levelOfDetail
            };
            var BlockMeshView = new BlockMeshView(Screen)
            {
                Width = 32 * 2,
                Height = 32 * 2,

                DataContext = new BlockMeshViewModel(mainViewModel)
                {
                    LevelOfDetail = levelOfDetail
                }
            };
            stackPanel.Children.Add(BlockMeshView);

            return stackPanel;
        }
    }
}
