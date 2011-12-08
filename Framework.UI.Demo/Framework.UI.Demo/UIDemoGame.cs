#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo
{
    public class UIDemoGame : Game
    {
        GraphicsDeviceManager graphics;

        AppearanceManager appearanceManager;

        public UIDemoGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            appearanceManager = new AppearanceManager(this);
            Components.Add(appearanceManager);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            var screen = new Screen();
            screen.Bounds = GraphicsDevice.Viewport.Bounds;

            var control_0 = new Control();
            control_0.Bounds = new Rectangle(16, 16, 256, 256);
            control_0.Appearance = new PlainColorAppearance(Services, Color.Red);
            screen.Children.Add(control_0);

            var control_1 = new Control();
            control_1.Bounds = new Rectangle(32, 32, 256, 256);
            control_1.Appearance = new PlainColorAppearance(Services, Color.Blue);
            screen.Children.Add(control_1);

            var control_3 = new Control();
            control_3.Bounds = new Rectangle(64, 64, 256, 256);
            control_3.Appearance = new PlainColorAppearance(Services, Color.Gray);
            screen.Children.Add(control_3);

            var control_3_0 = new Control();
            control_3_0.Bounds = new Rectangle(16, 16, 32, 32);
            control_3_0.Appearance = new PlainColorAppearance(Services, Color.Green);
            control_3.Children.Add(control_3_0);

            var control_3_1 = new Control();
            control_3_1.Bounds = new Rectangle(16 + 32, 16, 32, 32);
            control_3_1.Appearance = new PlainColorAppearance(Services, Color.Yellow);
            control_3.Children.Add(control_3_1);

            appearanceManager.Screen = screen;
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            base.Draw(gameTime);
        }
    }
}
