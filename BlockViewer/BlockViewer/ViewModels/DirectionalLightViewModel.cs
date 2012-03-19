#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Blocks.BlockViewer.Models;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels
{
    public sealed class DirectionalLightViewModel
    {
        Viewer viewer;

        int index;

        public int Index
        {
            get { return index; }
            set
            {
                if (value < 0 || 2 < value) throw new ArgumentOutOfRangeException("value");
                index = value;
            }
        }

        public bool Enabled
        {
            get { return Model.Enabled; }
            set { Model.Enabled = value; }
        }

        public Vector3 DiffuseColor
        {
            get { return Model.DiffuseColor; }
            set { Model.DiffuseColor = value; }
        }

        public Vector3 SpecularColor
        {
            get { return Model.SpecularColor; }
            set { Model.SpecularColor = value; }
        }

        DirectionalLightModel Model
        {
            get
            {
                switch (Index)
                {
                    case 0:
                        return viewer.DirectionalLightModel0;
                    case 1:
                        return viewer.DirectionalLightModel1;
                    case 2:
                        return viewer.DirectionalLightModel2;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public DirectionalLightViewModel(Viewer viewer)
        {
            if (viewer == null) throw new ArgumentNullException("viewer");
            this.viewer = viewer;
        }
    }
}
