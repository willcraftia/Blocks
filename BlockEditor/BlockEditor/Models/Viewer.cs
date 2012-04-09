#region Using

using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.Cameras;
using Willcraftia.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Models
{
    public sealed class Viewer : IDisposable
    {
        Workspace workspace;

        GraphicsDevice graphicsDevice;

        BasicEffect basicEffect;

        GeometricPrimitive cube;

        PerspectiveFov projection;

        ViewMode viewMode;

        public ChaseView CurrentView { get; private set; }

        public ChaseView CameraView { get; private set; }

        public DirectionalLightModel DirectionalLightModel0 { get; private set; }

        public DirectionalLightModel DirectionalLightModel1 { get; private set; }

        public DirectionalLightModel DirectionalLightModel2 { get; private set; }

        public Vector3 AmbientLightColor { get; private set; }

        public float CameraMoveScale { get; set; }

        public bool GridVisible { get; set; }

        public int LevelOfDetail { get; set; }

        public ViewMode ViewMode
        {
            get { return viewMode; }
            set
            {
                if (viewMode == value) return;

                viewMode = value;

                switch (viewMode)
                {
                    case ViewMode.Camera:
                        CurrentView = CameraView;
                        break;
                    case ViewMode.DirectionalLight0:
                        CurrentView = DirectionalLightModel0.View;
                        break;
                    case ViewMode.DirectionalLight1:
                        CurrentView = DirectionalLightModel1.View;
                        break;
                    case ViewMode.DirectionalLight2:
                        CurrentView = DirectionalLightModel2.View;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

        public Viewer(Workspace workspace)
        {
            if (workspace == null) throw new ArgumentNullException("workspace");
            this.workspace = workspace;

            graphicsDevice = workspace.GraphicsDevice;

            projection = new PerspectiveFov
            {
                NearPlaneDistance = 0.01f,
                FarPlaneDistance = 100
            };

            CameraMoveScale = 0.05f;

            CameraView = new ChaseView
            {
                Distance = 23.5f,
                Angle = new Vector2(-MathHelper.PiOver4 * 0.5f, MathHelper.PiOver4)
            };
            CurrentView = CameraView;

            DirectionalLightModel0 = new DirectionalLightModel
            {
                Enabled = true,
                DiffuseColor = Vector3.One
            };
            DirectionalLightModel1 = new DirectionalLightModel { Enabled = false };
            DirectionalLightModel2 = new DirectionalLightModel { Enabled = false };

            AmbientLightColor = new Vector3(0.05333332f, 0.09882354f, 0.1819608f);

            basicEffect = new BasicEffect(workspace.GraphicsDevice);
            basicEffect.EnableDefaultLighting();

            cube = CreateCubePrimitive();
        }

        public void MoveView(Vector2 angleSign)
        {
            // CurrentView の位置の角度を更新します。
            var angle = CurrentView.Angle;
            angle.X += angleSign.X * CameraMoveScale;
            angle.Y += angleSign.Y * CameraMoveScale;
            angle.X = MathHelper.WrapAngle(angle.X);
            angle.Y = MathHelper.WrapAngle(angle.Y);
            CurrentView.Angle = angle;
        }

        public void Draw()
        {
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            // Projection を更新します。
            projection.AspectRatio = graphicsDevice.Viewport.AspectRatio;
            projection.Update();

            // CameraView を更新します。
            CameraView.Update();

            // DirectionalLightModel.View を更新します。
            DirectionalLightModel0.View.Update();
            DirectionalLightModel1.View.Update();
            DirectionalLightModel2.View.Update();

            // GridBlockMesh を描画します。
            if (GridVisible) DrawGridBlockMesh();

            // Block を描画します。
            DrawBlock();
        }

        void DrawGridBlockMesh()
        {
            // GridBlockMesh の面の描画の有無を決定します。
            var grid = workspace.GridBlockMesh;

            grid.SetVisibilities(CurrentView.Position);

            var effect = grid.Effect;
            effect.View = CurrentView.Matrix;
            effect.Projection = projection.Matrix;

            grid.Draw();
        }

        void DrawBlock()
        {
            var block = workspace.Block;
            if (block == null) return;

            foreach (var element in block.Elements)
            {
                var position = new Vector3
                {
                    X = element.Position.X,
                    Y = element.Position.Y,
                    Z = element.Position.Z
                };
                Matrix translation;
                Matrix.CreateTranslation(ref position, out translation);

                var material = block.Materials[element.MaterialIndex];

                basicEffect.World = translation;
                basicEffect.View = CurrentView.Matrix;
                basicEffect.Projection = projection.Matrix;
                basicEffect.DiffuseColor = material.DiffuseColor.ToColor().ToVector3();
                basicEffect.EmissiveColor = material.EmissiveColor.ToColor().ToVector3();
                basicEffect.SpecularColor = material.SpecularColor.ToColor().ToVector3();
                basicEffect.SpecularPower = material.SpecularPower;

                cube.Draw(basicEffect);
            }
        }

        /// <summary>
        /// 立方体の GeometricPrimitive を生成します。
        /// </summary>
        /// <returns>生成された立方体の GeometricPrimitive。</returns>
        GeometricPrimitive CreateCubePrimitive()
        {
            var sourceFactory = new CubeVertexSourceFactory();
            var source = sourceFactory.CreateVertexSource();
            return GeometricPrimitive.Create(graphicsDevice, source);
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        bool disposed;

        ~Viewer()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                basicEffect.Dispose();
                cube.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
