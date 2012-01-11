#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.Debug;
using Willcraftia.Xna.Framework.Serialization;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics.Demo
{
    public class BlockModelViewGame : Game
    {
        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;

        string modelJson;

        BlockModel model;

        BasicEffect basicEffect;

        TimeRulerMarker updateMarker;

        TimeRulerMarker drawMarker;

        DepthStencilState modelDrawDepthStencilState = new DepthStencilState();

        public BlockModelViewGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            var fpsCounter = new FpsCounter(this);
            fpsCounter.Content.RootDirectory = "Content";
            fpsCounter.HorizontalAlignment = DebugHorizontalAlignment.Right;
            fpsCounter.SampleSpan = TimeSpan.FromSeconds(2);
            Components.Add(fpsCounter);

            var timeRuler = new TimeRuler(this);
            Components.Add(timeRuler);

            // テスト用にメモリ上で Block の JSON データを作ります。
            //var block = CreateSimpleBlock();
            var block = CreateFullFilledBlock(16);
            modelJson = JsonHelper.ToJson<Block>(block);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            basicEffect = new BasicEffect(GraphicsDevice);
            basicEffect.EnableDefaultLighting();

            // 実際のアプリケーションではファイルからロードします。
            var block = JsonHelper.FromJson<Block>(modelJson);

            var factory = new BlockModelFactory(GraphicsDevice);
            model = factory.CreateBlockModel(block);

            updateMarker = Services.GetRequiredService<ITimeRulerService>().CreateMarker();
            updateMarker.Name = "Draw";
            updateMarker.BarIndex = 0;
            updateMarker.Color = Color.Blue;

            drawMarker = Services.GetRequiredService<ITimeRulerService>().CreateMarker();
            drawMarker.Name = "Draw";
            updateMarker.BarIndex = 1;
            drawMarker.Color = Color.Yellow;
        }

        protected override void UnloadContent()
        {
            Services.GetRequiredService<ITimeRulerService>().ReleaseMarker(drawMarker);
            spriteBatch.Dispose();
            basicEffect.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            drawMarker.Begin();

            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1, 0);

            float time = (float) gameTime.TotalGameTime.TotalSeconds;
            float yaw = time * 0.4f;
            float pitch = time * 0.7f;
            float roll = time * 1.1f;

            Vector3 cameraPosition = new Vector3(0, 0, 50.0f);

            float aspect = GraphicsDevice.Viewport.AspectRatio;

            // 原点を BlockModel の中心にします。
            var originTransform = Matrix.CreateTranslation(new Vector3(-8));

            Matrix world = originTransform * Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);
            Matrix view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 100);

            basicEffect.View = view;
            basicEffect.Projection = projection;
            basicEffect.VertexColorEnabled = false;
            basicEffect.EnableDefaultLighting();

            // Z バッファを有効にします。
            // デバッグ機能などで SpriteBatch を用いていると他の状態へ設定されているため、
            // 必要なタイミングで常に上書きするようにします。
            GraphicsDevice.DepthStencilState = modelDrawDepthStencilState;

            DrawWithoutOptimization(world, view, projection);

            drawMarker.End();

            base.Draw(gameTime);
        }

        void DrawWithoutOptimization(Matrix world, Matrix view, Matrix projection)
        {
            foreach (var mesh in model.Meshes)
            {
                basicEffect.World = mesh.Transform * world;

                var material = mesh.Material;
                basicEffect.DiffuseColor = material.DiffuseColor;
                basicEffect.EmissiveColor = material.EmissiveColor;
                basicEffect.SpecularColor = material.SpecularColor;
                basicEffect.SpecularPower = material.SpecularPower;
                basicEffect.Alpha = material.Alpha;

                mesh.Draw(basicEffect);
            }
        }

        /// <summary>
        /// n * n * n 全てを使用した Block を生成します。
        /// </summary>
        /// <returns></returns>
        Block CreateFullFilledBlock(int n)
        {
            var block = new Block();
            block.Materials = new List<Material>();
            block.Elements = new List<Element>();

            block.Materials.Add(new Material()
            {
                DiffuseColor = new MaterialColor(255, 0, 0),
                Alpha = 1
            });
            block.Materials.Add(new Material()
            {
                DiffuseColor = new MaterialColor(0, 255, 0),
                EmissiveColor = new MaterialColor(127, 127, 0),
                Alpha = 0.3f
            });
            block.Materials.Add(new Material()
            {
                DiffuseColor = new MaterialColor(0, 0, 255),
                SpecularColor = new MaterialColor(255, 255, 255),
                SpecularPower = 0.5f,
                Alpha = 1
            });

            for (int x = 0; x < n; x++)
            {
                for (int y = 0; y < n; y++)
                {
                    for (int z = 0; z < n; z++)
                    {
                        int materialIndex = z % 3;
                        block.Elements.Add(new Element() { Position = new Position(x, y, z), MaterialIndex = materialIndex });
                    }
                }
            }

            return block;
        }

        /// <summary>
        /// 簡単な Block を生成します。
        /// </summary>
        /// <returns>生成された Block。</returns>
        Block CreateSimpleBlock()
        {
            var block = new Block();
            block.Materials = new List<Material>();
            block.Elements = new List<Element>();

            var material = new Material()
            {
                DiffuseColor = new MaterialColor(63, 127, 255)
            };
            block.Materials.Add(material);

            block.Elements.Add(new Element() { Position = new Position(0, 0, 0), MaterialIndex = 0 });
            block.Elements.Add(new Element() { Position = new Position(0, 0, 16), MaterialIndex = 0 });
            block.Elements.Add(new Element() { Position = new Position(16, 0, 16), MaterialIndex = 0 });
            block.Elements.Add(new Element() { Position = new Position(16, 0, 0), MaterialIndex = 0 });
            block.Elements.Add(new Element() { Position = new Position(0, 16, 0), MaterialIndex = 0 });
            block.Elements.Add(new Element() { Position = new Position(0, 16, 16), MaterialIndex = 0 });
            block.Elements.Add(new Element() { Position = new Position(16, 16, 16), MaterialIndex = 0 });
            block.Elements.Add(new Element() { Position = new Position(16, 16, 0), MaterialIndex = 0 });

            return block;
        }
    }
}
