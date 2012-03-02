#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.Cameras;
using Willcraftia.Xna.Blocks.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.ViewModels
{
    public sealed class BlockMeshViewModel
    {
        #region Light

        class Light
        {
            public ChaseView View { get; set; }

            public bool Enabled { get; set; }

            public Vector3 Direction
            {
                get
                {
                    var direction = -View.Position;
                    direction.Normalize();
                    return direction;
                }
            }

            public Vector3 DiffuseColor { get; set; }

            public Vector3 SpecularColor { get; set; }

            public Light()
            {
                View = new ChaseView
                {
                    Distance = 3.5f,
                    Angle = new Vector2(-MathHelper.PiOver4 * 0.5f, MathHelper.PiOver4)
                };
            }
        }

        #endregion

        /// <summary>
        /// 現在の ViewMode の View。
        /// </summary>
        ChaseView view;

        /// <summary>
        /// 通常のカメラの View。
        /// </summary>
        ChaseView camera;

        /// <summary>
        /// Directional Light 0。
        /// </summary>
        Light light0;

        /// <summary>
        /// Directional Light 1。
        /// </summary>
        Light light1;

        /// <summary>
        /// Directional Light 2。
        /// </summary>
        Light light2;

        /// <summary>
        /// Projection。
        /// Projection は通常カメラと Directional Light で共通。
        /// </summary>
        PerspectiveFov projection;

        /// <summary>
        /// Ambient。
        /// </summary>
        Vector3 ambientLightColor = new Vector3(0.05333332f, 0.09882354f, 0.1819608f);

        public MainViewModel MainViewModel { get; private set; }

        public int LevelOfDetail { get; set; }

        public float CameraMoveScale { get; set; }

        public bool CameraMovable { get; set; }

        public bool GridVisible { get; set; }

        public ViewMode ViewMode { get; set; }

        public BlockMeshViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;

            float defaultDistance = 3.5f;

            camera = new ChaseView
            {
                Distance = defaultDistance,
                Angle = new Vector2(-MathHelper.PiOver4 * 0.5f, MathHelper.PiOver4)
            };

            // DiffuseColor/SpecularColor は、BasicEffect のデフォルト値に合わせています。
            // Direction は値から角度を求めるのが面倒なので、
            // BasicEffect のデフォルト値には合わせません。
            // いずれも、デフォルト値に揃える必要性は特にありません。
            light0 = new Light
            {
                Enabled = true,
                DiffuseColor = new Vector3(1, 0.9607844f, 0.8078432f),
                SpecularColor = new Vector3(1, 0.9607844f, 0.8078432f)
            };
            light0.View.Distance = defaultDistance;
            light0.View.Angle = new Vector2(-MathHelper.PiOver4 * 0.5f, MathHelper.PiOver4);

            light1 = new Light
            {
                Enabled = true,
                DiffuseColor = new Vector3(0.9647059f, 0.7607844f, 0.4078432f),
                SpecularColor = Vector3.Zero
            };
            light1.View.Distance = defaultDistance;
            light1.View.Angle = new Vector2(MathHelper.PiOver4 * 0.5f, -MathHelper.PiOver4);
            
            light2 = new Light
            {
                Enabled = true,
                DiffuseColor = new Vector3(0.3231373f, 0.3607844f, 0.3937255f),
                SpecularColor = new Vector3(0.3231373f, 0.3607844f, 0.3937255f)
            };
            light2.View.Distance = defaultDistance;
            light2.View.Angle = new Vector2(MathHelper.PiOver4 * 0.5f, MathHelper.Pi);

            projection = new PerspectiveFov
            {
                NearPlaneDistance = 0.01f,
                FarPlaneDistance = 10
            };

            CameraMoveScale = 0.05f;
            ViewMode = ViewMode.Camera;
            view = camera;
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

        public void Draw()
        {
            var graphicsDevice = MainViewModel.GraphicsDevice;
            graphicsDevice.BlendState = BlendState.Opaque;
            graphicsDevice.DepthStencilState = DepthStencilState.Default;

            // カメラを更新します。
            projection.AspectRatio = graphicsDevice.Viewport.AspectRatio;
            camera.Update();
            projection.Update();

            // ライトを更新します。
            light0.View.Update();
            light1.View.Update();
            light2.View.Update();

            // GridBlockMesh を描画します。
            if (GridVisible) DrawGridBlockMesh();

            // BlockMesh を描画します。
            DrawBlockMesh();
        }

        void DrawGridBlockMesh()
        {
            var effect = MainViewModel.GridBlockMeshEffect;
            effect.View = view.Matrix;
            effect.Projection = projection.Matrix;

            // GridBlockMesh の面の描画の有無を決定します。
            var mesh = MainViewModel.GridBlockMesh;
            mesh.SetVisibilities(camera.Position);

            mesh.Draw(effect);
        }

        void DrawBlockMesh()
        {
            var mesh = MainViewModel.BlockMesh;
            if (mesh == null) return;

            mesh.LevelOfDetail = LevelOfDetail;

            foreach (var effect in mesh.Effects)
            {
                effect.View = view.Matrix;
                effect.Projection = projection.Matrix;

                SetLights(effect);
            }

            mesh.Draw();
        }

        void SetLights(IBlockEffect effect)
        {
            effect.AmbientLightColor = ambientLightColor;

            SetLight(light0, effect.DirectionalLight0);
            SetLight(light1, effect.DirectionalLight1);
            SetLight(light2, effect.DirectionalLight2);
        }

        void SetLight(Light light, DirectionalLight dirLight)
        {
            dirLight.Enabled = light.Enabled;
            if (light.Enabled)
            {
                dirLight.Direction = light.Direction;
                dirLight.DiffuseColor = light.DiffuseColor;
                dirLight.SpecularColor = light.SpecularColor;
            }
        }
    }
}
