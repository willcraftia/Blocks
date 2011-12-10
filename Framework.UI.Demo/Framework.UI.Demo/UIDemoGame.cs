#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI.Demo
{
    public class UIDemoGame : Game
    {
        GraphicsDeviceManager graphics;

        InputManager inputManager;

        UIManager uiManager;

        public UIDemoGame()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            inputManager = new InputManager(this);
            Components.Add(inputManager);

            uiManager = new UIManager(this);
            Components.Add(uiManager);

            IsMouseVisible = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            var screen = new Screen();
            screen.Bounds = GraphicsDevice.Viewport.Bounds;

            var control_0 = new ButtonControl();
            control_0.Bounds = new Rectangle(16, 16, 256, 256);
            control_0.Appearance = new PlainColorButtonAppearance(Services);
            screen.Children.Add(control_0);

            var control_1 = new ButtonControl();
            control_1.Bounds = new Rectangle(32, 32, 256, 256);
            control_1.Appearance = new PlainColorButtonAppearance(Services);
            screen.Children.Add(control_1);

            var control_3 = new ButtonControl();
            control_3.Bounds = new Rectangle(64, 64, 256, 256);
            control_3.Appearance = new PlainColorButtonAppearance(Services);
            screen.Children.Add(control_3);

            var control_3_0 = new ButtonControl();
            control_3_0.Bounds = new Rectangle(16, 16, 32, 32);
            control_3_0.Appearance = new PlainColorButtonAppearance(Services);
            control_3.Children.Add(control_3_0);

            var control_3_1 = new ButtonControl();
            control_3_1.Bounds = new Rectangle(16 + 32, 16, 32, 32);
            control_3_1.Appearance = new PlainColorButtonAppearance(Services);
            control_3.Children.Add(control_3_1);

            uiManager.Screen = screen;
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
