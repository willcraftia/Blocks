#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockEditor.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Screens
{
    public sealed class BlockEditWindow : Window
    {
        SectionControl sectionControl;

        EditorViewModel ViewModel
        {
            get { return DataContext as EditorViewModel; }
        }

        public BlockEditWindow(Screen screen)
            : base(screen)
        {
            var stackPanel = new StackPanel(screen)
            {
                Orientation = Orientation.Vertical,
                Margin = new Thickness(8)
            };
            Content = stackPanel;

            sectionControl = new SectionControl(screen)
            {
            };
            stackPanel.Children.Add(sectionControl);

            sectionControl.Focus();
        }

        protected override void OnDataContextChanged()
        {
            var editorViewModel = DataContext as EditorViewModel;

            if (editorViewModel != null)
            {
                sectionControl.DataContext = editorViewModel.SectionViewModel;
            }
            else
            {
                sectionControl.DataContext = null;
            }

            base.OnDataContextChanged();
        }
    }
}
