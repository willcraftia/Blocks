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
                    // UIContext をアンバインドします。
                    Unbind(screen);
                    // InputReceiver から Screen をアンバインドします。
                    inputCapturer.InputReceiver = null;
                }

                screen = value;

                if (screen != null)
                {
                    // UIContext をバインドします。
                    Bind(screen);
                    // InputReceiver に Screen をバインドします。
                    if (inputCapturer != null) inputCapturer.InputReceiver = screen;
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

        /// <summary>
        /// フォーカスを得ている Control を取得あるいは設定します。
        /// </summary>
        internal Control FocusedControl { get; private set; }

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

            if (ControlLafSource != null) ControlLafSource.LoadContent();

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            SpriteBatch.Dispose();
            FillTexture.Dispose();

            if (ControlLafSource != null) ControlLafSource.UnloadContent();

            Screen = null;

            base.UnloadContent();
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
                control.Draw();
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

            visibleControls.Add(control);

            // 子 Control を再帰的に描画します。
            foreach (var child in control.Children)
            {
                PushControlToVisibleControlStack(child);
            }
        }

        // I/F
        public void Bind(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");

            // Control が既にこの UIContext にバインドされているならばスキップします。
            if (control.UIContext == this) return;

            // Control が他の UIContext にバインドされているならばアンバインドします。
            if (control.UIContext != null && control.UIContext != this) control.UIContext.Unbind(control);

            control.UIContext = this;
            control.EnabledChanged += new EventHandler(OnControlEnabledChanged);
            control.VisibleChanged += new EventHandler(OnControlVisibleChanged);

            // 子にバインド処理を伝播します。
            foreach (var child in control.Children)
            {
                Bind(child);
            }
        }

        void OnControlEnabledChanged(object sender, EventArgs e)
        {
            var control = sender as Control;

            // 無効になったならばフォーカス解除を試行します。
            if (!control.Enabled) Defocus(control);
        }

        void OnControlVisibleChanged(object sender, EventArgs e)
        {
            var control = sender as Control;

            // 不可視になったならばフォーカス解除を試行します。
            if (!control.Visible) Defocus(control);
        }

        // I/F
        public void Unbind(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            ensureControlContext(control);

            // 子から先にアンバインドします。
            foreach (var child in control.Children)
            {
                Bind(child);
            }

            Defocus(control);
            control.EnabledChanged -= new EventHandler(OnControlEnabledChanged);
            control.VisibleChanged -= new EventHandler(OnControlVisibleChanged);

            if (visibleControls.Contains(control)) visibleControls.Remove(control);

            control.UIContext = null;
        }

        // I/F
        public bool HasFocus(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            ensureControlContext(control);

            return FocusedControl == control;
        }

        // I/F
        public void Focus(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            ensureControlContext(control);

            if (Screen == null || !control.Enabled || !control.Visible || !control.Focusable) return;

            FocusedControl = control;
        }

        // I/F
        public void Defocus(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            ensureControlContext(control);

            if (Screen == null) return;

            if (HasFocus(control)) FocusedControl = null;
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
            ensureControlContext(control);

            if (ControlLafSource == null) return null;

            return ControlLafSource.GetControlLaf(control);
        }

        void ensureControlContext(Control control)
        {
            if (control.UIContext != this) throw new InvalidOperationException("Control is in another context.");
        }
    }
}
