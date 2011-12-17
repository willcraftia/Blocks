#region Using

using System;
using System.Collections.Specialized;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Visuals;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// GUI コントロールの基底クラスです。
    /// </summary>
    public class Control
    {
        /// <summary>
        /// Enabled プロパティが変更された時に発生します。
        /// </summary>
        public event EventHandler EnabledChanged;

        /// <summary>
        /// Visible プロパティが変更された時に発生します。
        /// </summary>
        public event EventHandler VisibleChanged;

        /// <summary>
        /// 背景色。
        /// </summary>
        public Color BackgroundColor = Color.White;

        /// <summary>
        /// Control の描画時の幅。
        /// </summary>
        float actualWidth = float.NaN;

        /// <summary>
        /// Control の描画時の高さ。
        /// </summary>
        float actualHeight = float.NaN;

        /// <summary>
        /// 親 Control。
        /// </summary>
        Control parent;

        /// <summary>
        /// 属する Screen。
        /// </summary>
        Screen screen;

        /// <summary>
        /// 自身あるいは子についてのマウス オーバ状態の Control。
        /// </summary>
        Control mouseOverControl;

        /// <summary>
        /// true (Control が有効な場合)、false (それ以外の場合)。
        /// </summary>
        bool enabled;

        /// <summary>
        /// true (Control が可視の場合)、false (それ以外の場合)。
        /// </summary>
        bool visible;

        /// <summary>
        /// Control の外側の余白を取得または設定します。
        /// </summary>
        public Thickness Margin { get; set; }

        /// <summary>
        /// Control の幅の下限を取得または設定します。
        /// </summary>
        public float MinWidth { get; set; }

        /// <summary>
        /// Control の高さの下限を取得または設定します。
        /// </summary>
        public float MinHeight { get; set; }
        
        /// <summary>
        /// Control の幅の上限を取得または設定します。
        /// </summary>
        public float MaxWidth { get; set; }
        
        /// <summary>
        /// Control の高さの上限を取得または設定します。
        /// </summary>
        public float MaxHeight { get; set; }

        /// <summary>
        /// Control の幅を取得または設定します。
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// Controln の高さを取得または設定します。
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// Control の描画時の幅を取得します。
        /// </summary>
        public float ActualWidth
        {
            get { return actualWidth; }
            protected set
            {
                actualWidth = value;
                if (actualWidth < MinWidth) actualWidth = MinWidth;
                if (MaxWidth < actualWidth) actualWidth = MaxWidth;
            }
        }

        /// <summary>
        /// Control の描画時の高さを取得します。
        /// </summary>
        public float ActualHeight
        {
            get { return actualHeight; }
            protected set
            {
                actualHeight = value;
                if (actualHeight < MinHeight) actualHeight = MinHeight;
                if (MaxHeight < actualHeight) actualHeight = MaxHeight;
            }
        }

        /// <summary>
        /// 親 Control を取得または設定します。
        /// </summary>
        public Control Parent
        {
            get { return parent; }
            internal set
            {
                if (parent == value) return;

                parent = value;

                if (parent != null)
                {
                    // 親と同じ Screen を設定します。
                    if (parent.Screen != null) Screen = parent.Screen;
                }
                else
                {
                    // 親を失ったので Screen をリセットします。
                    Screen = null;
                }
            }
        }

        /// <summary>
        /// Screen を取得します。
        /// </summary>
        public Screen Screen
        {
            get { return screen; }
            internal set
            {
                if (screen == value) return;

                // 古い Screen でフォーカスを得ていたならば解除します。
                if (screen != null) Defocus();

                screen = value;

                // 子にも同じ Screen を設定します。
                foreach (var child in Children) child.Screen = screen;
            }
        }

        /// <summary>
        /// 子 Control のコレクションを取得します。
        /// </summary>
        public ControlCollection Children { get; private set; }

        /// <summary>
        /// Control が有効かどうかを取得または設定します。
        /// </summary>
        /// <value>true (Control が有効な場合)、false (それ以外の場合)。</value>
        public bool Enabled
        {
            get { return enabled; }
            set
            {
                if (enabled == value) return;

                enabled = value;

                // フォーカスを解除します。
                Defocus();

                // イベントを発生させます。
                RaiseEnabledChanged();
            }
        }

        /// <summary>
        /// Control が表示されるかどうかを取得または設定します。
        /// </summary>
        /// <value>true (Control が表示される場合)、false (それ以外の場合)。</value>
        public bool Visible
        {
            get { return visible; }
            set
            {
                if (visible == value) return;

                visible = value;

                // フォーカスを解除します。
                Defocus();

                // イベントを発生させます。
                OnVisibleChanged();
            }
        }

        /// <summary>
        /// Control がフォーカスを得られるかどうかを取得または設定します。
        /// </summary>
        /// <value>true (Control がフォーカスを得られる場合)、false (それ以外の場合)。</value>
        public bool Focusable { get; set; }

        /// <summary>
        /// Control がフォーカスを得ているかどうかを取得します。
        /// </summary>
        /// <value>true (Control がフォーカスを得ている場合)、false (それ以外の場合)。</value>
        public bool Focused
        {
            get { return Screen != null && Screen.HasFocus(this); }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Control()
        {
            Children = new ControlCollection(this);
            Children.CollectionChanged += new NotifyCollectionChangedEventHandler(OnChildrenCollectionChanged);

            Width = float.NaN;
            Height = float.NaN;
            MaxWidth = float.PositiveInfinity;
            MaxHeight = float.PositiveInfinity;

            Visible = true;
            Focusable = true;
        }

        public virtual void Arrange()
        {
            // 親から描画時サイズが設定されていないならば、まだ処理を行いません。
            if (ActualWidth == float.NaN || ActualHeight == float.NaN) return;

            foreach (var child in Children)
            {
                var childMargin = child.Margin;

                var childMarginWidth = childMargin.Left + childMargin.Right;
                if (child.Width == float.NaN)
                {
                    // 子の幅が未設定ならば自分の幅に収まる最大サイズで調整を試みます。
                    child.ActualWidth = ActualWidth - childMarginWidth;
                }
                else
                {
                    if (ActualWidth < child.Width + childMarginWidth)
                    {
                        // 子に幅が設定されていて自分の幅を越えるようならば、自分の幅に収まる最大サイズで調整を試みます。
                        child.ActualWidth = ActualWidth - childMarginWidth;
                    }
                    else
                    {
                        // それ以外は子に設定された幅をそのまま設定するように試みます。
                        child.ActualWidth = child.Width;
                    }
                }

                var childMarginHeight = childMargin.Top + childMargin.Bottom;
                if (child.Height == float.NaN)
                {
                    // 子の高さが未設定ならば自分の高さに収まる最大サイズで調整を試みます。
                    child.ActualHeight = ActualHeight - childMarginHeight;
                }
                else
                {
                    if (ActualHeight < child.Height + childMarginHeight)
                    {
                        // 子に高さが設定されていて自分の幅を越えるようならば、自分の高さに収まる最大サイズで調整を試みます。
                        child.ActualHeight = ActualHeight - childMarginHeight;
                    }
                    else
                    {
                        // それ以外は子に設定された高さをそのまま設定するように試みます。
                        child.ActualHeight = child.Height;
                    }
                }

                child.Arrange();
            }
        }

        /// <summary>
        /// スクリーン上の絶対座標による矩形サイズを取得します。
        /// </summary>
        /// <returns>スクリーン上の絶対座標。</returns>
        public Rectangle GetAbsoluteBounds()
        {
            if (parent == null) return new Rectangle(0, 0, (int) ActualWidth, (int) ActualHeight);

            var parentAbsoluteBounds = parent.GetAbsoluteBounds();

            var absoluteBounds = new Rectangle((int) Margin.Left, (int) Margin.Top, (int) ActualWidth, (int) ActualHeight);
            absoluteBounds.X += parentAbsoluteBounds.X;
            absoluteBounds.Y += parentAbsoluteBounds.Y;

            return absoluteBounds;
        }

        /// <summary>
        /// フォーカスを得ます。
        /// </summary>
        public void Focus()
        {
            if (Screen == null || !Enabled || !Visible || !Focusable) return;

            Screen.Focus(this);
        }

        /// <summary>
        /// フォーカスを解除します。
        /// </summary>
        public void Defocus()
        {
            if (!Focused) return;

            Screen.Defocus(this);
        }

        /// <summary>
        /// Control を更新します。
        /// </summary>
        public virtual void Update() { }

        /// <summary>
        /// Control を描画します。
        /// </summary>
        /// <remarks>
        /// Visible が false の場合、Draw メソッドは呼び出されません。
        /// </remarks>
        public virtual void Draw()
        {
            // ここでは IControlLaf のみで描画します。
            // 異なる方法で Control を描画したい場合には、そのための Control のサブクラスと IControlLaf の実装を作るか、
            // あるいは Draw メソッドをオーバライドして描画ロジックを記述します。

            if (Screen == null || !Visible) return;

            // IControlLaf を取得します。
            var laf = Screen.UIContext.GetControlLaf(this);
            // IControlLaf に描画を委譲します。
            if (laf != null) laf.Draw(this);
        }

        /// <summary>
        /// マウス カーソル移動を処理します。
        /// </summary>
        /// <param name="x">親 Control の矩形位置を基準としたカーソルの X 座標。</param>
        /// <param name="y">親 Control の矩形位置を基準としたカーソルの Y 座標。</param>
        internal void ProcessMouseMoved(float x, float y)
        {
            // 不可視の場合は処理しません。
            if (!Visible) return;

            // x と y は親を基準としたカーソルの相対座標です。

            // 自分を基準としたカーソルの相対座標を算出します。
            float localX = x - Margin.Left;
            float localY = y - Margin.Top;

            for (int i = Children.Count - 1; 0 <= i; i--)
            {
                var child = Children[i];

                // 不可視ならばスキップします。
                if (!child.Visible) continue;

                // 描画領域の外ならばスキップします。
                if (localX < child.Margin.Left || child.Margin.Left + child.ActualWidth < localX) continue;
                if (localY < child.Margin.Top || child.Margin.Top + child.ActualHeight < localY) continue;

                // 子をマウス オーバ状態にします。
                SwitchMouseOverControl(child);
                // 子にカーソル移動処理を転送します。
                child.ProcessMouseMoved(localX, localY);
                return;
            }

            // マウス オーバ状態にできる子がいないならば、自分をマウス オーバ状態にします。
            SwitchMouseOverControl(this);
            OnMouseMoved(localX, localY);
        }

        /// <summary>
        /// マウス カーソルが Control の矩形領域の外に移動したことを処理します。
        /// </summary>
        internal void ProcessMouseLeft()
        {
            // 不可視の場合は処理しません。
            if (!Visible) return;

            // マウス オーバ状態の Control がなければ処理しません。
            if (mouseOverControl == null) return;

            if (mouseOverControl != this)
            {
                // マウス オーバ状態の子へ処理を転送します。
                mouseOverControl.ProcessMouseLeft();
            }
            else
            {
                // 自分がマウス オーバ状態なのでイベント ハンドラを呼びます。
                OnMouseLeft();
            }

            // マウス オーバ状態を解除します。
            mouseOverControl = null;
        }

        /// <summary>
        /// マウス ボタン押下を処理します。
        /// </summary>
        /// <param name="button">押下されたマウス ボタン。</param>
        /// <returns></returns>
        internal bool ProcessMouseButtonPressed(MouseButtons button)
        {
            // 不可視の場合は処理しません。
            if (!Visible) return false;

            // マウス オーバ状態の Control がなければ処理しません。
            if (mouseOverControl == null) return false;

            // 子がマウス オーバ状態ならば処理を転送します。
            if (mouseOverControl != this) return mouseOverControl.ProcessMouseButtonPressed(button);

            // フォーカスを得ます。
            Focus();

            // マウス ボタンが押されたことを通知します。
            OnMouseButtonPressed(button);
            return true;
        }

        /// <summary>
        /// マウス ボタン押下の解除を処理します。
        /// </summary>
        /// <param name="button"></param>
        internal void ProcessMouseButtonReleased(MouseButtons button)
        {
            // 不可視の場合は処理しません。
            if (!Visible) return;

            // マウス オーバ状態の Control がなければ処理しません。
            if (mouseOverControl == null) return;

            // 子がマウス オーバ状態ならば処理を転送します。
            if (mouseOverControl != this)
            {
                mouseOverControl.ProcessMouseButtonReleased(button);
            }
            else
            {
                // マウス ボタンが離されたことを通知します。
                OnMouseButtonReleased(button);
            }

            // マウス オーバ状態を解除します。
            mouseOverControl = null;
        }

        /// <summary>
        /// マウス カーソルが移動した時に呼び出されます。
        /// </summary>
        /// <param name="x">この Control の矩形位置を基準としたカーソルの X 座標。</param>
        /// <param name="y">この Control の矩形位置を基準としたカーソルの Y 座標。</param>
        protected virtual void OnMouseMoved(float x, float y) { }

        /// <summary>
        /// マウス カーソルがこの Control に入った時 (この Control がマウス オーバ状態になった時) に呼び出されます。
        /// </summary>
        protected virtual void OnMouseEntered() { }

        /// <summary>
        /// マウス カーソルがこの Control から出た時 (この Control のマウス オーバ状態が解除された時) に呼び出されます。
        /// </summary>
        protected virtual void OnMouseLeft() { }

        /// <summary>
        /// マウス ボタンがこの Control で押された時に呼び出されます。
        /// </summary>
        /// <param name="button"></param>
        protected virtual void OnMouseButtonPressed(MouseButtons button) { }

        /// <summary>
        /// マウス ボタンがこの Control で離された時に呼び出されます。
        /// </summary>
        /// <param name="button"></param>
        protected virtual void OnMouseButtonReleased(MouseButtons button) { }

        /// <summary>
        /// Enabled プロパティが変更された時に呼び出されます。
        /// </summary>
        protected virtual void OnEnabledChanged() { }

        /// <summary>
        /// Visible プロパティが変更された時に呼び出されます。
        /// </summary>
        protected virtual void OnVisibleChanged() { }

        /// <summary>
        /// マウス オーバ状態の Control を新しい Control へ切り替えます。
        /// </summary>
        /// <param name="newControl"></param>
        void SwitchMouseOverControl(Control newControl)
        {
            if (mouseOverControl == newControl) return;

            // これまでマウス オーバ状態にあった Control に変更を通知します。
            if (mouseOverControl != null) mouseOverControl.ProcessMouseLeft();

            // 新たにマウス オーバ状態となった Control を設定し、変更を通知します。
            mouseOverControl = newControl;
            mouseOverControl.OnMouseEntered();
        }

        /// <summary>
        /// 子がマウス オーバ状態ならばそれを解除し、自身をマウス オーバ状態にします。
        /// マウス オーバ状態の Control が存在しなければ何もしません。
        /// </summary>
        void ResetChildMouseOverControl()
        {
            if (mouseOverControl == null) return;

            // 子がマウス オーバ状態ならばリセットします。
            if (mouseOverControl != this)
            {
                // これまでマウス オーバ状態にあった Control に変更を通知します。
                mouseOverControl.ProcessMouseLeft();
                // 自分をマウス オーバ状態にします。
                mouseOverControl = this;
            }
        }

        /// <summary>
        /// Children が変更された場合に呼び出されます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnChildrenCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            ResetChildMouseOverControl();
            Arrange();
        }

        /// <summary>
        /// EnabledChanged イベントを発生させます。
        /// </summary>
        void RaiseEnabledChanged()
        {
            OnEnabledChanged();
            if (EnabledChanged != null) EnabledChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// VisibleChanged イベントを発生させます。
        /// </summary>
        void RaiseVisibleChanged()
        {
            OnVisibleChanged();
            if (VisibleChanged != null) VisibleChanged(this, EventArgs.Empty);
        }
    }
}
