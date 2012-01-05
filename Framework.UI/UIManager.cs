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
    public class UIManager : DrawableGameComponent, IUIService
    {
        #region Scissor

        /// <summary>
        /// Control 描画のための GraphicsDevice の ScissorRectangle および SpriteBatch の Begin/End 状態を管理するクラスです。
        /// </summary>
        /// <remarks>
        /// Control の描画は再帰的に行うため、スタック的に ScissorRectangle の状態を維持する必要があります。
        /// ここで、GraphicsDevice の ScissorRectangle は、SpriteBatch の Begin 前に設定しなければ、
        /// SpriteBatch による描画で有効になりません。
        /// このため、using 区を用いてこのクラスのインスタンスを管理することを前提に、
        /// Scissor インスタンス生成で SpriteBatch の状態を End させ、
        /// GraphicsDevice に新たな ScissorRectangle を設定して SpriteBatch を Begin し、
        /// using 終了による Dispose 呼び出しにて SpriteBatch を End し、前回の ScissorRectangle を再設定して SpriteBatch を Begin します。
        /// これにより、GraphicsDevice の ScissorRectangle の状態と SpriteBatch の Begin/End の状態を論理的にスタック化できます。
        /// </remarks>
        class Scissor : IDisposable
        {
            UIManager uiManager;

            Rectangle previousScissorRectangle;

            public Scissor(UIManager uiManager, Rectangle scissorRectangle)
            {
                this.uiManager = uiManager;
                BeginClipping(ref scissorRectangle);
            }

            // I/F
            public void Dispose()
            {
                EndClipping();
                GC.SuppressFinalize(this);
            }

            void BeginClipping(ref Rectangle scissorRectangle)
            {
                var spriteBatch = uiManager.spriteBatch;
                var graphicsDevice = uiManager.GraphicsDevice;

                spriteBatch.End();

                previousScissorRectangle = graphicsDevice.ScissorRectangle;

                // Viewport の領域からはみ出ないように領域を調整します (はみ出ると例外が発生します)。
                var viewportBounds = graphicsDevice.Viewport.Bounds;
                Rectangle viewIntersectBounds;
                Rectangle.Intersect(ref viewportBounds, ref scissorRectangle, out viewIntersectBounds);
                // 親の ScissorRectangle を考慮した領域を計算します。
                Rectangle finalScissorRectangle;
                Rectangle.Intersect(ref viewIntersectBounds, ref previousScissorRectangle, out finalScissorRectangle);

                graphicsDevice.ScissorRectangle = finalScissorRectangle;
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, uiManager.scissorTestRasterizerState);
            }

            void EndClipping()
            {
                var spriteBatch = uiManager.spriteBatch;
                var graphicsDevice = spriteBatch.GraphicsDevice;

                spriteBatch.End();

                graphicsDevice.ScissorRectangle = previousScissorRectangle;
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, uiManager.scissorTestRasterizerState);
            }
        }

        #endregion

        #region IDrawContext

        class DrawContext : IDrawContext, IDisposable
        {
            UIManager uiManager;

            // I/F
            public SpriteBatch SpriteBatch
            {
                get { return uiManager.spriteBatch; }
            }

            // I/F
            public BasicEffect BasicEffect
            {
                get { return uiManager.basicEffect; }
            }

            // I/F
            public Rectangle Bounds { get; internal set; }

            // I/F
            public float Opacity { get; internal set; }

            // I/F
            public Texture2D FillTexture
            {
                get { return uiManager.fillTexture; }
            }

            public DrawContext(UIManager uiManager)
            {
                this.uiManager = uiManager;
            }

            // I/F
            public void Flush()
            {
                SpriteBatch.End();
                SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, uiManager.scissorTestRasterizerState);
            }

            // I/F
            public void Dispose()
            {
                GC.SuppressFinalize(this);
            }
        }

        #endregion

        IInputService inputService;

        IInputCapturer inputCapturer;

        Screen screen;

        IControlLafSource controlLafSource;

        RasterizerState scissorTestRasterizerState;

        SpriteBatch spriteBatch;

        Texture2D fillTexture;

        BasicEffect basicEffect;

        // I/F
        public Screen Screen
        {
            get { return screen; }
            set
            {
                if (screen == value) return;

                if (screen != null)
                {
                    // InputReceiver から Screen をアンバインドします。
                    inputCapturer.InputReceiver = null;

                    // TODO どうしよう？
                    // 破棄します。
                    //screen.Dispose();
                }

                screen = value;

                if (screen != null)
                {
                    // InputReceiver に Screen をバインドします。
                    if (inputCapturer != null) inputCapturer.InputReceiver = screen;

                    // 必要ならば初期化します。
                    if (!screen.Initialized) screen.Initialize();
                }
            }
        }

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
        /// IControlLafSource を取得あるいは設定します。
        /// </summary>
        public IControlLafSource ControlLafSource
        {
            get { return controlLafSource; }
            set
            {
                if (controlLafSource == value) return;

                controlLafSource = value;
            }
        }

        public IScreenFactory ScreenFactory { get; set; }

        public UIManager(Game game)
            : base(game)
        {
            // サービスとして登録
            Game.Services.AddService(typeof(IUIService), this);
            scissorTestRasterizerState = new RasterizerState()
            {
                ScissorTestEnable = true
            };
        }

        // I/F
        public ContentManager CreateContentManager()
        {
            return new ContentManager(Game.Services);
        }

        // I/F
        public void Show(string screenName)
        {
            Screen = ScreenFactory.CreateScreen(screenName);
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

            // Animation を更新します。
            foreach (var animation in Screen.Animations)
            {
                if (animation.Enabled) animation.Update(gameTime);
            }

            // Screen は自分のサイズで測定を開始します。
            if (!Screen.Desktop.Measured)
            {
                Screen.Desktop.Measure(new Size(Screen.Desktop.Width, Screen.Desktop.Height));
            }
            // Screen は自分のマージンとサイズで配置を開始します。
            if (!Screen.Desktop.Arranged)
            {
                var margin = Screen.Desktop.Margin;
                Screen.Desktop.Arrange(new Rect(margin.Left, margin.Top, Screen.Desktop.Width, Screen.Desktop.Height));
            }

            // Control を更新します。
            Screen.Desktop.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (Screen == null) return;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, scissorTestRasterizerState);

            using (var drawContext = new DrawContext(this))
            {
                drawContext.Bounds = Screen.Desktop.ArrangedBounds.ToXnaRectangle();
                drawContext.Opacity = Screen.Desktop.Opacity;
                DrawControl(gameTime, Screen.Desktop, drawContext);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fillTexture = Texture2DHelper.CreateFillTexture(GraphicsDevice);
            basicEffect = new BasicEffect(GraphicsDevice);

            if (ControlLafSource != null) ControlLafSource.Initialize();

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            if (spriteBatch != null) spriteBatch.Dispose();
            if (fillTexture != null) fillTexture.Dispose();
            if (ControlLafSource != null) ControlLafSource.Dispose();

            base.UnloadContent();
        }

        /// <summary>
        /// Control を再帰的に描画します。
        /// </summary>
        /// <remarks>
        /// Control はその親 Control の描画領域でクリッピングされます。
        /// </remarks>
        /// <param name="gameTime"></param>
        /// <param name="control"></param>
        /// <param name="drawContext"></param>
        void DrawControl(GameTime gameTime, Control control, DrawContext drawContext)
        {
            // IControlLaf を描画します。
            var laf = GetControlLaf(control);
            if (laf != null) laf.Draw(control, drawContext);

            // 独自の描画があるならば描画します。
            control.Draw(gameTime, drawContext);

            if (control.Children.Count != 0)
            {
                var renderBounds = drawContext.Bounds;

                // 子を再帰的に描画します。
                foreach (var child in control.Children)
                {
                    // 不可視ならば描画しません。
                    if (!child.Visible) continue;

                    var childRenderBounds = child.ArrangedBounds;
                    childRenderBounds.X += renderBounds.X;
                    childRenderBounds.Y += renderBounds.Y;

                    var childXnaBounds = childRenderBounds.ToXnaRectangle();

                    // 描画する必要のないサイズならばスキップします。
                    if (childXnaBounds.Width <= 0 || childXnaBounds.Height <= 0) continue;

                    using (var childDrawContext = new DrawContext(this))
                    {
                        if (control.OpacityInherited)
                        {
                            childDrawContext.Opacity = drawContext.Opacity * child.Opacity;
                        }
                        else
                        {
                            childDrawContext.Opacity = child.Opacity;
                        }
                        childDrawContext.Bounds = childXnaBounds;

                        if (child.Clipped)
                        {
                            // 描画領域をクリッピングします。
                            using (var scissor = new Scissor(this, childXnaBounds))
                            {
                                DrawControl(gameTime, child, childDrawContext);
                            }
                        }
                        else
                        {
                            DrawControl(gameTime, child, childDrawContext);
                        }
                    }
                }
            }
        }

        IControlLaf GetControlLaf(Control control)
        {
            if (ControlLafSource == null) return null;

            return ControlLafSource.GetControlLaf(control);
        }
    }
}
