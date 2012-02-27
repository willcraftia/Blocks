#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// スプライト イメージの配置場所を管理するクラスへのインタフェースです。
    /// </summary>
    public interface ISpriteSheetTemplate
    {
        /// <summary>
        /// 指定の ID に対するスプライト イメージの配置場所を取得します。
        /// </summary>
        /// <param name="id">スプライト イメージの ID。</param>
        /// <returns>スプライト イメージの配置場所。</returns>
        Rectangle this[string id] { get; }
    }
}
