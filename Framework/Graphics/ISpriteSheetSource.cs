#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// ID で管理される SpriteSheet を提供するクラスへのインタフェースです。
    /// </summary>
    public interface ISpriteSheetSource : IDisposable
    {
        /// <summary>
        /// Initialize メソッドが呼び出されたかどうかを示す値を取得します。
        /// </summary>
        /// <value>
        /// true (Initialize メソッドが呼び出された場合)、false (それ以外の場合)。
        /// </value>
        bool Initialized { get; }

        /// <summary>
        /// 初期化します。
        /// </summary>
        void Initialize();

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
