#region Using

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Lafs;
using Willcraftia.Xna.Blocks.Content;
using Willcraftia.Xna.Blocks.BlockViewer.Models;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer
{
    /// <summary>
    /// BlockViewer の Game クラスです。
    /// </summary>
    public class BlockViewerGame : Game
    {
        /// <summary>
        /// SpriteSheet で扱うスプライト イメージのサイズ。
        /// </summary>
        public const int SpriteSize = 32;

        /// <summary>
        /// GraphicsDeviceManager。
        /// </summary>
        GraphicsDeviceManager graphics;

        /// <summary>
        /// UI を管理する UIManager。
        /// </summary>
        UIManager uiManager;

        AsyncBlockMeshLoadManager asyncBlockMeshLoadManager;

        public StorageModel StorageModel { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public BlockViewerGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 720;
            graphics.PreferredBackBufferHeight = 480;
            graphics.PreferMultiSampling = true;

            Content.RootDirectory = "Content";

            // デフォルトは OS の CultureInfo に従います。
            Strings.Culture = CultureInfo.CurrentCulture;

            StorageModel = new StorageModel();
        }

        protected override void Initialize()
        {
            // UIManager を初期化して登録します。
            uiManager = new UIManager(this);
            uiManager.ScreenFactory = CreateScreenFactory();
            Components.Add(uiManager);

            asyncBlockMeshLoadManager = new AsyncBlockMeshLoadManager(this);

            // マウス カーソルを可視にします。
            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // StartScreen の表示から開始します。
            uiManager.Show(Screens.ScreenNames.Start);

            asyncBlockMeshLoadManager.Enabled = true;
        }

        protected override void UnloadContent()
        {
            asyncBlockMeshLoadManager.Enabled = false;
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override bool BeginDraw()
        {
            asyncBlockMeshLoadManager.Suspend();

            return base.BeginDraw();
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        protected override void EndDraw()
        {
            base.EndDraw();

            asyncBlockMeshLoadManager.Resume();
        }

        /// <summary>
        /// IScreenFactory を作成します。
        /// </summary>
        /// <returns>生成された IScreenFactory。</returns>
        IScreenFactory CreateScreenFactory()
        {
            var screenFactory = new DefaultScreenFactory(this);
            InitializeScreenDefinitions(screenFactory);
            return screenFactory;
        }

        /// <summary>
        /// DefaultScreenFactory に Screen 定義を設定します。 
        /// </summary>
        /// <param name="screenFactory">DefaultScreenFactory。</param>
        void InitializeScreenDefinitions(DefaultScreenFactory screenFactory)
        {
            screenFactory.Definitions.Add(new ScreenDefinition(Screens.ScreenNames.Start, typeof(Screens.StartScreen)));
            screenFactory.Definitions.Add(new ScreenDefinition(Screens.ScreenNames.Main, typeof(Screens.MainScreen)));
        }
    }
}
