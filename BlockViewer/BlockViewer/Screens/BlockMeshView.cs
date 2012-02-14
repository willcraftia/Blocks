﻿#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Blocks.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class BlockMeshView : Control
    {
        Vector3 defaultCameraPosition = new Vector3(2.5f, 2.5f, 2.5f);

        Vector3 cameraPosition;

        // MEMO
        //
        // BasicEffect.EnableDefaultLighting() が呼び出されると Normal0 が要求されるため、
        // 念のため GridBlockMesh 専用の BasicEffect を使用します。
        //

        BasicEffect gridEffect;

        GridBlockMesh gridBlockMesh;

        int mouseOffsetX;
        int mouseOffsetY;
        float cameraRotationScale = 0.01f;

        public BlockMesh BlockMesh { get; set; }

        public BlockMeshView(Screen screen)
            : base(screen)
        {
            gridEffect = new BasicEffect(screen.GraphicsDevice);
            gridEffect.Alpha = 1;
            gridEffect.VertexColorEnabled = true;

            gridBlockMesh = new GridBlockMesh(screen.GraphicsDevice, 16, 0.1f, Color.White);

            cameraPosition = defaultCameraPosition;
        }

        protected override void OnMouseDown(ref RoutedEventContext context)
        {
            var mouseState = Screen.MouseDevice.MouseState;
            mouseOffsetX = mouseState.X;
            mouseOffsetY = mouseState.Y;

            base.OnMouseDown(ref context);
        }

        protected override void OnMouseMove(ref RoutedEventContext context)
        {
            var mouseState = Screen.MouseDevice.MouseState;
            if (mouseState.RightButton == ButtonState.Pressed)
            {
                float dx = (float) (mouseState.X - mouseOffsetX) * cameraRotationScale;
                float dy = (float) (mouseState.Y - mouseOffsetY) * cameraRotationScale;
                var rotation = Matrix.CreateFromYawPitchRoll(dx, dy, 0);
                cameraPosition = Vector3.Transform(cameraPosition, rotation);

                mouseOffsetX = mouseState.X;
                mouseOffsetY = mouseState.Y;
            }

            base.OnMouseMove(ref context);
        }

        public override void Draw(GameTime gameTime, IDrawContext drawContext)
        {
            base.Draw(gameTime, drawContext);

            using (var draw3d = drawContext.BeginDraw3D())
            {
                Screen.GraphicsDevice.BlendState = BlendState.Opaque;
                Screen.GraphicsDevice.DepthStencilState = DepthStencilState.Default;

                var bounds = new Rect(0, 0, RenderSize.Width, RenderSize.Height);
                using (var localViewport = drawContext.BeginViewport(bounds))
                {
                    using (var localClipping = drawContext.BeginNewClip(bounds))
                    {
                        var aspect = ((float) RenderSize.Width / (float) RenderSize.Height);

                        var view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
                        var projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 0.1f, 10);

                        gridEffect.View = view;
                        gridEffect.Projection = projection;

                        gridBlockMesh.SetVisibilities(cameraPosition);
                        gridBlockMesh.Draw(gridEffect);

                        if (BlockMesh != null)
                        {
                            foreach (var effect in BlockMesh.Effects)
                            {
                                effect.Alpha = 1;
                                effect.View = view;
                                effect.Projection = projection;
                            }

                            BlockMesh.Draw();
                        }
                    }
                }
            }
        }
    }
}
