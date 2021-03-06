#region Using

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework;
using Willcraftia.Xna.Framework.Debug;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.IO;
using Willcraftia.Xna.Blocks.Content;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics.Demo
{
    public class BlockModelViewGame : Game
    {
        #region DrawMethod

        /// <summary>
        /// 描画方法の列挙値です。
        /// </summary>
        enum DrawMethod
        {
            /// <summary>
            /// 最適化無しの描画。
            /// </summary>
            NoOptimization,
            /// <summary>
            /// LOD + 最適化無しの描画。
            /// </summary>
            LodNoOptimization,
            /// <summary>
            /// マテリアルバッチを使った描画。
            /// </summary>
            MaterialBatch,
            /// <summary>
            /// LOD + マテリアルバッチを使った描画。
            /// </summary>
            LodMaterialBatch,
            /// <summary>
            /// HW インスタンスを使った描画。
            /// </summary>
            HardwareInstancing,
            /// <summary>
            /// LOD + HW インスタンスを使った描画。
            /// </summary>
            LodHardwareInstancing,
            /// <summary>
            /// GameObject 配列をそのまま使って描画。
            /// </summary>
            DirectMapping,
            /// <summary>
            /// LOD + GameObject 配列をそのまま使って描画。
            /// </summary>
            LodDirectMapping,

            /// <summary>
            /// 列挙値の総数。
            /// </summary>
            MaxCount,
        }

        #endregion

        #region GameObjectCollection

        /// <summary>
        /// GameObject を管理するクラスです。
        /// </summary>
        class GameObjectCollection
        {
            /// <summary>
            /// GameObject 配列。
            /// </summary>
            public GameObject[] Items;

            /// <summary>
            /// 確保している GameObject 数。
            /// </summary>
            public int Count;
        }

        #endregion

        /// <summary>
        /// 初期表示 GameObject 数。
        /// </summary>
        public const int InitialGameObjectCount = 500;

        /// <summary>
        /// 最大表示 GameObject 数。
        /// </summary>
        public const int MaxGameObjectCount = 50000;

        /// <summary>
        /// GameObject が移動できる領域。
        /// </summary>
        public static BoundingBox Sandbox = new BoundingBox();

        /// <summary>
        /// Block の XmlSerializer。
        /// </summary>
        static XmlSerializer blockSerializer = new XmlSerializer(typeof(Block));

        /// <summary>
        /// GraphicsDeviceManager。
        /// </summary>
        GraphicsDeviceManager graphics;

        /// <summary>
        /// SpriteBatch。
        /// </summary>
        SpriteBatch spriteBatch;

        /// <summary>
        /// SpriteFont。
        /// </summary>
        SpriteFont font;

        /// <summary>
        /// 背景塗り潰しのためのテクスチャ。
        /// </summary>
        Texture2D fillTexture;

        /// <summary>
        /// シリアライズされた Block。
        /// </summary>
        string blockData;

        /// <summary>
        /// BlockMeshManager。
        /// </summary>
        BlockMeshManager meshManager;

        /// <summary>
        /// LOD サイズ。
        /// </summary>
        int lodCount = 4;

        /// <summary>
        /// 通常の Mesh。
        /// </summary>
        BlockMesh mesh;

        /// <summary>
        /// 通常の IBlockEffect。
        /// </summary>
        IBlockEffect blockEffect;

        /// <summary>
        /// インスタンシング用の IBlockEffect。
        /// </summary>
        IBlockEffect instancingBlockEffect;

        /// <summary>
        /// LOD を切り替える距離 (の二乗) の配列。
        /// </summary>
        float[] lodDistanceSquareds = { 80 * 80, 160 * 160, 240 * 240, 480 * 480 };

        /// <summary>
        /// TimeRuler。
        /// </summary>
        TimeRuler timeRuler;

        /// <summary>
        /// Update メソッドを計測するための TimeRulerMarker。
        /// </summary>
        TimeRulerMarker updateMarker;

        /// <summary>
        /// Draw メソッドを計測するための TimeRulerMarker。
        /// </summary>
        TimeRulerMarker drawMarker;

        /// <summary>
        /// 現在使用している描画方法。
        /// </summary>
        DrawMethod drawMethod = DrawMethod.NoOptimization;

        /// <summary>
        /// ターゲット GameObject 表示数。
        /// </summary>
        int targetObjectCount = InitialGameObjectCount;

        /// <summary>
        /// GameObject コレクション。
        /// </summary>
        GameObjectCollection gameObjects = new GameObjectCollection();

        /// <summary>
        /// LOD 用 GameObject コレクション。
        /// </summary>
        GameObjectCollection[] lodGameObjects;

        /// <summary>
        /// GameObject の初期化に使う Random インスタンス。
        /// </summary>
        Random ramdom = new Random(0);

        /// <summary>
        /// ステータスに表示する文字列。
        /// </summary>
        StringBuilder statusString = new StringBuilder(128);

        /// <summary>
        /// カメラ座標。
        /// </summary>
        Vector3 cameraPosition;

        /// <summary>
        /// View 行列。
        /// </summary>
        Matrix view;

        /// <summary>
        /// Projection 行列。
        /// </summary>
        Matrix projection;

        /// <summary>
        /// 描画用のインスタンス情報を格納する為の配列。
        /// </summary>
        ObjectInstanceVertex[] objectInstances = new ObjectInstanceVertex[MaxGameObjectCount];

        /// <summary>
        /// HW インスタンスで使うインスタンス情報を入れるための頂点バッファ。
        /// </summary>
        WritableVertexBuffer<ObjectInstanceVertex> instanceVertexBuffer;

        /// <summary>
        /// HW インスタンスで使うインスタンス情報を入れるための頂点バッファ (DirectMapping 用)。
        /// </summary>
        WritableVertexBuffer<GameObject> directMappingVertexBuffer;

        /// <summary>
        /// 頂点バインディング情報。
        /// </summary>
        VertexBufferBinding[] vertexBufferBindings = new VertexBufferBinding[2];

        /// <summary>
        /// スペース キーが押されていたかどうかを示す値。
        /// </summary>
        bool pressedSpaceKey;

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
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

            // テスト用にメモリ上で Block データを作ります。
            //var block = CreateFullFilledBlock();
            var block = CreateOctahedronLikeBlock();
            using (var stream = new MemoryStream())
            {
                blockSerializer.Serialize(stream, block);
                blockData = Encoding.ASCII.GetString(stream.ToArray());
            }

            UpdateStatusString();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/Default");
            fillTexture = Texture2DHelper.CreateFillTexture(GraphicsDevice);

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

            // BlockMesh をロードします。
            LoadBlockMesh();

            // ゲームオブジェクトの初期化
            InitializeGameObjects();
        }

        protected override void UnloadContent()
        {
            Services.GetRequiredService<ITimeRulerService>().ReleaseMarker(updateMarker);
            Services.GetRequiredService<ITimeRulerService>().ReleaseMarker(drawMarker);
            if (spriteBatch != null) spriteBatch.Dispose();
            if (fillTexture != null) fillTexture.Dispose();

            UnloadBlockMesh();
        }

        protected override void Update(GameTime gameTime)
        {
            timeRuler.StartFrame();
            updateMarker.Begin();

            KeyboardState keyState = Keyboard.GetState();

            if (keyState.IsKeyDown(Keys.Escape)) Exit();

            float dt = (float) gameTime.ElapsedGameTime.TotalSeconds;

            // スペースキーで次の描画手法に変更する
            if (keyState.IsKeyDown(Keys.Space))
            {
                if (!pressedSpaceKey)
                    MoveToNextTechnique();

                pressedSpaceKey = true;
            }
            else
            {
                pressedSpaceKey = false;
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

        /// <summary>
        /// 正八面体風のデータを定義する Block を作成します。
        /// </summary>
        /// <returns>作成された Block。</returns>
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

        class StringBlockLoader : IBlockLoader
        {
            string blockString;

            public StringBlockLoader(string blockString)
            {
                if (blockString == null) throw new ArgumentNullException("blockString");
                this.blockString = blockString;
            }

            public Block LoadBlock(string name)
            {
                using (var stream = blockString.ToMemoryStream())
                {
                    return blockSerializer.Deserialize(stream) as Block;
                }
            }
        }

        /// <summary>
        /// BlockMesh をロードします。
        /// </summary>
        void LoadBlockMesh()
        {
            var blockLoader = new StringBlockLoader(blockData);

            var meshFactory = new BlockMeshFactory(GraphicsDevice);
            meshManager = new BlockMeshManager(blockLoader, meshFactory);

            // 通常の IBlockEffect をロードします。
            blockEffect = new BasicBlockEffect(GraphicsDevice);
            blockEffect.EnableDefaultLighting();

            // インスタンシング用の IBlockEffect をロードします。
            var instancingEffect = Content.Load<Effect>("Effects/Instancing");
            instancingBlockEffect = new InstancingBlockEffect(instancingEffect);
            instancingBlockEffect.EnableDefaultLighting();

            // 実際のアプリケーションではファイルの Block から BlockMesh をロードします。
            // BlockMesh をロードします。
            mesh = meshManager.Load("Dummy", lodCount);
        }

        /// <summary>
        /// BlockMesh をアンロードします。
        /// </summary>
        void UnloadBlockMesh()
        {
            if (blockEffect != null) blockEffect.Dispose();
            if (instancingBlockEffect != null) instancingBlockEffect.Dispose();
            if (meshManager != null) meshManager.Unload();
        }

        /// <summary>
        /// GameObject を初期化します。
        /// </summary>
        void InitializeGameObjects()
        {
            gameObjects.Items = new GameObject[MaxGameObjectCount];
            gameObjects.Count = InitialGameObjectCount;
            for (int i = 0; i < gameObjects.Count; ++i) gameObjects.Items[i].Initialize(ramdom);

            lodGameObjects = new GameObjectCollection[lodCount];
            for (int lod = 0; lod < lodCount; lod++)
            {
                lodGameObjects[lod] = new GameObjectCollection();
                lodGameObjects[lod].Items = new GameObject[MaxGameObjectCount];
                lodGameObjects[lod].Count = InitialGameObjectCount;
            }

            UpdateStatusString();
        }

        /// <summary>
        /// GameObject を更新します。
        /// </summary>
        void UpdateGameObjects(float dt)
        {
            for (int lod = 0; lod < lodCount; lod++) lodGameObjects[lod].Count = 0;

            // 全オブジェクトを更新する
            // ゲームオブジェクトのライフ時間によって、ゲームオブジェクトを
            // リストからはずすので単純なforeachはここでは使えない
            for (int i = 0; i < gameObjects.Count; )
            {
                if (gameObjects.Items[i].Update(dt))
                {
                    ++i;

                    float distanceSquared;
                    Vector3.DistanceSquared(ref cameraPosition, ref gameObjects.Items[i].Position, out distanceSquared);
                    for (int lod = 0; lod < lodCount; lod++)
                    {
                        if (distanceSquared < lodDistanceSquareds[lod])
                        {
                            lodGameObjects[lod].Items[lodGameObjects[lod].Count] = gameObjects.Items[i];
                            lodGameObjects[lod].Count += 1;
                            break;
                        }
                    }
                }
                else
                {
                    // ゲームオブジェクトの消去
                    // 最後尾のゲームオブジェクトと交換する
                    var temp = gameObjects.Items[i];
                    gameObjects.Items[i] = gameObjects.Items[--gameObjects.Count];
                    gameObjects.Items[gameObjects.Count] = temp;
                }
            }

            if (targetObjectCount < gameObjects.Count) gameObjects.Count = targetObjectCount;

            while (gameObjects.Count < targetObjectCount) gameObjects.Items[gameObjects.Count++].Initialize(ramdom);
        }

        /// <summary>
        /// GameObject を描画します。
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

        /// <summary>
        /// チュートリアルなどで用いられる方法で GameObject を描画します。
        /// </summary>
        void DrawGameObjectsWithoutOptimization()
        {
            blockEffect.View = view;
            blockEffect.Projection = projection;

            mesh.LevelOfDetail = 0;

            for (int i = 0; i < gameObjects.Count; i++)
            {
                var world =
                    Matrix.CreateScale(gameObjects.Items[i].Scale) *
                    Matrix.CreateFromAxisAngle(gameObjects.Items[i].RotateAxis, gameObjects.Items[i].Rotation);
                world.Translation = gameObjects.Items[i].Position;

                blockEffect.World = world;

                mesh.Draw(blockEffect);
            }
        }

        /// <summary>
        /// LOD を用いながらチュートリアルなどで用いられる方法で GameObject を描画します。
        /// </summary>
        void DrawGameObjectsLodWithoutOptimization()
        {
            blockEffect.View = view;
            blockEffect.Projection = projection;

            for (int lod = 0; lod < lodCount; lod++)
            {
                mesh.LevelOfDetail = lod;

                for (int i = 0; i < lodGameObjects[lod].Count; i++)
                {
                    var gameObject = lodGameObjects[lod].Items[i];
                    var world =
                        Matrix.CreateScale(gameObject.Scale) *
                        Matrix.CreateFromAxisAngle(gameObject.RotateAxis, gameObject.Rotation);
                    world.Translation = gameObject.Position;

                    blockEffect.World = world;

                    mesh.Draw(blockEffect);
                }
            }
        }

        /// <summary>
        /// 同じ Material を持つ GameObject をまとめて描画します。
        /// </summary>
        void DrawGameObjectsWithBatch()
        {
            blockEffect.View = view;
            blockEffect.Projection = projection;

            mesh.LevelOfDetail = 0;

            foreach (var meshPart in mesh.MeshParts)
            {
                GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer, meshPart.VertexOffset);
                GraphicsDevice.Indices = meshPart.IndexBuffer;

                for (int i = 0; i < gameObjects.Count; i++)
                {
                    var world =
                        Matrix.CreateScale(gameObjects.Items[i].Scale) *
                        Matrix.CreateFromAxisAngle(gameObjects.Items[i].RotateAxis, gameObjects.Items[i].Rotation);
                    world.Translation = gameObjects.Items[i].Position;

                    blockEffect.DiffuseColor = meshPart.MeshMaterial.DiffuseColor;
                    blockEffect.EmissiveColor = meshPart.MeshMaterial.EmissiveColor;
                    blockEffect.SpecularColor = meshPart.MeshMaterial.SpecularColor;
                    blockEffect.SpecularPower = meshPart.MeshMaterial.SpecularPower;
                    blockEffect.World = world;
                    blockEffect.Pass.Apply();

                    GraphicsDevice.DrawIndexedPrimitives(
                        PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
                }
            }
        }

        /// <summary>
        /// LOD を用いながら同じ Material を持つ GameObject をまとめて描画します。
        /// </summary>
        void DrawGameObjectsLodWithBatch()
        {
            blockEffect.View = view;
            blockEffect.Projection = projection;

            for (int lod = 0; lod < lodCount; lod++)
            {
                mesh.LevelOfDetail = lod;

                foreach (var meshPart in mesh.MeshParts)
                {
                    GraphicsDevice.SetVertexBuffer(meshPart.VertexBuffer, meshPart.VertexOffset);
                    GraphicsDevice.Indices = meshPart.IndexBuffer;

                    for (int i = 0; i < lodGameObjects[lod].Count; i++)
                    {
                        var gameObject = lodGameObjects[lod].Items[i];
                        var world =
                            Matrix.CreateScale(gameObject.Scale) *
                            Matrix.CreateFromAxisAngle(gameObject.RotateAxis, gameObject.Rotation);
                        world.Translation = gameObject.Position;

                        blockEffect.DiffuseColor = meshPart.MeshMaterial.DiffuseColor;
                        blockEffect.EmissiveColor = meshPart.MeshMaterial.EmissiveColor;
                        blockEffect.SpecularColor = meshPart.MeshMaterial.SpecularColor;
                        blockEffect.SpecularPower = meshPart.MeshMaterial.SpecularPower;
                        blockEffect.World = world;
                        blockEffect.Pass.Apply();

                        GraphicsDevice.DrawIndexedPrimitives(
                            PrimitiveType.TriangleList, 0, 0, meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount);
                    }
                }
            }
        }

        /// <summary>
        /// HW インスタンシングで GameObject を描画します。
        /// </summary>
        void DrawGameObjectsWithHardwareInstancing()
        {
            instancingBlockEffect.View = view;
            instancingBlockEffect.Projection = projection;

            // インスタンス情報を一旦コピー
            for (int i = 0; i < gameObjects.Count; ++i)
            {
                objectInstances[i].Position = gameObjects.Items[i].Position;
                objectInstances[i].Scale = gameObjects.Items[i].Scale;
                objectInstances[i].RotateAxis = gameObjects.Items[i].RotateAxis;
                objectInstances[i].Rotation = gameObjects.Items[i].Rotation;
            }

            // インスタンス用の頂点バッファへ書き込む
            int offset = instanceVertexBuffer.SetData(objectInstances, 0, gameObjects.Count);

            mesh.LevelOfDetail = 0;

            // ゲームオブジェクトを描画
            foreach (var meshPart in mesh.MeshParts)
            {
                vertexBufferBindings[0] = new VertexBufferBinding(meshPart.VertexBuffer, meshPart.VertexOffset);
                vertexBufferBindings[1] = new VertexBufferBinding(instanceVertexBuffer.VertexBuffer, offset, 1);

                GraphicsDevice.SetVertexBuffers(vertexBufferBindings);
                GraphicsDevice.Indices = meshPart.IndexBuffer;

                instancingBlockEffect.DiffuseColor = meshPart.MeshMaterial.DiffuseColor;
                instancingBlockEffect.EmissiveColor = meshPart.MeshMaterial.EmissiveColor;
                instancingBlockEffect.SpecularColor = meshPart.MeshMaterial.SpecularColor;
                instancingBlockEffect.SpecularPower = meshPart.MeshMaterial.SpecularPower;

                instancingBlockEffect.Pass.Apply();

                GraphicsDevice.DrawInstancedPrimitives(
                    PrimitiveType.TriangleList, 0, 0,
                    meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount, gameObjects.Count);
            }
        }

        /// <summary>
        /// LOD を用いながら HW インスタンシングで GameObject を描画します。
        /// </summary>
        void DrawGameObjectsLodWithHardwareInstancing()
        {
            instancingBlockEffect.View = view;
            instancingBlockEffect.Projection = projection;

            for (int lod = 0; lod < lodCount; lod++)
            {
                if (lodGameObjects[lod].Count == 0) continue;

                // インスタンス情報を一旦コピー
                for (int i = 0; i < lodGameObjects[lod].Count; ++i)
                {
                    var gameObject = lodGameObjects[lod].Items[i];
                    objectInstances[i].Position = gameObject.Position;
                    objectInstances[i].Scale = gameObject.Scale;
                    objectInstances[i].RotateAxis = gameObject.RotateAxis;
                    objectInstances[i].Rotation = gameObject.Rotation;
                }

                // インスタンス用の頂点バッファへ書き込む
                int offset = instanceVertexBuffer.SetData(objectInstances, 0, lodGameObjects[lod].Count);

                mesh.LevelOfDetail = lod;

                // ゲームオブジェクトを描画
                foreach (var meshPart in mesh.MeshParts)
                {
                    vertexBufferBindings[0] = new VertexBufferBinding(meshPart.VertexBuffer, meshPart.VertexOffset);
                    vertexBufferBindings[1] = new VertexBufferBinding(instanceVertexBuffer.VertexBuffer, offset, 1);

                    GraphicsDevice.SetVertexBuffers(vertexBufferBindings);
                    GraphicsDevice.Indices = meshPart.IndexBuffer;

                    instancingBlockEffect.DiffuseColor = meshPart.MeshMaterial.DiffuseColor;
                    instancingBlockEffect.EmissiveColor = meshPart.MeshMaterial.EmissiveColor;
                    instancingBlockEffect.SpecularColor = meshPart.MeshMaterial.SpecularColor;
                    instancingBlockEffect.SpecularPower = meshPart.MeshMaterial.SpecularPower;

                    instancingBlockEffect.Pass.Apply();

                    GraphicsDevice.DrawInstancedPrimitives(
                        PrimitiveType.TriangleList, 0, 0,
                        meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount, lodGameObjects[lod].Count);
                }
            }
        }

        /// <summary>
        /// GameObject をそのまま頂点バッファへ設定する HW インスタンシングで GameObject を描画します。
        /// </summary>
        void DrawGameObjectsWithDirectMapping()
        {
            instancingBlockEffect.View = view;
            instancingBlockEffect.Projection = projection;

            //　インスタンスをそのまま頂点バッファへコピー
            int offset = directMappingVertexBuffer.SetData(gameObjects.Items, 0, gameObjects.Count);

            mesh.LevelOfDetail = 0;

            // ゲームオブジェクトを描画
            foreach (var meshPart in mesh.MeshParts)
            {
                vertexBufferBindings[0] = new VertexBufferBinding(meshPart.VertexBuffer, meshPart.VertexOffset);
                vertexBufferBindings[1] = new VertexBufferBinding(directMappingVertexBuffer.VertexBuffer, offset, 1);

                GraphicsDevice.SetVertexBuffers(vertexBufferBindings);
                GraphicsDevice.Indices = meshPart.IndexBuffer;

                instancingBlockEffect.DiffuseColor = meshPart.MeshMaterial.DiffuseColor;
                instancingBlockEffect.EmissiveColor = meshPart.MeshMaterial.EmissiveColor;
                instancingBlockEffect.SpecularColor = meshPart.MeshMaterial.SpecularColor;
                instancingBlockEffect.SpecularPower = meshPart.MeshMaterial.SpecularPower;

                instancingBlockEffect.Pass.Apply();

                GraphicsDevice.DrawInstancedPrimitives(
                    PrimitiveType.TriangleList, 0, 0,
                    meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount, gameObjects.Count);
            }
        }

        /// <summary>
        /// LOD を用いながら GameObject をそのまま頂点バッファへ設定する HW インスタンシングで GameObject を描画します。
        /// </summary>
        void DrawGameObjectsLodWithDirectMapping()
        {
            instancingBlockEffect.View = view;
            instancingBlockEffect.Projection = projection;

            for (int lod = 0; lod < lodCount; lod++)
            {
                if (lodGameObjects[lod].Count == 0) continue;

                //　インスタンスをそのまま頂点バッファへコピー
                int offset = directMappingVertexBuffer.SetData(lodGameObjects[lod].Items, 0, lodGameObjects[lod].Count);

                mesh.LevelOfDetail = lod;

                // ゲームオブジェクトを描画
                foreach (var meshPart in mesh.MeshParts)
                {
                    vertexBufferBindings[0] = new VertexBufferBinding(meshPart.VertexBuffer, meshPart.VertexOffset);
                    vertexBufferBindings[1] = new VertexBufferBinding(directMappingVertexBuffer.VertexBuffer, offset, 1);

                    GraphicsDevice.SetVertexBuffers(vertexBufferBindings);
                    GraphicsDevice.Indices = meshPart.IndexBuffer;

                    instancingBlockEffect.DiffuseColor = meshPart.MeshMaterial.DiffuseColor;
                    instancingBlockEffect.EmissiveColor = meshPart.MeshMaterial.EmissiveColor;
                    instancingBlockEffect.SpecularColor = meshPart.MeshMaterial.SpecularColor;
                    instancingBlockEffect.SpecularPower = meshPart.MeshMaterial.SpecularPower;

                    instancingBlockEffect.Pass.Apply();

                    GraphicsDevice.DrawInstancedPrimitives(
                        PrimitiveType.TriangleList, 0, 0,
                        meshPart.NumVertices, meshPart.StartIndex, meshPart.PrimitiveCount, lodGameObjects[lod].Count);
                }
            }
        }

        /// <summary>
        /// 次の描画方法に変更します。
        /// </summary>
        void MoveToNextTechnique()
        {
            drawMethod = (DrawMethod) (drawMethod + 1);
            if (drawMethod == DrawMethod.MaxCount) drawMethod = 0;

            UpdateStatusString();
        }

        /// <summary>
        /// ステータス文字列を更新します。
        /// </summary>
        void UpdateStatusString()
        {
            statusString.Length = 0;
            statusString.Append(" Draw Method: ");
            statusString.Append(drawMethod.ToString());
            statusString.Append(" \n Objects: ");
            statusString.Append(gameObjects.Count);
        }
    }
}
