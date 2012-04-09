#region Using

using System;
using Willcraftia.Xna.Blocks.BlockEditor.Models;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.ViewModels
{
    public sealed class WorkspaceViewModel
    {
        Workspace workspace;

        public ViewerViewModel ViewerViewModel { get; private set; }

        public WorkspaceViewModel(Workspace workspace)
        {
            if (workspace == null) throw new ArgumentNullException("workspace");
            this.workspace = workspace;

            ViewerViewModel = new ViewerViewModel(workspace.Viewer)
            {
                ViewMovable = true,
                GridVisible = true
            };
        }

        public ViewerViewModel CreateLodViewerViewModel(int levelOfDetail)
        {
            return new ViewerViewModel(workspace.Viewer)
            {
                LevelOfDetail = levelOfDetail
            };
        }
    }
}
