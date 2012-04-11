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
        SectionEditControl sectionEditControl;

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

            sectionEditControl = new SectionEditControl(screen)
            {
                CellSize = 12,
                ForegroundColor = Color.White,
                BackgroundColor = Color.Black
            };
            stackPanel.Children.Add(sectionEditControl);

            sectionEditControl.Focus();
        }

        protected override void OnDataContextChanged()
        {
            var editorViewModel = DataContext as EditorViewModel;

            if (editorViewModel != null)
            {
                sectionEditControl.DataContext = editorViewModel.SectionViewModel;
            }
            else
            {
                sectionEditControl.DataContext = null;
            }

            base.OnDataContextChanged();
        }
    }
}
