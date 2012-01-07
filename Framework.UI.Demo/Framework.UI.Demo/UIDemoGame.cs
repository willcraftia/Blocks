#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI.Lafs;
using Willcraftia.Xna.Framework.UI.Lafs.Debug;
using Willcraftia.Xna.Framework.UI.Lafs.Sprite;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo
{
    public class UIDemoGame : Game
    {
        GraphicsDeviceManager graphics;

        InputManager inputManager;

        UIManager uiManager;

        DebugControlLafSource debugControlLafSource;

        SpriteControlLafSource spriteControlLafSource;

        Content.AsyncLoadManager asyncLoadManager = new Content.AsyncLoadManager();

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
            inputManager = new InputManager(this);
            Components.Add(inputManager);

            uiManager = new UIManager(this);
            {
                var screenFactory = new ScreenFactories.DefaultScreenFactory(this);
                screenFactory.Definitions.Add(new ScreenFactories.ScreenDefinition("WindowDemoScreen", typeof(WindowDemoScreen)));
                screenFactory.Definitions.Add(new ScreenFactories.ScreenDefinition("MainMenuDemoScreen", typeof(MainMenuDemoScreen)));
                uiManager.ScreenFactory = screenFactory;
            }
            Components.Add(uiManager);

            debugControlLafSource = new DebugControlLafSource(this);
            debugControlLafSource.Content.RootDirectory = "Content/UI/Debug";

            spriteControlLafSource = new SpriteControlLafSource(this);
            spriteControlLafSource.SpriteSize = 32;
            spriteControlLafSource.Content.RootDirectory = "Content/UI/Sprite";

            //uiManager.ControlLafSource = debugControlLafSource;
            uiManager.ControlLafSource = spriteControlLafSource;

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            uiManager.Show("MainMenuDemoScreen");

            asyncLoadManager.Execute(new LongSleepingLoader(), LongSleepingLoaderCompleteCallback);
        }

        void LongSleepingLoaderCompleteCallback(object result)
        {
            Console.WriteLine("LongSleepingLoader was completed: result=" + result);
        }

        protected override void UnloadContent()
        {
            debugControlLafSource.Dispose();
            spriteControlLafSource.Dispose();
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
