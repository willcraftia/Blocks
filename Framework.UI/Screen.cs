#region Using

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Control で構成される画面を表すクラスです。
    /// </summary>
    public class Screen : IDisposable
    {
        /// <summary>
        /// フォーカスを持つ Control の弱参照。
        /// </summary>
        WeakReference focusedControlWeakReference = new WeakReference(null);

        /// <summary>
        /// Screen が初期化されているかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (Screen が初期化されている場合)、false (それ以外の場合)。
        /// </value>
        public bool Initialized { get; private set; }

        /// <summary>
        /// Game を取得します。
        /// </summary>
        public Game Game { get; private set; }

        /// <summary>
        /// MouseDevice を取得します。
        /// </summary>
        public MouseDevice MouseDevice { get; internal set; }

        /// <summary>
        /// KeyboardDevice を取得します。
        /// </summary>
        public KeyboardDevice KeyboardDevice { get; internal set; }

        /// <summary>
        /// GraphicsDevice を取得します。
        /// </summary>
        public GraphicsDevice GraphicsDevice { get; private set; }

        /// <summary>
        /// ContentManager を取得します。
        /// </summary>
        /// <remarks>
        /// この ContentManager は、Screen の生成と同時に生成され、Screen の破棄と同時に破棄されます。
        /// </remarks>
        public ContentManager Content { get; private set; }

        /// <summary>
        /// デフォルトの BasicEffect を取得します。
        /// </summary>
        public BasicEffect BasicEffect { get; private set; }

        /// <summary>
        /// デフォルトの SpriteFont を取得または設定します。
        /// </summary>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// ILookAndFeelSource を取得または設定します。
        /// </summary>
        public ILookAndFeelSource LookAndFeelSource { get; set; }

        /// <summary>
        /// Desktop を取得します。
        /// </summary>
        public Desktop Desktop { get; private set; }

        /// <summary>
        /// Animation コレクションを取得します。
        /// </summary>
        public AnimationCollection Animations { get; private set; }

        /// <summary>
        /// キーと FocusNavigation のマップを取得します。
        /// </summary>
        protected Dictionary<Keys, FocusNavigation> KeyFocusNavigationMap { get; private set; }

        /// <summary>
        /// フォーカスを持つ Control を取得します。
        /// </summary>
        internal Control FocusedControl
        {
            get { return focusedControlWeakReference.Target as Control; }
            set { focusedControlWeakReference.Target = value; }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="game">Game。</param>
        protected Screen(Game game)
        {
            Game = game;
            GraphicsDevice = game.GraphicsDevice;
            Content = new ContentManager(game.Services);

            Animations = new AnimationCollection(this);
            
            KeyFocusNavigationMap = new Dictionary<Keys, FocusNavigation>();
            KeyFocusNavigationMap[Keys.Up] = FocusNavigation.Up;
            KeyFocusNavigationMap[Keys.Down] = FocusNavigation.Down;
            KeyFocusNavigationMap[Keys.Left] = FocusNavigation.Left;
            KeyFocusNavigationMap[Keys.Right] = FocusNavigation.Right;

            Desktop = new Desktop(this);

            // フォーカスは Desktop に設定しておきます。
            FocusedControl = Desktop;
        }

        /// <summary>
        /// コンテンツをロードします。
        /// </summary>
        /// <remarks>
        /// このメソッドは Initialize メソッドから呼び出されます。
        /// サブクラスでは、このメソッドをオーバライドして Control の配置などを行います。
        /// </remarks>
        protected virtual void LoadContent() { }

        /// <summary>
        /// コンテンツをアンロードします。
        /// </summary>
        /// <remarks>
        /// このメソッドは、Dispose メソッドの呼び出しか、ガベージコレクションによるインスタンス破棄の際に呼び出されます。
        /// </remarks>
        protected virtual void UnloadContent() { }

        /// <summary>
        /// 更新します。
        /// </summary>
        /// <param name="gameTime"></param>
        protected internal virtual void Update(GameTime gameTime)
        {
            // Animation を更新します。
            foreach (var animation in Animations)
            {
                if (animation.Enabled) animation.Update(gameTime);
            }

            // Screen のレイアウトを更新します。
            UpdateLayout();
            // Control の前後関係が変化している可能性があるため、カーソル位置について再処理します。
            Desktop.ProcessMouseMove();

            // Control を更新します。
            Desktop.Update(gameTime);
        }

        /// <summary>
        /// レイアウトを更新します。
        /// </summary>
        protected internal void UpdateLayout()
        {
            // 測定を開始します。
            Desktop.Measure(new Size(Desktop.Width, Desktop.Height));

            // 配置を開始します。
            var margin = Desktop.Margin;
            Desktop.Arrange(new Rect(margin.Left, margin.Top, Desktop.Width, Desktop.Height));
        }

        /// <summary>
        /// マウス カーソルが移動した時に呼び出されます。
        /// </summary>
        protected internal void ProcessMouseMove()
        {
            Desktop.ProcessMouseMove();
        }

        /// <summary>
        /// マウス ボタンが押された時に呼び出されます。
        /// </summary>
        protected internal void ProcessMouseDown()
        {
            Desktop.ProcessMouseDown();
        }

        /// <summary>
        /// マウス ボタンが離された時に呼び出されます。
        /// </summary>
        protected internal void ProcessMouseUp()
        {
            Desktop.ProcessMouseUp();
        }

        /// <summary>
        /// マウス ホイールが回転した時に呼び出されます。
        /// </summary>
        protected internal void ProcessMouseWheel()
        {
            // TODO
        }

        /// <summary>
        /// キーが押された時に呼び出されます。
        /// </summary>
        protected internal void ProcessKeyDown()
        {
            if (FocusedControl.ProcessKeyDown()) return;

            bool focusChanged = false;
            foreach (var entry in KeyFocusNavigationMap)
            {
                if (KeyboardDevice.IsKeyPressed(entry.Key))
                {
                    focusChanged = FocusedControl.MoveFocus(entry.Value);
                    if (focusChanged) break;
                }
            }
        }

        /// <summary>
        /// キーが離された時に呼び出されます。
        /// </summary>
        protected internal void ProcessKeyUp()
        {
            FocusedControl.ProcessKeyUp();
        }

        /// <summary>
        /// Screen を初期化します。
        /// </summary>
        /// <remarks>
        /// このメソッドの呼び出しにより Initialized プロパティが true に設定されます。
        /// </remarks>
        internal void Initialize()
        {
            if (Initialized) throw new InvalidOperationException("Screen is already initialized.");

            BasicEffect = new BasicEffect(GraphicsDevice);

            LoadContent();

            // Screen のレイアウトを更新します。
            UpdateLayout();

            Initialized = true;
        }

        /// <summary>
        /// 指定の Control にフォーカスを設定します。
        /// </summary>
        /// <param name="control">フォーカスを設定する Control。</param>
        internal void SetFocus(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");

            if (!control.Enabled || !control.Visible || !control.Focusable) return;

            // Control をフォーカス解除状態にします。
            if (FocusedControl != null) FocusedControl.Focused = false;

            FocusedControl = control;

            // Control をフォーカス設定状態にします。
            if (FocusedControl != null) FocusedControl.Focused = true;
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        bool disposed;

        ~Screen()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing)
            {
                UnloadContent();

                if (BasicEffect != null) BasicEffect.Dispose();
            }

            disposed = true;
        }

        #endregion
    }
}
