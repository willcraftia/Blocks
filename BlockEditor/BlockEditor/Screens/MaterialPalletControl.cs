#region Using

using System;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Blocks.BlockEditor.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Screens
{
    public sealed class MaterialPalletControl : Control
    {
        MaterialPalletViewModel ViewModel
        {
            get { return DataContext as MaterialPalletViewModel; }
        }

        public MaterialPalletControl(Screen screen)
            : base(screen)
        {
        }
    }
}
