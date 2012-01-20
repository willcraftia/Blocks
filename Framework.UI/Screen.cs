﻿#region Using

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
    public class Screen : IInputReceiver, IDisposable
    {
        /// <summary>
        /// 最後に Screen が得たマウス カーソルの位置。
        /// </summary>
        Point lastMousePosition;

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
        /// Desktop を取得します。
        /// </summary>
        public Desktop Desktop { get; private set; }

        /// <summary>
        /// Animation コレクションを取得します。
        /// </summary>
        public AnimationCollection Animations { get; private set; }

        /// <summary>
        /// フォーカスを得ている Control。
        /// </summary>
        internal Control FocusedControl { get; private set; }

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
            Desktop = new Desktop(this);

            // フォーカスは Desktop に設定しておきます。
            FocusedControl = Desktop;
        }

        // I/F
        public void NotifyMouseMove(int x, int y)
        {
            Desktop.ProcessMouseMove(x, y);

            lastMousePosition.X = x;
            lastMousePosition.Y = y;
        }

        // I/F
        public void NotifyMouseDown(MouseButtons button)
        {
            Desktop.ProcessMouseDown(button);
        }

        // I/F
        public void NotifyMouseUp(MouseButtons button)
        {
            Desktop.ProcessMouseUp(button);
        }

        // I/F
        public void NotifyMouseWheel(int ticks)
        {
        }

        // I/F
        public void NotifyKeyDown(Keys key)
        {
            FocusedControl.ProcessKeyDown(key);
        }

        // I/F
        public void NotifyKeyUp(Keys key)
        {
            FocusedControl.ProcessKeyUp(key);
        }

        // I/F
        public void NotifyCharacterEnter(char character)
        {
            FocusedControl.ProcessCharacterEnter(character);
        }

        /// <summary>
        /// Screen を初期化します。
        /// </summary>
        /// <remarks>
        /// このメソッドの呼び出しにより Initialized プロパティが true に設定されます。
        /// このメソッドは、明示的に呼び出すか、あるいは、UIManager に Screen を設定する際に、Initialized プロパティが false ならば、
        /// UIManager により呼び出されます。
        /// </remarks>
        public void Initialize()
        {
            if (Initialized) throw new InvalidOperationException("Screen is already initialized.");

            BasicEffect = new BasicEffect(GraphicsDevice);

            LoadContent();

            UpdateLayout();

            Initialized = true;
        }

        /// <summary>
        /// 更新します。
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            // Animation を更新します。
            foreach (var animation in Animations)
            {
                if (animation.Enabled) animation.Update(gameTime);
            }

            // Screen のレイアウトを更新します。
            UpdateLayout();

            // Control を更新します。
            Desktop.Update(gameTime);
        }

        /// <summary>
        /// レイアウトを更新します。
        /// </summary>
        public void UpdateLayout()
        {
            // 測定を開始します。
            Desktop.Measure(new Size(Desktop.Width, Desktop.Height));

            // 配置を開始します。
            var margin = Desktop.Margin;
            Desktop.Arrange(new Rect(margin.Left, margin.Top, Desktop.Width, Desktop.Height));

            // マウス オーバ状態が変化する可能性があるので、
            // 最後に Screen が得たマウス カーソルの位置を再送します。
            Desktop.ProcessMouseMove(lastMousePosition.X, lastMousePosition.Y);
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
        /// 指定の Control がフォーカスを持つかどうかを判定します。
        /// </summary>
        /// <param name="control">フォーカスを持つかどうかを判定したい Control。</param>
        /// <returns>
        /// true (指定の Control がフォーカスを持つ場合)、false (それ以外の場合)。
        /// </returns>
        internal bool HasFocus(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            EnsureControlState(control);

            return FocusedControl == control;
        }

        /// <summary>
        /// 指定の Control にフォーカスを与えます。
        /// </summary>
        /// <param name="control">フォーカスを与えたい Control。</param>
        internal void Focus(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            EnsureControlState(control);

            if (!control.Enabled || !control.Visible || !control.Focusable) return;

            // フォーカスを失った事を通知します。
            if (FocusedControl != null) FocusedControl.OnLostFocus();

            FocusedControl = control;

            // フォーカスを得た事を通知します。
            if (FocusedControl != null) FocusedControl.OnGotFocus();
        }

        /// <summary>
        /// 指定の Control のフォーカスを解除し、Desktop にフォーカスを移動させます。
        /// </summary>
        /// <param name="control">フォーカスを解除したい Control。</param>
        internal void Defocus(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            EnsureControlState(control);

            if (HasFocus(control)) FocusedControl = Desktop;
        }

        /// <summary>
        /// 指定の Control がこの Screen で操作できる状態であるかどうかを保証します。
        /// </summary>
        /// <param name="control"></param>
        void EnsureControlState(Control control)
        {
            if (control.Screen != this) throw new InvalidOperationException("Control is in another screen.");
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
