#region Using

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class UIManager : DrawableGameComponent, IUIService
    {
        IInputService inputService;

        IInputCapturer inputCapturer;

        Screen screen;

        // I/F
        public Screen Screen
        {
            get { return screen; }
            set
            {
                if (screen == value) return;

                screen = value;

                // InputReceiver にバインド
                if (inputCapturer != null && screen != null) inputCapturer.InputReceiver = screen;
            }
        }

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

                // 旧 InputCapturer から Screen をアンバインド
                if (inputCapturer != null) inputCapturer.InputReceiver = null;

                inputCapturer = value;

                // 新 InputCapturer に Screen をバインド
                if (inputCapturer != null && screen != null) inputCapturer.InputReceiver = screen;
            }
        }

        public UIManager(Game game)
            : base(game)
        {
            // サービスとして登録
            Game.Services.AddService(typeof(IUIService), this);
        }

        public override void Initialize()
        {
            inputService = Game.Services.GetService(typeof(IInputService)) as IInputService;
            InputCapturer = new DefaultInputCapturer(inputService);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            FillTexture = new Texture2D(GraphicsDevice, 1, 1);
            FillTexture.SetData<Color>(new Color[] { Color.White });

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            if (Screen == null) return;

            DrawControl(Screen);
        }

        void DrawControl(Control control)
        {
            if (control.Appearance != null)
            {
                control.Appearance.Draw(control);
            }

            foreach (var child in control.Children)
            {
                DrawControl(child);
            }
        }
    }
}
