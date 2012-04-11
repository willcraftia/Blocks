#region Using

using System;
using Willcraftia.Xna.Blocks.BlockEditor.Models;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.ViewModels
{
    public sealed class WorkspaceViewModel
    {
        Workspace workspace;

        public EditorViewModel EditorViewModel { get; private set; }

        public ViewerViewModel ViewerViewModel { get; private set; }

        public WorkspaceViewModel(Workspace workspace)
        {
            if (workspace == null) throw new ArgumentNullException("workspace");
            this.workspace = workspace;

            EditorViewModel = new EditorViewModel(workspace.Editor);

            ViewerViewModel = new ViewerViewModel(workspace.Viewer)
            {
                ViewMovable = true,
                GridVisible = true
            };
        }
    }
}
