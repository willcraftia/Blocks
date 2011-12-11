#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class UIManager : DrawableGameComponent, IUIService
    {
        IInputService inputService;

        IInputCapturer inputCapturer;

        Screen screen;

        List<Control> visibleControls;

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

            visibleControls = new List<Control>();
        }

        public override void Initialize()
        {
            inputService = Game.Services.GetRequiredService<IInputService>();
            InputCapturer = new DefaultInputCapturer(inputService);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            FillTexture = Texture2DHelper.CreateFillTexture(GraphicsDevice);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            visibleControls.Clear();

            if (Screen == null) return;

            PushControlToVisibleControlStack(Screen);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // 描画用リストにある Control を描画します。
            foreach (var control in visibleControls)
            {
                control.Appearance.Draw(control);
            }
        }

        /// <summary>
        /// Control が描画対象であるかどうかを判定し、描画対象であるならば描画用リストに追加します。
        /// </summary>
        /// <param name="control">Control。</param>
        void PushControlToVisibleControlStack(Control control)
        {
            // 不可視の場合は自分も子も描画しません。
            if (!control.Visible) return;

            // Appearance を持つならば描画します。
            if (control.Appearance != null)
            {
                visibleControls.Add(control);
            }

            // 子 Control を再帰的に描画します。
            foreach (var child in control.Children)
            {
                PushControlToVisibleControlStack(child);
            }
        }
    }
}
