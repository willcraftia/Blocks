#region Using

using System;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Blocks.BlockEditor.Models;
using Willcraftia.Xna.Blocks.BlockEditor.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Screens
{
    public sealed class SectionOrientationControl : Button
    {
        public SectionOrientationControl(Screen screen)
            : base(screen)
        {
        }

        protected override void OnClick(ref RoutedEventContext context)
        {
            base.OnClick(ref context);

            var viewModel = DataContext as SectionViewModel;
            switch (viewModel.Orientation)
            {
                case SectionOrientation.Z:
                    viewModel.Orientation = SectionOrientation.Y;
                    break;
                case SectionOrientation.Y:
                    viewModel.Orientation = SectionOrientation.X;
                    break;
                case SectionOrientation.X:
                    viewModel.Orientation = SectionOrientation.Z;
                    break;
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
