#region Using

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
        /// この Control でマウスが移動した時に発生します。
        /// </summary>
        public event MouseEventHandler MouseMove;

        /// <summary>
        /// この Control でマウスが押下された時に発生します。
        /// </summary>
        public event MouseButtonEventHandler MouseDown;

        /// <summary>
        /// この Control でマウス押下が解放された時に発生します。
        /// </summary>
        public event MouseButtonEventHandler MouseUp;

        /// <summary>
        /// マウス カーソルが Control に入った時に発生します。
        /// </summary>
        public event MouseEventHandler MouseEnter;

        /// <summary>
        /// マウス カーソルが Control から出た時に発生します。
        /// </summary>
        public event MouseEventHandler MouseLeave;

        /// <summary>
        /// この Control でキーが押下された時に発生します。
        /// </summary>
        public event KeyEventHandler KeyDown;

        /// <summary>
        /// この Control でキー押下が解放された時に発生します。
        /// </summary>
        public event KeyEventHandler KeyUp;

        /// <summary>
        /// この Control で文字が入力された時に発生します。
        /// </summary>
        public event CharacterInputEventHandler CharacterEnter;

        /// <summary>
        /// フォーカスを得た時に発生します。
        /// </summary>
        public event EventHandler GotFocus;

        /// <summary>
        /// フォーカスを失った時に発生します。
        /// </summary>
        public event EventHandler LostFocus;

        /// <summary>
        /// 名前。
        /// </summary>
        string name;

        /// <summary>
        /// 親 Control。
        /// </summary>
        Control parent;

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
        /// true (フォーカスが設定されている場合)、false (それ以外の場合)。
        /// </summary>
        bool focused;

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
        public Thickness Margin { get; set; }

        /// <summary>
        /// 内側の余白を取得または設定します。
        /// </summary>
        public Thickness Padding { get; set; }

        /// <summary>
        /// 幅の下限を取得または設定します。
        /// </summary>
        public float MinWidth { get; set; }

        /// <summary>
        /// 高さの下限を取得または設定します。
        /// </summary>
        public float MinHeight { get; set; }
        
        /// <summary>
        /// 幅の上限を取得または設定します。
        /// </summary>
        public float MaxWidth { get; set; }
        
        /// <summary>
        /// 高さの上限を取得または設定します。
        /// </summary>
        public float MaxHeight { get; set; }

        /// <summary>
        /// 幅を取得または設定します。
        /// </summary>
        public float Width { get; set; }

        /// <summary>
        /// 高さを取得または設定します。
        /// </summary>
        public float Height { get; set; }

        /// <summary>
        /// 測定後のサイズを取得します。
        /// </summary>
        public Size MeasuredSize { get; private set; }

        /// <summary>
        /// 親の座標系における自身の描画座標を取得または設定します。
        /// </summary>
        public Point RenderOffset { get; set; }

        /// <summary>
        /// 配置後の描画サイズを取得します。
        /// </summary>
        public Size RenderSize { get; private set; }

        /// <summary>
        /// 配置後の幅を取得します。
        /// </summary>
        public float ActualWidth
        {
            get { return RenderSize.Width; }
        }

        /// <summary>
        /// 配置後の高さを取得します。
        /// </summary>
        public float ActualHeight
        {
            get { return RenderSize.Height; }
        }

        /// <summary>
        /// 自身の描画領域で子 Control をクリップするかどうかを示す値を取得または設定します。
        /// </summary>
        /// <value>
        /// true (自身の描画領域で子 Control をクリップする場合)、false (それ以外の場合)。
        /// </value>
        public bool ClipEnabled { get; set; }

        /// <summary>
        /// SpriteFont を取得または設定します。
        /// </summary>
        public SpriteFont Font { get; set; }

        /// <summary>
        /// フォントの拡大縮小の度合いを取得または設定します。
        /// </summary>
        public Vector2 FontStretch { get; set; }

        /// <summary>
        /// 透明度を取得または設定します。
        /// </summary>
        public float Opacity { get; set; }

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
            }
        }

        /// <summary>
        /// Screen を取得します。
        /// </summary>
        public Screen Screen { get; private set; }

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

                // イベントを発生させます。
                OnEnabledChanged();
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

                // イベントを発生させます。
                OnVisibleChanged();
            }
        }

        /// <summary>
        /// HitTest が有効かどうかを示す値を取得または設定します。
        /// </summary>
        /// <value>
        /// true (HitTest が有効な場合)、false (それ以外の場合)。
        /// </value>
        public bool HitTestEnabled { get; set; }

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
        /// フォーカスを設定できるかを示す値を取得または設定します。
        /// </summary>
        /// <value>true (フォーカスを設定できる場合)、false (それ以外の場合)。</value>
        public bool Focusable { get; set; }

        /// <summary>
        /// フォーカスが設定されているかどうかを示す値を取得します。
        /// </summary>
        /// <value>true (フォーカスが設定されている場合)、false (それ以外の場合)。</value>
        public bool Focused
        {
            get { return focused; }
            internal set
            {
                if (focused == value) return;

                focused = value;

                if (focused)
                {
                    OnGotFocus();
                }
                else
                {
                    OnLostFocus();
                }
            }
        }

        /// <summary>
        /// KeyNavigationMode を取得または設定します。
        /// </summary>
        public FocusNavigationMode FocusNavigationMode { get; set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="screen">Screen。</param>
        public Control(Screen screen)
        {
            if (screen == null) throw new ArgumentNullException("screen");
            Screen = screen;

            Children = new ParentingControlCollection(this);

            Width = float.NaN;
            Height = float.NaN;
            MaxWidth = float.PositiveInfinity;
            MaxHeight = float.PositiveInfinity;
            Opacity = 1;
            ForegroundColor = Color.White;
            BackgroundColor = Color.Black;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            FontStretch = Vector2.One;
            ClipEnabled = true;
            Focusable = true;
            HitTestEnabled = true;
        }

        /// <summary>
        /// 測定します。
        /// </summary>
        /// <param name="availableSize">親 Control が指定する利用可能なサイズ。</param>
        public void Measure(Size availableSize)
        {
            MeasuredSize = MeasureOverride(availableSize);
        }

        /// <summary>
        /// 配置します。
        /// </summary>
        /// <param name="finalBounds">親 Control が指定する配置に使用可能な領域。</param>
        public void Arrange(Rect finalBounds)
        {
            RenderSize = ArrangeOverride(finalBounds.Size);
            RenderOffset = finalBounds.TopLeft;
        }

        /// <summary>
        /// フォーカスを得ます。
        /// </summary>
        public void Focus()
        {
            if (Screen == null || !Enabled || !Visible || !Focusable) return;

            Screen.SetFocus(this);
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
        public virtual void Draw(GameTime gameTime, IDrawContext drawContext)
        {
            // IControlLaf を描画します。
            var laf = drawContext.GetControlLaf(this);
            if (laf != null) laf.Draw(this, drawContext);

            if (ClipEnabled)
            {
                using (var setClip = drawContext.SetClip(new Rect(RenderSize)))
                {
                    DrawChildren(gameTime, drawContext);
                }
            }
            else
            {
                DrawChildren(gameTime, drawContext);
            }
        }

        /// <summary>
        /// 画面座標における Point を Control の座標系の Point へ変換します。
        /// </summary>
        /// <param name="point">画面座標における Point。</param>
        /// <returns>Control の座標系に変換された Point。</returns>
        public Point PointFromScreen(Point point)
        {
            var offset = RenderOffset;
            var result = new Point(point.X - offset.X, point.Y - offset.Y);
            return (Parent != null) ? Parent.PointFromScreen(result) : result;
        }

        /// <summary>
        /// Control の座標系の Point を画面座標における Point へ変換します。
        /// </summary>
        /// <param name="point">Control の座標系を表す Point。</param>
        /// <returns>画面座標に変換された Point。</returns>
        public Point PointToScreen(Point point)
        {
            var offset = RenderOffset;
            var result = new Point(point.X + offset.X, point.Y + offset.Y);
            return (Parent != null) ? Parent.PointToScreen(result) : result;
        }

        /// <summary>
        /// Hit Test を行います。
        /// </summary>
        /// <param name="point">画面座標における Point。</param>
        /// <returns>
        /// true (Hit した場合)、false (それ以外の場合)。
        /// </returns>
        public virtual bool HitTest(Point point)
        {
            if (!HitTestEnabled) return false;

            var localPoint = PointFromScreen(point);
            return (0 <= localPoint.X) && (localPoint.X < RenderSize.Width)
                && (0 <= localPoint.Y) && (localPoint.Y < RenderSize.Height);
        }

        /// <summary>
        /// この Control 内でフォーカス設定可能な先頭の Control となる子孫あるいは自身にフォーカスを設定します。
        /// </summary>
        /// <returns>
        /// true (子孫あるいは自身にフォーカスを設定した場合)、false (それ以外の場合)。
        /// </returns>
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

        /// <summary>
        /// この Control 内でフォーカス設定可能な末尾の Control となる子孫あるいは自身にフォーカスを設定します。
        /// </summary>
        /// <returns>
        /// true (子孫あるいは自身にフォーカスを設定した場合)、false (それ以外の場合)。
        /// </returns>
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
        /// 測定します。
        /// </summary>
        /// <param name="availableSize">親が指定する利用可能なサイズ。</param>
        /// <returns>自身が希望するサイズ。</returns>
        protected virtual Size MeasureOverride(Size availableSize)
        {
            // 暫定的に自身の希望サイズを計算します。
            var size = new Size();
            size.Width = !float.IsNaN(Width) ? Width : CalculateWidth(availableSize.Width);
            size.Height = !float.IsNaN(Height) ? Height : CalculateHeight(availableSize.Height);

            // 子が利用可能なサイズを計算します。
            var childAvailableSize = size;
            childAvailableSize.Width -= Padding.Left + Padding.Right;
            childAvailableSize.Height -= Padding.Top + Padding.Bottom;

            // 子の希望サイズを定めます。
            float measuredWidth = 0;
            float measuredHeight = 0;
            foreach (var child in Children)
            {
                child.Measure(childAvailableSize);

                measuredWidth = Math.Max(measuredWidth, child.MeasuredSize.Width + child.Margin.Left + child.Margin.Right);
                measuredHeight = Math.Max(measuredHeight, child.MeasuredSize.Height + child.Margin.Top + child.Margin.Bottom);
            }

            // 幅や高さが未設定ならば子のうちの最大の値に合わせます。
            if (float.IsNaN(Width)) size.Width = ClampWidth(measuredWidth + Padding.Left + Padding.Right);
            if (float.IsNaN(Height)) size.Height = ClampHeight(measuredHeight + Padding.Top + Padding.Bottom);

            return size;
        }

        /// <summary>
        /// 指定された幅を [MinWidth, MaxWidth] でクランプします。
        /// </summary>
        /// <param name="width">幅。</param>
        /// <returns>クランプされた幅。</returns>
        protected float ClampWidth(float width)
        {
            if (MinWidth < MaxWidth)
            {
                return MathHelper.Clamp(width, MinWidth, MaxWidth);
            }
            else
            {
                // 上限と下限の関係に不整合があるならば下限と比較して大きい方を取ります。
                return MathHelper.Max(width, MinWidth);
            }
        }

        /// <summary>
        /// 指定された高さを [MinHeight, MaxHeight] でクランプします。
        /// </summary>
        /// <param name="height">高さ。</param>
        /// <returns>クランプされた高さ。</returns>
        protected float ClampHeight(float height)
        {
            if (MinHeight < MaxHeight)
            {
                return MathHelper.Clamp(height, MinHeight, MaxHeight);
            }
            else
            {
                // 上限と下限の関係に不整合があるならば下限と比較して大きい方を取ります。
                return MathHelper.Max(height, MinHeight);
            }
        }

        /// <summary>
        /// MinWidth、MaxWidth の関係に従い、指定された利用可能な幅から自身が希望する幅を計算します。
        /// </summary>
        /// <param name="availableWidth">利用可能な幅。</param>
        /// <returns>自身が希望する幅。</returns>
        protected virtual float CalculateWidth(float availableWidth)
        {
            float desiredWidth = ClampWidth(availableWidth);
            // 余白で調整します。
            return MathHelper.Max(MinWidth, desiredWidth - Margin.Left - Margin.Right);
        }

        /// <summary>
        /// MinHeight、MaxHeight の関係に従い、指定された利用可能な高さから自身が希望する高さを計算します。
        /// </summary>
        /// <param name="availableHeight">利用可能な高さ。</param>
        /// <returns>自身が希望する高さ。</returns>
        protected virtual float CalculateHeight(float availableHeight)
        {
            float desiredHeight = ClampHeight(availableHeight);
            // 余白で調整します。
            return MathHelper.Max(MinHeight, desiredHeight - Margin.Top - Margin.Bottom);
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
                var childBounds = new Rect(child.MeasuredSize);

                switch (child.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        childBounds.X = Padding.Left + child.Margin.Left;
                        break;
                    case HorizontalAlignment.Right:
                        childBounds.X = finalSize.Width - child.MeasuredSize.Width - Padding.Right - child.Margin.Right;
                        break;
                    case HorizontalAlignment.Center:
                    default:
                        // Padding された座標系から Magin を含む子領域の相対位置を計算します。
                        var paddedWidth = (finalSize.Width - Padding.Left - Padding.Right);
                        childBounds.X = (paddedWidth - child.MeasuredSize.Width - child.Margin.Left - child.Margin.Right) * 0.5f;
                        // Magin を含む子領域からの実際の子領域の位置をもとめます。
                        childBounds.X += child.Margin.Left;
                        // Padding された座標系を指定領域の座標系へ変換します。
                        childBounds.X += Padding.Left;
                        break;
                }

                switch (child.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        childBounds.Y = Padding.Top + child.Margin.Top;
                        break;
                    case VerticalAlignment.Bottom:
                        childBounds.Y = finalSize.Height - child.MeasuredSize.Height - Padding.Bottom - child.Margin.Bottom;
                        break;
                    case UI.VerticalAlignment.Center:
                    default:
                        var paddedHeight = (finalSize.Height - Padding.Top - Padding.Bottom);
                        childBounds.Y = (paddedHeight - child.MeasuredSize.Height - child.Margin.Top - child.Margin.Bottom) * 0.5f;
                        childBounds.Y += child.Margin.Top;
                        childBounds.Y += Padding.Top;
                        break;
                }

                child.Arrange(childBounds);
            }

            return finalSize;
        }

        /// <summary>
        /// 指定された移動方向に基づいてフォーカスを移動させます。
        /// </summary>
        /// <param name="navigation">フォーカスの移動方向。</param>
        /// <returns>
        /// true (フォーカスを移動させた場合)、false (それ以外の場合)。
        /// </returns>
        public virtual bool MoveFocus(FocusNavigation navigation)
        {
            switch (navigation)
            {
                case FocusNavigation.Forward:
                case FocusNavigation.Down:
                case FocusNavigation.Right:
                    return ForwardFocus();
                case FocusNavigation.Backward:
                case FocusNavigation.Up:
                case FocusNavigation.Left:
                    return BackwardFocus();
            }
            return false;
        }

        /// <summary>
        /// この Control が descendant の先祖かどうかを判定します。
        /// </summary>
        /// <param name="descendant">Control。</param>
        /// <returns>
        /// true (この Control が descendant の先祖である場合)、false (それ以外の場合)。
        /// </returns>
        public bool IsAncestorOf(Control descendant)
        {
            for (var parent = descendant.Parent; parent != null; parent = parent.Parent)
            {
                if (parent == this) return true;
            }
            return false;
        }

        /// <summary>
        /// この Control が ancestor の子孫かどうかを判定します。
        /// </summary>
        /// <param name="ancestor"></param>
        /// <returns>
        /// true (この Control が ancestor の子孫である場合)、false (それ以外の場合)。
        /// </returns>
        public bool IsDescendantOf(Control ancestor)
        {
            return ancestor.IsAncestorOf(this);
        }

        /// <summary>
        /// 次の Control へフォーカスを移動させます。
        /// </summary>
        /// <returns>
        /// true (フォーカスを移動させた場合)、false (それ以外の場合)。
        /// </returns>
        protected virtual bool ForwardFocus()
        {
            // ルートならば処理失敗で終えます。
            if (Parent == null) return false;

            // コンテナに自分の次の移動先を探索させてフォーカス移動を試みます。
            return Parent.ForwardFocus(this);
        }

        /// <summary>
        /// 前の Control へフォーカスを移動させます。
        /// </summary>
        /// <returns>
        /// true (フォーカスを移動させた場合)、false (それ以外の場合)。
        /// </returns>
        protected virtual bool BackwardFocus()
        {
            // ルートならば処理失敗で終えます。
            if (Parent == null) return false;

            // コンテナに自分の次の移動先を探索させてフォーカス移動を試みます。
            return Parent.BackwardFocus(this);
        }

        /// <summary>
        /// Enabled プロパティが変更された時に呼び出されます。
        /// EnabledChanged イベントを発生させます。
        /// </summary>
        protected virtual void OnEnabledChanged()
        {
            if (EnabledChanged != null) EnabledChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Visible プロパティが変更された時に呼び出されます。
        /// VisibleChanged イベントを発生させます。
        /// </summary>
        protected virtual void OnVisibleChanged()
        {
            if (VisibleChanged != null) VisibleChanged(this, EventArgs.Empty);
        }

        /// <summary>
        /// マウス カーソルが移動した時に呼び出されます。
        /// MouseMove イベントを発生させます。
        /// </summary>
        /// <param name="mouseDevice">MouseDevice。</param>
        protected virtual void OnMouseMove(MouseDevice mouseDevice)
        {
            // イベント送信者を Control にします。
            if (MouseMove != null) MouseMove(this, mouseDevice);
        }

        /// <summary>
        /// マウス カーソルが入った時に呼び出されます。
        /// MouseEnter イベントを発生させます。
        /// </summary>
        /// <param name="mouseDevice">MouseDevice。</param>
        protected virtual void OnMouseEnter(MouseDevice mouseDevice)
        {
            if (MouseEnter != null) MouseEnter(this, mouseDevice);
        }

        /// <summary>
        /// マウス カーソルが出た時に呼び出されます。
        /// MouseLeave イベントを発生させます。
        /// </summary>
        /// <param name="mouseDevice">MouseDevice。</param>
        protected virtual void OnMouseLeave(MouseDevice mouseDevice)
        {
            if (MouseLeave != null) MouseLeave(this, mouseDevice);
        }

        /// <summary>
        /// マウス ボタンが押下された時に呼び出されます。
        /// MouseDown イベントを発生させます。
        /// </summary>
        /// <param name="mouseDevice">MouseDevice。</param>
        /// <param name="buttons">押下されたボタン。</param>
        protected virtual void OnMouseDown(MouseDevice mouseDevice, MouseButtons buttons)
        {
            if (MouseDown != null) MouseDown(this, mouseDevice, buttons);
        }

        /// <summary>
        /// マウス ボタン押下が解放された時に呼び出されます。
        /// MouseUp イベントを発生させます。
        /// </summary>
        /// <param name="mouseDevice">MouseDevice。</param>
        /// <param name="buttons">押下が解放されたボタン。</param>
        protected virtual void OnMouseUp(MouseDevice mouseDevice, MouseButtons buttons)
        {
            if (MouseUp != null) MouseUp(this, mouseDevice, buttons);
        }

        /// <summary>
        /// キーが押下された時に呼び出されます。
        /// KeyDown イベントを発生させます。
        /// </summary>
        /// <param name="keyboardDevice">KeyboardDevice。</param>
        /// <param name="key">押下されているキー。</param>
        /// <returns>
        /// true (キー押下を処理した場合)、false (それ以外の場合)。
        /// </returns>
        protected virtual bool OnKeyDown(KeyboardDevice keyboardDevice, Keys key)
        {
            if (KeyDown != null) KeyDown(this, keyboardDevice, key);
            return false;
        }

        /// <summary>
        /// キー押下が解放された時に呼び出されます。
        /// KeyDown イベントを発生させます。
        /// </summary>
        /// <param name="keyboardDevice">KeyboardDevice。</param>
        /// <param name="key">押下が解放されたキー。</param>
        protected virtual void OnKeyUp(KeyboardDevice keyboardDevice, Keys key)
        {
            if (KeyUp != null) KeyUp(this, keyboardDevice, key);
        }

        /// <summary>
        /// 文字が入力された時に呼び出されます。
        /// CharacterEnter イベントを発生させます。
        /// </summary>
        /// <param name="keyboardDevice">KeyboardDevice。</param>
        /// <param name="character">入力された文字。</param>
        protected virtual void OnCharacterEnter(KeyboardDevice keyboardDevice, char character)
        {
            if (CharacterEnter != null) CharacterEnter(this, keyboardDevice, character);
        }

        /// <summary>
        /// フォーカスが設定された時に発生します。
        /// GotFocus イベントを発生させます。
        /// </summary>
        protected virtual void OnGotFocus()
        {
            if (GotFocus != null) GotFocus(this, EventArgs.Empty);
        }

        /// <summary>
        /// フォーカスが解除された時に発生します。
        /// LostFocus イベントを発生させます。
        /// </summary>
        protected virtual void OnLostFocus()
        {
            if (LostFocus != null) LostFocus(this, EventArgs.Empty);
        }

        /// <summary>
        /// 子 Control を描画します。
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="drawContext"></param>
        /// <param name="child">子 Control。</param>
        protected virtual void DrawChild(GameTime gameTime, IDrawContext drawContext, Control child)
        {
            //
            // TODO
            //
            // 暫定的な描画領域決定アルゴリズムです。
            // スクロール処理なども考慮して描画領域を算出する必要があります。
            drawContext.Location = child.PointToScreen(Point.Zero);
            drawContext.PushOpacity(child.Opacity);

            child.Draw(gameTime, drawContext);

            drawContext.PopOpacity();
        }

        /// <summary>
        /// マウス カーソル移動を処理します。
        /// </summary>
        /// <param name="mouseDevice">MouseDevice。</param>
        internal void ProcessMouseMove(MouseDevice mouseDevice)
        {
            var mouseState = mouseDevice.MouseState;

            OnMouseMove(mouseDevice);

            for (int i = Children.Count - 1; 0 <= i; i--)
            {
                var child = Children[i];

                // 不可視ならばスキップします。
                if (!child.Visible) continue;

                // Hit Test に失敗するならばスキップします。
                if (!child.HitTestEnabled || !child.HitTest(new Point(mouseState.X, mouseState.Y)))
                    continue;

                // 子に通知します。
                child.ProcessMouseMove(mouseDevice);

                // 子をマウス オーバ状態にします。
                SwitchMouseOverControl(mouseDevice, child);
                return;
            }

            // マウス オーバ状態にできる子がないならば、自分をマウス オーバ状態にします。
            SwitchMouseOverControl(mouseDevice, this);
        }

        /// <summary>
        /// マウス カーソルが Control の領域外に移動したことを処理します。
        /// </summary>
        /// <param name="mouseDevice">MouseDevice。</param>
        internal void ProcessMouseLeave(MouseDevice mouseDevice)
        {
            // マウス オーバ状態の Control がなければ処理しません。
            if (mouseOverControl == null) return;

            if (mouseOverControl != this)
            {
                // マウス オーバ状態の子へ処理を転送します。
                mouseOverControl.ProcessMouseLeave(mouseDevice);
            }
            else
            {
                // 自分がマウス オーバ状態なのでイベント ハンドラを呼びます。
                OnMouseLeave(mouseDevice);
            }

            // マウス オーバ状態を解除します。
            mouseOverControl = null;
        }

        /// <summary>
        /// マウス ボタン押下を処理します。
        /// </summary>
        /// <param name="mouseDevice">MouseDevice。</param>
        /// <param name="buttons">押下されたボタン。</param>
        /// <returns></returns>
        internal bool ProcessMouseDown(MouseDevice mouseDevice, MouseButtons buttons)
        {
            // 非マウス オーバ状態で呼ばれたならば処理しません。
            if (mouseOverControl == null) return false;

            // 子がマウス オーバ状態ならば子へ通知します。
            if (mouseOverControl != this) return mouseOverControl.ProcessMouseDown(mouseDevice, buttons);

            // フォーカスを得ます。
            Focus();

            // 自分へ通知します。
            OnMouseDown(mouseDevice, buttons);
            return true;
        }

        /// <summary>
        /// マウス ボタン押下の解放を処理します。
        /// </summary>
        /// <param name="mouseDevice">MouseDevice。</param>
        /// <param name="buttons">押下が解放されたボタン。</param>
        internal void ProcessMouseUp(MouseDevice mouseDevice, MouseButtons buttons)
        {
            // 非マウス オーバ状態で呼ばれたならば処理しません。
            if (mouseOverControl == null) return;

            // 子がマウス オーバ状態ならば子へ通知します。
            if (mouseOverControl != this)
            {
                mouseOverControl.ProcessMouseUp(mouseDevice, buttons);
                return;
            }

            // 自分へ通知します。
            OnMouseUp(mouseDevice, buttons);
        }

        /// <summary>
        /// キー押下を処理します。
        /// </summary>
        /// <param name="keyboardDevice">KeyboardDevice。</param>
        /// <param name="key">押下されているキー。</param>
        /// <returns>
        /// true (キー押下を処理した場合)、false (それ以外の場合)。
        /// </returns>
        internal bool ProcessKeyDown(KeyboardDevice keyboardDevice, Keys key)
        {
            if (OnKeyDown(keyboardDevice, key)) return true;

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
        /// <param name="keyboardDevice">KeyboardDevice。</param>
        /// <param name="key">押下が解放されたキー。</param>
        internal void ProcessKeyUp(KeyboardDevice keyboardDevice, Keys key)
        {
            OnKeyUp(keyboardDevice, key);
        }

        /// <summary>
        /// 文字の入力を処理します。
        /// </summary>
        /// <param name="keyboardDevice">KeyboardDevice。</param>
        /// <param name="character">入力された文字。</param>
        internal void ProcessCharacterEnter(KeyboardDevice keyboardDevice, char character)
        {
            OnCharacterEnter(keyboardDevice, character);
        }

        /// <summary>
        /// マウス オーバ状態の Control を新しい Control へ切り替えます。
        /// </summary>
        /// <param name="mouseDevice">MouseDevice。</param>
        /// <param name="newControl">Control。</param>
        void SwitchMouseOverControl(MouseDevice mouseDevice, Control newControl)
        {
            if (mouseOverControl == newControl) return;

            // これまでマウス オーバ状態にあった Control に変更を通知します。
            if (mouseOverControl != null) mouseOverControl.ProcessMouseLeave(mouseDevice);

            // 新たにマウス オーバ状態となった Control を設定し、変更を通知します。
            mouseOverControl = newControl;
            mouseOverControl.OnMouseEnter(mouseDevice);
        }

        /// <summary>
        /// 指定の Control について次の Control へフォーカスを移動させます。
        /// </summary>
        /// <param name="child">子 Control。</param>
        /// <returns>
        /// true (フォーカスを移動させた場合)、false (それ以外の場合)。
        /// </returns>
        bool ForwardFocus(Control child)
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

        /// <summary>
        /// 指定の Control について前の Control へフォーカスを移動させます。
        /// </summary>
        /// <param name="child">子 Control。</param>
        /// <returns>
        /// true (フォーカスを移動させた場合)、false (それ以外の場合)。
        /// </returns>
        bool BackwardFocus(Control child)
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

        /// <summary>
        /// 子 Control を描画します。
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="drawContext"></param>
        void DrawChildren(GameTime gameTime, IDrawContext drawContext)
        {
            // 子を再帰的に描画します。
            foreach (var child in Children)
            {
                // 不可視ならば描画しません。
                if (!child.Visible) continue;

                // 描画する必要のないサイズならばスキップします。
                // 最終的に扱う精度の問題から int で判定する点に注意してください。
                var childRenderSize = child.RenderSize;
                if ((int) childRenderSize.Width <= 0 || (int) childRenderSize.Height <= 0)
                    continue;

                DrawChild(gameTime, drawContext, child);
            }
        }
    }
}
