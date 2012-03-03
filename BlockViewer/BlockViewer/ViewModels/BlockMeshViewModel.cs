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
        /// <summary>
        /// 現在の ViewMode の View。
        /// </summary>
        ChaseView currentView;

        /// <summary>
        /// 通常のカメラの View。
        /// </summary>
        ChaseView camera;

        /// <summary>
        /// Projection。
        /// Projection は通常カメラと Directional Light で共通。
        /// </summary>
        PerspectiveFov projection;

        /// <summary>
        /// Ambient。
        /// </summary>
        Vector3 ambientLightColor = new Vector3(0.05333332f, 0.09882354f, 0.1819608f);

        Mode mode;

        public MainViewModel MainViewModel { get; private set; }

        public int LevelOfDetail { get; set; }

        public float CameraMoveScale { get; set; }

        public bool CameraMovable { get; set; }

        public bool GridVisible { get; set; }

        /// <summary>
        /// 選択されている Directional Light を取得します。
        /// </summary>
        public LightViewModel SelectedLightViewModel { get; private set; }

        /// <summary>
        /// Directional Light 0 を取得します。
        /// </summary>
        public LightViewModel Light0ViewModel { get; private set; }

        /// <summary>
        /// Directional Light 1 を取得します。
        /// </summary>
        public LightViewModel Light1ViewModel { get; private set; }

        /// <summary>
        /// Directional Light 2 を取得します。
        /// </summary>
        public LightViewModel Light2ViewModel { get; private set; }

        public Mode Mode
        {
            get { return mode; }
            set
            {
                if (mode == value) return;

                mode = value;

                switch (mode)
                {
                    case Mode.Camera:
                        currentView = camera;
                        break;
                    case Mode.DirectionalLight0:
                        SelectedLightViewModel = Light0ViewModel;
                        currentView = SelectedLightViewModel.View;
                        break;
                    case Mode.DirectionalLight1:
                        SelectedLightViewModel = Light1ViewModel;
                        currentView = SelectedLightViewModel.View;
                        break;
                    case Mode.DirectionalLight2:
                        SelectedLightViewModel = Light2ViewModel;
                        currentView = SelectedLightViewModel.View;
                        break;
                    default:
                        throw new InvalidOperationException();
                }
            }
        }

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
            Light0ViewModel = new LightViewModel
            {
                Enabled = true,
                DiffuseColor = new Vector3(1, 0.9607844f, 0.8078432f),
                SpecularColor = new Vector3(1, 0.9607844f, 0.8078432f)
            };
            Light0ViewModel.View.Distance = defaultDistance;
            Light0ViewModel.View.Angle = new Vector2(-MathHelper.PiOver4, MathHelper.PiOver4);

            Light1ViewModel = new LightViewModel
            {
                Enabled = true,
                DiffuseColor = new Vector3(0.9647059f, 0.7607844f, 0.4078432f),
                SpecularColor = Vector3.Zero
            };
            Light1ViewModel.View.Distance = defaultDistance;
            Light1ViewModel.View.Angle = new Vector2(MathHelper.PiOver4, -MathHelper.PiOver4);
            
            Light2ViewModel = new LightViewModel
            {
                Enabled = true,
                DiffuseColor = new Vector3(0.3231373f, 0.3607844f, 0.3937255f),
                SpecularColor = new Vector3(0.3231373f, 0.3607844f, 0.3937255f)
            };
            Light2ViewModel.View.Distance = defaultDistance;
            Light2ViewModel.View.Angle = new Vector2(MathHelper.PiOver4, MathHelper.Pi);

            projection = new PerspectiveFov
            {
                NearPlaneDistance = 0.01f,
                FarPlaneDistance = 10
            };

            CameraMoveScale = 0.05f;
            Mode = Mode.Camera;
            currentView = camera;
        }

        public void MoveCamera(Vector2 angleSign)
        {
            // カメラの位置の角度を更新します。
            var angle = currentView.Angle;
            angle.X += angleSign.X * CameraMoveScale;
            angle.Y += angleSign.Y * CameraMoveScale;
            angle.X = MathHelper.WrapAngle(angle.X);
            angle.Y = MathHelper.WrapAngle(angle.Y);
            currentView.Angle = angle;
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
            Light0ViewModel.View.Update();
            Light1ViewModel.View.Update();
            Light2ViewModel.View.Update();

            // GridBlockMesh を描画します。
            if (GridVisible) DrawGridBlockMesh();

            // BlockMesh を描画します。
            DrawBlockMesh();
        }

        void DrawGridBlockMesh()
        {
            var effect = MainViewModel.GridBlockMeshEffect;
            effect.View = currentView.Matrix;
            effect.Projection = projection.Matrix;

            // GridBlockMesh の面の描画の有無を決定します。
            var mesh = MainViewModel.GridBlockMesh;
            mesh.SetVisibilities(currentView.Position);

            mesh.Draw(effect);
        }

        void DrawBlockMesh()
        {
            var mesh = MainViewModel.BlockMesh;
            if (mesh == null) return;

            mesh.LevelOfDetail = LevelOfDetail;

            foreach (var effect in mesh.Effects)
            {
                effect.View = currentView.Matrix;
                effect.Projection = projection.Matrix;

                SetLights(effect);
            }

            mesh.Draw();
        }

        void SetLights(IBlockEffect effect)
        {
            effect.AmbientLightColor = ambientLightColor;

            SetLight(Light0ViewModel, effect.DirectionalLight0);
            SetLight(Light1ViewModel, effect.DirectionalLight1);
            SetLight(Light2ViewModel, effect.DirectionalLight2);
        }

        void SetLight(LightViewModel light, DirectionalLight dirLight)
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
