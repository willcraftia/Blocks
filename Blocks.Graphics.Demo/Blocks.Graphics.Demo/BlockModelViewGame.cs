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
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Serialization;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics.Demo
{
    public class BlockModelViewGame : Game
    {
        // 描画方法
        enum DrawMethod
        {
            // 最適化無しの描画
            NoOptimization,
            LodNoOptimization,
            // マテリアルバッチを使った描画
            MaterialBatch,
            LodMaterialBatch,
            // ハードウェアインスタンスを使った描画
            HardwareInstancing,
            LodHardwareInstancing,
            // ゲーム用の配列をそのまま使って描画
            DirectMapping,
            LodDirectMapping,
            MaxCount,
        }

        // 初期表示モデル数
        public const int InitialGameObjectCount = 500;

        // 最大表示モデル数
        public const int MaxGameObjectCount = 50000;

        // ゲームオブジェクトが移動できる領域
        public static BoundingBox Sandbox = new BoundingBox();

        GraphicsDeviceManager graphics;

        SpriteBatch spriteBatch;

        SpriteFont font;

        Texture2D fillTexture;

        string modelJson;

        Effect instancingEffect;

        BlockMeshFactory blockMeshFactory;
        BlockMeshFactory instancingBlockMeshFactory;

        int lodSize = 4;
        BlockMesh mesh;
        BlockMesh instancedMesh;
        float[] lodDistanceSquareds = { 80 * 80, 160 * 160, 240 * 240, 480 * 480 };

        TimeRuler timeRuler;

        TimeRulerMarker updateMarker;

        TimeRulerMarker drawMarker;

        // 現在使用している描画方法
        DrawMethod drawMethod = DrawMethod.NoOptimization;

        // ターゲットゲームオブジェクト表示数
        int targetObjectCount = InitialGameObjectCount;

        // ゲームオブジェクト配列
        GameObject[] gameObjects;

        // 確保しているゲームオブジェクト数
        int gameObjectCount;

        GameObject[][] lodGameObjects;
        int[] lodGameObjectCount;

        // ゲームオブジェクト初期化に使う乱数発生インスタンス
        Random ramdom = new Random(0);

        // ステータスに表示する文字列
        StringBuilder statusString = new StringBuilder(128);

        // カメラ座標
        Vector3 cameraPosition;

        // ビュー行列
        Matrix view;

        // 射影行列
        Matrix projection;

        // 描画用のインスタンス情報を格納する為の配列
        ObjectInstanceVertex[] objectInstances = new ObjectInstanceVertex[MaxGameObjectCount];

        // HWインスタンスで使うインスタンス情報を入れるための頂点バッファ
        WritableVertexBuffer<ObjectInstanceVertex> instanceVertexBuffer;
        WritableVertexBuffer<GameObject> directMappingVertexBuffer;

        // 頂点バインディング情報
        VertexBufferBinding[] vertexBufferBindings = new VertexBufferBinding[2];

        // Aボタンが押されていたか
        bool pressedA;

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

            timeRuler = new TimeRuler(this);
            Components.Add(timeRuler);

            updateMarker = timeRuler.CreateMarker();
            updateMarker.Name = "Draw";
            updateMarker.BarIndex = 0;
            updateMarker.Color = Color.Cyan;

            drawMarker = timeRuler.CreateMarker();
            drawMarker.Name = "Draw";
            drawMarker.BarIndex = 1;
            drawMarker.Color = Color.Yellow;

            // テスト用にメモリ上で Block の JSON データを作ります。
            //var block = CreateFullFilledBlock();
            var block = CreateOctahedronLikeBlock();
            modelJson = JsonHelper.ToJson<Block>(block);

            UpdateStatusString();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/Default");
            fillTexture = Texture2DHelper.CreateFillTexture(GraphicsDevice);

            blockMeshFactory = new BlockMeshFactory(GraphicsDevice, new BasicBlockEffectFactory(GraphicsDevice));

            instancingEffect = Content.Load<Effect>("Effects/Instancing");
            instancingBlockMeshFactory = new BlockMeshFactory(GraphicsDevice, new InstancingBlockEffectFactory(instancingEffect));

            // 実際のアプリケーションではファイルからロードします。
            var block = JsonHelper.FromJson<Block>(modelJson);

            mesh = blockMeshFactory.CreateBlockMesh(block, lodSize);
            foreach (var effect in mesh.Effects) effect.EnableDefaultLighting();

            instancedMesh = instancingBlockMeshFactory.CreateBlockMesh(block, lodSize);
            foreach (var effect in instancedMesh.Effects) effect.EnableDefaultLighting();

            float aspectRatio = GraphicsDevice.Viewport.AspectRatio;

            cameraPosition = new Vector3(0, 0, 30);
            view = Matrix.CreateLookAt(cameraPosition, Vector3.Zero, Vector3.Up);
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.ToRadians(30), aspectRatio, 1, 1000);

            // インスタンス情報を格納する為の頂点バッファの生成
            instanceVertexBuffer = new WritableVertexBuffer<ObjectInstanceVertex>(GraphicsDevice, MaxGameObjectCount * 2);
            directMappingVertexBuffer = new WritableVertexBuffer<GameObject>(GraphicsDevice, MaxGameObjectCount * 2);

            // モデルの移動範囲の設定
            float sandBoxSize = 20.0f;
            Sandbox.Min.X = sandBoxSize * -0.5f * aspectRatio;
            Sandbox.Min.Y = sandBoxSize * -0.5f;
            Sandbox.Max.X = sandBoxSize * 0.5f * aspectRatio;
            Sandbox.Max.Y = sandBoxSize * 0.5f;

            // ゲームオブジェクトの初期化
            InitializeGameObjects();
        }

        protected override void UnloadContent()
        {
            Services.GetRequiredService<ITimeRulerService>().ReleaseMarker(updateMarker);
            Services.GetRequiredService<ITimeRulerService>().ReleaseMarker(drawMarker);
            if (spriteBatch != null) spriteBatch.Dispose();
            if (fillTexture != null) fillTexture.Dispose();
            if (instancingEffect != null) instancingEffect.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            timeRuler.StartFrame();
            updateMarker.Begin();

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Escape)) Exit();

            float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

            // Aボタン、またはスペースキーで次の描画手法に変更する
            if (keyState.IsKeyDown(Keys.Space))
            {
                if (!pressedA)
                    MoveToNextTechnique();

                pressedA = true;
            }
            else
            {
                pressedA = false;
            }

            // ゲームオブジェクト数を増やす
            float trigger = keyState.IsKeyDown(Keys.Up) ? 0.1f : 0.0f;
            if (trigger > 0.0f)
            {
                int addCount = Math.Max(1, (int) (trigger * 2000.0f * dt));
                targetObjectCount = Math.Min(targetObjectCount + addCount, MaxGameObjectCount);

                UpdateStatusString();
            }

            // ゲームオブジェクト数を減らす
            trigger = keyState.IsKeyDown(Keys.Down) ? 0.1f : 0.0f;

            if (trigger > 0.0f)
            {
                int subCount = Math.Max(1, (int) (trigger * 2000.0f * dt));

                targetObjectCount = Math.Max(targetObjectCount - subCount, 1);

                UpdateStatusString();
            }

            // 全ゲームオブジェクトの更新
            UpdateGameObjects(dt);

            updateMarker.End();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            drawMarker.Begin();

            GraphicsDevice.Clear(ClearOptions.Target | ClearOptions.DepthBuffer, Color.CornflowerBlue, 1, 0);

            // ゲームオブジェクトの描画
            DrawGameObjects();

            // ステータス表示
            float margin = font.LineSpacing * 0.2f;
            Vector2 size = font.MeasureString(statusString);
            size.Y += margin * 2.0f;

            var layout = new DebugLayout();
            layout.ContainerBounds = GraphicsDevice.Viewport.Bounds;
            layout.Width = (int) size.X;
            layout.Height = (int) size.Y;
            layout.HorizontalAlignment = DebugHorizontalAlignment.Left;
            layout.VerticalAlignment = DebugVerticalAlignment.Top;
            layout.HorizontalMargin = 8;
            layout.VerticalMargin = 8;
            layout.Arrange();

            spriteBatch.Begin();
            spriteBatch.Draw(fillTexture, layout.ArrangedBounds, new Color(0, 0, 0, 200));
            spriteBatch.DrawString(
                font, statusString, new Vector2(layout.ArrangedBounds.X, layout.ArrangedBounds.Y + margin), Color.White);
            spriteBatch.End();

            drawMarker.End();

            base.Draw(gameTime);
        }

        Block CreateOctahedronLikeBlock()
        {
            var block = new Block();
            block.Materials = new List<Material>();
            block.Elements = new List<Element>();

            MaterialColor[] diffuses =
            {
                new MaterialColor(255, 255, 255),
                new MaterialColor(255,   0,   0),
                new MaterialColor(  0, 255,   0),
                new MaterialColor(  0,   0, 255),
                new MaterialColor(127, 127,   0),
                new MaterialColor(127,   0, 127),
                new MaterialColor(  0, 127, 127),
                new MaterialColor(  0,   0,   0),
            };
            Material[] materials = new Material[8];
            for (int i = 0; i < 8; i++)
            {
                materials[i] = new Material();
                materials[i].DiffuseColor = diffuses[i];
                block.Materials.Add(materials[i]);
            }

            int materialIndex;
            for (int x = -8; x < 8; x++)
            {
                for (int y = -8; y < 8; y++)
                {
                    for (int z = -8; z < 8; z++)
                    {
                        int testX = (x < 0) ? -x : x + 1;
                        int testY = (y < 0) ? -y : y + 1;
                        int testZ = (z < 0) ? -z : z + 1;

                        if (testX + testY + testZ <= 10)
                        {
                            materialIndex = 0;
                            if (x < 0) materialIndex |= 1;
                            if (y < 0) materialIndex |= 2;
                            if (z < 0) materialIndex |= 4;

                            var element = new Element();
                            element.Position = new Position(x, y, z);
                            element.MaterialIndex = materialIndex;
                            block.Elements.Add(element);
                        }
                    }
                }
            }

            return block;
        }

        /// <summary>
        /// 16 * 16 * 16 の全てを使用した Block を生成します。
        /// </summary>
        Block CreateFullFilledBlock()
        {
            var block = new Block();
            block.Materials = new List<Material>();
            block.Elements = new List<Element>();

            MaterialColor[] diffuses =
            {
                new MaterialColor(255, 255, 255),
                new MaterialColor(255,   0,   0),
                new MaterialColor(  0, 255,   0),
                new MaterialColor(  0,   0, 255),
                new MaterialColor(127, 127,   0),
                new MaterialColor(127,   0, 127),
                new MaterialColor(  0, 127, 127),
                new MaterialColor(  0,   0,   0),
            };
            Material[] materials = new Material[8];
            for (int i = 0; i < 8; i++)
            {
                materials[i] = new Material();
                materials[i].DiffuseColor = diffuses[i];
                block.Materials.Add(materials[i]);
            }

            int materialIndex;
            for (int x = -8; x < 8; x++)
            {
                for (int y = -8; y < 8; y++)
                {
                    for (int z = -8; z < 8; z++)
                    {
                        materialIndex = 0;
                        if (x < 0) materialIndex |= 1;
                        if (y < 0) materialIndex |= 2;
                        if (z < 0) materialIndex |= 4;

                        var element = new Element();
                        element.Position = new Position(x, y, z);
                        element.MaterialIndex = materialIndex;
                        block.Elements.Add(element);
                    }
                }
            }

            return block;
        }

        /// <summary>
        /// ゲームオブジェクトの初期化
        /// </summary>
        void InitializeGameObjects()
        {
            gameObjects = new GameObject[MaxGameObjectCount];
            gameObjectCount = InitialGameObjectCount;
            for (int i = 0; i < gameObjectCount; ++i) gameObjects[i].Initialize(ramdom);

            lodGameObjects = new GameObject[lodSize][];
            for (int i = 0; i < lodSize; i++) lodGameObjects[i] = new GameObject[MaxGameObjectCount];
            lodGameObjectCount = new int[lodSize];

            UpdateStatusString();
        }

        /// <summary>
        /// ゲームオブジェクトの更新
        /// </summary>
        void UpdateGameObjects(float dt)
        {
            for (int i = 0; i < lodGameObjectCount.Length; i++) lodGameObjectCount[i] = 0;

            // 全オブジェクトを更新する
            // ゲームオブジェクトのライフ時間によって、ゲームオブジェクトを
            // リストからはずすので単純なforeachはここでは使えない
            for (int i = 0; i < gameObjectCount; )
            {
                if (gameObjects[i].Update(dt))
                {
                    ++i;

                    float distanceSquared;
                    Vector3.DistanceSquared(ref cameraPosition, ref gameObjects[i].Position, out distanceSquared);
                    for (int lod = 0; lod < lodGameObjectCount.Length; lod++)
                    {
                        if (distanceSquared < lodDistanceSquareds[lod])
                        {
                            lodGameObjects[lod][lodGameObjectCount[lod]] = gameObjects[i];
                            lodGameObjectCount[lod] += 1;
                            break;
                        }
                    }
                }
                else
                {
                    // ゲームオブジェクトの消去
                    // 最後尾のゲームオブジェクトと交換する
                    var temp = gameObjects[i];
                    gameObjects[i] = gameObjects[--gameObjectCount];
                    gameObjects[gameObjectCount] = temp;
                }
            }

            if (targetObjectCount < gameObjectCount) gameObjectCount = targetObjectCount;

            while (gameObjectCount < targetObjectCount) gameObjects[gameObjectCount++].Initialize(ramdom);
        }

        /// <summary>
        /// ゲームオブジェクトの描画
        /// </summary>
        void DrawGameObjects()
        {
            // デバッグ機能などで SpriteBatch を用いていると他の状態へ設定されているため、
            // 必要なタイミングで常に上書きするようにします。
            GraphicsDevice.BlendState = BlendState.Opaque;
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

            switch (drawMethod)
            {
                case DrawMethod.NoOptimization:
                    DrawGameObjectsWithoutOptimization();
                    break;
                case DrawMethod.LodNoOptimization:
                    DrawGameObjectsLodWithoutOptimization();
                    break;
                case DrawMethod.MaterialBatch:
                    DrawGameObjectsWithBatch();
                    break;
                case DrawMethod.LodMaterialBatch:
                    DrawGameObjectsLodWithBatch();
                    break;
                case DrawMethod.HardwareInstancing:
                    DrawGameObjectsWithHardwareInstancing();
                    break;
                case DrawMethod.LodHardwareInstancing:
                    DrawGameObjectsLodWithHardwareInstancing();
                    break;
                case DrawMethod.DirectMapping:
                    DrawGameObjectsWithDirectMapping();
                    break;
                case DrawMethod.LodDirectMapping:
                    DrawGameObjectsLodWithDirectMapping();
                    break;
            }
        }

        void DrawGameObjectsWithoutOptimization()
        {
            foreach (var effect in mesh.Effects)
            {
                effect.View = view;
                effect.Projection = projection;
            }

            mesh.LevelOfDetail = 0;

            for (int i = 0; i < gameObjectCount; i++)
            {
                Matrix world =
                    Matrix.CreateScale(gameObjects[i].Scale) *
                    Matrix.CreateFromAxisAngle(gameObjects[i].RotateAxis, gameObjects[i].Rotation);
                world.Translation = gameObjects[i].Position;

                foreach (var effect in mesh.Effects) effect.World = world;

                mesh.Draw();
            }
        }

        void DrawGameObjectsLodWithoutOptimization()
        {
            foreach (var effect in mesh.Effects)
            {
                effect.View = view;
                effect.Projection = projection;
            }

            for (int lod = 0; lod < lodSize; lod++)
            {
                mesh.LevelOfDetail = lod;

                for (int i = 0; i < lodGameObjectCount[lod]; i++)
                {
                    var gameObject = lodGameObjects[lod][i];
                    Matrix world =
                        Matrix.CreateScale(gameObject.Scale) *
                        Matrix.CreateFromAxisAngle(gameObject.RotateAxis, gameObject.Rotation);
                    world.Translation = gameObject.Position;

                    foreach (var effect in mesh.Effects) effect.World = world;

                    mesh.Draw();
                }
            }
        }

        void DrawGameObjectsWithBatch()
        {
            foreach (var effect in mesh.Effects)
            {
                effect.View = view;
                effect.Projection = projection;
            }

            mesh.LevelOfDetail = 0;

            foreach (var meshPart in mesh.MeshParts)
            {
                GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer, meshPart.VertexOffset);
                GraphicsDevice.Indices = meshPart.IndexBuffer;

                for (int i = 0; i < gameObjectCount; i++)
                {
                    Matrix world =
                        Matrix.CreateScale(gameObjects[i].Scale) *
                        Matrix.CreateFromAxisAngle(gameObjects[i].RotateAxis, gameObjects[i].Rotation);
                    world.Translation = gameObjects[i].Position;

                    var effect = meshPart.Effect;
                    effect.World = world;
                    effect.Pass.Apply();

                    GraphicsDevice.DrawIndexedPrimitives(
                        PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
                }
            }
        }

        void DrawGameObjectsLodWithBatch()
        {
            foreach (var effect in mesh.Effects)
            {
                effect.View = view;
                effect.Projection = projection;
            }

            for (int lod = 0; lod < lodSize; lod++)
            {
                mesh.LevelOfDetail = lod;

                foreach (var meshPart in mesh.MeshParts)
                {
                    GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer, meshPart.VertexOffset);
                    GraphicsDevice.Indices = meshPart.IndexBuffer;

                    for (int i = 0; i < lodGameObjectCount[lod]; i++)
                    {
                        var gameObject = lodGameObjects[lod][i];
                        Matrix world =
                            Matrix.CreateScale(gameObject.Scale) *
                            Matrix.CreateFromAxisAngle(gameObject.RotateAxis, gameObject.Rotation);
                        world.Translation = gameObject.Position;

                        var effect = meshPart.Effect;
                        effect.World = world;
                        effect.Pass.Apply();

                        GraphicsDevice.DrawIndexedPrimitives(
                            PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
                    }
                }
            }
        }

        void DrawGameObjectsWithHardwareInstancing()
        {
            foreach (InstancingBlockEffect effect in instancedMesh.Effects)
            {
                effect.View = view;
                effect.Projection = projection;
            }

            // インスタンス情報を一旦コピー
            for (int i = 0; i < gameObjectCount; ++i)
            {
                objectInstances[i].Position = gameObjects[i].Position;
                objectInstances[i].Scale = gameObjects[i].Scale;
                objectInstances[i].RotateAxis = gameObjects[i].RotateAxis;
                objectInstances[i].Rotation = gameObjects[i].Rotation;
            }

            // インスタンス用の頂点バッファへ書き込む
            int offset = instanceVertexBuffer.SetData(objectInstances, 0, gameObjectCount);

            instancedMesh.LevelOfDetail = 0;

            // ゲームオブジェクトを描画
            foreach (var meshPart in instancedMesh.MeshParts)
            {
                vertexBufferBindings[0] = new VertexBufferBinding(meshPart.VertexBuffer, meshPart.VertexOffset);
                vertexBufferBindings[1] = new VertexBufferBinding(instanceVertexBuffer.VertexBuffer, offset, 1);

                GraphicsDevice.SetVertexBuffers(vertexBufferBindings);
                GraphicsDevice.Indices = meshPart.IndexBuffer;

                meshPart.Effect.Pass.Apply();

                GraphicsDevice.DrawInstancedPrimitives(
                    PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount, gameObjectCount);
            }
        }

        void DrawGameObjectsLodWithHardwareInstancing()
        {
            foreach (InstancingBlockEffect effect in instancedMesh.Effects)
            {
                effect.View = view;
                effect.Projection = projection;
            }

            for (int lod = 0; lod < lodSize; lod++)
            {
                if (lodGameObjectCount[lod] == 0) continue;

                // インスタンス情報を一旦コピー
                for (int i = 0; i < lodGameObjectCount[lod]; ++i)
                {
                    var gameObject = lodGameObjects[lod][i];
                    objectInstances[i].Position = gameObject.Position;
                    objectInstances[i].Scale = gameObject.Scale;
                    objectInstances[i].RotateAxis = gameObject.RotateAxis;
                    objectInstances[i].Rotation = gameObject.Rotation;
                }

                // インスタンス用の頂点バッファへ書き込む
                int offset = instanceVertexBuffer.SetData(objectInstances, 0, lodGameObjectCount[lod]);

                instancedMesh.LevelOfDetail = lod;

                // ゲームオブジェクトを描画
                foreach (var meshPart in instancedMesh.MeshParts)
                {
                    vertexBufferBindings[0] = new VertexBufferBinding(meshPart.VertexBuffer, meshPart.VertexOffset);
                    vertexBufferBindings[1] = new VertexBufferBinding(instanceVertexBuffer.VertexBuffer, offset, 1);

                    GraphicsDevice.SetVertexBuffers(vertexBufferBindings);
                    GraphicsDevice.Indices = meshPart.IndexBuffer;

                    meshPart.Effect.Pass.Apply();

                    GraphicsDevice.DrawInstancedPrimitives(
                        PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount, lodGameObjectCount[lod]);
                }
            }
        }

        void DrawGameObjectsWithDirectMapping()
        {
            foreach (InstancingBlockEffect effect in instancedMesh.Effects)
            {
                effect.View = view;
                effect.Projection = projection;
            }

            //　インスタンスをそのまま頂点バッファへコピー
            int offset = directMappingVertexBuffer.SetData(gameObjects, 0, gameObjectCount);

            instancedMesh.LevelOfDetail = 0;

            // ゲームオブジェクトを描画
            foreach (var meshPart in instancedMesh.MeshParts)
            {
                vertexBufferBindings[0] = new VertexBufferBinding(meshPart.VertexBuffer, meshPart.VertexOffset);
                vertexBufferBindings[1] = new VertexBufferBinding(directMappingVertexBuffer.VertexBuffer, offset, 1);

                GraphicsDevice.SetVertexBuffers(vertexBufferBindings);
                GraphicsDevice.Indices = meshPart.IndexBuffer;

                meshPart.Effect.Pass.Apply();

                GraphicsDevice.DrawInstancedPrimitives(
                    PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount, gameObjectCount);
            }
        }

        void DrawGameObjectsLodWithDirectMapping()
        {
            foreach (InstancingBlockEffect effect in instancedMesh.Effects)
            {
                effect.View = view;
                effect.Projection = projection;
            }

            for (int lod = 0; lod < lodSize; lod++)
            {
                if (lodGameObjectCount[lod] == 0) continue;

                //　インスタンスをそのまま頂点バッファへコピー
                int offset = directMappingVertexBuffer.SetData(lodGameObjects[lod], 0, lodGameObjectCount[lod]);

                instancedMesh.LevelOfDetail = lod;

                // ゲームオブジェクトを描画
                foreach (var meshPart in instancedMesh.MeshParts)
                {
                    vertexBufferBindings[0] = new VertexBufferBinding(meshPart.VertexBuffer, meshPart.VertexOffset);
                    vertexBufferBindings[1] = new VertexBufferBinding(directMappingVertexBuffer.VertexBuffer, offset, 1);

                    GraphicsDevice.SetVertexBuffers(vertexBufferBindings);
                    GraphicsDevice.Indices = meshPart.IndexBuffer;

                    meshPart.Effect.Pass.Apply();

                    GraphicsDevice.DrawInstancedPrimitives(
                        PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount, lodGameObjectCount[lod]);
                }
            }
        }

        /// <summary>
        /// 次の描画方法に変更する
        /// </summary>
        void MoveToNextTechnique()
        {
            drawMethod = (DrawMethod) (drawMethod + 1);
            if (drawMethod == DrawMethod.MaxCount) drawMethod = 0;

            UpdateStatusString();
        }

        /// <summary>
        /// ステータス文字列の更新
        /// </summary>
        void UpdateStatusString()
        {
            statusString.Length = 0;
            statusString.Append(" Draw Method: ");
            statusString.Append(drawMethod.ToString());
            statusString.Append(" \n Objects: ");
            statusString.Append(gameObjectCount);
        }
    }
}
