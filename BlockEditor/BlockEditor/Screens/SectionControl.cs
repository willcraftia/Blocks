#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockEditor.Models;

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
                //Margin = new Thickness(8)
            };
            stackPanel.Children.Add(selectorPanel);

            var changeOrientationButton = new Button(screen)
            {
                Width = cellSize,
                Height = cellSize,
                Margin = new Thickness(0, 0, 8, 0)
            };
            changeOrientationButton.Click += OnChangeOrientationButtonClick;
            selectorPanel.Children.Add(changeOrientationButton);

            var indexControl = new SectionIndexControl(screen)
            {
                CellSize = cellSize
            };
            selectorPanel.Children.Add(indexControl);

            var editControl = new SectionEditControl(screen)
            {
                CellSize = cellSize,
                HorizontalAlignment = HorizontalAlignment.Right,
                Margin = new Thickness(0, 8, 0, 0)
            };
            stackPanel.Children.Add(editControl);
        }

        void OnChangeOrientationButtonClick(Control sender, ref RoutedEventContext context)
        {
            (DataContext as Section).ChangeOrientation();
        }
    }
}
