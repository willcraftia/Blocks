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
        #region ScissorManager

        /// <summary>
        /// Control 描画のための GraphicsDevice の ScissorRectangle
        /// および SpriteBatch の Begin/End 状態を管理するクラスです。
        /// </summary>
        /// <remarks>
        /// Control の描画は再帰的に行うため、スタック的に ScissorRectangle の状態を維持する必要があります。
        /// ここで、GraphicsDevice の ScissorRectangle は、SpriteBatch の Begin 前に設定しなければ、
        /// SpriteBatch による描画で有効になりません。
        /// このため、using 区を用いてこのクラスのインスタンスを管理することを前提に、
        /// Scissor インスタンス生成で SpriteBatch の状態を End させ、
        /// GraphicsDevice に新たな ScissorRectangle を設定して SpriteBatch を Begin し、
        /// using 終了による Dispose 呼び出しにて SpriteBatch を End し、
        /// 前回の ScissorRectangle を再設定して SpriteBatch を Begin します。
        /// これにより、GraphicsDevice の ScissorRectangle の状態と
        /// SpriteBatch の Begin/End の状態をスタック化できます。
        /// </remarks>
        class ScissorManager : IDisposable
        {
            /// <summary>
            /// UIManager。
            /// </summary>
            UIManager uiManager;

            /// <summary>
            /// クリップ開始前に設定されていた GraphicsDevice のシザー テスト領域のスタック。
            /// </summary>
            Stack<Rectangle> scissorRectangleStack = new Stack<Rectangle>();

            bool spriteBatchActive;

            /// <summary>
            /// インスタンスを生成します。
            /// </summary>
            /// <param name="uiManager">UIManager。</param>
            public ScissorManager(UIManager uiManager)
            {
                this.uiManager = uiManager;
            }

            // I/F
            public void Dispose()
            {
                End();
            }

            /// <summary>
            /// クリップを開始します。
            /// </summary>
            /// <param name="scissorRectangle">ScissorRectangle。</param>
            /// <param name="inherite">
            /// true (既存のクリップ領域のサブセットとして用いる場合)、
            /// false (指定のクリップ領域をそのまま用いる場合)。
            /// </param>
            /// <param name="spriteBatchActive">
            /// true (SpriteBatch がアクティブな場合)、false (それ以外の場合)。
            /// </param>
            public void Begin(ref Rectangle scissorRectangle, bool inherite, bool spriteBatchActive)
            {
                this.spriteBatchActive = spriteBatchActive;

                var spriteBatch = uiManager.spriteBatch;
                var graphicsDevice = spriteBatch.GraphicsDevice;

                // これまでの SpriteBatch を一旦終えます。
                if (spriteBatchActive) uiManager.EndSpriteBetch();

                // これまでの ScissorRectangle をスタックへ退避させます。
                var previousScissorRectangle = graphicsDevice.ScissorRectangle;
                scissorRectangleStack.Push(previousScissorRectangle);

                // Viewport からはみ出ないように調整します (はみ出ると例外が発生します)。
                var viewportBounds = graphicsDevice.Viewport.Bounds;
                Rectangle viewIntersectBounds;
                Rectangle.Intersect(ref viewportBounds, ref scissorRectangle, out viewIntersectBounds);

                Rectangle finalScissorRectangle;
                if (inherite)
                {
                    // 親の ScissorRectangle を考慮した領域を計算します。
                    Rectangle.Intersect(ref viewIntersectBounds, ref previousScissorRectangle, out finalScissorRectangle);
                }
                else
                {
                    // 親の ScissorRectangle を考慮しません。
                    finalScissorRectangle = viewIntersectBounds;
                }

                // サイズを持つ場合にだけ設定するようにします。
                if (0 < finalScissorRectangle.Width && 0 < finalScissorRectangle.Height)
                    graphicsDevice.ScissorRectangle = finalScissorRectangle;

                // 設定された ScissorRectangle で SpriteBatch を再開します。
                if (spriteBatchActive) uiManager.BeingSpriteBatch();
            }

            /// <summary>
            /// クリップを終了します。
            /// </summary>
            /// <remarks>
            /// クリップ開始前に設定されていたシザー テスト領域を GraphicsDevice に再設定します。
            /// </remarks>
            public void End()
            {
                var spriteBatch = uiManager.spriteBatch;
                var graphicsDevice = spriteBatch.GraphicsDevice;

                // Begin で始めた SpriteBatch を一旦終えます。
                if (spriteBatchActive) uiManager.EndSpriteBetch();

                // Begin でスタックに退避させていた ScissorRectangle を戻します。
                graphicsDevice.ScissorRectangle = scissorRectangleStack.Pop();

                // 戻した ScissorRectangle で SpriteBatch を再開します。
                if (spriteBatchActive) uiManager.BeingSpriteBatch();
            }
        }

        #endregion

        #region ViewportManager

        /// <summary>
        /// Viewport の変更をスタック管理するクラスです。
        /// </summary>
        class ViewportManager : IDisposable
        {
            /// <summary>
            /// UIManager。
            /// </summary>
            UIManager uiManager;

            /// <summary>
            /// Viewport 変更開始前に設定されていた Viewport のスタック。
            /// </summary>
            Stack<Viewport> viewportStack = new Stack<Viewport>();

            /// <summary>
            /// インスタンスを生成します。
            /// </summary>
            /// <param name="uiManager">UIManager。</param>
            public ViewportManager(UIManager uiManager)
            {
                this.uiManager = uiManager;
            }

            // I/F
            public void Dispose()
            {
                End();
            }

            /// <summary>
            /// Viewport の変更を開始します。
            /// </summary>
            /// <param name="viewportBounds"></param>
            public void Begin(ref Rect viewportBounds)
            {
                var graphicsDevice = uiManager.GraphicsDevice;

                // これまでの Viewport をスタックへ退避させます。
                var previousViewport = graphicsDevice.Viewport;
                viewportStack.Push(previousViewport);

                // 新たな Viewport を計算します。
                var offset = uiManager.drawContext.Location;
                var viewportRectangle = new Rectangle()
                {
                    X = (int) (offset.X + viewportBounds.X),
                    Y = (int) (offset.Y + viewportBounds.Y),
                    Width = (int) viewportBounds.Width,
                    Height = (int) viewportBounds.Height
                };

                // 親の Viewport を考慮した領域を計算します。
                var newBounds = Rectangle.Intersect(previousViewport.Bounds, viewportRectangle);

                // サイズを持つ場合にだけ設定するようにします。
                if (0 < newBounds.Width && 0 < newBounds.Height)
                    graphicsDevice.Viewport = new Viewport(newBounds);
            }

            /// <summary>
            /// Viewport の変更を終了します。
            /// </summary>
            public void End()
            {
                var graphicsDevice = uiManager.GraphicsDevice;

                // Begin でスタックに退避させていた Viewport を戻します。
                graphicsDevice.Viewport = viewportStack.Pop();
            }
        }

        #endregion

        #region Draw3DManager

        class Draw3DManager : IDisposable
        {
            /// <summary>
            /// UIManager。
            /// </summary>
            UIManager uiManager;

            public bool Active { get; set; }

            /// <summary>
            /// インスタンスを生成します。
            /// </summary>
            /// <param name="uiManager">UIManager。</param>
            public Draw3DManager(UIManager uiManager)
            {
                this.uiManager = uiManager;
            }

            // I/F
            public void Dispose()
            {
                End();
            }

            public void Begin()
            {
                // これまでの SpriteBatch を一旦終えます。
                uiManager.EndSpriteBetch();

                Active = true;
            }

            public void End()
            {
                // SpriteBatch を再開します。
                uiManager.BeingSpriteBatch();

                Active = false;
            }
        }

        #endregion

        #region IDrawContext

        /// <summary>
        /// IDrawContext の実装クラスです。
        /// </summary>
        class DrawContext : IDrawContext
        {
            /// <summary>
            /// UIManager。
            /// </summary>
            UIManager uiManager;

            /// <summary>
            /// 使用する座標系の画面座標における位置。
            /// </summary>
            Vector2 location;

            /// <summary>
            /// クリッピングを制御する ScissorManager。
            /// </summary>
            ScissorManager scissorManager;

            /// <summary>
            /// Viewport 変更を管理する ViewportManager。
            /// </summary>
            ViewportManager viewportManager;

            /// <summary>
            /// 透明度スタック。
            /// </summary>
            List<float> opacityStack = new List<float>();

            /// <summary>
            /// 透明度スタックにある全ての値の乗算結果。
            /// </summary>
            float currentOpacity;

            Draw3DManager draw3DManager;

            // I/F
            public SpriteBatch SpriteBatch
            {
                get { return uiManager.spriteBatch; }
            }

            // I/F
            public Vector2 Location
            {
                get { return location; }
                set { location = value; }
            }

            // I/F
            public float Opacity
            {
                get { return currentOpacity; }
            }

            /// <summary>
            /// インスタンスを生成します。
            /// </summary>
            /// <param name="uiManager">UIManager。</param>
            public DrawContext(UIManager uiManager)
            {
                this.uiManager = uiManager;

                scissorManager = new ScissorManager(uiManager);
                viewportManager = new ViewportManager(uiManager);
                draw3DManager = new Draw3DManager(uiManager);
            }

            // I/F
            public void PushOpacity(float opacity)
            {
                opacityStack.Add(opacity);
                CalculateCurrentOpacity();
            }

            // I/F
            public void PopOpacity()
            {
                opacityStack.RemoveAt(opacityStack.Count - 1);
                CalculateCurrentOpacity();
            }

            // I/F
            public IDisposable BeginClip(Rect clipBounds)
            {
                // 新たな ScissorRectangle を計算します。
                var scissorRectangle = new Rectangle()
                {
                    X = (int) (location.X + clipBounds.X),
                    Y = (int) (location.Y + clipBounds.Y),
                    Width = (int) clipBounds.Width,
                    Height = (int) clipBounds.Height
                };

                scissorManager.Begin(ref scissorRectangle, true, !draw3DManager.Active);
                return scissorManager;
            }

            // I/F
            public IDisposable BeginNewClip(Rect clipBounds)
            {
                // 新たな ScissorRectangle を計算します。
                var scissorRectangle = new Rectangle()
                {
                    X = (int) (location.X + clipBounds.X),
                    Y = (int) (location.Y + clipBounds.Y),
                    Width = (int) clipBounds.Width,
                    Height = (int) clipBounds.Height
                };

                scissorManager.Begin(ref scissorRectangle, false, !draw3DManager.Active);
                return scissorManager;
            }

            // I/F
            public IDisposable BeginViewport(Rect viewportBounds)
            {
                viewportManager.Begin(ref viewportBounds);
                return viewportManager;
            }

            // I/F
            public IDisposable BeginDraw3D()
            {
                draw3DManager.Begin();
                return draw3DManager;
            }

            // I/F
            public ILookAndFeel GetLookAndFeel(Control control)
            {
                return control.Screen.LookAndFeelSource.GetLookAndFeel(control);
            }

            // I/F
            public void Flush()
            {
                uiManager.EndSpriteBetch();
                uiManager.BeingSpriteBatch();
            }

            // I/F
            public void DrawRectangle(Rect rect, Color color)
            {
                DrawTexture(rect, uiManager.fillTexture, color);
            }

            // I/F
            public void DrawTexture(Rect rect, Texture2D texture, Color color)
            {
                // todo
                var rectangle = new Rectangle()
                {
                    X = (int) (location.X + rect.X),
                    Y = (int) (location.Y + rect.Y),
                    Width = (int) rect.Width,
                    Height = (int) rect.Height
                };

                SpriteBatch.Draw(texture, rectangle, color * currentOpacity);
            }

            // I/F
            public void DrawTexture(Rect rect, Texture2D texture, Color color, Rectangle sourceRectangle)
            {
                // todo
                var rectangle = new Rectangle()
                {
                    X = (int) (location.X + rect.X),
                    Y = (int) (location.Y + rect.Y),
                    Width = (int) rect.Width,
                    Height = (int) rect.Height
                };

                SpriteBatch.Draw(texture, rectangle, sourceRectangle, color * currentOpacity);
            }

            // I/F
            public void DrawString(Rect clientBounds, SpriteFont font, string text, Vector2 stretch,
                HorizontalAlignment hAlign, VerticalAlignment vAlign, Color color, Thickness padding)
            {
                DrawString(clientBounds, font, text, stretch, hAlign, vAlign, color, padding, Vector2.Zero);
            }

            // I/F
            public void DrawString(Rect clientBounds, SpriteFont font, string text, Vector2 stretch,
                HorizontalAlignment hAlign, VerticalAlignment vAlign, Color color, Thickness padding, Vector2 offset)
            {
                // todo
                var rectangle = new Rectangle()
                {
                    X = (int) (location.X + clientBounds.X),
                    Y = (int) (location.Y + clientBounds.Y),
                    Width = (int) clientBounds.Width,
                    Height = (int) clientBounds.Height
                };

                TextHelper.DrawString(
                    SpriteBatch, rectangle, font, text, stretch,
                    hAlign, vAlign,
                    color * currentOpacity, padding, offset);
            }

            /// <summary>
            /// 透明度スタックにある値で現在の透明度を計算します。
            /// </summary>
            void CalculateCurrentOpacity()
            {
                currentOpacity = 1;
                foreach (var opacity in opacityStack)
                {
                    currentOpacity *= opacity;
                }
            }
        }

        #endregion

        /// <summary>
        /// MouseDevice。
        /// </summary>
        MouseDevice mouseDevice = new MouseDevice();

        /// <summary>
        /// KeyboardDevice。
        /// </summary>
        KeyboardDevice keyboardDevice = new KeyboardDevice();

        /// <summary>
        /// 表示対象の Screen。
        /// </summary>
        Screen screen;

        /// <summary>
        /// 次に表示する Screen。
        /// </summary>
        Screen nextScreen;

        /// <summary>
        /// nextScreen の更新で lock するオブジェクト。
        /// </summary>
        object nextScreenLock = new object();

        /// <summary>
        /// シザー テストのための RasterizerState。
        /// </summary>
        RasterizerState scissorTestRasterizerState;

        /// <summary>
        /// 描画に用いる SpriteBatch。
        /// </summary>
        SpriteBatch spriteBatch;

        /// <summary>
        /// DrawContext。
        /// </summary>
        DrawContext drawContext;

        /// <summary>
        /// 塗り潰しに利用するテクスチャ。
        /// </summary>
        Texture2D fillTexture;

        /// <summary>
        /// Screen の生成に使用する IScreenFactory を取得または設定します。
        /// </summary>
        /// <remarks>
        /// Show メソッドによる Screen の名前からの Screen の決定と生成は IScreenFactory に移譲されます。
        /// </remarks>
        public IScreenFactory ScreenFactory { get; set; }

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

            drawContext = new DrawContext(this);
        }

        // I/F
        public ContentManager CreateContentManager()
        {
            return new ContentManager(Game.Services);
        }

        // I/F
        public void Show(string screenName)
        {
            Show(ScreenFactory.CreateScreen(screenName));
        }

        // I/F
        public void Show(Screen screen)
        {
            if (screen == null) throw new ArgumentNullException("screen");

            // 必要ならば初期化します。
            if (!screen.Initialized) screen.Initialize();

            lock (nextScreenLock)
            {
                this.nextScreen = screen;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // 次の Screen が設定されているならば切り替えます。
            if (nextScreen != null)
            {
                if (screen != null)
                {
                    // MouseDevice をアンバインドします。
                    screen.MouseDevice = null;
                    // KeyboardDevice をアンバインドします。
                    screen.KeyboardDevice = null;

                    // 破棄します。
                    // MEMO: 恐らく他のタイミングでは明示的に Dispose を呼ぶのが難しい。
                    screen.Dispose();
                }

                screen = nextScreen;

                if (screen != null)
                {
                    // MouseDevice をバインドします。
                    screen.MouseDevice = mouseDevice;
                    // KeyboardDevice をバインドします。
                    screen.KeyboardDevice = keyboardDevice;
                }

                nextScreen = null;
            }

            // ユーザ入力を処理します。
            ProcessInput();

            // Screen を更新します。
            screen.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            if (screen == null) return;

            BeingSpriteBatch();

            drawContext.Location = Vector2.Zero;
            screen.Root.Draw(gameTime, drawContext);

            EndSpriteBetch();

            base.Draw(gameTime);
        }

        internal void BeingSpriteBatch()
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, scissorTestRasterizerState);
        }

        internal void EndSpriteBetch()
        {
            spriteBatch.End();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            fillTexture = Texture2DHelper.CreateFillTexture(GraphicsDevice);
            if (ScreenFactory != null && !ScreenFactory.Initialized) ScreenFactory.Initialize();

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            if (nextScreen != null) nextScreen.Dispose();
            if (screen != null) screen.Dispose();

            if (spriteBatch != null) spriteBatch.Dispose();
            if (fillTexture != null) fillTexture.Dispose();
            if (ScreenFactory != null && ScreenFactory.Initialized) ScreenFactory.Dispose();

            base.UnloadContent();
        }

        /// <summary>
        /// ユーザ入力を処理します。
        /// </summary>
        void ProcessInput()
        {
            // MouseDevice を更新します。
            mouseDevice.Update();
            // KeyboardDevice を更新します。
            keyboardDevice.Update();

            // マウス カーソルが移動したことを Screen へ通知します。
            if (mouseDevice.MouseMoved) screen.ProcessMouseMove();
            // マウス ボタンが押されたことを Screen へ通知します。
            if (mouseDevice.ButtonPressed) screen.ProcessMouseDown();
            // マウス ボタンが離されたことを Screen へ通知します。
            if (mouseDevice.ButtonReleased) screen.ProcessMouseUp();
            // マウス ホイールが回転したことを Screen へ通知します。
            if (mouseDevice.WheelScrolled) screen.ProcessMouseWheel();

            // キーが押されたことを Screen へ通知します。
            if (keyboardDevice.KeyPressed) screen.ProcessKeyDown();
            // キーが離されたことを Screen へ通知します。
            if (keyboardDevice.KeyReleased) screen.ProcessKeyUp();
        }
    }
}
