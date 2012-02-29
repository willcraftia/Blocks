#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// ID で管理される SpriteSheet を提供するクラスへのインタフェースです。
    /// </summary>
    public interface ISpriteSheetSource
    {
        /// <summary>
        /// 指定の ID に対応する SpriteSheet を取得します。
        /// そのような SpriteSheet が存在しない場合は null を返します。
        /// </summary>
        /// <param name="id">SpriteSheet の ID。</param>
        /// <returns>
        /// 指定の ID に対応する SpriteSheet。
        /// そのような SpriteSheet が存在しない場合は null。
        /// </returns>
        SpriteSheet GetSpriteSheet(string id);
    }
}
