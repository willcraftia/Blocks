#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Blocks.Graphics;
using Willcraftia.Xna.Blocks.BlockViewer.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class BlockMeshView : Control
    {
        int mouseOffsetX;
        
        int mouseOffsetY;

        Vector3 cameraPosition;

        Matrix view;
        
        Matrix projection;

        public float CameraDistance { get; set; }

        public float CameraPositionYaw { get; set; }
        
        public float CameraPositionPitch { get; set; }

        public float CameraMoveScale { get; set; }

        public bool GridVisible { get; set; }

        public bool CameraMovable { get; set; }

        public BlockMeshView(Screen screen, BlockMeshViewModel viewModel)
            : base(screen)
        {
            DataContext = viewModel;

            CameraDistance = 3.5f;
            CameraPositionYaw = MathHelper.PiOver4;
            CameraPositionPitch = -MathHelper.PiOver4 * 0.5f;
            CameraMoveScale = 0.01f;
        }

        protected override void OnMouseMove(ref RoutedEventContext context)
        {
            if (!CameraMovable) return;

            var mouseDevice = Screen.MouseDevice;
            var mouseState = mouseDevice.MouseState;

            if (mouseDevice.IsButtonPressed(MouseButtons.Right))
            {
                // OnMouseDown で判定しようとすると OnMouseMove の後になるため、
                // ここで押下判定を行なっています。
                mouseOffsetX = mouseState.X;
                mouseOffsetY = mouseState.Y;
            }

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                float dy = (float) (mouseState.X - mouseOffsetX) * CameraMoveScale;
                float dp = (float) (mouseState.Y - mouseOffsetY) * CameraMoveScale;

                CameraPositionYaw += dy;
                CameraPositionPitch += dp;

                CameraPositionYaw = MathHelper.WrapAngle(CameraPositionYaw);
                CameraPositionPitch = MathHelper.WrapAngle(CameraPositionPitch);

                mouseOffsetX = mouseState.X;
                mouseOffsetY = mouseState.Y;
            }

            base.OnMouseMove(ref context);
        }

        public override void Draw(GameTime gameTime, IDrawContext drawContext)
        {
            base.Draw(gameTime, drawContext);

            var bounds = new Rect(0, 0, RenderSize.Width, RenderSize.Height);

            using (var draw3d = drawContext.BeginDraw3D())
            {
                using (var localViewport = drawContext.BeginViewport(bounds))
                {
                    using (var localClipping = drawContext.BeginNewClip(bounds))
                    {
                        Draw3D(gameTime, drawContext);
                    }
                }
            }
        }

        void Draw3D(GameTime gameTime, IDrawContext drawContext)
        {
            var graphicsDevice = Screen.GraphicsDevice;
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            // カメラ座標を算出します。
            var cameraScale = Matrix.CreateScale(CameraDistance);
            var cameraRotation = Matrix.CreateFromYawPitchRoll(CameraPositionYaw, CameraPositionPitch, 0);
            cameraPosition = Vector3.Transform(Vector3.Backward, cameraScale * cameraRotation);

            // View 行列を算出します。
            var cameraUp = Vector3.Transform(Vector3.Up, cameraRotation);
            view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, cameraUp);

            // Projection 行列を算出します。
            var aspect = ((float) RenderSize.Width / (float) RenderSize.Height);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, aspect, 0.01f, 10);

            // GridBlockMesh を描画します。
            if (GridVisible) DrawGridBlockMesh();

            // BlockMesh を描画します。
            DrawBlockMesh();
        }

        void DrawGridBlockMesh()
        {
            var viewModel = DataContext as BlockMeshViewModel;

            // GridBlockMesh 描画用 Effect に View と Projection を設定します。
            var effect = viewModel.MainViewModel.GridBlockMeshEffect;
            effect.View = view;
            effect.Projection = projection;

            // GridBlockMesh の面の描画の有無を決定します。
            var mesh = viewModel.MainViewModel.GridBlockMesh;
            mesh.SetVisibilities(cameraPosition);
            mesh.Draw(effect);
        }

        void DrawBlockMesh()
        {
            var viewModel = DataContext as BlockMeshViewModel;

            var mesh = viewModel.MainViewModel.BlockMesh;
            if (mesh == null) return;

            mesh.LevelOfDetail = viewModel.LevelOfDetail;
            foreach (var effect in mesh.Effects)
            {
                effect.View = view;
                effect.Projection = projection;
            }

            mesh.Draw();
        }
    }
}
