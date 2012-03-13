﻿#region Using

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

        BlockMeshFactory blockMeshFactory;

        GraphicsDevice graphicsDevice;

        ViewMode viewMode;

        readonly object loadSyncRoot = new object();

        string meshName;

        Block block;

        BlockMesh mesh;

        public string MeshName
        {
            get
            {
                lock (loadSyncRoot)
                {
                    return meshName;
                }
            }
            set
            {
                lock (loadSyncRoot)
                {
                    if (meshName == value) return;

                    meshName = value;

                    if (meshName != null)
                    {
                        if (mesh != null)
                        {
                            mesh.Dispose();
                            mesh = null;
                        }

                        var blockLoader = workspace.StorageModel.BlockLoader;
                        if (blockLoader != null)
                        {
                            BlockLoadTask.Start(blockLoader, meshName, BlockLoadTaskCallback);
                        }
                    }
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

        public Viewer(Workspace workspace, BlockMeshFactory blockMeshFactory)
        {
            if (workspace == null) throw new ArgumentNullException("workspace");
            if (blockMeshFactory == null) throw new ArgumentNullException("blockMeshFactory");
            this.workspace = workspace;
            this.blockMeshFactory = blockMeshFactory;

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

        public void Update(GameTime gameTime)
        {
            lock (loadSyncRoot)
            {
                if (block != null)
                {
                    // Block から BlockMesh を生成します。
                    mesh = blockMeshFactory.CreateBlockMesh(block);
                    // デフォルト ライティングを有効にしておきます。
                    foreach (var effect in mesh.Effects) effect.EnableDefaultLighting();

                    block = null;
                }
            }
        }

        void BlockLoadTaskCallback(string name, Block result)
        {
            Thread.Sleep(1000);

            lock (loadSyncRoot)
            {
                if (meshName == name)
                {
                    block = result;
                }
            }
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

            mesh.LevelOfDetail = LevelOfDetail;

            foreach (var effect in mesh.Effects)
            {
                effect.View = CurrentView.Matrix;
                effect.Projection = Projection.Matrix;

                SetDirectionalLights(effect);
            }

            mesh.Draw();
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
