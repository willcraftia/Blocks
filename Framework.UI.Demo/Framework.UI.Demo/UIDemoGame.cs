#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Visuals;

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
            //LoadGameDemoGui();
            LoadSimpleWindowDemoGui();
        }

        void LoadGameDemoGui()
        {
            var screen = new Screen();
            screen.Bounds = GraphicsDevice.Viewport.Bounds;

            {
                var button = new Button();
            }
        }

        void LoadSimpleWindowDemoGui()
        {
            int unit = 32;

            var oldPixelWindowAppearance = new OldStylePixelWindowAppearance(Services);
            oldPixelWindowAppearance.Unit = unit;
            oldPixelWindowAppearance.TopLeft = Content.Load<Texture2D>("Textures/FF6WinTopLeft");
            oldPixelWindowAppearance.TopRight = Content.Load<Texture2D>("Textures/FF6WinTopRight");
            oldPixelWindowAppearance.BottomLeft = Content.Load<Texture2D>("Textures/FF6WinBottomLeft");
            oldPixelWindowAppearance.BottomRight = Content.Load<Texture2D>("Textures/FF6WinBottomRight");
            oldPixelWindowAppearance.Top = Content.Load<Texture2D>("Textures/FF6WinTop");
            oldPixelWindowAppearance.Bottom = Content.Load<Texture2D>("Textures/FF6WinBottom");
            oldPixelWindowAppearance.Left = Content.Load<Texture2D>("Textures/FF6WinLeft");
            oldPixelWindowAppearance.Right = Content.Load<Texture2D>("Textures/FF6WinRight");
            oldPixelWindowAppearance.Fill = Content.Load<Texture2D>("Textures/FF6WinFill");

            var screen = new Screen();
            screen.Bounds = GraphicsDevice.Viewport.Bounds;

            var control_0 = new Window();
            control_0.Bounds = new Rectangle(unit, unit, unit * 10, unit * 10);
            control_0.Appearance = oldPixelWindowAppearance;
            screen.Children.Add(control_0);

            var control_1 = new Window();
            control_1.Bounds = new Rectangle(unit * 3, unit * 3, unit * 10, unit * 10);
            control_1.Appearance = oldPixelWindowAppearance;
            screen.Children.Add(control_1);

            var control_3 = new Window();
            control_3.Bounds = new Rectangle(unit * 4, unit * 4, unit * 10, unit * 10);
            control_3.Appearance = oldPixelWindowAppearance;
            screen.Children.Add(control_3);

            var control_3_0 = new Button();
            control_3_0.Bounds = new Rectangle(unit, unit, unit * 3, unit);
            control_3_0.Appearance = new PlainColorButtonAppearance(Services);
            control_3.Children.Add(control_3_0);

            var control_3_1 = new Button();
            control_3_1.Bounds = new Rectangle(unit * 4, unit, unit * 3, unit);
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
