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
        private class Scissor : IDisposable
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
            }

            void BeginClipping(ref Rectangle scissorRectangle)
            {
                var spriteBatch = uiManager.SpriteBatch;
                var graphicsDevice = uiManager.GraphicsDevice;

                spriteBatch.End();

                previousScissorRectangle = graphicsDevice.ScissorRectangle;

                // Viewport の領域からはみ出ないように領域を調整します (はみ出ると例外が発生します)。
                var viewportBounds = graphicsDevice.Viewport.Bounds;
                var finalScissorRectangle = new Rectangle();
                finalScissorRectangle.X = (int) MathHelper.Max(viewportBounds.X, scissorRectangle.X);
                finalScissorRectangle.Y = (int) MathHelper.Max(viewportBounds.Y, scissorRectangle.Y);
                finalScissorRectangle.Width = (int) MathHelper.Min(viewportBounds.Right, scissorRectangle.Right) - finalScissorRectangle.X;
                finalScissorRectangle.Height = (int) MathHelper.Min(viewportBounds.Bottom, scissorRectangle.Bottom) - finalScissorRectangle.Y;

                // 親の ScissorRectangle を考慮した領域を計算します。
                finalScissorRectangle = Rectangle.Intersect(finalScissorRectangle, previousScissorRectangle);

                graphicsDevice.ScissorRectangle = finalScissorRectangle;
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, uiManager.scissorTestRasterizerState);
            }

            void EndClipping()
            {
                var spriteBatch = uiManager.SpriteBatch;
                var graphicsDevice = spriteBatch.GraphicsDevice;

                spriteBatch.End();

                graphicsDevice.ScissorRectangle = previousScissorRectangle;
                spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, uiManager.scissorTestRasterizerState);
            }
        }

        #endregion

        IInputService inputService;

        IInputCapturer inputCapturer;

        Screen screen;

        IControlLafSource controlLafSource;

        RasterizerState scissorTestRasterizerState;

        // I/F
        public SpriteBatch SpriteBatch { get; private set; }

        // I/F
        public Texture2D FillTexture { get; private set; }

        // I/F
        public BasicEffect BasicEffect { get; private set; }

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

            SpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, null, null, scissorTestRasterizerState);
            DrawControl(gameTime, Screen, Screen.ArrangedBounds.ToXnaRectangle());
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);
            FillTexture = Texture2DHelper.CreateFillTexture(GraphicsDevice);
            BasicEffect = new BasicEffect(GraphicsDevice);

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

        /// <summary>
        /// Control を再帰的に描画します。
        /// </summary>
        /// <remarks>
        /// Control はその親 Control の描画領域でクリッピングされます。
        /// </remarks>
        /// <param name="gameTime"></param>
        /// <param name="control"></param>
        /// <param name="renderBounds"></param>
        void DrawControl(GameTime gameTime, Control control, Rectangle renderBounds)
        {
            // IControlLaf を描画します。
            var laf = GetControlLaf(control);
            if (laf != null) laf.Draw(control, renderBounds);

            // 独自の描画があるならば描画します。
            control.Draw(gameTime, renderBounds);

            if (control.Children.Count != 0)
            {
                // 子を再帰的に描画します。
                foreach (var child in control.Children)
                {
                    // 不可視ならば描画しません。
                    if (!child.Visible) continue;

                    var childRenderBounds = child.ArrangedBounds;
                    childRenderBounds.X += renderBounds.X;
                    childRenderBounds.Y += renderBounds.Y;

                    var xnaRectangle = childRenderBounds.ToXnaRectangle();

                    // 描画する必要のないサイズならばスキップします。
                    if (xnaRectangle.Width <= 0 || xnaRectangle.Height <= 0) continue;

                    if (child.Clipped)
                    {
                        // 描画領域をクリッピングします。
                        using (var scissor = new Scissor(this, xnaRectangle))
                        {
                            DrawControl(gameTime, child, xnaRectangle);
                        }
                    }
                    else
                    {
                        DrawControl(gameTime, child, xnaRectangle);
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
