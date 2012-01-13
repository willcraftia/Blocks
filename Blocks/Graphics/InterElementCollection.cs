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
    }
}
