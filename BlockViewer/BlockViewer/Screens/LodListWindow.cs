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
    public sealed class LodListWindow : Window
    {
        Control[] controls = new Control[4];

        public LodListWindow(Screen screen, MainViewModel mainViewModel)
            : base(screen)
        {
            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                Padding = new Thickness(8)
            };
            Content = stackPanel;

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
                Text = string.Format(Strings.LevelOfDetailLabelText, levelOfDetail),
                ForegroundColor = Color.White,
                HorizontalAlignment = HorizontalAlignment.Left,
                TextHorizontalAlignment = HorizontalAlignment.Left
            };
            stackPanel.Children.Add(textBlock);

            var BlockMeshView = new BlockMeshView(Screen, new BlockMeshViewModel(mainViewModel, levelOfDetail));
            BlockMeshView.Width = 32 * 2;
            BlockMeshView.Height = 32 * 2;
            stackPanel.Children.Add(BlockMeshView);

            return stackPanel;
        }
    }
}
