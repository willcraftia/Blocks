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
    /// <summary>
    /// ユーザ インタフェースを管理する GameComponent です。
    /// </summary>
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
            /// <summary>
            /// UIManager。
            /// </summary>
            UIManager uiManager;

            /// <summary>
            /// BeginClipping を開始する前に設定されていたシザー テスト領域。
            /// </summary>
            Rectangle previousScissorRectangle;

            /// <summary>
            /// インスタンスを生成します。
            /// </summary>
            /// <param name="uiManager">UIManager。</param>
            /// <param name="scissorRectangle">GraphicsDevice に設定するシザー テスト領域。</param>
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

            /// <summary>
            /// クリッピングを開始します。
            /// </summary>
            /// <param name="scissorRectangle">GraphicsDevice に設定するシザー テスト領域。</param>
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

            /// <summary>
            /// クリッピングを終了します。
            /// </summary>
            /// <remarks>
            /// BeginClipping メソッドの呼び出し前に設定されていたシザー テスト領域を GraphicsDevice に再設定します。
            /// </remarks>
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

        /// <summary>
        /// IDrawContext の実装クラスです。
        /// </summary>
        class DrawContext : IDrawContext, IDisposable
        {
            /// <summary>
            /// UIManager。
            /// </summary>
            UIManager uiManager;

            // I/F
            public SpriteBatch SpriteBatch
            {
                get { return uiManager.spriteBatch; }
            }

            // I/F
            public Rectangle Bounds { get; internal set; }

            // I/F
            public float Opacity { get; internal set; }

            /// <summary>
            /// インスタンスを生成します。
            /// </summary>
            /// <param name="uiManager">UIManager。</param>
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

        /// <summary>
        /// IInputService。
        /// </summary>
        IInputService inputService;

        /// <summary>
        /// IInputCapturer。
        /// </summary>
        IInputCapturer inputCapturer;

        /// <summary>
        /// Screen の生成に使用する IScreenFactory。
        /// </summary>
        IScreenFactory screenFactory;

        /// <summary>
        /// 表示対象の Screen。
        /// </summary>
        Screen screen;

        /// <summary>
        /// 描画に用いる IControlLafSource。
        /// </summary>
        IControlLafSource controlLafSource;

        /// <summary>
        /// シザー テストのための RasterizerState。
        /// </summary>
        RasterizerState scissorTestRasterizerState;

        /// <summary>
        /// 描画に用いる SpriteBatch。
        /// </summary>
        SpriteBatch spriteBatch;

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

                    // 破棄します。
                    // MEMO: 恐らく他のタイミングでは明示的に Dispose を呼ぶのが難しい。
                    screen.Dispose();
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

        /// <summary>
        /// IInputCapturer を取得または設定します。
        /// </summary>
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
        /// IControlLafSource を取得または設定します。
        /// </summary>
        /// <value>
        /// Look & Feel を使用しない場合には null を設定します。
        /// </value>
        public IControlLafSource ControlLafSource
        {
            get { return controlLafSource; }
            set
            {
                if (controlLafSource == value) return;

                controlLafSource = value;
            }
        }

        /// <summary>
        /// Screen の生成に使用する IScreenFactory を取得または設定します。
        /// </summary>
        /// <remarks>
        /// Show メソッドによる Screen の名前からの Screen の決定と生成は IScreenFactory に移譲されます。
        /// </remarks>
        public IScreenFactory ScreenFactory
        {
            get { return screenFactory; }
            set
            {
                if (screenFactory == value) return;

                screenFactory = value;
            }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game">Game。</param>
        public UIManager(Game game)
            : base(game)
        {
            // サービスとして登録します。
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
                var desktop = Screen.Desktop;
                drawContext.Bounds = desktop.ArrangedBounds.ToXnaRectangle();
                drawContext.Opacity = desktop.Opacity;
                DrawControl(gameTime, desktop, drawContext);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            if (ControlLafSource != null && !ControlLafSource.Initialized) ControlLafSource.Initialize();
            if (ScreenFactory != null && !ScreenFactory.Initialized) ScreenFactory.Initialize();

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            if (spriteBatch != null) spriteBatch.Dispose();
            if (ControlLafSource != null && ControlLafSource.Initialized) ControlLafSource.Dispose();
            if (ScreenFactory != null && ScreenFactory.Initialized) ScreenFactory.Dispose();

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

        /// <summary>
        /// IControlLafSource から指定の Control のための IControlLaf を取得します。
        /// </summary>
        /// <param name="control">Control。</param>
        /// <returns>
        /// 指定の Control のための IControlLaf。
        /// ControlLafSource プロパティが null、あるいは、IControlLafSource 内で見つからない場合は null を返します。
        /// </returns>
        IControlLaf GetControlLaf(Control control)
        {
            if (ControlLafSource == null) return null;

            return ControlLafSource.GetControlLaf(control);
        }
    }
}
