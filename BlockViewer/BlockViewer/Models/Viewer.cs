#region Using

using System;
using System.Threading;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.Cameras;
using Willcraftia.Xna.Blocks.Content;
using Willcraftia.Xna.Blocks.Graphics;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Models
{
    public sealed class Viewer : IDisposable
    {
        Workspace workspace;

        GraphicsDevice graphicsDevice;

        ViewMode viewMode;

        string meshName;

        BlockMesh mesh;

        public string MeshName
        {
            get { return meshName; }
            set
            {
                if (meshName == value) return;

                if (meshName != null)
                {
                    if (mesh == null)
                    {
                        workspace.CancelLoadInterBlockMeshAsync(meshName);
                    }
                    else
                    {
                        workspace.CancelLoadBlockMesh(mesh);

                        mesh.Dispose();
                        mesh = null;
                    }
                }

                meshName = value;

                if (meshName != null)
                {
                    workspace.LoadInterBlockMeshAsync(meshName, 4, InterBlockMeshLoadTaskCallback);
                }
            }
        }

        public ChaseView CurrentView { get; private set; }

        public ChaseView CameraView { get; private set; }

        public PerspectiveFov Projection { get; private set; }

        public DirectionalLightModel DirectionalLightModel0 { get; private set; }

        public DirectionalLightModel DirectionalLightModel1 { get; private set; }

        public DirectionalLightModel DirectionalLightModel2 { get; private set; }

        public Vector3 AmbientLightColor { get; private set; }

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

        public float CameraMoveScale { get; set; }

        public bool GridVisible { get; set; }

        public int LevelOfDetail { get; set; }

        public Viewer(Workspace workspace)
        {
            if (workspace == null) throw new ArgumentNullException("workspace");
            this.workspace = workspace;

            graphicsDevice = workspace.GraphicsDevice;

            CameraView = new ChaseView
            {
                Distance = 3.5f,
                Angle = new Vector2(-MathHelper.PiOver4 * 0.5f, MathHelper.PiOver4)
            };

            CurrentView = CameraView;
            viewMode = ViewMode.Camera;

            Projection = new PerspectiveFov
            {
                NearPlaneDistance = 0.01f,
                FarPlaneDistance = 10
            };

            DirectionalLightModel0 = new DirectionalLightModel
            {
                Enabled = true,
                DiffuseColor = Vector3.One
            };
            DirectionalLightModel1 = new DirectionalLightModel { Enabled = false };
            DirectionalLightModel2 = new DirectionalLightModel { Enabled = false };

            AmbientLightColor = new Vector3(0.05333332f, 0.09882354f, 0.1819608f);

            CameraMoveScale = 0.05f;
        }

        void InterBlockMeshLoadTaskCallback(string name, InterBlockMesh result)
        {
            if (meshName == name) mesh = workspace.LoadBlockMesh(result);
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
            Projection.AspectRatio = graphicsDevice.Viewport.AspectRatio;
            Projection.Update();

            // CameraView を更新します。
            CameraView.Update();

            // DirectionalLightModel.View を更新します。
            DirectionalLightModel0.View.Update();
            DirectionalLightModel1.View.Update();
            DirectionalLightModel2.View.Update();

            // GridBlockMesh を描画します。
            if (GridVisible) DrawGridBlockMesh();

            // BlockMesh を描画します。
            DrawBlockMesh();
        }

        void DrawGridBlockMesh()
        {
            // GridBlockMesh の面の描画の有無を決定します。
            var grid = workspace.GridBlockMesh;

            grid.SetVisibilities(CurrentView.Position);

            var effect = grid.Effect;
            effect.View = CurrentView.Matrix;
            effect.Projection = Projection.Matrix;

            grid.Draw();
        }

        void DrawBlockMesh()
        {
            if (mesh == null) return;

            // LOD が無効な場合は描画しません。
            if (mesh.LevelOfDetailCount <= LevelOfDetail) return;

            // 指定の BlockMeshLod がロード済みではないならば、
            // より荒いロード済みの LOD を探します。
            int targetLod = LevelOfDetail;
            while (targetLod < mesh.LevelOfDetailCount && !mesh.MeshLods[targetLod].IsLoaded)
            {
                targetLod++;
            }

            // 描画に使える BlockMeshLod がまだ無いならば描画しません。
            if (mesh.LevelOfDetailCount == targetLod || !mesh.MeshLods[targetLod].IsLoaded)
                return;

            mesh.LevelOfDetail = targetLod;

            var effect = workspace.BasicBlockEffect;

            effect.View = CurrentView.Matrix;
            effect.Projection = Projection.Matrix;
            SetDirectionalLights(effect);

            mesh.Draw(effect);
        }

        void SetDirectionalLights(IBlockEffect effect)
        {
            effect.AmbientLightColor = AmbientLightColor;

            SetDirectionalLight(DirectionalLightModel0, effect.DirectionalLight0);
            SetDirectionalLight(DirectionalLightModel1, effect.DirectionalLight1);
            SetDirectionalLight(DirectionalLightModel2, effect.DirectionalLight2);
        }

        void SetDirectionalLight(DirectionalLightModel model, DirectionalLight light)
        {
            light.Enabled = model.Enabled;

            if (model.Enabled)
            {
                light.Direction = model.Direction;
                light.DiffuseColor = model.DiffuseColor;
                light.SpecularColor = model.SpecularColor;
            }
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
                if (mesh != null) mesh.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
