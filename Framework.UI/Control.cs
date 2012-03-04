#region Using

using System;
using System.Collections.Generic;
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
        /// ルーティング イベントの到達で呼び出されるクラス ハンドラを表します。
        /// </summary>
        /// <param name="context"></param>
        protected delegate void ClassRoutedEventHandler(ref RoutedEventContext context);

        #region Routing Events

        /// <summary>
        /// マウス カーソルが移動した時に発生します。
        /// </summary>
        public static readonly string PreviewMouseMoveEvent = "PreviewMouseMove";

        /// <summary>
        /// マウス カーソルが移動した時に発生します。
        /// </summary>
        public static readonly string MouseMoveEvent = "MouseMove";

        /// <summary>
        /// マウス カーソルが入った時に発生します。
        /// </summary>
        public static readonly string PreviewMouseEnterEvent = "PreviewMouseEnter";

        /// <summary>
        /// マウス カーソルが入った時に発生します。
        /// </summary>
        public static readonly string MouseEnterEvent = "MouseEnter";

        /// <summary>
        /// マウス カーソルが出た時に発生します。
        /// </summary>
        public static readonly string PreviewMouseLeaveEvent = "PreviewMouseLeave";

        /// <summary>
        /// マウス カーソルが出た時に発生します。
        /// </summary>
        public static readonly string MouseLeaveEvent = "MouseLeave";

        /// <summary>
        /// マウス ボタンが押された時に発生します。
        /// </summary>
        public static readonly string PreviewMouseDownEvent = "PreviewMouseDown";

        /// <summary>
        /// マウス ボタンが押された時に発生します。
        /// </summary>
        public static readonly string MouseDownEvent = "MouseDown";

        /// <summary>
        /// マウス ボタンが離された時に発生します。
        /// </summary>
        public static readonly string PreviewMouseUpEvent = "PreviewMouseUp";

        /// <summary>
        /// マウス ボタンが離された時に発生します。
        /// </summary>
        public static readonly string MouseUpEvent = "MouseUp";

        /// <summary>
        /// キーが押された時に発生します。
        /// </summary>
        public static readonly string PreviewKeyDownEvent = "PreviewKeyDown";

        /// <summary>
        /// キーが押された時に発生します。
        /// </summary>
        public static readonly string KeyDownEvent = "KeyDown";

        /// <summary>
        /// キーが離された時に発生します。
        /// </summary>
        public static readonly string PreviewKeyUpEvent = "PreviewKeyUp";

        /// <summary>
        /// キーが離された時に発生します。
        /// </summary>
        public static readonly string KeyUpEvent = "KeyUp";

        /// <summary>
        /// フォーカスが設定された時に発生します。
        /// </summary>
        public static readonly string GotFocusEvent = "GotFocus";

        /// <summary>
        /// フォーカスが解除された時に発生します。
        /// </summary>
        public static readonly string LostFocusEvent = "LostFocus";

        /// <summary>
        /// 論理フォーカスが設定された時に発生します。
        /// </summary>
        public static readonly string GotLogicalFocusEvent = "GotLogicalFocus";

        /// <summary>
        /// 論理フォーカスが解除された時に発生します。
        /// </summary>
        public static readonly string LostLogicalFocusEvent = "LostLogicalFocus";

        #endregion

        #region Event Handlers

        /// <summary>
        /// Enabled プロパティが変更された時に発生します。
        /// </summary>
        public event EventHandler EnabledChanged = delegate { };

        /// <summary>
        /// Visible プロパティが変更された時に発生します。
        /// </summary>
        public event EventHandler VisibleChanged = delegate { };

        #endregion

        #region Routing Event Handlers

        /// <summary>
        /// PreviewMouseMove イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler PreviewMouseMove
        {
            add { AddHandler(PreviewMouseMoveEvent, value); }
            remove { RemoveHandler(PreviewMouseMoveEvent, value); }
        }

        /// <summary>
        /// MouseMove イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler MouseMove
        {
            add { AddHandler(MouseMoveEvent, value); }
            remove { RemoveHandler(MouseMoveEvent, value); }
        }

        /// <summary>
        /// PreviewMouseEnter イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler PreviewMouseEnter
        {
            add { AddHandler(PreviewMouseEnterEvent, value); }
            remove { RemoveHandler(PreviewMouseEnterEvent, value); }
        }

        /// <summary>
        /// MouseEnter イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler MouseEnter
        {
            add { AddHandler(MouseEnterEvent, value); }
            remove { RemoveHandler(MouseEnterEvent, value); }
        }

        /// <summary>
        /// PreviewMouseLeave イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler PreviewMouseLeave
        {
            add { AddHandler(PreviewMouseLeaveEvent, value); }
            remove { RemoveHandler(PreviewMouseLeaveEvent, value); }
        }

        /// <summary>
        /// MouseLeave イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler MouseLeave
        {
            add { AddHandler(MouseLeaveEvent, value); }
            remove { RemoveHandler(MouseLeaveEvent, value); }
        }

        /// <summary>
        /// PreviewMouseDown イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler PreviewMouseDown
        {
            add { AddHandler(PreviewMouseDownEvent, value); }
            remove { RemoveHandler(PreviewMouseDownEvent, value); }
        }

        /// <summary>
        /// MouseDown イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler MouseDown
        {
            add { AddHandler(MouseDownEvent, value); }
            remove { RemoveHandler(MouseDownEvent, value); }
        }

        /// <summary>
        /// PreviewMouseUp イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler PreviewMouseUp
        {
            add { AddHandler(PreviewMouseUpEvent, value); }
            remove { RemoveHandler(PreviewMouseUpEvent, value); }
        }

        /// <summary>
        /// MouseUp イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler MouseUp
        {
            add { AddHandler(MouseUpEvent, value); }
            remove { RemoveHandler(MouseUpEvent, value); }
        }

        /// <summary>
        /// PreviewKeyDown イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler PreviewKeyDown
        {
            add { AddHandler(PreviewKeyDownEvent, value); }
            remove { RemoveHandler(PreviewKeyDownEvent, value); }
        }

        /// <summary>
        /// KeyDown イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler KeyDown
        {
            add { AddHandler(KeyDownEvent, value); }
            remove { RemoveHandler(KeyDownEvent, value); }
        }

        /// <summary>
        /// PreviewKeyUp イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler PreviewKeyUp
        {
            add { AddHandler(PreviewKeyUpEvent, value); }
            remove { RemoveHandler(PreviewKeyUpEvent, value); }
        }

        /// <summary>
        /// KeyUp イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler KeyUp
        {
            add { AddHandler(KeyUpEvent, value); }
            remove { RemoveHandler(KeyUpEvent, value); }
        }

        /// <summary>
        /// GotFocus イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler GotFocus
        {
            add { AddHandler(GotFocusEvent, value); }
            remove { RemoveHandler(GotFocusEvent, value); }
        }

        /// <summary>
        /// LostFocus イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler LostFocus
        {
            add { AddHandler(LostFocusEvent, value); }
            remove { RemoveHandler(LostFocusEvent, value); }
        }

        /// <summary>
        /// GotLogicalFocus イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler GotLogicalFocus
        {
            add { AddHandler(GotLogicalFocusEvent, value); }
            remove { RemoveHandler(GotLogicalFocusEvent, value); }
        }

        /// <summary>
        /// LostLogicalFocus イベントのハンドラを追加または削除します。
        /// </summary>
        public event RoutedEventHandler LostLogicalFocus
        {
            add { AddHandler(LostLogicalFocusEvent, value); }
            remove { RemoveHandler(LostLogicalFocusEvent, value); }
        }

        #endregion

        /// <summary>
        /// イベント名をキーに RoutedEventHandler を値とするマップ。
        /// </summary>
        Dictionary<string, List<RoutedEventHandler>> handlerMap = new Dictionary<string, List<RoutedEventHandler>>();

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
        /// true (論理フォーカスが設定されている場合)、false (それ以外の場合)。
        /// </summary>
        bool logicalFocused;

        /// <summary>
        /// Data Context。
        /// </summary>
        object dataContext;

        /// <summary>
        /// Screen を取得します。
        /// </summary>
        public Screen Screen { get; private set; }

        /// <summary>
        /// 親 Control を取得します。
        /// </summary>
        public Control Parent { get; private set; }

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
        /// 配置で希望するサイズを取得します。
        /// </summary>
        public Size MeasuredSize { get; private set; }

        /// <summary>
        /// 親の座標系における自身の描画座標を取得または設定します。
        /// </summary>
        public Vector2 RenderOffset { get; set; }

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
        /// 影の描画位置を取得または設定します。
        /// [0, 0] は影を描画しないことを表します。
        /// デフォルトは [0, 0] です。
        /// </summary>
        public Vector2 ShadowOffset { get; set; }

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
        /// フォーカスを設定できるかどうかを示す値を取得または設定します。
        /// </summary>
        /// <value>true (フォーカスを設定できる場合)、false (それ以外の場合)。</value>
        public bool Focusable { get; set; }

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
                RaiseEvent(null, focused ? GotFocusEvent : LostFocusEvent);
            }
        }

        /// <summary>
        /// 論理フォーカスが設定されているかどうかを示す値を取得します。
        /// </summary>
        /// <value>true (論理フォーカスが設定されている場合)、false (それ以外の場合)。</value>
        public bool LogicalFocused
        {
            get { return logicalFocused; }
            internal set
            {
                if (logicalFocused == value) return;

                logicalFocused = value;
                RaiseEvent(null, logicalFocused ? GotLogicalFocusEvent : LostLogicalFocusEvent);
            }
        }

        /// <summary>
        /// Animation のリストを取得します。
        /// </summary>
        public List<Animation> Animations { get; private set; }

        /// <summary>
        /// Data Context を取得または設定します。
        /// </summary>
        public object DataContext
        {
            get
            {
                // 明示的に設定されているならば、それを返します。
                if (dataContext != null) return dataContext;

                // 明示的に設定されていないならば、親に従います。
                // Desktop にも DataContext が設定されていないならば、Screen に従います。
                return (Parent != null) ? Parent.DataContext : Screen.DataContext;
            }
            set { dataContext = value; }
        }

        /// <summary>
        /// 子 Control の数を取得します。
        /// </summary>
        protected virtual int ChildrenCount
        {
            get { return 0; }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="screen">Screen。</param>
        protected Control(Screen screen)
        {
            if (screen == null) throw new ArgumentNullException("screen");
            Screen = screen;

            PreviewMouseMove += CreateRoutedEventHandler(OnPreviewMouseMove);
            MouseMove += CreateRoutedEventHandler(OnMouseMove);
            PreviewMouseEnter += CreateRoutedEventHandler(OnPreviewMouseEnter);
            MouseEnter += CreateRoutedEventHandler(OnMouseEnter);
            PreviewMouseLeave += CreateRoutedEventHandler(OnPreviewMouseLeave);
            MouseLeave += CreateRoutedEventHandler(OnMouseLeave);
            PreviewMouseDown += CreateRoutedEventHandler(OnPreviewMouseDown);
            MouseDown += CreateRoutedEventHandler(OnMouseDown);
            PreviewMouseUp += CreateRoutedEventHandler(OnPreviewMouseUp);
            MouseUp += CreateRoutedEventHandler(OnMouseUp);

            PreviewKeyDown += CreateRoutedEventHandler(OnPreviewKeyDown);
            KeyDown += CreateRoutedEventHandler(OnKeyDown);
            PreviewKeyUp += CreateRoutedEventHandler(OnPreviewKeyUp);
            KeyUp += CreateRoutedEventHandler(OnKeyUp);
            
            GotFocus += CreateRoutedEventHandler(OnGotFocus);
            LostFocus += CreateRoutedEventHandler(OnLostFocus);
            GotLogicalFocus += CreateRoutedEventHandler(OnGotLogicalFocus);
            LostLogicalFocus += CreateRoutedEventHandler(OnLostLogicalFocus);

            Width = float.NaN;
            Height = float.NaN;
            MaxWidth = float.PositiveInfinity;
            MaxHeight = float.PositiveInfinity;
            Opacity = 1;
            ForegroundColor = Color.Black;
            BackgroundColor = Color.White;
            HorizontalAlignment = HorizontalAlignment.Center;
            VerticalAlignment = VerticalAlignment.Center;
            FontStretch = Vector2.One;
            ClipEnabled = true;

            Animations = new List<Animation>();
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
        /// フォーカスを設定します。
        /// </summary>
        /// <returns></returns>
        public bool Focus()
        {
            if (!Focusable || !Enabled || !Visible) return false;

            var focusScope = FocusScope.GetFocusScope(this);
            if (focusScope == null) return false;

            return focusScope.MoveFocusTo(this);
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
        /// Control を更新します。
        /// </summary>
        /// <param name="gameTime"></param>
        public virtual void Update(GameTime gameTime)
        {
            // Animation を更新します。
            foreach (var animation in Animations)
            {
                if (animation.Enabled) animation.Update(gameTime);
            }

            for (int i = 0; i < ChildrenCount; i++)
            {
                var child = GetChild(i);
                child.Update(gameTime);
            }
        }

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
            // Look & Feel を描画します。
            DrawLookAndFeel(gameTime, drawContext);

            if (ClipEnabled)
            {
                // TODO: Padding の考慮が必要かも？
                using (var localClipping = drawContext.BeginClip(new Rect(RenderSize)))
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
        /// 画面座標における点を Control の座標系の点へ変換します。
        /// </summary>
        /// <param name="point">画面座標における点。</param>
        /// <returns>Control の座標系に変換された点。</returns>
        public Vector2 PointFromScreen(Vector2 point)
        {
            var offset = RenderOffset;
            var result = new Vector2(point.X - offset.X - Margin.Left, point.Y - offset.Y - Margin.Top);
            return (Parent != null) ? Parent.PointFromScreen(result) : result;
        }

        /// <summary>
        /// Control の座標系の点を画面座標における点へ変換します。
        /// </summary>
        /// <param name="point">Control の座標系における点。</param>
        /// <returns>画面座標に変換された点。</returns>
        public Vector2 PointToScreen(Vector2 point)
        {
            var offset = RenderOffset;
            var result = new Vector2(point.X + offset.X + Margin.Left, point.Y + offset.Y + Margin.Top);
            return (Parent != null) ? Parent.PointToScreen(result) : result;
        }

        /// <summary>
        /// Hit Test を行います。
        /// </summary>
        /// <param name="point">画面座標における点。</param>
        /// <returns>
        /// true (Hit した場合)、false (それ以外の場合)。
        /// </returns>
        public virtual bool HitTest(Vector2 point)
        {
            var localPoint = PointFromScreen(point);
            return (0 <= localPoint.X) && (localPoint.X < RenderSize.Width)
                && (0 <= localPoint.Y) && (localPoint.Y < RenderSize.Height);
        }

        /// <summary>
        /// 指定の Control をとして関連付けます。
        /// </summary>
        /// <param name="child">子として関連付ける Control。</param>
        protected void AddChild(Control child)
        {
            if (child.IsAncestorOf(this))
                throw new InvalidOperationException("Control can not be the descendant of one's own.");

            child.Parent = this;
        }

        /// <summary>
        /// 子 Control との関連付けを削除します。
        /// </summary>
        /// <param name="child">関連付けを削除する子 Control。</param>
        protected void RemoveChild(Control child)
        {
            if (this != child.Parent)
                throw new InvalidOperationException("This control is not a parent of the specified control.");

            if (mouseOverControl == child) mouseOverControl = this;

            child.Parent = null;
        }

        /// <summary>
        /// 子 Control を取得します。
        /// </summary>
        /// <remarks>
        /// 既定の実装では常に ArgumentOutOfRangeException を発生させます。
        /// Control に子を持たせるには、
        /// GetChild メソッドと ChildrenCount プロパティを派生クラスで適切にオーバライドします。
        /// </remarks>
        /// <param name="index">子 Control のインデックス。</param>
        /// <returns></returns>
        protected virtual Control GetChild(int index)
        {
            throw new ArgumentOutOfRangeException("index");
        }

        /// <summary>
        /// 測定します。
        /// </summary>
        /// <param name="availableSize">親が指定する利用可能なサイズ。</param>
        /// <returns>配置で希望するサイズ。</returns>
        protected virtual Size MeasureOverride(Size availableSize)
        {
            // 暫定的に Control のサイズを計算します。
            var controlSize = new Size(Width, Height);
            if (float.IsNaN(controlSize.Width))
                controlSize.Width = CalculateWidth(availableSize.Width - Margin.Left - Margin.Right);
            if (float.IsNaN(controlSize.Height))
                controlSize.Height = CalculateWidth(availableSize.Height - Margin.Top - Margin.Bottom);

            // 子の利用できるサイズは、内側の余白をとった領域のサイズです。
            var widthPadding = Padding.Left + Padding.Right;
            var heightPadding = Padding.Top + Padding.Bottom;
            var childAvailableSize = new Size
            {
                Width = controlSize.Width - widthPadding,
                Height = controlSize.Height - heightPadding
            };

            // 子の希望サイズを定めます。
            float maxChildMeasuredWidth = 0;
            float maxChildMeasuredHeight = 0;
            for (int i = 0; i < ChildrenCount; i++)
            {
                var child = GetChild(i);
                child.Measure(childAvailableSize);

                var childMeasuredSize = child.MeasuredSize;
                maxChildMeasuredWidth = Math.Max(maxChildMeasuredWidth, childMeasuredSize.Width);
                maxChildMeasuredHeight = Math.Max(maxChildMeasuredHeight, childMeasuredSize.Height);
            }

            // 幅や高さが未設定ならば子のうちの最大の値を Control のサイズにします。
            if (float.IsNaN(Width)) controlSize.Width = ClampWidth(maxChildMeasuredWidth + widthPadding);
            if (float.IsNaN(Height)) controlSize.Height = ClampHeight(maxChildMeasuredHeight + heightPadding);

            // 外側の余白を含めて描画に必要な希望サイズとします。
            return new Size
            {
                Width = controlSize.Width + Margin.Left + Margin.Right,
                Height = controlSize.Height + Margin.Top + Margin.Bottom
            };
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
            float clampedWidth = ClampWidth(availableWidth);
            // 余白で調整します。
            return MathHelper.Max(MinWidth, clampedWidth - Margin.Left - Margin.Right);
        }

        /// <summary>
        /// MinHeight、MaxHeight の関係に従い、指定された利用可能な高さから自身が希望する高さを計算します。
        /// </summary>
        /// <param name="availableHeight">利用可能な高さ。</param>
        /// <returns>自身が希望する高さ。</returns>
        protected virtual float CalculateHeight(float availableHeight)
        {
            float clampedHeight = ClampHeight(availableHeight);
            // 余白で調整します。
            return MathHelper.Max(MinHeight, clampedHeight - Margin.Top - Margin.Bottom);
        }

        /// <summary>
        /// 配置します。
        /// </summary>
        /// <param name="arrangeSize">親 Control が指定する配置に利用可能な領域。</param>
        /// <returns>配置による Control の最終的なサイズ。</returns>
        protected virtual Size ArrangeOverride(Size arrangeSize)
        {
            // デフォルトは自由配置を仮定しておきます。

            var controlSize = new Size
            {
                Width = arrangeSize.Width - Margin.Left - Margin.Right,
                Height = arrangeSize.Height - Margin.Top - Margin.Bottom
            };

            var paddedBounds = new Rect
            {
                X = Padding.Left,
                Y = Padding.Top,
                Width = controlSize.Width - Padding.Left - Padding.Right,
                Height = controlSize.Height - Padding.Top - Padding.Bottom
            };

            for (int i = 0; i < ChildrenCount; i++)
            {
                var child = GetChild(i);
                var childBounds = new Rect
                {
                    Width = child.MeasuredSize.Width,
                    Height = child.MeasuredSize.Height
                };

                switch (child.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        childBounds.X = paddedBounds.Left;
                        break;
                    case HorizontalAlignment.Right:
                        childBounds.X = paddedBounds.Right - child.MeasuredSize.Width;
                        break;
                    case HorizontalAlignment.Center:
                        childBounds.X = paddedBounds.Left + (paddedBounds.Width - child.MeasuredSize.Width) * 0.5f;
                        break;
                    case HorizontalAlignment.Stretch:
                    default:
                        childBounds.X = paddedBounds.Left;
                        childBounds.Width = paddedBounds.Width;
                        break;
                }

                switch (child.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        childBounds.Y = paddedBounds.Top;
                        break;
                    case VerticalAlignment.Bottom:
                        childBounds.Y = paddedBounds.Bottom - child.MeasuredSize.Height;
                        break;
                    case VerticalAlignment.Center:
                        childBounds.Y = paddedBounds.Top + (paddedBounds.Height - child.MeasuredSize.Height) * 0.5f;
                        break;
                    case VerticalAlignment.Stretch:
                    default:
                        childBounds.Y = paddedBounds.Top;
                        childBounds.Height = paddedBounds.Height;
                        break;
                }

                child.Arrange(childBounds);
            }

            return controlSize;
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
        /// PreviewMouseMove イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnPreviewMouseMove(ref RoutedEventContext context) { }

        /// <summary>
        /// MouseMove イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnMouseMove(ref RoutedEventContext context) { }

        /// <summary>
        /// PreviewMouseEnter イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnPreviewMouseEnter(ref RoutedEventContext context) { }

        /// <summary>
        /// MouseEnter イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnMouseEnter(ref RoutedEventContext context) { }

        /// <summary>
        /// PreviewMouseLeave イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnPreviewMouseLeave(ref RoutedEventContext context) { }

        /// <summary>
        /// MouseLeave イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnMouseLeave(ref RoutedEventContext context) { }

        /// <summary>
        /// PreviewMouseDown イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnPreviewMouseDown(ref RoutedEventContext context)
        {
            // TODO
            // ここだけ既定実装があるのは嫌だけど、
            // フォーカス設定を制御できる所がルーティング イベントしか思いつかない。

            // フォーカスを設定します。
            Focus();
        }

        /// <summary>
        /// MouseDown イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnMouseDown(ref RoutedEventContext context) { }

        /// <summary>
        /// PreviewMouseUp イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnPreviewMouseUp(ref RoutedEventContext context) { }

        /// <summary>
        /// MouseUp イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnMouseUp(ref RoutedEventContext context) { }

        /// <summary>
        /// PreviewKeyDown イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnPreviewKeyDown(ref RoutedEventContext context) { }

        /// <summary>
        /// KeyDown イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnKeyDown(ref RoutedEventContext context) { }

        /// <summary>
        /// PreviewKeyUp イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnPreviewKeyUp(ref RoutedEventContext context) { }

        /// <summary>
        /// KeyUp イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnKeyUp(ref RoutedEventContext context) { }

        /// <summary>
        /// GotFocus イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnGotFocus(ref RoutedEventContext context) { }

        /// <summary>
        /// LostFocus イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnLostFocus(ref RoutedEventContext context) { }

        /// <summary>
        /// LogicalGotFocus イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnGotLogicalFocus(ref RoutedEventContext context) { }

        /// <summary>
        /// LogicalLostFocus イベントの到達で呼び出されます。
        /// </summary>
        /// <param name="context">RoutedEventContext。</param>
        protected virtual void OnLostLogicalFocus(ref RoutedEventContext context) { }

        /// <summary>
        /// Look and Feel を描画します。
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="drawContext"></param>
        protected virtual void DrawLookAndFeel(GameTime gameTime, IDrawContext drawContext)
        {
            // Look & Feel を描画します。
            var lookAndFeel = Screen.LookAndFeelSource.GetLookAndFeel(this);
            if (lookAndFeel != null) lookAndFeel.Draw(this, drawContext);
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

            drawContext.Location = child.PointToScreen(Vector2.Zero);
            drawContext.PushOpacity(child.Opacity);

            child.Draw(gameTime, drawContext);

            drawContext.PopOpacity();
        }

        /// <summary>
        /// RoutedEventHandler を追加します。
        /// </summary>
        /// <param name="eventName">イベント名。</param>
        /// <param name="handler">イベントに追加する RoutedEventHandler。</param>
        protected void AddHandler(string eventName, RoutedEventHandler handler)
        {
            List<RoutedEventHandler> handlers;
            if (!handlerMap.TryGetValue(eventName, out handlers))
            {
                handlers = new List<RoutedEventHandler>();
                handlerMap[eventName] = handlers;
            }

            handlers.Add(handler);
        }

        /// <summary>
        /// RoutedEventHandler を削除します。
        /// </summary>
        /// <param name="eventName">イベント名。</param>
        /// <param name="handler">イベントから削除する RoutedEventHandler。</param>
        protected void RemoveHandler(string eventName, RoutedEventHandler handler)
        {
            List<RoutedEventHandler> handlers;
            if (!handlerMap.TryGetValue(eventName, out handlers))
                throw new InvalidOperationException(string.Format(
                    "Handler '{0}' could not be found.", eventName));

            handlers.Remove(handler);
        }

        /// <summary>
        /// ルーティング イベントを発生させます。
        /// </summary>
        /// <param name="tunnelEventName">Tunnel イベント名。</param>
        /// <param name="bubbleEventName">Bubble イベント名。</param>
        protected void RaiseEvent(string tunnelEventName, string bubbleEventName)
        {
            var context = new RoutedEventContext(this);

            // tunnelEventName が非 null ならば Tunnel イベントを発生させます。
            if (tunnelEventName != null)
                RaiseTunnelEvent(tunnelEventName, ref context);

            // bubbleEventName が非 null ならば Bubble イベントを発生させます。
            if (!context.Handled && bubbleEventName != null)
                RaiseBubbleEvent(bubbleEventName, ref context);
        }

        /// <summary>
        /// Tunnel イベントを発生させます。
        /// </summary>
        /// <param name="eventName">イベント名。</param>
        /// <param name="context">RoutedEventContext。</param>
        protected void RaiseTunnelEvent(string eventName, ref RoutedEventContext context)
        {
            if (Parent != null)
            {
                Parent.RaiseTunnelEvent(eventName, ref context);
                if (context.Handled) return;
            }

            InvokeHandlers(eventName, ref context);
        }

        /// <summary>
        /// Bubble イベントを発生させます。
        /// </summary>
        /// <param name="eventName">イベント名。</param>
        /// <param name="context">RoutedEventContext。</param>
        protected void RaiseBubbleEvent(string eventName, ref RoutedEventContext context)
        {
            //RaiseBubbleEvent(eventName, ref context, this);
            InvokeHandlers(eventName, ref context);

            if (context.Handled) return;

            if (Parent != null) Parent.RaiseBubbleEvent(eventName, ref context);
        }

        /// <summary>
        /// ClassRoutedEventHandler を呼び出す RoutedEventHandler を生成します。
        /// </summary>
        /// <param name="method">ClassRoutedEventHandler。</param>
        /// <returns>
        /// ClassRoutedEventHandler を呼び出す RoutedEventHandler。
        /// </returns>
        protected RoutedEventHandler CreateRoutedEventHandler(ClassRoutedEventHandler method)
        {
            return delegate(Control s, ref RoutedEventContext c)
            {
                method(ref c);
            };
        }

        /// <summary>
        /// 指定の方向にあるフォーカス設定可能な Control を取得します。
        /// そのような Control が存在しない場合には null を返します。
        /// </summary>
        /// <param name="direction">フォーカス移動方向。</param>
        /// <param name="mode">既にフォーカスが先頭あるいは末尾に到達している場合の振る舞い。</param>
        /// <returns>
        /// 指定の方向にあるフォーカス設定可能な Control。
        /// そのような Control が存在しない場合には null。
        /// </returns>
        internal Control GetFocusableControl(FocusNavigationDirection direction, FocusNavigationMode mode)
        {
            if (mode == FocusNavigationMode.None) return null;

            var focusedControl = Screen.FocusedControl;
            var baseBounds = new Rect(focusedControl.PointToScreen(Vector2.Zero), focusedControl.RenderSize);
            float minDistance = float.PositiveInfinity;

            var candidate = GetFocusableControl(direction, ref baseBounds, ref minDistance);
            if (candidate != null) return candidate;

            if (mode == FocusNavigationMode.Wrapped) return null;

            var containerBounds = new Rect(PointToScreen(Vector2.Zero), RenderSize);
            switch (direction)
            {
                case FocusNavigationDirection.Up:
                    baseBounds.Y = containerBounds.Bottom;
                    baseBounds.Height = 0;
                    break;
                case FocusNavigationDirection.Down:
                    baseBounds.Y = containerBounds.Top;
                    baseBounds.Height = 0;
                    break;
                case FocusNavigationDirection.Left:
                    baseBounds.X = containerBounds.Right;
                    baseBounds.Width = 0;
                    break;
                case FocusNavigationDirection.Right:
                    baseBounds.X = containerBounds.Left;
                    baseBounds.Width = 0;
                    break;
                default:
                    throw new InvalidOperationException();
            }
            candidate = GetFocusableControl(direction, ref baseBounds, ref minDistance);

            return candidate;
        }

        /// <summary>
        /// マウス カーソルの移動を処理します。
        /// </summary>
        internal void ProcessMouseMove()
        {
            var mouseState = Screen.MouseDevice.MouseState;

            for (int i = ChildrenCount - 1; 0 <= i; i--)
            {
                var child = GetChild(i);

                // 不可視ならばスキップします。
                if (!child.Visible) continue;

                // Hit Test に失敗するならばスキップします。
                if (!child.HitTest(new Vector2(mouseState.X, mouseState.Y)))
                    continue;

                // 子に通知します。
                child.ProcessMouseMove();

                // 子を mouseOverControl にします。
                SwitchMouseOverControl(child);
                return;
            }

            // イベントを発生させます。
            RaiseEvent(PreviewMouseMoveEvent, MouseMoveEvent);

            // mouseOverControl にできる子がないならば、自分を mouseOverControl にします。
            SwitchMouseOverControl(this);
        }

        /// <summary>
        /// マウス カーソルが領域外に移動した時に呼び出されます。
        /// </summary>
        internal void ProcessMouseLeave()
        {
            // 元々カーソルが Control 上にないならば、mouseOverControl は null です。
            if (mouseOverControl == null) return;

            if (mouseOverControl != this)
            {
                // mouseOverControl が子であるならば、子に通知します。
                mouseOverControl.ProcessMouseLeave();
            }
            else
            {
                // mouseOverControl が自分であるならば、自分に通知します。
                RaiseEvent(PreviewMouseLeaveEvent, MouseLeaveEvent);
            }

            // mouseOverControl を解除します。
            mouseOverControl = null;
        }

        /// <summary>
        /// マウス ボタンが押されたことを処理します。
        /// </summary>
        /// <returns></returns>
        internal void ProcessMouseDown()
        {
            // カーソルが Control 上にないならば、ボタンが押されたことに関知しません。
            if (mouseOverControl == null) return;

            // mouseOverControl が子であるならば、子に通知します。
            if (mouseOverControl != this)
            {
                mouseOverControl.ProcessMouseDown();
                return;
            }

            // mouseOverControl が自分であるならば、自分に通知します。
            RaiseEvent(PreviewMouseDownEvent, MouseDownEvent);
        }

        /// <summary>
        /// マウス ボタンが離されたことを処理します。
        /// </summary>
        internal void ProcessMouseUp()
        {
            // カーソルが Control 上にないならば、ボタンが離されたことに関知しません。
            if (mouseOverControl == null) return;

            // mouseOverControl が子であるならば、子に通知します。
            if (mouseOverControl != this)
            {
                mouseOverControl.ProcessMouseUp();
                return;
            }

            // mouseOverControl が自分であるならば、自分に通知します。
            RaiseEvent(PreviewMouseUpEvent, MouseUpEvent);
        }

        /// <summary>
        /// キーが押されたことを処理します。
        /// </summary>
        internal void ProcessKeyDown()
        {
            RaiseEvent(PreviewKeyDownEvent, KeyDownEvent);
        }

        /// <summary>
        /// キーが離されたことを処理します。
        /// </summary>
        internal void ProcessKeyUp()
        {
            RaiseEvent(PreviewKeyUpEvent, KeyUpEvent);
        }

        Control GetFocusableControl(FocusNavigationDirection direction, ref Rect baseBounds, ref float minDistance)
        {
            Control candidate = null;

            for (int i = 0; i < ChildrenCount; i++)
            {
                var child = GetChild(i);

                // 現在フォーカスが設定されている Control についての判定はスキップします。
                if (child == Screen.FocusedControl) continue;

                if (child.Focusable)
                {
                    var testBounds = new Rect(child.PointToScreen(Vector2.Zero), child.RenderSize);

                    var distance = MeasureDistanceSquared(direction, ref baseBounds, ref testBounds);

                    // MEMO
                    // 面倒なので少し雑に判定します。
                    // 子の時点で対象外となるならば、その子孫の判定も諦めることにします。
                    if (float.IsNaN(distance)) continue;

                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        candidate = child;
                    }

                    // MEMO
                    // フォーカス設定可能な Control に到達した場合には、
                    // その子孫にフォーカス設定可能な Control があったとしても、
                    // まずはこの子にフォーカスを設定するように仕向けます。
                }
                else
                {
                    // フォーカス設定不能であっても、その子孫について判定を進めます。
                    var descendantCandidate = child.GetFocusableControl(direction, ref baseBounds, ref minDistance);
                    if (descendantCandidate != null)
                    {
                        candidate = descendantCandidate;
                    }
                }
            }

            return candidate;
        }

        float MeasureDistanceSquared(FocusNavigationDirection direction, ref Rect baseBounds, ref Rect testBounds)
        {
            //
            // MEMO:
            //
            // Nuclex.UserInterface.Screen の判定ロジックを応用してます。
            // Nuclex とは異なり、先頭や末尾に到達した場合の循環を前提にしています。
            // また、Nuclex では Control が格子状に隣接した状態が考慮されていないため、
            // それを修正しています。
            //

            //
            // MEMO:
            //
            // Nuclex 方式の挙動は、恐らく WPF の挙動に等しいですが、
            // この挙動には問題があり、必ずしも人が期待するフォーカス移動先が決定されるとは限りません。
            // 例えば、右キー押下で右に隣接した Control への移動を期待しても、
            // 直下にある Control の判定点がそれよりもより右に近いと判定されたならば、そちらへ移動します。
            // なお、単一 StackPanel に Control を並べるような、上下左右が明確な配置の場合には期待通りとなります。
            //
            // WPF ならばタブ ナビゲーションで代替することもできますが、
            // ゲームパッド操作を考えるとこの代案にも無理があります。
            //
            // この問題は、同サイズの Control が隣接するように工夫するなどで対応できます。
            //

            var basePosition = new Vector2();
            var testPosition = new Vector2();

            if (direction == FocusNavigationDirection.Up || direction == FocusNavigationDirection.Down)
            {
                basePosition.X = baseBounds.X + baseBounds.Width * 0.5f;
                testPosition.X = Math.Min(Math.Max(basePosition.X, testBounds.Left), testBounds.Right);
                testPosition.Y = testBounds.Y + testBounds.Height * 0.5f;

                bool leavesLeft = (testPosition.X < baseBounds.Left);
                bool leavesRight = (baseBounds.Right < testPosition.X);

                if (direction == FocusNavigationDirection.Up)
                {
                    basePosition.Y = baseBounds.Top;
                    if (basePosition.Y < testPosition.Y) return float.NaN;
                }
                else
                {
                    basePosition.Y = baseBounds.Bottom;
                    if (testPosition.Y < basePosition.Y) return float.NaN;
                }

                float distanceY = Math.Abs(basePosition.Y - testPosition.Y);
                if (leavesLeft)
                {
                    float distanceX = baseBounds.Left - testPosition.X;
                    if (distanceY < distanceX) return float.NaN;
                }
                else if (leavesRight)
                {
                    float distanceX = testPosition.X - baseBounds.Right;
                    if (distanceY < distanceX) return float.NaN;
                }
            }
            else
            {
                basePosition.Y = baseBounds.Y + baseBounds.Height * 0.5f;
                testPosition.X = testBounds.X + testBounds.Width * 0.5f;
                testPosition.Y = Math.Min(Math.Max(basePosition.Y, testBounds.Top), testBounds.Bottom);

                bool leavesTop = (testPosition.Y < baseBounds.Top);
                bool leavesBottom = (baseBounds.Bottom < testPosition.Y);

                if (direction == FocusNavigationDirection.Left)
                {
                    basePosition.X = baseBounds.Left;
                    if (basePosition.X < testPosition.X) return float.NaN;
                }
                else
                {
                    basePosition.X = baseBounds.Right;
                    if (testPosition.X < basePosition.X) return float.NaN;
                }

                float distanceX = Math.Abs(basePosition.X - testPosition.X);
                if (leavesTop)
                {
                    float distanceY = baseBounds.Top - testPosition.Y;
                    if (distanceX < distanceY) return float.NaN;
                }
                else if (leavesBottom)
                {
                    float distanceY = testPosition.Y - baseBounds.Bottom;
                    if (distanceX < distanceY) return float.NaN;
                }
            }

            float distanceSquared;
            Vector2.DistanceSquared(ref basePosition, ref testPosition, out distanceSquared);
            return distanceSquared;
        }

        /// <summary>
        /// mouseOverControl を newControl に設定します。
        /// </summary>
        /// <param name="newControl">Control。</param>
        void SwitchMouseOverControl(Control newControl)
        {
            if (mouseOverControl == newControl) return;

            // これまでの mouseOverControl へ変更を通知します。
            if (mouseOverControl != null) mouseOverControl.ProcessMouseLeave();

            // 新たな mouseOverControl を設定して変更を通知します。
            mouseOverControl = newControl;
            mouseOverControl.RaiseEvent(PreviewMouseEnterEvent, MouseEnterEvent);
        }

        /// <summary>
        /// 子 Control を描画します。
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="drawContext"></param>
        void DrawChildren(GameTime gameTime, IDrawContext drawContext)
        {
            for (int i = 0; i < ChildrenCount; i++)
            {
                var child = GetChild(i);
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

        /// <summary>
        /// RoutedEventHandler を呼び出します。
        /// </summary>
        /// <param name="eventName">イベント名。</param>
        /// <param name="context">RoutedEventContext。</param>
        void InvokeHandlers(string eventName, ref RoutedEventContext context)
        {
            List<RoutedEventHandler> handlers;
            if (handlerMap.TryGetValue(eventName, out handlers))
            {
                foreach (RoutedEventHandler handler in handlers)
                {
                    handler(this, ref context);
                    if (context.Handled) return;
                }
            }
        }
    }
}
