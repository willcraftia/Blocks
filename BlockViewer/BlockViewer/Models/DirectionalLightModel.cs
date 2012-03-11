#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Cameras;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Models
{
    public sealed class DirectionalLightModel
    {
        public bool Enabled { get; set; }

        public Vector3 DiffuseColor { get; set; }

        public Vector3 SpecularColor { get; set; }

        public ChaseView View { get; private set; }

        public Vector3 Direction
        {
            get
            {
                var direction = -View.Position;
                direction.Normalize();
                return direction;
            }
        }

        public DirectionalLightModel()
        {
            View = new ChaseView
            {
                Distance = 3.5f,
                Angle = new Vector2(-MathHelper.PiOver4 * 0.5f, MathHelper.PiOver4)
            };
        }
    }
}
