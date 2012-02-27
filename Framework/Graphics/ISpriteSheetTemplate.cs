#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// SpriteSheet の配置を管理するクラスへのインタフェースです。
    /// </summary>
    public interface ISpriteSheetTemplate
    {
        /// <summary>
        /// 指定の ID に対する領域を取得します。
        /// </summary>
        /// <param name="id">ID。</param>
        /// <returns>指定の ID に対する領域。</returns>
        Rectangle this[string id] { get; }
    }
}
