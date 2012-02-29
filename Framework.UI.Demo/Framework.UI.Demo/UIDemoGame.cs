#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Lafs;
using Willcraftia.Xna.Framework.UI.Lafs.Debug;
using Willcraftia.Xna.Framework.UI.Demo.Screens;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo
{
    public class UIDemoGame : Game
    {
        GraphicsDeviceManager graphics;

        UIManager uiManager;

        DefaultSpriteSheetSource spriteSheetSource;

        ILookAndFeelSource debugLookAndFeelSource;

        DefaultLookAndFeelSource spriteLookAndFeelSource;

        public UIDemoGame()
        {
            graphics = new GraphicsDeviceManager(this);

            //
            // REFERENCE: http://creators.xna.com/ja-jp/education/bestpractices
            //            http://social.msdn.microsoft.com/Forums/ja-JP/xnagameja/thread/60a6d9d9-1ede-4657-a77c-9ff65edd563a
            //
            // Xbox360:
            // 640x480
            // 720x480
            // 1280x720 (720p)
            // 1980x1080
            // 
            graphics.PreferredBackBufferWidth = 720;
            graphics.PreferredBackBufferHeight = 480;
            //graphics.PreferredBackBufferWidth = 1280;
            //graphics.PreferredBackBufferHeight = 720;
            // for recording settings
            //graphics.PreferredBackBufferWidth = 640;
            //graphics.PreferredBackBufferHeight = 480;
            //graphics.PreferredBackBufferWidth = 320;
            //graphics.PreferredBackBufferHeight = 240;

            graphics.PreferMultiSampling = true;

            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            InitializeSpriteSheetSource();

            uiManager = new UIManager(this)
            {
                ScreenFactory = CreateScreenFactory()
            };
            Components.Add(uiManager);

            IsMouseVisible = true;

            base.Initialize();
        }

        void InitializeSpriteSheetSource()
        {
            var windowTemplate = new WindowSpriteSheetTemplate(16, 16);
            var windowShadowConverter = new DecoloringTexture2DConverter(new Color(0, 0, 0, 0.5f));
            var windowTexture = Content.Load<Texture2D>("UI/Sprite/Window");
            var windowShadowTexture = windowShadowConverter.Convert(windowTexture);

            spriteSheetSource = new DefaultSpriteSheetSource();
            spriteSheetSource.SpriteSheetMap["Window"] = new SpriteSheet(windowTemplate, windowTexture);
            spriteSheetSource.SpriteSheetMap["WindowShadow"] = new SpriteSheet(windowTemplate, windowShadowTexture);
        }

        IScreenFactory CreateScreenFactory()
        {
            var screenFactory = new DefaultScreenFactory(this);
            InitializeScreenDefinitions(screenFactory);
            InitializeLookAndFeelSource(screenFactory);
            return screenFactory;
        }

        void InitializeLookAndFeelSource(DefaultScreenFactory screenFactory)
        {
            debugLookAndFeelSource = DebugLooAndFeelUtil.CreateLookAndFeelSource(this);

            spriteLookAndFeelSource = new DefaultLookAndFeelSource();
            spriteLookAndFeelSource.LookAndFeelMap[typeof(Desktop)] = new DesktopLookAndFeel();
            spriteLookAndFeelSource.LookAndFeelMap[typeof(Window)] = new SpriteSheetWindowLookAndFeel
            {
                SpriteSheetSource = spriteSheetSource
            };
            spriteLookAndFeelSource.LookAndFeelMap[typeof(TextBlock)] = new TextBlockLookAndFeel();
            spriteLookAndFeelSource.LookAndFeelMap[typeof(Overlay)] = new OverlayLookAndFeel();

            screenFactory.LookAndFeelSource = debugLookAndFeelSource;
            //screenFactory.LookAndFeelSource = spriteLookAndFeelSource;
        }

        void InitializeScreenDefinitions(DefaultScreenFactory screenFactory)
        {
            screenFactory.Definitions.Add(new ScreenDefinition("MainMenuDemoScreen", typeof(MainMenuDemoScreen)));

            var loadingWindowDemoScreen = new ScreenDefinition("WindowDemoScreen", typeof(DemoLoadingScreen));
            loadingWindowDemoScreen.Properties["LoadedScreenName"] = "WindowDemoScreenImpl";

            screenFactory.Definitions.Add(loadingWindowDemoScreen);
            screenFactory.Definitions.Add(new ScreenDefinition("WindowDemoScreenImpl", typeof(WindowDemoScreen)));
        }

        protected override void LoadContent()
        {
            uiManager.Show("MainMenuDemoScreen");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
