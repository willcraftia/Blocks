#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.Cameras;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels
{
    public sealed class LightViewModel
    {
        public ChaseView View { get; set; }

        public bool Enabled { get; set; }

        public Vector3 Direction
        {
            get
            {
                var direction = -View.Position;
                direction.Normalize();
                return direction;
            }
        }

        public Vector3 DiffuseColor { get; set; }

        public Vector3 SpecularColor { get; set; }

        public LightViewModel()
        {
            View = new ChaseView
            {
                Distance = 3.5f,
                Angle = new Vector2(-MathHelper.PiOver4 * 0.5f, MathHelper.PiOver4)
            };
        }
    }
}
