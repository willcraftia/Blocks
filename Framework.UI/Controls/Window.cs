#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    /// <summary>
    /// Window を表す Control です。
    /// </summary>
    public class Window : Control
    {
        /// <summary>
        /// Window が閉じる前に発生します。
        /// </summary>
        public event EventHandler Closing;

        /// <summary>
        /// Window が閉じた後に発生します。
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// この Window を所有する Window。
        /// </summary>
        Window owner;

        public SizeToContent SizeToContent { get; set; }

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

                if (owner != null)
                {
                    owner.Closing -= new EventHandler(OnOwnerClosing);
                }

                owner = value;

                if (owner != null)
                {
                    owner.Closing += new EventHandler(OnOwnerClosing);
                }
            }
        }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Window() : base(true)
        {
            SizeToContent = SizeToContent.Manual;
        }

        /// <summary>
        /// Window を表示します。
        /// </summary>
        /// <param name="screen"></param>
        public void Show(Screen screen)
        {
            // Screen へ登録します。
            screen.Children.Add(this);
        }

        /// <summary>
        /// Window を閉じます。
        /// </summary>
        public void Close()
        {
            // Closing イベントを発生させます。
            RaiseClosing();
            // Screen から登録を解除します。
            Parent.Children.Remove(this);
            // Closed イベントを発生させます。
            RaiseClosed();
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            switch (SizeToContent)
            {
                case SizeToContent.Width:
                    {
                        return MeasureWidthToContent(availableSize);
                    }
                case SizeToContent.Height:
                    {
                        return MeasureHeightToContent(availableSize);
                    }
                case SizeToContent.WidthAndHeight:
                    {
                        return MeasureWidthAndHeightToContent(availableSize);
                    }
                case SizeToContent.Manual:
                default:
                    {
                        return base.MeasureOverride(availableSize);
                    }
            }
        }

        Size MeasureWidthToContent(Size availableSize)
        {
            var size = availableSize;

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
                var margin = Margin;
                size.Height = MathHelper.Max(MinHeight, size.Height - margin.Top - margin.Bottom);
            }
            else
            {
                // 高さが設定されているならばそのまま希望します。
                size.Height = Height;
            }

            // 一旦、自分が希望するサイズで子の希望サイズを定めます。
            var finalSize = new Size(0, size.Height);
            foreach (var child in Children)
            {
                // 自身の希望サイズを測定したので、子が測定済かどうかによらず再測定します。
                child.Measure(size);
                var childMeasuredSize = child.MeasuredSize;
                var childMargin = child.Margin;
                finalSize.Width += childMeasuredSize.Width + childMargin.Left + childMargin.Right;
            }

            return finalSize;
        }

        Size MeasureHeightToContent(Size availableSize)
        {
            var size = availableSize;

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
                var margin = Margin;
                size.Width = MathHelper.Max(MinWidth, size.Width - margin.Left - margin.Right);
            }
            else
            {
                // 幅が設定されているならばそのまま希望します。
                size.Width = Width;
            }

            // 一旦、自分が希望するサイズで子の希望サイズを定めます。
            var finalSize = new Size(size.Width, 0);
            foreach (var child in Children)
            {
                // 自身の希望サイズを測定したので、子が測定済かどうかによらず再測定します。
                child.Measure(size);
                var childMeasuredSize = child.MeasuredSize;
                var childMargin = child.Margin;
                finalSize.Height += childMeasuredSize.Height + childMargin.Top + childMargin.Bottom;
            }

            return finalSize;
        }

        Size MeasureWidthAndHeightToContent(Size availableSize)
        {
            var size = availableSize;

            // 一旦、自分が希望するサイズで子の希望サイズを定めます。
            var finalSize = new Size(0, 0);
            foreach (var child in Children)
            {
                // 自身の希望サイズを測定したので、子が測定済かどうかによらず再測定します。
                child.Measure(size);
                var childMeasuredSize = child.MeasuredSize;
                var childMargin = child.Margin;
                finalSize.Width += childMeasuredSize.Width + childMargin.Left + childMargin.Right;
                finalSize.Height += childMeasuredSize.Height + childMargin.Top + childMargin.Bottom;
            }

            return finalSize;
        }

        /// <summary>
        /// この Window を所有する Window が閉じられる前に呼び出され、この Window を閉じます。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnOwnerClosing(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// Closing イベントを発生させます。
        /// </summary>
        void RaiseClosing()
        {
            if (Closing != null) Closing(this, EventArgs.Empty);
        }

        /// <summary>
        /// Closed イベントを発生させます。
        /// </summary>
        void RaiseClosed()
        {
            if (Closed != null) Closed(this, EventArgs.Empty);
        }
    }
}
