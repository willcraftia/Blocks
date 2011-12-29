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
        /// マウス カーソルが Control に入った時に発生します。
        /// </summary>
        public event EventHandler MouseEntered;

        /// <summary>
        /// マウス カーソルが Control から出た時に発生します。
        /// </summary>
        public event EventHandler MouseLeft;

        /// <summary>
        /// 外側の余白。
        /// </summary>
        Thickness margin;

        /// <summary>
        /// 幅の下限。
        /// </summary>
        float minWidth = 0;

        /// <summary>
        /// 高さの下限。
        /// </summary>
        float minHeight = 0;

        /// <summary>
        /// 幅の上限。
        /// </summary>
        float maxWidth = float.PositiveInfinity;

        /// <summary>
        /// 高さの上限。
        /// </summary>
        float maxHeight = float.PositiveInfinity;

        /// <summary>
        /// 幅。
        /// </summary>
        float width = float.NaN;

        /// <summary>
        /// 高さ。
        /// </summary>
        float height = float.NaN;

        /// <summary>
        /// 希望するサイズ。
        /// </summary>
        Size measuredSize = Size.Empty;

        /// <summary>
        /// 配置後の領域。
        /// </summary>
        Rect arrangedBounds = Rect.Empty;

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
        /// true (測定済みである場合)、false (それ以外の場合)。
        /// </summary>
        bool measured;

        /// <summary>
        /// true (配置済みである場合)、false (それ以外の場合)。
        /// </summary>
        bool arranged;

        /// <summary>
        /// true (アクティブになった時に最前面へ移動する場合)、false (それ以外の場合)。
        /// </summary>
        bool affectsOrdering;

        /// <summary>
        /// 外側の余白を取得または設定します。
        /// </summary>
        public Thickness Margin
        {
            get { return margin; }
            set
            {
                if (margin == value) return;
                margin = value;
            }
        }

        /// <summary>
        /// 幅の下限を取得または設定します。
        /// </summary>
        public float MinWidth
        {
            get { return minWidth; }
            set
            {
                if (minWidth == value) return;
                minWidth = value;
            }
        }

        /// <summary>
        /// 高さの下限を取得または設定します。
        /// </summary>
        public float MinHeight
        {
            get { return minHeight; }
            set
            {
                if (minHeight == value) return;
                minHeight = value;
            }
        }
        
        /// <summary>
        /// 幅の上限を取得または設定します。
        /// </summary>
        public float MaxWidth
        {
            get { return maxWidth; }
            set
            {
                if (maxWidth == value) return;
                maxWidth = value;
            }
        }
        
        /// <summary>
        /// 高さの上限を取得または設定します。
        /// </summary>
        public float MaxHeight
        {
            get { return maxHeight; }
            set
            {
                if (maxHeight == value) return;
                maxHeight = value;
            }
        }

        /// <summary>
        /// 幅を取得または設定します。
        /// </summary>
        public float Width
        {
            get { return width; }
            set
            {
                if (width == value) return;
                width = value;
                // 再測定させます。
                Measured = false;
            }
        }

        /// <summary>
        /// 高さを取得または設定します。
        /// </summary>
        public float Height
        {
            get { return height; }
            set
            {
                if (height == value) return;
                height = value;
                // 再測定させます。
                Measured = false;
            }
        }

        /// <summary>
        /// 測定後のサイズを取得します。
        /// </summary>
        public Size MeasuredSize
        {
            get { return measuredSize; }
        }

        /// <summary>
        /// 配置後の領域を取得します。
        /// </summary>
        public Rect ArrangedBounds
        {
            get { return arrangedBounds; }
        }

        /// <summary>
        /// 配置後の幅を取得します。
        /// </summary>
        public float ActualWidth
        {
            get { return arrangedBounds.Width; }
        }

        /// <summary>
        /// 配置後の高さを取得します。
        /// </summary>
        public float ActualHeight
        {
            get { return arrangedBounds.Height; }
        }

        /// <summary>
        /// 測定済みかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (測定済みである場合)、false (それ以外の場合)。
        /// </value>
        public bool Measured
        {
            get { return measured; }
            protected set
            {
                if (measured == value) return;
                measured = value;
            }
        }

        /// <summary>
        /// 配置済みであるかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (配置済みである場合)、false (それ以外の場合)。
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

            Clipped = true;
            ForegroundColor = Color.White;
            BackgroundColor = Color.Black;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;

            Focusable = true;
        }

        /// <summary>
        /// 測定します。
        /// </summary>
        /// <param name="availableSize">親 Control が指定する利用可能なサイズ。</param>
        public void Measure(Size availableSize)
        {
            measuredSize = MeasureOverride(availableSize);

            // 測定済みにします。
            Measured = true;
            // 未配置にします。
            Arranged = false;
        }

        /// <summary>
        /// 配置します。
        /// </summary>
        /// <param name="finalBounds">親 Control が指定する配置に使用可能な領域。</param>
        public void Arrange(Rect finalBounds)
        {
            var size = ArrangeOverride(finalBounds.Size);

            // 配置結果の領域を設定します。
            arrangedBounds = new Rect(finalBounds.TopLeft, size);

            // 配置済にします。
            Arranged = true;
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

            // 未測定の子があるならば測定します。
            foreach (var child in Children)
            {
                if (!child.Measured) Measure(MeasuredSize);
            }

            // 未配置の子があるならば配置します。
            foreach (var child in Children)
            {
                if (!child.Arranged) Arrange(ArrangedBounds);
            }

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
        /// <param name="renderBounds"></param>
        public virtual void Draw(GameTime gameTime, Rectangle renderBounds) { }

        /// <summary>
        /// マウス カーソル移動を処理します。
        /// </summary>
        /// <param name="x">親を基準としたカーソルの X 座標。</param>
        /// <param name="y">親を基準としたカーソルの Y 座標。</param>
        internal void ProcessMouseMoved(float x, float y)
        {
            // 不可視の場合は処理しません。
            if (!Visible) return;

            // x と y は親を基準としたカーソルの相対座標です。

            // 自分を基準としたカーソルの相対座標を算出します。
            float localX = x - arrangedBounds.X;
            float localY = y - arrangedBounds.Y;

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

                // 領域の外ならばスキップします。
                if (!child.ArrangedBounds.Contains(localX, localY)) continue;

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
            if (arrangedBounds.Contains(x, y))
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
            // 再測定と再配置を行います。
            Measured = false;
            Arranged = false;
        }

        /// <summary>
        /// 測定します。
        /// </summary>
        /// <param name="availableSize">親 Control が指定する利用可能なサイズ。</param>
        /// <returns>測定により自身が希望するサイズ。</returns>
        protected virtual Size MeasureOverride(Size availableSize)
        {
            var size = new Size();

            if (float.IsNaN(Width))
            {
                // 幅が未設定ならば可能な限り最大の幅を希望します。
                if (MinWidth < MaxWidth)
                {
                    size.Width = MathHelper.Clamp(availableSize.Width, MinWidth, MaxWidth);
                }
                else
                {
                    // 上限と下限の関係に不整合があるならば最大の下限を希望します。
                    size.Width = MathHelper.Max(availableSize.Width, MinWidth);
                }
                // 余白で調整します。
                size.Width = MathHelper.Max(MinWidth, size.Width - margin.Left - margin.Right);
            }
            else
            {
                // 幅が設定されているならばそのまま希望します。
                size.Width = Width;
            }

            if (float.IsNaN(Height))
            {
                // 高さが未設定ならば可能な限り最大の幅を希望します。
                if (MinHeight < MaxHeight)
                {
                    size.Height = MathHelper.Clamp(availableSize.Height, MinHeight, MaxHeight);
                }
                else
                {
                    // 上限と下限の関係に不整合があるならば最大の下限を希望します。
                    size.Height = MathHelper.Max(availableSize.Height, MinHeight);
                }
                // 余白で調整します。
                size.Height = MathHelper.Max(MinHeight, size.Height - margin.Top - margin.Bottom);
            }
            else
            {
                // 高さが設定されているならばそのまま希望します。
                size.Height = Height;
            }

            // 自分が希望するサイズで子の希望サイズを定めます。
            foreach (var child in Children)
            {
                // 自身の希望サイズを測定したので、子が測定済かどうかによらず再測定します。
                child.Measure(size);
            }

            return size;
        }

        /// <summary>
        /// 配置します。
        /// </summary>
        /// <param name="finalSize">親 Control が指定する配置に利用可能な領域。</param>
        /// <returns>配置により自身が希望する最終的なサイズ。</returns>
        protected virtual Size ArrangeOverride(Size finalSize)
        {
            foreach (var child in Children)
            {
                var childMargin = child.Margin;
                var childMeasuredSize = child.MeasuredSize;
                var bounds = new Rect(childMargin.Left, childMargin.Top, childMeasuredSize.Width, childMeasuredSize.Height);
                child.Arrange(bounds);
            }

            return finalSize;
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
