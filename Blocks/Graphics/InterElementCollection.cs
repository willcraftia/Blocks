#region Using

using System;
using System.Collections.ObjectModel;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics
{
    /// <summary>
    /// グリッド内位置をキーとして Element を管理するコレクションです。
    /// </summary>
    public sealed class InterElementCollection : KeyedCollection<Position, Element>
    {
        protected override Position GetKeyForItem(Element item)
        {
            return item.Position;
        }

        /// <summary>
        /// 指定したキーに関連付けられている要素を取得します。
        /// </summary>
        /// <param name="key">取得する要素のキー。</param>
        /// <param name="item">
        /// このメソッドから制御が戻るときに、キーが見つかった場合は、指定したキーに関連付けられている値が格納されます。
        /// それ以外の場合は null が格納されます。
        /// </param>
        public void TryGetItem(Position key, out Element item)
        {
            item = Contains(key) ? this[key] : null;
        }
    }
}
