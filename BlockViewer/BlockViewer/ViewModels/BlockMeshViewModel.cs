#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.Cameras;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels
{
    public sealed class BlockMeshViewModel
    {
        ChaseView view;

        PerspectiveFov projection;

        public MainViewModel MainViewModel { get; private set; }

        public int LevelOfDetail { get; set; }

        public View View
        {
            get { return view; }
        }

        public Projection Projection
        {
            get { return projection; }
        }

        public float CameraMoveScale { get; set; }

        public bool CameraMovable { get; set; }

        public bool GridVisible { get; set; }

        public BlockMeshViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;

            view = new ChaseView
            {
                Distance = 3.5f,
                Angle = new Vector2(-MathHelper.PiOver4 * 0.5f, MathHelper.PiOver4)
            };

            projection = new PerspectiveFov
            {
                NearPlaneDistance = 0.01f,
                FarPlaneDistance = 10
            };

            CameraMoveScale = 0.05f;
        }

        public void MoveCamera(Vector2 angleSign)
        {
            // カメラの位置の角度を更新します。
            var angle = view.Angle;
            angle.X += angleSign.X * CameraMoveScale;
            angle.Y += angleSign.Y * CameraMoveScale;
            angle.X = MathHelper.WrapAngle(angle.X);
            angle.Y = MathHelper.WrapAngle(angle.Y);
            view.Angle = angle;
        }

        public void SetAspectRatio(float aspectRatio)
        {
            projection.AspectRatio = aspectRatio;
        }

        public void UpdateCamera()
        {
            view.Update();
            projection.Update();
        }
    }
}
