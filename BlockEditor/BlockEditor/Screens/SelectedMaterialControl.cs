#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Cameras;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Blocks.BlockEditor.Models;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Screens
{
    public sealed class SelectedMaterialControl : Control
    {
        CubeMesh cubeMesh;

        ChaseView view;

        PerspectiveFov projection;

        Workspace Workspace
        {
            get { return DataContext as Workspace; }
        }

        public SelectedMaterialControl(Screen screen)
            : base(screen)
        {
            cubeMesh = new CubeMesh(screen.GraphicsDevice, 1);
            cubeMesh.Effect.EnableDefaultLighting();

            view = new ChaseView
            {
                Distance = 3.5f,
                Angle = new Vector2(-MathHelper.PiOver4 * 0.5f, MathHelper.PiOver4)
            };
            projection = new PerspectiveFov
            {
                NearPlaneDistance = 0.01f,
                FarPlaneDistance = 100
            };
        }

        public override void Draw(GameTime gameTime, IDrawContext drawContext)
        {
            base.Draw(gameTime, drawContext);

            if (Workspace.SelectedMaterial == null) return;

            var bounds = new Rect(0, 0, RenderSize.Width, RenderSize.Height);
            using (var draw3d = drawContext.BeginDraw3D())
            using (var localViewport = drawContext.BeginViewport(bounds))
            using (var localClipping = drawContext.BeginNewClip(bounds))
            {
                var graphicsDevice = Screen.GraphicsDevice;
                graphicsDevice.BlendState = BlendState.Opaque;
                graphicsDevice.DepthStencilState = DepthStencilState.Default;

                // View を更新します。
                view.Update();

                // Projection を更新します。
                projection.AspectRatio = graphicsDevice.Viewport.AspectRatio;
                projection.Update();

                var material = Workspace.SelectedMaterial;
                var effect = cubeMesh.Effect;
                effect.View = view.Matrix;
                effect.Projection = projection.Matrix;
                effect.DiffuseColor = material.DiffuseColor.ToVector3();
                effect.EmissiveColor = material.EmissiveColor.ToVector3();
                effect.SpecularColor = material.SpecularColor.ToVector3();
                effect.SpecularPower = material.SpecularPower;

                cubeMesh.Draw();
            }
        }
    }
}
