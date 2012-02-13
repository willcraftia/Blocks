#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class MainScreen : Screen
    {
        BlockMeshView blockMeshView;

        public MainScreen(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            blockMeshView = new BlockMeshView(this);
            Desktop.Content = blockMeshView;

            base.LoadContent();
        }
    }
}
