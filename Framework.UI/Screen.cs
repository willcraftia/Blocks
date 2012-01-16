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
    public class Screen : IInputReceiver, IDisposable
    {
        /// <summary>
        /// フォーカスを得ている Control。
        /// </summary>
        Control focusedControl;

        /// <summary>
        /// 最後に Screen が得たマウス カーソルの位置。
        /// </summary>
        Point lastMousePosition;

        /// <summary>
        /// 測定する必要のある Control のリスト。
        /// </summary>
        List<Control> measuredControls = new List<Control>();

        /// <summary>
        /// 配置する必要のある Control のリスト。
        /// </summary>
        List<Control> arrangedControls = new List<Control>();

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
        /// UpdateLayput メソッドの呼び出しが必要かどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (UpdateLayput メソッドの呼び出しが必要な場合)、false (それ以外の場合)。
        /// </value>
        public bool UpdateLayoutNeeded
        {
            get { return measuredControls.Count != 0 || arrangedControls.Count != 0; }
        }

        // TODO
        // protected にする？
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Screen(Game game)
        {
            Game = game;
            GraphicsDevice = game.GraphicsDevice;
            Content = new ContentManager(game.Services);

            Animations = new AnimationCollection(this);
            Desktop = new Desktop()
            {
                Screen = this
            };
        }

        // I/F
        public void NotifyMouseMoved(int x, int y)
        {
            Desktop.ProcessMouseMoved(x, y);

            lastMousePosition.X = x;
            lastMousePosition.Y = y;
        }

        // I/F
        public void NotifyMouseButtonPressed(MouseButtons button)
        {
            Desktop.ProcessMouseButtonPressed(button);
        }

        // I/F
        public void NotifyMouseButtonReleased(MouseButtons button)
        {
            Desktop.ProcessMouseButtonReleased(button);
        }

        // I/F
        public void NotifyMouseWheelRotated(int ticks)
        {
        }

        // I/F
        public void NotifyKeyPressed(Keys key)
        {
            if (focusedControl != null)
            {
                if (focusedControl.ProcessKeyPressed(key)) return;
            }
        }

        // I/F
        public void NotifyKeyReleased(Keys key)
        {
            if (focusedControl != null)
            {
                focusedControl.ProcessKeyReleased(key);
            }
        }

        // I/F
        public void NotifyCharacterEntered(char character)
        {
            if (focusedControl != null)
            {
                focusedControl.ProcessCharacterEntered(character);
            }
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

            // Desktop を測定します。
            // 自分のサイズで測定を開始します。
            Desktop.Measure(new Size(Desktop.Width, Desktop.Height));
            // Desktop を配置します。
            // 自分のマージンとサイズで配置を開始します。
            var margin = Desktop.Margin;
            Desktop.Arrange(new Rect(margin.Left, margin.Top, Desktop.Width, Desktop.Height));

            Initialized = true;
        }

        /// <summary>
        /// レイアウトを更新します。
        /// </summary>
        public void UpdateLayout()
        {
            foreach (var control in measuredControls)
            {
                // 親も再計測が必要ならば、親の再計測の中で再計測するものとし、スキップします。
                if (control.Parent != null && !control.Parent.Measured) continue;

                // 再計測します。
                control.Remeasure();
            }
            // リストをリセットします。
            measuredControls.Clear();

            foreach (var control in arrangedControls)
            {
                // 親も再配置が必要ならば、親の再配置の中で再配置するものとし、スキップします。
                if (control.Parent != null && !control.Parent.Arranged) continue;

                // 再配置します。
                // 配置で親が指定する矩形領域は、計測結果を元に算出されます。
                // したがって、親に対して再配置を要求します (親は配置済み)。
                if (control.Parent == null)
                {
                    // ルートは例外です。
                    control.Rearrange();
                }
                else
                {
                    control.Parent.Rearrange();
                }
            }
            // リストをリセットします。
            arrangedControls.Clear();

            // マウス オーバ状態が変化する可能性があるので、
            // 最後に Screen が得たマウス カーソルの位置を再送します。
            Desktop.ProcessMouseMoved(lastMousePosition.X, lastMousePosition.Y);
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
        /// 計測実行リストへ Control を追加します。
        /// </summary>
        /// <param name="control">Control。</param>
        internal void AddMeasuredControl(Control control)
        {
            if (control.Measured) throw new InvalidOperationException("Control is measured.");

            if (!measuredControls.Contains(control)) measuredControls.Add(control);
        }

        /// <summary>
        /// 配置実行リストへ Control を追加します。
        /// </summary>
        /// <param name="control">Control。</param>
        internal void AddArrangedControl(Control control)
        {
            if (control.Arranged) throw new InvalidOperationException("Control is arranged.");

            if (!arrangedControls.Contains(control)) arrangedControls.Add(control);
        }

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

            return focusedControl == control;
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

            focusedControl = control;
        }

        /// <summary>
        /// 指定の Control のフォーカスを解除します。
        /// </summary>
        /// <param name="control">フォーカスを解除したい Control。</param>
        internal void Defocus(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");
            EnsureControlState(control);

            if (HasFocus(control)) focusedControl = null;
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
