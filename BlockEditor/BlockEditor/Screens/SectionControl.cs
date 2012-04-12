#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Screens
{
    public sealed class SectionControl : ContentControl
    {
        public SectionControl(Screen screen)
            : base(screen)
        {
            const float cellSize = 16;

            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical
            };
            Content = stackPanel;

            var selectorPanel = new StackPanel(screen)
            {
                Margin = new Thickness(8)
            };
            stackPanel.Children.Add(selectorPanel);

            var orientationControl = new SectionOrientationControl(screen)
            {
                Width = cellSize,
                Height = cellSize
            };
            selectorPanel.Children.Add(orientationControl);

            var indexControl = new SectionIndexControl(screen)
            {
                CellSize = cellSize
            };
            selectorPanel.Children.Add(indexControl);

            var editControl = new SectionEditControl(screen)
            {
                CellSize = cellSize,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(8, 0, 8, 8)
            };
            stackPanel.Children.Add(editControl);
        }
    }
}
