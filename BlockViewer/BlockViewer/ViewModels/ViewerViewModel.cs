#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Blocks.BlockViewer.Models;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels
{
    public sealed class ViewerViewModel
    {
        Viewer viewer;

        public string MeshName
        {
            get { return viewer.MeshName; }
            set { viewer.MeshName = value; }
        }

        public bool ViewMovable { get; set; }

        public bool GridVisible { get; set; }

        public int LevelOfDetail { get; set; }

        public DirectionalLightViewModel DirectionalLightViewModel { get; private set; }

        public ViewMode ViewMode
        {
            get { return viewer.ViewMode; }
            set
            {
                if (viewer.ViewMode == value) return;

                viewer.ViewMode = value;

                switch (viewer.ViewMode)
                {
                    case ViewMode.DirectionalLight0:
                        DirectionalLightViewModel.Index = 0;
                        break;
                    case ViewMode.DirectionalLight1:
                        DirectionalLightViewModel.Index = 1;
                        break;
                    case ViewMode.DirectionalLight2:
                        DirectionalLightViewModel.Index = 2;
                        break;
                }
            }
        }

        public ViewerViewModel(Viewer viewer)
        {
            if (viewer == null) throw new ArgumentNullException("viewer");
            this.viewer = viewer;

            DirectionalLightViewModel = new DirectionalLightViewModel(viewer);
        }

        public void MoveView(Vector2 angleSign)
        {
            viewer.MoveView(angleSign);
        }

        public void Draw()
        {
            viewer.GridVisible = GridVisible;
            viewer.LevelOfDetail = LevelOfDetail;
            viewer.Draw();
        }

        public void UnloadMesh()
        {
            viewer.MeshName = null;
        }
    }
}
