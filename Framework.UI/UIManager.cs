#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class UIManager : DrawableGameComponent, IUIService, IUIContext
    {
        IInputService inputService;

        IInputCapturer inputCapturer;

        Screen screen;

        IControlLafSource controlLafSource;

        // I/F
        public SpriteBatch SpriteBatch { get; private set; }

        // I/F
        public Texture2D FillTexture { get; private set; }

        public IInputCapturer InputCapturer
        {
            get { return inputCapturer; }
            set
            {
                if (inputCapturer == value) return;

                // InputCapturer から Screen をアンバインドします。
                if (inputCapturer != null) inputCapturer.InputReceiver = null;

                inputCapturer = value;

                // InputCapturer に Screen をバインドします。
                if (inputCapturer != null && Screen != null) inputCapturer.InputReceiver = Screen;
            }
        }

        /// <summary>
        /// Screen を取得または設定します。
        /// </summary>
        public Screen Screen
        {
            // I/F
            get { return screen; }
            set
            {
                if (screen == value) return;

                if (screen != null)
                {
                    // InputReceiver から Screen をアンバインドします。
                    inputCapturer.InputReceiver = null;
                    // Screen から自分をアンバインドします。
                    screen.UIContext = null;
                }

                screen = value;

                if (screen != null)
                {
                    // InputReceiver に Screen をバインドします。
                    if (inputCapturer != null) inputCapturer.InputReceiver = screen;
                    // Screen に自分をバインドします。
                    screen.UIContext = this;
                }
            }
        }

        /// <summary>
        /// IControlLafSource を取得あるいは設定します。
        /// </summary>
        public IControlLafSource ControlLafSource
        {
            get { return controlLafSource; }
            set
            {
                if (controlLafSource == value) return;

                if (controlLafSource != null) controlLafSource.UIContext = null;

                controlLafSource = value;
                controlLafSource.UIContext = this;
            }
        }

        public UIManager(Game game)
            : base(game)
        {
            // サービスとして登録
            Game.Services.AddService(typeof(IUIService), this);
        }

        // I/F
        public ContentManager CreateContentManager()
        {
            return new ContentManager(Game.Services);
        }

        // I/F
        public IControlLaf GetControlLaf(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            if (ControlLafSource == null) return null;

            return ControlLafSource.GetControlLaf(control);
        }

        public override void Initialize()
        {
            inputService = Game.Services.GetRequiredService<IInputService>();
            InputCapturer = new DefaultInputCapturer(inputService);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (Screen == null) return;

            Screen.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Screen == null) return;

            Screen.Draw(gameTime);

            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            FillTexture = Texture2DHelper.CreateFillTexture(GraphicsDevice);

            if (ControlLafSource != null) ControlLafSource.Initialize();

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            if (SpriteBatch != null) SpriteBatch.Dispose();
            if (FillTexture != null) FillTexture.Dispose();
            if (ControlLafSource != null) ControlLafSource.Dispose();

            base.UnloadContent();
        }
    }
}
