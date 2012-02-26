#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Window を表す Control です。
    /// </summary>
    public class Window : ContentControl, IFocusScopeControl
    {
        /// <summary>
        /// Window が閉じる前に発生します。
        /// </summary>
        public event EventHandler Closing = delegate { };

        /// <summary>
        /// Window が閉じた後に発生します。
        /// </summary>
        public event EventHandler Closed = delegate { };

        /// <summary>
        /// アクティブ化された時に発生します。
        /// </summary>
        public event EventHandler Activated = delegate { };

        /// <summary>
        /// 非アクティブ化された時に発生します。
        /// </summary>
        public event EventHandler Deactivated = delegate { };

        /// <summary>
        /// この Window を所有する Window。
        /// </summary>
        Window owner;

        /// <summary>
        /// タイトル Control。
        /// </summary>
        Control titleContent;

        /// <summary>
        /// true (アクティブ化されている場合)、false (それ以外の場合)。
        /// </summary>
        bool active;

        // I/F
        public FocusScope FocusScope { get; private set; }

        public float Left
        {
            get { return Margin.Left; }
            set
            {
                var m = Margin;
                m.Left = value;
                Margin = m;
            }
        }

        public float Top
        {
            get { return Margin.Top; }
            set
            {
                var m = Margin;
                m.Top = value;
                Margin = m;
            }
        }

        /// <summary>
        /// タイトル Control を取得または設定します。
        /// </summary>
        public Control TitleContent
        {
            get { return titleContent; }
            set
            {
                if (titleContent == value) return;

                if (titleContent != null) RemoveChild(titleContent);

                titleContent = value;

                if (titleContent != null) AddChild(titleContent);
            }
        }

        /// <summary>
        /// この Window を所有する Window を取得あるいは設定します。
        /// </summary>
        /// <remarks>
        /// Window は、Owner で設定した Window が閉じられると自動的に閉じます。
        /// </remarks>
        public Window Owner
        {
            get { return owner; }
            set
            {
                if (owner == value) return;

                if (owner != null) owner.Closed -= new EventHandler(OnOwnerClosed);

                owner = value;

                if (owner != null) owner.Closed += new EventHandler(OnOwnerClosed);
            }
        }

        /// <summary>
        /// アクティブ化されているかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (アクティブ化されている場合)、false (それ以外の場合)。
        /// </value>
        public bool Active
        {
            get { return active; }
            internal set
            {
                if (active == value) return;

                active = value;

                if (active)
                {
                    OnActivated();
                }
                else
                {
                    OnDeactivated();
                }
            }
        }

        protected override int ChildrenCount
        {
            get
            {
                return (TitleContent != null) ? base.ChildrenCount + 1 : base.ChildrenCount;
            }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        /// <param name="screen">Screen。</param>
        public Window(Screen screen)
            : base(screen)
        {
            FocusScope = new FocusScope(this);
        }

        /// <summary>
        /// 指定の Control の親 Window を取得します。
        /// Window を指定した場合には、それをそのまま返します。
        /// </summary>
        /// <param name="control">Control。</param>
        /// <returns>
        /// 指定の Control を管理している親  Window。
        /// Window に配置されていない Control の場合には null。
        /// </returns>
        public static Window GetWindow(Control control)
        {
            var window = control as Window;
            if (window != null) return window;

            if (control.Parent == null) return null;

            return GetWindow(control.Parent);
        }

        /// <summary>
        /// Window を表示します。
        /// 新たに追加する Window の場合には最前面に Window が追加されます。
        /// 既に存在する Window の場合には最前面へ移動されます。
        /// Active プロパティおよび Visible プロパティが true に設定されます。
        /// </summary>
        public virtual void Show()
        {
            Screen.ShowWindow(this);
        }

        /// <summary>
        /// Window を非表示にします。
        /// Active プロパティおよび Visible プロパティが false に設定されます。
        /// </summary>
        public virtual void Hide()
        {
            Screen.HideWindow(this);
        }

        /// <summary>
        /// Window を閉じます。
        /// </summary>
        public void Close()
        {
            // Closing イベントを発生させます。
            OnClosing();

            Screen.CloseWindow(this);

            // Closed イベントを発生させます。
            OnClosed();
        }

        /// <summary>
        /// アクティブ化します。
        /// Active プロパティおよび Visible プロパティが true に設定されます。
        /// </summary>
        public void Activate()
        {
            Screen.ActivateWindow(this);
        }

        /// <summary>
        /// SizeToContent に Width が設定された場合の測定を行います。
        /// </summary>
        /// <param name="availableSize">親が指定する利用可能なサイズ。</param>
        /// <returns>配置で希望するサイズ。</returns>
        protected virtual Size MeasureWidthToContent(Size availableSize)
        {
            var windowSize = new Size
            {
                Width = availableSize.Width,
                Height = Height
            };

            var titleMeasuredSize = new Size();
            if (titleContent != null)
            {
                titleContent.Measure(windowSize);

                titleMeasuredSize = titleContent.MeasuredSize;
            }

            var contentMeasuredSize = new Size();
            if (Content != null)
            {
                // Content の利用できるサイズは、タイトルの高さ、および、内側の余白をとった領域のサイズです。
                var widthPadding = Padding.Left + Padding.Right;
                var heightPadding = Padding.Top + Padding.Bottom;
                var childAvailableSize = new Size
                {
                    Width = windowSize.Width - widthPadding,
                    Height = windowSize.Height - titleMeasuredSize.Height - heightPadding
                };

                Content.Measure(childAvailableSize);

                contentMeasuredSize = Content.MeasuredSize;
            }

            windowSize.Width = Math.Max(titleMeasuredSize.Width, contentMeasuredSize.Width + Padding.Left + Padding.Right);

            return new Size
            {
                Width = windowSize.Width + Margin.Left + Margin.Right,
                Height = windowSize.Height + Margin.Top + Margin.Bottom
            };
        }

        /// <summary>
        /// SizeToContent に Height が設定された場合の測定を行います。
        /// </summary>
        /// <param name="availableSize">親が指定する利用可能なサイズ。</param>
        /// <returns>配置で希望するサイズ。</returns>
        protected virtual Size MeasureHeightToContent(Size availableSize)
        {
            // 暫定的に Control のサイズを計算します。
            var windowSize = new Size
            {
                Width = Width,
                Height = availableSize.Height - Margin.Top - Margin.Bottom
            };

            var titleMeasuredSize = new Size();
            if (titleContent != null)
            {
                titleContent.Measure(windowSize);

                titleMeasuredSize = titleContent.MeasuredSize;
            }

            var contentMeasuredSize = new Size();
            if (Content != null)
            {
                // Content の利用できるサイズは、タイトルの高さ、および、内側の余白をとった領域のサイズです。
                var widthPadding = Padding.Left + Padding.Right;
                var heightPadding = Padding.Top + Padding.Bottom;
                var childAvailableSize = new Size
                {
                    Width = windowSize.Width - widthPadding,
                    Height = windowSize.Height - titleMeasuredSize.Height - heightPadding
                };

                Content.Measure(childAvailableSize);

                contentMeasuredSize = Content.MeasuredSize;
            }

            windowSize.Height = titleMeasuredSize.Height + contentMeasuredSize.Height + Padding.Top + Padding.Bottom;

            return new Size
            {
                Width = windowSize.Width + Margin.Left + Margin.Right,
                Height = windowSize.Height + Margin.Top + Margin.Bottom
            };
        }

        /// <summary>
        /// SizeToContent に WidthAndHeight が設定された場合の測定を行います。
        /// </summary>
        /// <param name="availableSize">親が指定する利用可能なサイズ。</param>
        /// <returns>配置で希望するサイズ。</returns>
        protected virtual Size MeasureWidthAndHeightToContent(Size availableSize)
        {
            // 暫定的に Control のサイズを計算します。
            var windowSize = new Size
            {
                Width = availableSize.Width - Margin.Left + Margin.Right,
                Height = availableSize.Height - Margin.Top - Margin.Bottom
            };

            var titleMeasuredSize = new Size();
            if (titleContent != null)
            {
                titleContent.Measure(windowSize);

                titleMeasuredSize = titleContent.MeasuredSize;
            }

            var contentMeasuredSize = new Size();
            if (Content != null)
            {
                // Content の利用できるサイズは、タイトルの高さ、および、内側の余白をとった領域のサイズです。
                var widthPadding = Padding.Left + Padding.Right;
                var heightPadding = Padding.Top + Padding.Bottom;
                var childAvailableSize = new Size
                {
                    Width = windowSize.Width - widthPadding,
                    Height = windowSize.Height - titleMeasuredSize.Height - heightPadding
                };

                Content.Measure(childAvailableSize);

                contentMeasuredSize = Content.MeasuredSize;
            }

            windowSize.Width = Math.Max(titleMeasuredSize.Width, contentMeasuredSize.Width + Padding.Left + Padding.Right);
            windowSize.Height = titleMeasuredSize.Height + contentMeasuredSize.Height + Padding.Top + Padding.Bottom;

            return new Size
            {
                Width = windowSize.Width + Margin.Left + Margin.Right,
                Height = windowSize.Height + Margin.Top + Margin.Bottom
            };
        }

        /// <summary>
        /// SizeToContent に Manual が設定された場合の測定を行います。
        /// </summary>
        /// <param name="availableSize">親が指定する利用可能なサイズ。</param>
        /// <returns>配置で希望するサイズ。</returns>
        protected virtual Size MeasureManual(Size availableSize)
        {
            // Control のサイズは Width と Height で確定しています。
            var windowSize = new Size(Width, Height);

            var titleMeasuredSize = new Size();
            if (titleContent != null)
            {
                titleContent.Measure(windowSize);

                titleMeasuredSize = titleContent.MeasuredSize;
            }

            if (Content != null)
            {
                // Content の利用できるサイズは、タイトルの高さ、および、内側の余白をとった領域のサイズです。
                var widthPadding = Padding.Left + Padding.Right;
                var heightPadding = Padding.Top + Padding.Bottom;
                var childAvailableSize = new Size
                {
                    Width = windowSize.Width - widthPadding,
                    Height = windowSize.Height - titleMeasuredSize.Height - heightPadding
                };

                Content.Measure(childAvailableSize);
            }

            return new Size
            {
                Width = windowSize.Width + Margin.Left + Margin.Right,
                Height = windowSize.Height + Margin.Top + Margin.Bottom
            };
        }

        /// <summary>
        /// Window を閉じる直前に呼び出されます。
        /// Closing イベントを発生させます。
        /// </summary>
        protected virtual void OnClosing()
        {
            if (Closing != null) Closing(this, EventArgs.Empty);
        }

        /// <summary>
        /// Window を閉じた直後に呼び出されます。
        /// Closed イベントを発生させます。
        /// </summary>
        protected virtual void OnClosed()
        {
            if (Closed != null) Closed(this, EventArgs.Empty);
        }

        /// <summary>
        /// アクティブ化された時に呼び出されます。
        /// Activated イベントを発生させます。
        /// </summary>
        protected virtual void OnActivated()
        {
            if (Activated != null) Activated(this, EventArgs.Empty);
        }

        /// <summary>
        /// 非アクティブ化された時に呼び出されます。
        /// Deactivated イベントを発生させます。
        /// </summary>
        protected virtual void OnDeactivated()
        {
            if (Deactivated != null) Deactivated(this, EventArgs.Empty);
        }

        protected override Control GetChild(int index)
        {
            if (TitleContent != null)
            {
                if (index < 0 || 1 < index) throw new ArgumentOutOfRangeException("index");

                if (index == 0) return TitleContent;

                if (Content == null) throw new ArgumentOutOfRangeException("index");
                return Content;
            }

            return base.GetChild(index);
        }

        protected override void OnKeyDown(ref RoutedEventContext context)
        {
            base.OnKeyDown(ref context);

            if (context.Handled) return;

            // フォーカス移動を処理します。
            MoveFocus();
        }

        protected override Size MeasureOverride(Size availableSize)
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

                maxChildMeasuredWidth = Math.Max(maxChildMeasuredWidth, child.MeasuredSize.Width);
                maxChildMeasuredHeight += child.MeasuredSize.Height;
            }

            if (float.IsNaN(Width)) controlSize.Width = ClampWidth(maxChildMeasuredWidth + widthPadding);
            if (float.IsNaN(Height)) controlSize.Height = ClampHeight(maxChildMeasuredHeight + heightPadding);

            // 外側の余白を含めて描画に必要な希望サイズとします。
            return new Size
            {
                Width = controlSize.Width + Margin.Left + Margin.Right,
                Height = controlSize.Height + Margin.Top + Margin.Bottom
            };
        }

        protected override Size ArrangeOverride(Size arrangeSize)
        {
            var controlSize = new Size
            {
                Width = arrangeSize.Width - Margin.Left - Margin.Right,
                Height = arrangeSize.Height - Margin.Top - Margin.Bottom
            };

            var paddedBounds = new Rect
            {
                X = Padding.Left,
                Y = Padding.Top,
                Width = controlSize.Width - Padding.Left + Padding.Right,
                Height = controlSize.Height - Padding.Top + Padding.Bottom
            };

            if (titleContent != null)
            {
                // TitleContent は常に上部に張り付きます。
                var childBounds = new Rect
                {
                    Y = paddedBounds.Top,
                    Width = titleContent.MeasuredSize.Width,
                    Height = titleContent.MeasuredSize.Height
                };

                switch (titleContent.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        childBounds.X = paddedBounds.Left;
                        break;
                    case HorizontalAlignment.Right:
                        childBounds.X = paddedBounds.Right - titleContent.MeasuredSize.Width;
                        break;
                    case HorizontalAlignment.Center:
                        childBounds.X = paddedBounds.Left + (paddedBounds.Width - titleContent.MeasuredSize.Width) * 0.5f;
                        break;
                    case HorizontalAlignment.Stretch:
                    default:
                        childBounds.X = paddedBounds.Left;
                        childBounds.Width = paddedBounds.Width;
                        break;
                }

                titleContent.Arrange(childBounds);

                // Content の配置に備えて、TitleContent の領域分を取り除きます。
                paddedBounds.Y = childBounds.Bottom;
                paddedBounds.Height -= childBounds.Height;
            }

            if (Content != null)
            {
                // Content は TitleContent を除いた領域で配置します。
                var childBounds = new Rect
                {
                    Width = Content.MeasuredSize.Width,
                    Height = Content.MeasuredSize.Height
                };

                switch (Content.HorizontalAlignment)
                {
                    case HorizontalAlignment.Left:
                        childBounds.X = paddedBounds.Left;
                        break;
                    case HorizontalAlignment.Right:
                        childBounds.X = paddedBounds.Right - Content.MeasuredSize.Width;
                        break;
                    case HorizontalAlignment.Center:
                        childBounds.X = paddedBounds.Left + (paddedBounds.Width - Content.MeasuredSize.Width) * 0.5f;
                        break;
                    case HorizontalAlignment.Stretch:
                    default:
                        childBounds.X = paddedBounds.Left;
                        childBounds.Width = paddedBounds.Width;
                        break;
                }

                switch (Content.VerticalAlignment)
                {
                    case VerticalAlignment.Top:
                        childBounds.Y = paddedBounds.Top;
                        break;
                    case VerticalAlignment.Bottom:
                        childBounds.Y = paddedBounds.Bottom - Content.MeasuredSize.Height;
                        break;
                    case VerticalAlignment.Center:
                        childBounds.Y = paddedBounds.Top + (paddedBounds.Height - Content.MeasuredSize.Height) * 0.5f;
                        break;
                    case VerticalAlignment.Stretch:
                    default:
                        childBounds.Y = paddedBounds.Top;
                        childBounds.Height = paddedBounds.Height;
                        break;
                }

                Content.Arrange(childBounds);
            }

            return controlSize;
        }

        /// <summary>
        /// 押されたキーに応じてフォーカスの移動を試みます。
        /// </summary>
        void MoveFocus()
        {
            if (Screen.FocusedControl == null) return;

            var focusScope = FocusScope.GetFocusScope(Screen.FocusedControl);
            if (focusScope == null) return;

            if (Screen.KeyboardDevice.IsKeyPressed(Keys.Up))
            {
                focusScope.MoveFocus(FocusNavigationDirection.Up);
            }
            else if (Screen.KeyboardDevice.IsKeyPressed(Keys.Down))
            {
                focusScope.MoveFocus(FocusNavigationDirection.Down);
            }
            else if (Screen.KeyboardDevice.IsKeyPressed(Keys.Left))
            {
                focusScope.MoveFocus(FocusNavigationDirection.Left);
            }
            else if (Screen.KeyboardDevice.IsKeyPressed(Keys.Right))
            {
                focusScope.MoveFocus(FocusNavigationDirection.Right);
            }
        }

        /// <summary>
        /// この Window を所有する Window が閉じられた時に呼び出され、この Window を閉じます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnOwnerClosed(object sender, EventArgs e)
        {
            Close();
        }
    }
}
