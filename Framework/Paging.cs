#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework
{
    /// <summary>
    /// ページング処理を支援するクラスです。
    /// このクラスでは複雑な制御は行いません。
    /// 総項目数と 1 ページに含める項目数の関係からページ数を算出し、
    /// 現在ページがその範囲内でめくられるように管理するのみです。
    /// </summary>
    public sealed class Paging
    {
        /// <summary>
        /// 総項目数。
        /// </summary>
        int itemCount;

        /// <summary>
        /// 現在ページ。
        /// </summary>
        int currentPageIndex;

        /// <summary>
        /// 1 ページに含める項目数を取得します。
        /// </summary>
        public int ItemCountPerPage { get; private set; }

        /// <summary>
        /// 総項目数を取得または設定します。
        /// 総項目数の変化により、現在ページの位置が最終ページを越えるようであれば、
        /// 現在ページの位置を最終ページにするように調整されます。
        /// </summary>
        public int ItemCount
        {
            get { return itemCount; }
            set
            {
                if (value < 0) throw new ArgumentOutOfRangeException("value");
                if (itemCount == value) return;

                itemCount = value;

                if (PageCount - 1 < currentPageIndex) CurrentPageIndex = PageCount - 1;
            }
        }

        /// <summary>
        /// 現在ページを取得または設定します。
        /// ページ範囲を超えた値を設定すると例外が発生します。
        /// このため、このプロパティを設定する場合には、
        /// 先に適切な値を ItemCount プロパティに設定し、
        /// PageCount プロパティを確定させる必要があります。
        /// </summary>
        public int CurrentPageIndex
        {
            get { return currentPageIndex; }
            set
            {
                if (value < 0 || PageCount < value)
                    throw new ArgumentOutOfRangeException("value");

                currentPageIndex = value;
            }
        }

        /// <summary>
        /// 先頭あるいは末尾ページに到達した場合に、
        /// 末尾あるいは先頭へ現在ページを移動させられるかどうかを示す値を取得または設定します。
        /// </summary>
        public bool Cycled { get; set; }

        /// <summary>
        /// ページ数を取得します。
        /// </summary>
        public int PageCount
        {
            get
            {
                var pageCount = itemCount / ItemCountPerPage;
                if (itemCount % ItemCountPerPage != 0) pageCount++;
                return pageCount;
            }
        }

        /// <summary>
        /// 次ページへ移動させられるかどうかを示す値を取得します。
        /// </summary>
        public bool CanForward
        {
            get
            {
                if (Cycled) return true;
                return currentPageIndex < PageCount - 1;
            }
        }

        /// <summary>
        /// 前ページへ移動させられるかどうかを示す値を取得します。
        /// </summary>
        public bool CanBack
        {
            get
            {
                if (Cycled) return true;
                return 0 < currentPageIndex;
            }
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="itemCountPerPage">1 ページに含める項目数。</param>
        public Paging(int itemCountPerPage)
        {
            if (itemCountPerPage < 0) throw new ArgumentOutOfRangeException("itemCountPerPage");
            ItemCountPerPage = itemCountPerPage;

            Cycled = true;
        }

        /// <summary>
        /// ページ内でのインデックスから、項目全体でのインデックを取得します。
        /// </summary>
        /// <param name="indexInPage">ページ内でのインデックス。</param>
        /// <returns>項目全体でのインデックス。</returns>
        public int GetItemIndex(int indexInPage)
        {
            return currentPageIndex * ItemCountPerPage + indexInPage;
        }

        /// <summary>
        /// 現在ページを次のページヘ進めます。
        /// </summary>
        public void Forward()
        {
            var newPageIndex = currentPageIndex;

            newPageIndex++;
            // 末尾を越える場合
            if (PageCount - 1 < newPageIndex) newPageIndex = Cycled ? 0 : PageCount - 1;

            CurrentPageIndex = newPageIndex;
        }

        /// <summary>
        /// 現在ページを前のページヘ戻します。
        /// </summary>
        public void Back()
        {
            var newPageIndex = currentPageIndex;

            newPageIndex--;
            // 先頭を越える場合
            if (newPageIndex < 0) newPageIndex = Cycled ? PageCount - 1 : 0;

            CurrentPageIndex = newPageIndex;
        }
    }
}
