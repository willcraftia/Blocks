#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Blocks.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class BlockMeshView : Control
    {
        // MEMO
        //
        // BasicEffect.EnableDefaultLighting() が呼び出されると Normal0 が要求されるため、
        // 念のため GridBlockMesh 専用の BasicEffect を使用します。
        //

        BasicEffect gridEffect;

        GridBlockMesh gridBlockMesh;

        int mouseOffsetX;
        int mouseOffsetY;

        float cameraMoveScale = 0.01f;

        float cameraDistance = 2;
        float cameraPositionYaw = MathHelper.PiOver4;
        float cameraPositionPitch = -MathHelper.PiOver4;

        public BlockMesh BlockMesh { get; set; }

        public BlockMeshView(Screen screen)
            : base(screen)
        {
            gridEffect = new BasicEffect(screen.GraphicsDevice);
            gridEffect.VertexColorEnabled = true;

            gridBlockMesh = new GridBlockMesh(screen.GraphicsDevice, 16, 0.1f, Color.White);
        }

        protected override void OnMouseMove(ref RoutedEventContext context)
        {
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
                float dy = (float) (mouseState.X - mouseOffsetX) * cameraMoveScale;
                float dp = (float) (mouseState.Y - mouseOffsetY) * cameraMoveScale;

                cameraPositionYaw += dy;
                cameraPositionPitch += dp;

                cameraPositionYaw = MathHelper.WrapAngle(cameraPositionYaw);
                cameraPositionPitch = MathHelper.WrapAngle(cameraPositionPitch);

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

            // View 行列を算出します。
            var cameraScale = Matrix.CreateScale(cameraDistance);
            var cameraRotation = Matrix.CreateFromYawPitchRoll(cameraPositionYaw, cameraPositionPitch, 0);
            var cameraPosition = Vector3.Transform(Vector3.Backward, cameraScale * cameraRotation);
            var cameraUp = Vector3.Transform(Vector3.Up, cameraRotation);
            var view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, cameraUp);

            // Projection 行列を算出します。
            var aspect = ((float) RenderSize.Width / (float) RenderSize.Height);
            var projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 0.1f, 10);

            // GridBlockMesh 描画用 Effect に View と Projection を設定します。
            gridEffect.View = view;
            gridEffect.Projection = projection;

            // GridBlockMesh の面の描画の有無を決定します。
            gridBlockMesh.SetVisibilities(cameraPosition);
            gridBlockMesh.Draw(gridEffect);

            if (BlockMesh != null)
            {
                foreach (var effect in BlockMesh.Effects)
                {
                    effect.View = view;
                    effect.Projection = projection;
                }

                BlockMesh.Draw();
            }
        }
    }
}
