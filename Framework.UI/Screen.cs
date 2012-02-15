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
        /// コンストラクタ。
        /// </summary>
        /// <param name="game">Game。</param>
        protected Screen(Game game)
        {
            Game = game;
            GraphicsDevice = game.GraphicsDevice;
            Content = new ContentManager(game.Services);

            Desktop = new Desktop(this);

            // Desktop のプロパティへデフォルト値を設定します。
            var viewportBounds = GraphicsDevice.Viewport.TitleSafeArea;
            Desktop.BackgroundColor = Color.CornflowerBlue;
            Desktop.Margin = new Thickness(viewportBounds.Left, viewportBounds.Top, 0, 0);
            Desktop.Width = viewportBounds.Width;
            Desktop.Height = viewportBounds.Height;
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
            UpdateLayout();

            Initialized = true;
        }

        /// <summary>
        /// 指定の Window を表示します。
        /// </summary>
        /// <param name="window">表示する Window。</param>
        internal void ShowWindow(Window window)
        {
            if (Desktop.Windows.Contains(window))
                throw new InvalidOperationException(
                    "The specified window is already registered.");

            var oldActiveWindow = Desktop.GetTopMostWindow();
            if (oldActiveWindow != null) oldActiveWindow.Active = false;

            Desktop.Windows.Add(window);
            window.Active = true;
            FocusedControl = window.LogicalFocusedControl;
        }

        /// <summary>
        /// 指定の Window を閉じます。
        /// </summary>
        /// <param name="window">閉じる Window。</param>
        internal void CloseWindow(Window window)
        {
            if (window == null) throw new ArgumentNullException("window");
            if (!Desktop.Windows.Contains(window))
                throw new InvalidOperationException(
                    "The specified window could not be found in this screen.");

            Desktop.Windows.Remove(window);

            if (window.Active)
            {
                window.Active = false;
                var newActiveWindow = Desktop.GetTopMostWindow();
                if (newActiveWindow != null)
                {
                    newActiveWindow.Active = true;
                    FocusedControl = newActiveWindow.LogicalFocusedControl;
                }
                else
                {
                    FocusedControl = null;
                }
            }
        }

        /// <summary>
        /// Window をアクティブ化します。
        /// </summary>
        /// <param name="window">アクティブ化する Window。</param>
        internal void ActivateWindow(Window window)
        {
            if (window == null) throw new ArgumentNullException("window");
            if (!Desktop.Windows.Contains(window))
                throw new InvalidOperationException(
                    "The specified window could not be found in this screen.");

            // 既にアクティブな場合には処理を終えます。
            if (window.Active) return;

            var oldActiveWindow = Desktop.GetTopMostWindow();
            if (oldActiveWindow != null) oldActiveWindow.Active = false;

            // 最前面へ移動させます。
            Desktop.Windows.Remove(window);
            Desktop.Windows.Add(window);
            window.Active = true;
            FocusedControl = window.LogicalFocusedControl;
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

            var window = Window.GetWindow(control);
            if (window == null) return false;

            // 論理フォーカスを設定します。
            window.LogicalFocusedControl = control;

            // 非アクティブ Window の Control ならば論理フォーカスの設定のみで終えます。
            if (!window.Active) return false;

            // アクティブ Window の Control ならばフォーカスを設定します。
            FocusedControl = control;
            return true;
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
