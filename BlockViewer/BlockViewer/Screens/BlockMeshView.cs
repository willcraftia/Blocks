﻿#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Cameras;
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

        ChaseView view = new ChaseView();

        PerspectiveFov projection = new PerspectiveFov();

        public float CameraMoveScale { get; set; }

        public bool GridVisible { get; set; }

        public bool CameraMovable { get; set; }

        public BlockMeshView(Screen screen, BlockMeshViewModel viewModel)
            : base(screen)
        {
            DataContext = viewModel;

            view.Distance = 3.5f;
            view.Angle = new Vector2(-MathHelper.PiOver4 * 0.5f, MathHelper.PiOver4);

            projection.NearPlaneDistance = 0.01f;
            projection.FarPlaneDistance = 10;

            CameraMoveScale = 0.05f;
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
                float dy = (float) (Math.Sign(mouseState.X - mouseOffsetX)) * CameraMoveScale;
                float dp = (float) (Math.Sign(mouseState.Y - mouseOffsetY)) * CameraMoveScale;

                // カメラの位置の角度を更新します。
                var angle = view.Angle;
                angle.X += dp;
                angle.Y += dy;
                angle.X = MathHelper.WrapAngle(angle.X);
                angle.Y = MathHelper.WrapAngle(angle.Y);
                view.Angle = angle;

                mouseOffsetX = mouseState.X;
                mouseOffsetY = mouseState.Y;
            }

            base.OnMouseMove(ref context);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
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

            // View 行列を更新します。
            view.Update();

            // Projection 行列を更新します。
            projection.AspectRatio = graphicsDevice.Viewport.AspectRatio;
            projection.Update();

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
            effect.View = view.Matrix;
            effect.Projection = projection.Matrix;

            // GridBlockMesh の面の描画の有無を決定します。
            var mesh = viewModel.MainViewModel.GridBlockMesh;
            mesh.SetVisibilities(view.Position);
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
                effect.View = view.Matrix;
                effect.Projection = projection.Matrix;
            }

            mesh.Draw();
        }
    }
}
