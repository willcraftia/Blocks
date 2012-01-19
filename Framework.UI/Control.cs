﻿#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Input;

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
        public event EventHandler MouseEnter;

        /// <summary>
        /// マウス カーソルが Control から出た時に発生します。
        /// </summary>
        public event EventHandler MouseLeave;

        /// <summary>
        /// 名前。
        /// </summary>
        string name;

        /// <summary>
        /// 属する Screen。
        /// </summary>
        Screen screen;

        /// <summary>
        /// 親 Control。
        /// </summary>
        Control parent;

        /// <summary>
        /// 外側の余白。
        /// </summary>
        Thickness margin;

        /// <summary>
        /// 内側の余白。
        /// </summary>
        Thickness padding;

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
        /// 自身あるいは子についてのマウス オーバ状態の Control。
        /// 自身がマウス オーバかつ子がマウス オーバの場合、子を設定します。
        /// 自身がマウス オーバかつ子が非マウス オーバの場合、自身を設定します。
        /// 自身が非マウス オーバの場合、null を設定します。
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
        /// true (アクティブになった時に最前面へ移動する場合)、false (それ以外の場合)。
        /// </summary>
        bool affectsOrdering;

        /// <summary>
        /// フォント。
        /// </summary>
        SpriteFont font;

        /// <summary>
        /// フォントの拡大縮小の度合い。
        /// </summary>
        Vector2 fontStretch = Vector2.One;

        /// <summary>
        /// 名前を取得または設定します。
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                if (name == value) return;

                // コレクション内のキーを更新します。
                if (Parent != null && Parent.Children.Contains(this)) Parent.Children.ChangeKey(this, value);
                // 名前を設定します。
                name = value;
            }
        }

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
        /// 内側の余白を取得または設定します。
        /// </summary>
        public Thickness Padding
        {
            get { return padding; }
            set
            {
                if (padding == value) return;
                padding = value;
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
        /// 親の描画領域でクリップするかどうかを示す値を取得または設定します。
        /// </summary>
        /// <value>
        /// true (親の描画領域でクリップする場合)、false (それ以外の場合)。
        /// </value>
        public bool Clipped { get; set; }

        /// <summary>
        /// SpriteFont を取得または設定します。
        /// </summary>
        public SpriteFont Font
        {
            get { return font; }
            set
            {
                if (font == value) return;
                font = value;
            }
        }

        /// <summary>
        /// フォントの拡大縮小の度合いを取得または設定します。
        /// </summary>
        public Vector2 FontStretch
        {
            get { return fontStretch; }
            set
            {
                if (fontStretch == value) return;
                fontStretch = value;
            }
        }

        /// <summary>
        /// 透明度を取得または設定します。
        /// </summary>
        public float Opacity { get; set; }

        /// <summary>
        /// 透明度が子 Control に継承されるかどうかを示す値を取得または設定します。
        /// </summary>
        /// <value>
        /// true (透明度が子 Control に継承される場合)、false (それ以外の場合)。
        /// </value>
        public bool OpacityInherited { get; set; }

        /// <summary>
        /// 背景色を取得または設定します。
        /// </summary>
        public Color BackgroundColor { get; set; }

        /// <summary>
        /// 前景色を取得または設定します。
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
        /// 親 Control を取得します。
        /// </summary>
        public Control Parent
        {
            get { return parent; }
            internal set
            {
                if (parent == value) return;

                // 親の mouseOverControl から切り離します。
                if (parent != null && parent.mouseOverControl == this) parent.mouseOverControl = parent;

                parent = value;

                // 親と同じ Screen に属します。
                // 親が null ならば Screen から切り離します。
                Screen = (parent != null) ? parent.Screen : null;
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

                // フォーカスを解除します。
                if (screen != null) Defocus();

                screen = value;

                // 子も同じ Screen の状態にします (Screen が null の場合も含めて)。
                foreach (var child in Children) child.Screen = screen;
            }
        }

        /// <summary>
        /// 子 Control のコレクションを取得します。
        /// </summary>
        /// <remarks>
        /// Children プロパティへの操作は同期化されないため、
        /// 非同期に操作を行う場合は、その操作側で同期化を行なってください。
        /// </remarks>
        public ParentingControlCollection Children { get; private set; }

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
        /// マウス カーソルが自身あるいは子の上にあるかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (マウス カーソルが自身あるいは子の上にある場合)、false (それ以外の場合)。
        /// </value>
        public bool MouseOver
        {
            get { return mouseOverControl != null; }
        }

        /// <summary>
        /// マウス カーソルが自身の上にあるかどうかを示す値を取得します。
        /// </summary>
        /// <remarks>
        /// MouseOver プロパティが true であっても、子の上にマウス カーソルがある場合、
        /// MouseDirectlyOver プロパティは false となります。
        /// </remarks>
        /// <value>
        /// true (マウス カーソルが自身の上にある場合)、false (それ以外の場合)。
        /// </value>
        public bool MouseDirectlyOver
        {
            get { return mouseOverControl == this; }
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

            Children = new ParentingControlCollection(this);

            Clipped = true;
            Opacity = 1;
            OpacityInherited = true;
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
        public virtual void Update(GameTime gameTime) { }

        /// <summary>
        /// Control を描画します。
        /// </summary>
        /// <remarks>
        /// Visible が false の場合、Draw メソッドは呼び出されません。
        /// </remarks>
        /// <param name="gameTime"></param>
        /// <param name="drawContext"></param>
        public virtual void Draw(GameTime gameTime, IDrawContext drawContext) { }

        /// <summary>
        /// マウス カーソル移動を処理します。
        /// </summary>
        /// <param name="x">親を基準としたカーソルの X 座標。</param>
        /// <param name="y">親を基準としたカーソルの Y 座標。</param>
        internal void ProcessMouseMove(float x, float y)
        {
            // x と y は親を基準としたカーソルの相対座標です。

            // 自分を基準としたカーソルの相対座標を算出します。
            float localX = x - arrangedBounds.X;
            float localY = y - arrangedBounds.Y;

            OnMouseMove(localX, localY);

            for (int i = Children.Count - 1; 0 <= i; i--)
            {
                var child = Children[i];

                // 不可視ならばスキップします。
                if (!child.Visible) continue;

                // 領域の外ならばスキップします。
                if (!child.ArrangedBounds.Contains(localX, localY)) continue;

                // 子に通知します。
                child.ProcessMouseMove(localX, localY);

                // 子をマウス オーバ状態にします。
                SwitchMouseOverControl(child);
                return;
            }

            // マウス オーバ状態にできる子がないならば、自分をマウス オーバ状態にします。
            SwitchMouseOverControl(this);
        }

        /// <summary>
        /// マウス カーソルが Control の領域外に移動したことを処理します。
        /// </summary>
        internal void ProcessMouseLeave()
        {
            // マウス オーバ状態の Control がなければ処理しません。
            if (mouseOverControl == null) return;

            if (mouseOverControl != this)
            {
                // マウス オーバ状態の子へ処理を転送します。
                mouseOverControl.ProcessMouseLeave();
            }
            else
            {
                // 自分がマウス オーバ状態なのでイベント ハンドラを呼びます。
                RaiseMouseLeave();
            }

            // マウス オーバ状態を解除します。
            mouseOverControl = null;
        }

        /// <summary>
        /// マウス ボタン押下を処理します。
        /// </summary>
        /// <param name="button">押下されたマウス ボタン。</param>
        /// <returns></returns>
        internal bool ProcessMouseDown(MouseButtons button)
        {
            // 非マウス オーバ状態で呼ばれたならば処理しません。
            if (mouseOverControl == null) return false;

            // 子がマウス オーバ状態ならば子へ通知します。
            if (mouseOverControl != this) return mouseOverControl.ProcessMouseDown(button);

            // フォーカスを得ます。
            Focus();

            // 自分へ通知します。
            OnMouseDown(button);
            return true;
        }

        /// <summary>
        /// マウス ボタン押下の解放を処理します。
        /// </summary>
        /// <param name="button"></param>
        internal void ProcessMouseUp(MouseButtons button)
        {
            // 非マウス オーバ状態で呼ばれたならば処理しません。
            if (mouseOverControl == null) return;

            // 子がマウス オーバ状態ならば子へ通知します。
            if (mouseOverControl != this)
            {
                mouseOverControl.ProcessMouseUp(button);
                return;
            }

            // 自分へ通知します。
            OnMouseUp(button);
        }

        /// <summary>
        /// キー押下を処理します。
        /// </summary>
        /// <param name="key">キー。</param>
        /// <returns>
        /// true (キー押下を処理した場合)、false (それ以外の場合)。
        /// </returns>
        internal bool ProcessKeyDown(Keys key)
        {
            if (OnKeyDown(key)) return true;

            switch (key)
            {
                case Keys.Up:
                    return MoveFocus(FocusNavigation.Up);
                case Keys.Down:
                    return MoveFocus(FocusNavigation.Down);
                case Keys.Left:
                    return MoveFocus(FocusNavigation.Left);
                case Keys.Right:
                    return MoveFocus(FocusNavigation.Right);
            }

            return false;
        }

        /// <summary>
        /// キー押下の解放を処理します。
        /// </summary>
        /// <param name="key">キー。</param>
        internal void ProcessKeyUp(Keys key)
        {
            OnKeyUp(key);
        }

        /// <summary>
        /// 文字の入力を処理します。
        /// </summary>
        /// <param name="character">文字。</param>
        internal void ProcessCharacterEnter(char character)
        {
            OnCharacterEnter(character);
        }

        /// <summary>
        /// 測定します。
        /// </summary>
        /// <param name="availableSize">親が指定する利用可能なサイズ。</param>
        /// <returns>自身が希望するサイズ。</returns>
        protected virtual Size MeasureOverride(Size availableSize)
        {
            var size = new Size();
            size.Width = CalculateBaseWidth(availableSize.Width);
            size.Height = CalculateBaseHeight(availableSize.Height);

            // 自分が希望するサイズで子の希望サイズを定めます。
            foreach (var child in Children)
            {
                // 自身の希望サイズを測定したので、子が測定済かどうかによらず再測定します。
                child.Measure(size);
            }

            return size;
        }

        /// <summary>
        /// 子を測定する前の自身の幅を計算します。
        /// </summary>
        /// <param name="availableWidth">親が指定する利用可能な幅。</param>
        /// <returns>幅。</returns>
        protected virtual float CalculateBaseWidth(float availableWidth)
        {
            var w = Width;

            // 幅が設定されているならばそのまま希望します。
            // 幅が未設定ならば可能な限り最大の幅を希望します。
            if (float.IsNaN(w))
            {
                if (MinWidth < MaxWidth)
                {
                    w = MathHelper.Clamp(availableWidth, MinWidth, MaxWidth);
                }
                else
                {
                    // 上限と下限の関係に不整合があるならば最大の下限を希望します。
                    w = MathHelper.Max(availableWidth, MinWidth);
                }
                // 余白で調整します。
                w = MathHelper.Max(MinWidth, w - margin.Left - margin.Right);
            }

            return w;
        }

        /// <summary>
        /// 子を測定する前の自身の高さを計算します。
        /// </summary>
        /// <param name="availableHeight">親が指定する利用可能な高さ。</param>
        /// <returns>高さ。</returns>
        protected virtual float CalculateBaseHeight(float availableHeight)
        {
            var h = Height;

            // 高さが設定されているならばそのまま希望します。
            // 高さが未設定ならば可能な限り最大の幅を希望します。
            if (float.IsNaN(Height))
            {
                if (MinHeight < MaxHeight)
                {
                    h = MathHelper.Clamp(availableHeight, MinHeight, MaxHeight);
                }
                else
                {
                    // 上限と下限の関係に不整合があるならば最大の下限を希望します。
                    h = MathHelper.Max(availableHeight, MinHeight);
                }
                // 余白で調整します。
                h = MathHelper.Max(MinHeight, h - margin.Top - margin.Bottom);
            }

            return h;
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
        /// KeyNavigationMode を取得または設定します。
        /// </summary>
        public FocusNavigationMode FocusNavigationMode { get; set; }

        protected virtual bool MoveFocus(FocusNavigation navigation)
        {
            switch (navigation)
            {
                case FocusNavigation.Forward:
                case FocusNavigation.Down:
                case FocusNavigation.Right:
                    {
                        return ForwardFocus();
                    }
                case FocusNavigation.Backward:
                case FocusNavigation.Up:
                case FocusNavigation.Left:
                    {
                        return BackwardFocus();
                    }
            }
            return false;
        }

        protected virtual bool ForwardFocus()
        {
            // ルートならば処理失敗で終えます。
            if (Parent == null) return false;

            // コンテナに自分の次の移動先を探索させてフォーカス移動を試みます。
            return Parent.ForwardFocus(this);
        }

        protected bool ForwardFocus(Control child)
        {
            // ナビゲーション不可ならば処理失敗で終えます。
            if (FocusNavigationMode == FocusNavigationMode.None) return false;

            // 指定された子の兄弟で次のフォーカス移動先となるものを探索して移動させます。
            for (int i = Children.IndexOf(child) + 1; i < Children.Count; i++)
            {
                var sibling = Children[i];
                if (sibling.Focusable)
                {
                    // Focusable な兄弟へフォーカスを移動します。
                    sibling.Focus();
                    return true;
                }

                // 兄弟が Focusable ではなくとも、その子孫に移動できないかどうかを調べます。
                if (sibling.FocusFirstFocusableDesendent()) return true;
            }

            // Cycle ならば先頭へフォーカスを移動させて処理を終えます。
            if (FocusNavigationMode == FocusNavigationMode.Cycle) return FocusFirstFocusableDesendent();

            // 以下、Continue の場合となります。

            // ルートでフォーカス可能な Control がないならば処理失敗で終えます。
            if (Parent == null) return false;

            // コンテナに自分の次の移動先を探索させてフォーカス移動を試みます。
            return Parent.ForwardFocus(this);
        }

        public bool FocusFirstFocusableDesendent()
        {
            foreach (var child in Children)
            {
                if (child.FocusFirstFocusableDesendent()) return true;
            }

            if (Focusable)
            {
                Focus();
                return true;
            }

            return false;
        }

        protected virtual bool BackwardFocus()
        {
            // ルートならば処理失敗で終えます。
            if (Parent == null) return false;

            // コンテナに自分の次の移動先を探索させてフォーカス移動を試みます。
            return Parent.BackwardFocus(this);
        }

        protected bool BackwardFocus(Control child)
        {
            // ナビゲーション不可ならば処理失敗で終えます。
            if (FocusNavigationMode == FocusNavigationMode.None) return false;

            // 指定された子の兄弟で次のフォーカス移動先となるものを探索して移動させます。
            for (int i = Children.IndexOf(child) - 1; 0 <= i; i--)
            {
                var sibling = Children[i];
                if (sibling.Focusable)
                {
                    // Focusable な兄弟へフォーカスを移動します。
                    sibling.Focus();
                    return true;
                }

                // 兄弟が Focusable ではなくとも、その子孫に移動できないかどうかを調べます。
                if (sibling.FocusLastFocusableDesendent()) return true;
            }

            // Cycle ならば先頭へフォーカスを移動させて処理を終えます。
            if (FocusNavigationMode == FocusNavigationMode.Cycle) return FocusLastFocusableDesendent();

            // 以下、Continue の場合となります。

            // ルートでフォーカス可能な Control がないならば処理失敗で終えます。
            if (Parent == null) return false;

            // コンテナに自分の次の移動先を探索させてフォーカス移動を試みます。
            return Parent.BackwardFocus(this);
        }

        public bool FocusLastFocusableDesendent()
        {
            for (int i = Children.Count - 1; 0 <= i; i--)
            {
                if (Children[i].FocusLastFocusableDesendent()) return true;
            }

            if (Focusable)
            {
                Focus();
                return true;
            }

            return false;
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
        protected virtual void OnMouseMove(float x, float y) { }

        /// <summary>
        /// マウス カーソルが入った時に呼び出されます。
        /// </summary>
        protected virtual void OnMouseEnter() { }

        /// <summary>
        /// マウス カーソルが出た時に呼び出されます。
        /// </summary>
        protected virtual void OnMouseLeave() { }

        /// <summary>
        /// マウス ボタンが押下された時に呼び出されます。
        /// </summary>
        /// <param name="button">マウス ボタン。</param>
        protected virtual void OnMouseDown(MouseButtons button) { }

        /// <summary>
        /// マウス ボタン押下が解放された時に呼び出されます。
        /// </summary>
        /// <param name="button">マウス ボタン。</param>
        protected virtual void OnMouseUp(MouseButtons button) { }

        /// <summary>
        /// キーが押下された時に呼び出されます。
        /// </summary>
        /// <param name="key">キー。</param>
        /// <returns>
        /// true (キー押下を処理した場合)、false (それ以外の場合)。
        /// </returns>
        protected virtual bool OnKeyDown(Keys key) { return false; }

        /// <summary>
        /// キー押下が解放された時に呼び出されます。
        /// </summary>
        /// <param name="key"></param>
        protected virtual void OnKeyUp(Keys key) { }

        /// <summary>
        /// 文字が入力された時に呼び出されます。
        /// </summary>
        /// <param name="character"></param>
        protected virtual void OnCharacterEnter(char character) { }

        /// <summary>
        /// マウス オーバ状態の Control を新しい Control へ切り替えます。
        /// </summary>
        /// <param name="newControl">Control。</param>
        void SwitchMouseOverControl(Control newControl)
        {
            if (mouseOverControl == newControl) return;

            // これまでマウス オーバ状態にあった Control に変更を通知します。
            if (mouseOverControl != null) mouseOverControl.ProcessMouseLeave();

            // 新たにマウス オーバ状態となった Control を設定し、変更を通知します。
            mouseOverControl = newControl;
            mouseOverControl.RaiseMouseEnter();
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
        void RaiseMouseEnter()
        {
            OnMouseEnter();
            if (MouseEnter != null) MouseEnter(this, EventArgs.Empty);
        }

        /// <summary>
        /// MouseLeft イベントを発生させます。
        /// </summary>
        void RaiseMouseLeave()
        {
            OnMouseLeave();
            if (MouseLeave != null) MouseLeave(this, EventArgs.Empty);
        }
    }
}
