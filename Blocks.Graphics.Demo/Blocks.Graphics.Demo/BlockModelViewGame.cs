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

        TimeRulerMarker drawMarker;

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
            var block = CreateSimpleBlock();
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

            var factory = new BlockModelFactory();
            model = factory.CreateBlockModel(GraphicsDevice, block);

            drawMarker = Services.GetRequiredService<ITimeRulerService>().CreateMarker();
            drawMarker.Name = "Draw";
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

            GraphicsDevice.Clear(Color.CornflowerBlue);

            float time = (float) gameTime.TotalGameTime.TotalSeconds;
            float yaw = time * 0.4f;
            float pitch = time * 0.7f;
            float roll = time * 1.1f;

            Vector3 cameraPosition = new Vector3(0, 0, 50.0f);

            float aspect = GraphicsDevice.Viewport.AspectRatio;

            Matrix world = Matrix.CreateFromYawPitchRoll(yaw, pitch, roll);
            Matrix view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            Matrix projection = Matrix.CreatePerspectiveFieldOfView(1, aspect, 1, 100);

            basicEffect.View = view;
            basicEffect.Projection = projection;

            foreach (var mesh in model.Meshes)
            {
                basicEffect.World = mesh.Transform * world;

                var material = model.Materials[mesh.MaterialIndex];
                basicEffect.DiffuseColor = material.DiffuseColor;
                basicEffect.EmissiveColor = material.EmissiveColor;
                basicEffect.SpecularColor = material.SpecularColor;
                basicEffect.SpecularPower = material.SpecularPower;
                basicEffect.Alpha = material.Alpha;

                mesh.Draw(basicEffect);
            }

            base.Draw(gameTime);

            drawMarker.End();
        }

        /// <summary>
        /// 簡単な Block を生成します。
        /// </summary>
        /// <returns>生成された Block。</returns>
        static Block CreateSimpleBlock()
        {
            var block = new Block();
            block.Materials = new List<Material>();
            block.Meshes = new List<BlockMesh>();

            var material = new Material()
            {
                DiffuseColor = new MaterialColor(63, 127, 255),
                Alpha = 1
            };
            block.Materials.Add(material);

            var cube_0_0_0 = new BlockMesh()
            {
                Position = new Position(0, 0, 0),
                MaterialIndex = 0
            };
            block.Meshes.Add(cube_0_0_0);

            var cube_0_0_16 = new BlockMesh()
            {
                Position = new Position(0, 0, 16),
                MaterialIndex = 0
            };
            block.Meshes.Add(cube_0_0_16);

            var cube_16_0_16 = new BlockMesh()
            {
                Position = new Position(16, 0, 16),
                MaterialIndex = 0
            };
            block.Meshes.Add(cube_16_0_16);

            var cube_16_0_0 = new BlockMesh()
            {
                Position = new Position(16, 0, 0),
                MaterialIndex = 0
            };
            block.Meshes.Add(cube_16_0_0);


            var cube_0_16_0 = new BlockMesh()
            {
                Position = new Position(0, 16, 0),
                MaterialIndex = 0
            };
            block.Meshes.Add(cube_0_16_0);

            var cube_0_16_16 = new BlockMesh()
            {
                Position = new Position(0, 16, 16),
                MaterialIndex = 0
            };
            block.Meshes.Add(cube_0_16_16);

            var cube_16_16_16 = new BlockMesh()
            {
                Position = new Position(16, 16, 16),
                MaterialIndex = 0
            };
            block.Meshes.Add(cube_16_16_16);

            var cube_16_16_0 = new BlockMesh()
            {
                Position = new Position(16, 16, 0),
                MaterialIndex = 0
            };
            block.Meshes.Add(cube_16_16_0);

            return block;
        }
    }
}
