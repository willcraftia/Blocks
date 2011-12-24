#region Using

using System;
using System.Collections.Specialized;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Lafs;

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
        /// RenderBounds プロパティが変更された時に発生します。
        /// </summary>
        public event EventHandler RenderBoundsChanged;

        /// <summary>
        /// マウス カーソルが Control に入った時に発生します。
        /// </summary>
        public event EventHandler MouseEntered;

        /// <summary>
        /// マウス カーソルが Control から出た時に発生します。
        /// </summary>
        public event EventHandler MouseLeft;

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
        /// 自身あるいは子のうちのアクティブな Control。
        /// </summary>
        Control activatedControl;

        /// <summary>
        /// 自身あるいは子についてのマウス オーバ状態の Control。
        /// </summary>
        Control mouseOverControl;

        /// <summary>
        /// true (Control が有効な場合)、false (それ以外の場合)。
        /// </summary>
        bool enabled = true;

        /// <summary>
        /// true (Control が可視の場合)、false (それ以外の場合)。
        /// </summary>
        bool visible = true;

        /// <summary>
        /// true (自身と子 Control の描画サイズに有効な値が設定されている場合)、false (それ以外の場合)。
        /// </summary>
        bool arranged;

        /// <summary>
        /// true (Control がアクティブになった時に最前面へ移動する場合)、false (それ以外の場合)。
        /// </summary>
        bool affectsOrdering;

        /// <summary>
        /// 絶対座標としての描画領域。
        /// </summary>
        Rectangle renderBounds;

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
            protected internal set { actualWidth = MathHelper.Clamp(value, MinWidth, MaxWidth); }
        }

        /// <summary>
        /// Control の描画時の高さを取得します。
        /// </summary>
        public float ActualHeight
        {
            get { return actualHeight; }
            protected internal set { actualHeight = MathHelper.Clamp(value, MinHeight, MaxHeight); }
        }

        /// <summary>
        /// 親の描画領域でクリップするかどうかを示す値を取得または設定します。
        /// </summary>
        /// <value>
        /// true (親の描画領域でクリップする場合)、false (それ以外の場合)。
        /// </value>
        public bool Clipped { get; set; }

        /// <summary>
        /// 背景色。
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// 前景色
        /// </summary>
        public Color ForegroundColor { get; set; }

        /// <summary>
        /// 親 Control で配置される際に適用される、水平方向の配置方法を取得または設定します。
        /// </summary>
        public HorizontalAlignment HorizontalAlignment { get; set; }

        /// <summary>
        /// 親 Control で配置される際に適用される、垂直方向の配置方法を取得または設定します。
        /// </summary>
        public VerticalAlignment VerticalAlignment { get; set; }

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
        /// Animation コレクションを取得します。
        /// </summary>
        public AnimationCollection Animations { get; private set; }

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
                RaiseVisibleChanged();
            }
        }

        /// <summary>
        /// 自身と子 Control の描画サイズに有効な値が設定されているかどうかを示す値を取得または設定します。
        /// </summary>
        /// <value>
        /// true (自身と子 Control の描画サイズに有効な値が設定されている場合)、false (それ以外の場合)。
        /// </value>
        public bool Arranged
        {
            get { return arranged; }
            protected set
            {
                if (arranged == value) return;
                arranged = value;
            }
        }

        /// <summary>
        /// 絶対座標としての描画領域を取得します。
        /// </summary>
        /// <remarks>
        /// このプロパティは Arrange の呼び出しで算出されます。
        /// </remarks>
        public Rectangle RenderBounds
        {
            get { return renderBounds; }
            protected set
            {
                if (renderBounds == value) return;

                renderBounds = value;

                // イベントを発生させます。
                RaiseRenderBoundsChanged();
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

        protected internal float ClampedWidth
        {
            get { return MathHelper.Clamp(Width, MinWidth, MaxWidth); }
        }

        protected internal float ClampedHeight
        {
            get { return MathHelper.Clamp(Height, MinHeight, MaxHeight); }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Control() : this(false) { }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="affectsOrdering">
        /// true (Control がアクティブになった時に最前面へ移動する場合)、false (それ以外の場合)。
        /// </param>
        public Control(bool affectsOrdering)
        {
            this.affectsOrdering = affectsOrdering;

            Children = new ControlCollection(this);
            Animations = new AnimationCollection(this);

            Width = float.NaN;
            Height = float.NaN;
            MaxWidth = float.PositiveInfinity;
            MaxHeight = float.PositiveInfinity;
            Clipped = true;
            ForegroundColor = Color.White;
            BackgroundColor = Color.Black;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;

            Focusable = true;
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
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            if (!Enabled) throw new InvalidOperationException("This control is disabled.");

            // 必要ならば Control の描画サイズを計算します。
            if (!Arranged) Arrange();

            // Animation を更新します。
            foreach (var animation in Animations)
            {
                if (animation.Enabled) animation.Update(gameTime);
            }

            // 再帰的に子 Control を更新します。
            foreach (var child in Children)
            {
                if (child.Enabled) child.Update(gameTime);
            }
        }

        /// <summary>
        /// Control を描画します。
        /// </summary>
        /// <remarks>
        /// Visible が false の場合、Draw メソッドは呼び出されません。
        /// </remarks>
        /// <param name="gameTime"></param>
        public virtual void Draw(GameTime gameTime) { }

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

            if (activatedControl != null)
            {
                if (activatedControl != this)
                {
                    // アクティブな子 Control へ通知します。
                    activatedControl.ProcessMouseMoved(localX, localY);
                }
                else
                {
                    OnMouseMoved(localX, localY);
                }
            }

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

                // アクティブな子 Control は先の処理で通知を受けているので、その他ならば通知します。
                if (mouseOverControl != activatedControl)
                {
                    child.ProcessMouseMoved(localX, localY);
                }
                return;
            }

            // マウス オーバ状態にできる子がなく、
            if (0 <= localX && localX <= ActualWidth && 0 <= localY && localY <= ActualHeight)
            {
                // 自分の領域内ならば、自分をマウス オーバ状態にします。
                SwitchMouseOverControl(this);
                // 自身がアクティブの場合は先の処理で通知を受けているので、非アクティブならば通知します。
                if (activatedControl == null) OnMouseMoved(localX, localY);
            }
            else
            {
                // 自分の領域外ならばマウスが外にでたことを通知します。
                ProcessMouseLeft();
            }
        }

        /// <summary>
        /// マウス カーソルが Control の領域外に移動したことを処理します。
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
                RaiseMouseLeft();
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

            if (activatedControl == null)
            {
                // マウス オーバ状態の Control をアクティブにします。
                activatedControl = mouseOverControl;
                // アクティブな Control がなければ処理を終えます。
                if (activatedControl == null) return false;

                // 子がアクティブならば、子リストの中で最前面への移動を試みます。
                if (activatedControl != this && activatedControl.affectsOrdering) Children.MoveToTopMost(activatedControl);
            }

            // 子がアクティブならばマウス ボタンが押されたことを通知します。
            if (activatedControl != this) return activatedControl.ProcessMouseButtonPressed(button);

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
            //if (!Visible) return;

            // アクティブな Control がなければ処理しません。
            if (activatedControl == null) return;

            // 子がアクティブならばマウス ボタン押下の解除を通知します。
            if (activatedControl != this)
            {
                activatedControl.ProcessMouseButtonReleased(button);
            }
            else
            {
                // マウス ボタン押下の解除を通知します。
                OnMouseButtonReleased(button);
            }

            activatedControl = null;
        }

        /// <summary>
        /// Children が変更された場合に呼び出されます。
        /// </summary>
        internal void ProcessChildrenCollectionChanged()
        {
            ResetChildMouseOverControl();
            // 再配置させます。
            Arranged = false;
        }

        protected internal virtual void Arrange()
        {
            if (Arranged) return;

            if (parent == null)
            {
                RenderBounds = new Rectangle(0, 0, (int) ActualWidth, (int) ActualHeight);
            }
            else
            {
                var parentBounds = parent.RenderBounds;
                var bounds = new Rectangle((int) Margin.Left, (int) Margin.Top, (int) ActualWidth, (int) ActualHeight);
                bounds.X += parentBounds.X;
                bounds.Y += parentBounds.Y;
                RenderBounds = bounds;
            }

            ArrangeChildren();

            Arranged = true;
        }

        protected virtual void ArrangeChildren()
        {
            foreach (var child in Children)
            {
                var childMargin = child.Margin;

                var childMarginWidth = childMargin.Left + childMargin.Right;
                if (float.IsNaN(child.Width))
                {
                    // 子の幅が未設定ならば自分の幅に収まる最大サイズで調整を試みます。
                    child.ActualWidth = ActualWidth - childMarginWidth;
                }
                else
                {
                    var childWidth = child.ClampedWidth;
                    if (ActualWidth < childWidth + childMarginWidth)
                    {
                        // 子に幅が設定されていて自分の幅を越えるようならば、自分の幅に収まる最大サイズで調整を試みます。
                        child.ActualWidth = ActualWidth - childMarginWidth;
                    }
                    else
                    {
                        // それ以外は子に設定された幅をそのまま設定するように試みます。
                        child.ActualWidth = childWidth;
                    }
                }

                var childMarginHeight = childMargin.Top + childMargin.Bottom;
                if (float.IsNaN(child.Height))
                {
                    // 子の高さが未設定ならば自分の高さに収まる最大サイズで調整を試みます。
                    child.ActualHeight = ActualHeight - childMarginHeight;
                }
                else
                {
                    var childHeight = child.ClampedHeight;
                    if (ActualHeight < childHeight + childMarginHeight)
                    {
                        // 子に高さが設定されていて自分の幅を越えるようならば、自分の高さに収まる最大サイズで調整を試みます。
                        child.ActualHeight = ActualHeight - childMarginHeight;
                    }
                    else
                    {
                        // それ以外は子に設定された高さをそのまま設定するように試みます。
                        child.ActualHeight = childHeight;
                    }
                }

                child.Arrange();
            }
        }

        /// <summary>
        /// Enabled プロパティが変更された時に呼び出されます。
        /// </summary>
        protected virtual void OnEnabledChanged() { }

        /// <summary>
        /// Visible プロパティが変更された時に呼び出されます。
        /// </summary>
        protected virtual void OnVisibleChanged() { }

        /// <summary>
        /// RenderBounds プロパティが変更された時に呼び出されます。
        /// </summary>
        protected virtual void OnRenderBoundsChanged() { }

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
            mouseOverControl.RaiseMouseEntered();
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

        /// <summary>
        /// RenderBoundsChanged イベントを発生させます。
        /// </summary>
        void RaiseRenderBoundsChanged()
        {
            OnRenderBoundsChanged();
            if (RenderBoundsChanged != null) RenderBoundsChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// MouseEntered イベントを発生させます。
        /// </summary>
        void RaiseMouseEntered()
        {
            OnMouseEntered();
            if (MouseEntered != null) MouseEntered(this, EventArgs.Empty);
        }

        /// <summary>
        /// MouseLeft イベントを発生させます。
        /// </summary>
        void RaiseMouseLeft()
        {
            OnMouseLeft();
            if (MouseLeft != null) MouseLeft(this, EventArgs.Empty);
        }
    }
}
