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

        List<Control> visibleControls;

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

            if (ControlLafSource != null) ControlLafSource.Initialize();

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            if (SpriteBatch != null) SpriteBatch.Dispose();
            if (FillTexture != null) FillTexture.Dispose();
            if (ControlLafSource != null) ControlLafSource.Dispose();

            Screen = null;

            base.UnloadContent();
        }

        public override void Update(GameTime gameTime)
        {
            visibleControls.Clear();

            if (Screen == null) return;

            // Control の Update を再帰的に呼び出します。
            UpdateControl(Screen, gameTime);

            // 可視 Control を探索し、可視 Control リストに追加します。
            PushControlToVisibleControlStack(Screen);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // 描画用リストにある Control を描画します。
            foreach (var control in visibleControls) control.Draw(gameTime);
        }

        /// <summary>
        /// Control の Update メソッドを呼び出します。
        /// </summary>
        /// <remarks>
        /// このメソッドは、Control の Update メソッドを呼び出した後、その子 Control の Update メソッドを再帰的に呼び出します。
        /// Enabled が false の場合、Update メソッドは呼び出されません。
        /// </remarks>
        /// <param name="control"></param>
        void UpdateControl(Control control, GameTime gameTime)
        {
            if (!control.Enabled) return;

            // Control を更新します。
            control.Update(gameTime);

            // 子 Control を再帰的に更新します。
            foreach (var child in control.Children) UpdateControl(child, gameTime);
        }

        /// <summary>
        /// Control が描画対象であるかどうかを判定し、描画対象であるならば描画用リストに追加します。
        /// </summary>
        /// <param name="control">Control。</param>
        void PushControlToVisibleControlStack(Control control)
        {
            // 不可視の場合は自分も子も描画しません。
            if (!control.Visible) return;

            visibleControls.Add(control);

            // 子 Control を再帰的に描画します。
            foreach (var child in control.Children) PushControlToVisibleControlStack(child);
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
    }
}
