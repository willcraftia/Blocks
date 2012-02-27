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
using Willcraftia.Xna.Framework.UI.Lafs.Sprite;

using Willcraftia.Xna.Blocks.BlockViewer.Lafs.Sprite;
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
        }

        protected override void Initialize()
        {
            // UIManager を初期化して登録します。
            uiManager = new UIManager(this);
            uiManager.ScreenFactory = CreateScreenFactory();
            Components.Add(uiManager);

            // マウス カーソルを可視にします。
            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // StartScreen の表示から開始します。
            uiManager.Show(Screens.ScreenNames.Start);
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }

        /// <summary>
        /// IScreenFactory を作成します。
        /// </summary>
        /// <returns>生成された IScreenFactory。</returns>
        IScreenFactory CreateScreenFactory()
        {
            var screenFactory = new DefaultScreenFactory(this);
            InitializeScreenDefinitions(screenFactory);
            screenFactory.LookAndFeelSource = CreateLookAndFeelSource();
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

        /// <summary>
        /// ILookAndFeelSource を生成します。
        /// </summary>
        /// <returns>生成された ILookAndFeelSource。</returns>
        ILookAndFeelSource CreateLookAndFeelSource()
        {
            var windowTemplate = new WindowSpriteSheetTemplate(32, 32, true);
            var windowShadowConverter = new DecoloringTexture2DConverter(new Color(0, 0, 0, 0.5f));

            var spriteSheetSource = new DefaultSpriteSheetSource(this);
            spriteSheetSource.Content.RootDirectory = "Content/UI/Sprite";
            spriteSheetSource.DefinitionMap["Window"] = new SpriteSheetDefinition(windowTemplate, "Window");
            spriteSheetSource.DefinitionMap["WindowShadow"] = new SpriteSheetDefinition(windowTemplate, "Window", windowShadowConverter);

            var lookAndFeelSource = new SpriteLookAndFeelSource(this, spriteSheetSource);
            lookAndFeelSource.Content.RootDirectory = "Content/UI/Sprite";

            lookAndFeelSource.Register(typeof(Button), new ViewerButtonLookAndFeel());

            var windowLookAndFeel = new WindowLookAndFeel();
            lookAndFeelSource.Register(typeof(Window), windowLookAndFeel);

            return lookAndFeelSource;

            //return new Framework.UI.Lafs.Debug.DebugLookAndFeelSource(this);
        }
    }
}
