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
            var oldPixelWindowAppearance = new OldStylePixelWindowAppearance(Services);
            oldPixelWindowAppearance.TopLeft = Content.Load<Texture2D>("Textures/FFWinTopLeft");
            oldPixelWindowAppearance.TopRight = Content.Load<Texture2D>("Textures/FFWinTopRight");
            oldPixelWindowAppearance.BottomLeft = Content.Load<Texture2D>("Textures/FFWinBottomLeft");
            oldPixelWindowAppearance.BottomRight = Content.Load<Texture2D>("Textures/FFWinBottomRight");
            oldPixelWindowAppearance.Top = Content.Load<Texture2D>("Textures/FFWinTop");
            oldPixelWindowAppearance.Bottom = Content.Load<Texture2D>("Textures/FFWinBottom");
            oldPixelWindowAppearance.Left = Content.Load<Texture2D>("Textures/FFWinLeft");
            oldPixelWindowAppearance.Right = Content.Load<Texture2D>("Textures/FFWinRight");
            oldPixelWindowAppearance.Fill = Content.Load<Texture2D>("Textures/FFWinFill");

            var screen = new Screen();
            screen.Bounds = GraphicsDevice.Viewport.Bounds;

            var control_0 = new Window();
            control_0.Bounds = new Rectangle(32, 32, 32 * 10, 32 * 10);
            control_0.Appearance = oldPixelWindowAppearance;
            screen.Children.Add(control_0);

            var control_1 = new Window();
            control_1.Bounds = new Rectangle(32 * 3, 32 * 3, 32 * 10, 32 * 10);
            control_1.Appearance = oldPixelWindowAppearance;
            screen.Children.Add(control_1);

            var control_3 = new Window();
            control_3.Bounds = new Rectangle(32 * 4, 32 * 4, 32 * 10, 32 * 10);
            control_3.Appearance = oldPixelWindowAppearance;
            screen.Children.Add(control_3);

            var control_3_0 = new Button();
            control_3_0.Bounds = new Rectangle(32, 32, 32 * 3, 32);
            control_3_0.Appearance = new PlainColorButtonAppearance(Services);
            control_3.Children.Add(control_3_0);

            var control_3_1 = new Button();
            control_3_1.Bounds = new Rectangle(32 * 4, 32, 32 * 3, 32);
            control_3_1.Appearance = new PlainColorButtonAppearance(Services);
            control_3.Children.Add(control_3_1);

            //control_3.Visible = false;

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
