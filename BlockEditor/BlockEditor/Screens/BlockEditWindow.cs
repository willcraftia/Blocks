﻿#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockEditor.Models;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Screens
{
    public sealed class BlockEditWindow : Window
    {
        SectionControl sectionControl;

        MaterialEditWindow materialEditWindow;

        public BlockEditWindow(Screen screen)
            : base(screen)
        {
            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                Padding = new Thickness(8)
            };
            Content = stackPanel;

            sectionControl = new SectionControl(screen)
            {
                DataContext = (DataContext as Workspace).Section
            };
            stackPanel.Children.Add(sectionControl);

            var selectedMaterialControl = new SelectedMaterialControl(screen)
            {
                Width = 64,
                Height = 64
            };
            stackPanel.Children.Add(selectedMaterialControl);

            sectionControl.Focus();
        }
    }
}
