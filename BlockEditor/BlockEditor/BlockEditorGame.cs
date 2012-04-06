#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Willcraftia.Xna.Framework.Debug;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Blocks.BlockEditor.Screens;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor
{
    public class BlockEditorGame : Game
    {
        GraphicsDeviceManager graphics;

        /// <summary>
        /// UI を管理する UIManager。
        /// </summary>
        UIManager uiManager;

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

        public BlockEditorGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 720;
            graphics.PreferredBackBufferHeight = 480;
            // for recording settings
            //graphics.PreferredBackBufferWidth = 640;
            //graphics.PreferredBackBufferHeight = 480;
            graphics.PreferMultiSampling = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // UIManager を初期化して登録します。
            uiManager = new UIManager(this);
            uiManager.ScreenFactory = CreateScreenFactory();
            Components.Add(uiManager);

            var fpsCounter = new FpsCounter(this);
            fpsCounter.Content.RootDirectory = "Content";
            fpsCounter.HorizontalAlignment = DebugHorizontalAlignment.Right;
            fpsCounter.SampleSpan = TimeSpan.FromSeconds(2);
            Components.Add(fpsCounter);

            timeRuler = new TimeRuler(this);
            Components.Add(timeRuler);

            updateMarker = timeRuler.CreateMarker();
            updateMarker.Name = "Update";
            updateMarker.BarIndex = 0;
            updateMarker.Color = Color.Cyan;

            drawMarker = timeRuler.CreateMarker();
            drawMarker.Name = "Draw";
            drawMarker.BarIndex = 1;
            drawMarker.Color = Color.Yellow;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            uiManager.Show(Screens.ScreenNames.Main);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            timeRuler.StartFrame();
            updateMarker.Begin();

            base.Update(gameTime);

            updateMarker.End();
        }

        protected override void Draw(GameTime gameTime)
        {
            drawMarker.Begin();

            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);

            drawMarker.End();
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
            screenFactory.Definitions.Add(new ScreenDefinition(Screens.ScreenNames.Main, typeof(Screens.MainScreen)));
        }
    }
}
