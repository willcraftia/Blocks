#region Using

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

        BlockMeshViewModel ViewModel
        {
            get { return DataContext as BlockMeshViewModel; }
        }

        public BlockMeshView(Screen screen)
            : base(screen)
        {
        }

        protected override void OnMouseMove(ref RoutedEventContext context)
        {
            if (!ViewModel.CameraMovable) return;

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
                // 指定の角度方向へカメラを移動させます。
                var angleSign = new Vector2
                {
                    X = Math.Sign(mouseState.Y - mouseOffsetY),
                    Y = Math.Sign(mouseState.X - mouseOffsetX)
                };
                ViewModel.MoveCamera(angleSign);

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

            // カメラを更新します。
            ViewModel.SetAspectRatio(graphicsDevice.Viewport.AspectRatio);
            ViewModel.UpdateCamera();

            // GridBlockMesh を描画します。
            if (ViewModel.GridVisible) DrawGridBlockMesh();

            // BlockMesh を描画します。
            DrawBlockMesh();
        }

        void DrawGridBlockMesh()
        {
            // GridBlockMesh 描画用 Effect に View と Projection を設定します。
            var effect = ViewModel.MainViewModel.GridBlockMeshEffect;
            effect.View = ViewModel.View.Matrix;
            effect.Projection = ViewModel.Projection.Matrix;

            // GridBlockMesh の面の描画の有無を決定します。
            var mesh = ViewModel.MainViewModel.GridBlockMesh;
            mesh.SetVisibilities(ViewModel.View.Position);
            mesh.Draw(effect);
        }

        void DrawBlockMesh()
        {
            var mesh = ViewModel.MainViewModel.BlockMesh;
            if (mesh == null) return;

            mesh.LevelOfDetail = ViewModel.LevelOfDetail;

            foreach (var effect in mesh.Effects)
            {
                effect.View = ViewModel.View.Matrix;
                effect.Projection = ViewModel.Projection.Matrix;
            }

            mesh.Draw();
        }
    }
}
