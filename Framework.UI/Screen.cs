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
        /// フォーカスが設定されている Control への弱参照。
        /// </summary>
        WeakReference focusedControl = new WeakReference(null);

        /// <summary>
        /// Root。
        /// </summary>
        Root root;

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
        /// ルート Control を取得します。
        /// </summary>
        public Control Root
        {
            get { return root; }
        }

        /// <summary>
        /// Desktop を取得します。
        /// </summary>
        public Desktop Desktop
        {
            get { return root.Desktop; }
        }

        /// <summary>
        /// フォーカスが設定されている Control を取得します。
        /// </summary>
        public Control FocusedControl
        {
            get { return focusedControl.Target as Control; }
            internal set
            {
                if (focusedControl.Target != null)
                {
                    (focusedControl.Target as Control).Focused = false;
                }

                focusedControl.Target = value;

                if (focusedControl.Target != null)
                {
                    (focusedControl.Target as Control).Focused = true;
                }
            }
        }

        /// <summary>
        /// Data Context を取得または設定します。
        /// </summary>
        public object DataContext { get; set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="game">Game。</param>
        protected Screen(Game game)
        {
            Game = game;
            GraphicsDevice = game.GraphicsDevice;
            Content = new ContentManager(game.Services);
            root = new Root(this);

            // 初期状態では Desktop をアクティブにします。
            root.Desktop.Activate();
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
            // Screen のレイアウトを更新します。
            root.UpdateLayout();
            // Control の前後関係が変化している可能性があるため、カーソル位置について再処理します。
            root.ProcessMouseMove();

            // Control を更新します。
            root.Update(gameTime);
        }

        /// <summary>
        /// マウス カーソルが移動した時に呼び出されます。
        /// </summary>
        protected internal void ProcessMouseMove()
        {
            root.ProcessMouseMove();
        }

        /// <summary>
        /// マウス ボタンが押された時に呼び出されます。
        /// </summary>
        protected internal void ProcessMouseDown()
        {
            root.ProcessMouseDown();
        }

        /// <summary>
        /// マウス ボタンが離された時に呼び出されます。
        /// </summary>
        protected internal void ProcessMouseUp()
        {
            root.ProcessMouseUp();
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
            // フォーカスが設定された Control が無いならば何も処理しません。
            if (FocusedControl == null) return;

            // キーが押されたことを Control へ通知します。
            FocusedControl.ProcessKeyDown();
        }

        /// <summary>
        /// キーが離された時に呼び出されます。
        /// </summary>
        protected internal void ProcessKeyUp()
        {
            // フォーカスが設定された Control が無いならば何も処理しません。
            if (FocusedControl == null) return;

            // キーが離されたことを Control へ通知します。
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
            root.UpdateLayout();

            Initialized = true;
        }

        /// <summary>
        /// 指定の Control へフォーカスを設定します。
        /// </summary>
        /// <param name="control">フォーカスを設定する Control。</param>
        /// <returns>
        /// true (フォーカスが設定された場合)、
        /// false (論理フォーカスの設定のみが行われた場合、
        /// あるいは、フォーカス設定不能な Control の場合)。
        /// </returns>
        internal bool MoveFocusTo(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");

            if (!control.Focusable || !control.Enabled || !control.Visible) return false;

            // アクティブ Window の Control ならばフォーカスを設定します。
            var window = Window.GetWindow(control);
            if (window != null && window.Active)
            {
                FocusedControl = control;
                return true;
            }

            return false;
        }

        /// <summary>
        /// 指定の Window を表示します。
        /// </summary>
        /// <param name="window">表示する Window。</param>
        internal void ShowWindow(Window window)
        {
            root.ShowWindow(window);
        }

        /// <summary>
        /// Window をアクティブ化します。
        /// Window の Visible プロパティが true に設定されます。
        /// </summary>
        /// <param name="window">アクティブ化する Window。</param>
        internal void ActivateWindow(Window window)
        {
            root.ActivateWindow(window);
        }

        /// <summary>
        /// 指定の Window を非表示にします。
        /// </summary>
        /// <param name="window">非表示にする Window。</param>
        internal void HideWindow(Window window)
        {
            root.HideWindow(window);
        }

        /// <summary>
        /// 指定の Window を閉じます。
        /// </summary>
        /// <remarks>
        /// Desktop を指定した場合には例外が発生します (Desktop を閉じることはできません)。
        /// </remarks>
        /// <param name="window">閉じる Window。</param>
        internal void CloseWindow(Window window)
        {
            root.CloseWindow(window);
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
