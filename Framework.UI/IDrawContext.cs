#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Control の描画処理へのアクセスを提供するインタフェースです。
    /// </summary>
    public interface IDrawContext
    {
        /// <summary>
        /// Control の描画に使用する SpriteBatch を取得します。
        /// </summary>
        SpriteBatch SpriteBatch { get; }

        /// <summary>
        /// 使用する座標系の画面座標における位置を設定します。
        /// </summary>
        Vector2 Location { get; set; }

        /// <summary>
        /// 描画処理が指定する Control の透明度を取得します。
        /// </summary>
        float Opacity { get; }

        /// <summary>
        /// 透明度をスタックへプッシュします。
        /// </summary>
        /// <param name="opacity">スタックへプッシュする透明度。</param>
        void PushOpacity(float opacity);

        /// <summary>
        /// 透明度をスタックからポップします。
        /// </summary>
        void PopOpacity();

        /// <summary>
        /// 既存のクリップ領域を継承してクリップを開始します。
        /// </summary>
        /// <param name="clipBounds">クリップ領域。</param>
        /// <returns>クリップを制御するオブジェクト。</returns>
        IDisposable BeginClip(Rect clipBounds);

        /// <summary>
        /// 新たなクリップ領域でクリップを開始します。
        /// </summary>
        /// <param name="clipBounds">クリップ領域。</param>
        /// <returns>クリップを制御するオブジェクト。</returns>
        IDisposable BeginNewClip(Rect clipBounds);

        /// <summary>
        /// Viewport の変更を開始します。
        /// </summary>
        /// <param name="viewportBounds">Viewport 領域。</param>
        /// <returns>Viewport の変更を管理するオブジェクト。</returns>
        IDisposable BeginViewport(Rect viewportBounds);

        IDisposable BeginDraw3D();

        /// <summary>
        /// Control の描画処理を反映させます。
        /// </summary>
        void Flush();

        // todo:
        void DrawRectangle(Rect rect, Color color);

        // todo:
        void DrawTexture(Rect rect, Texture2D texture, Color color);

        // todo:
        void DrawTexture(Rect rect, Texture2D texture, Color color, Rectangle sourceRectangle);

        // todo:
        void DrawString(Rect clientBounds, SpriteFont font, string text, Vector2 stretch,
            HorizontalAlignment hAlign, VerticalAlignment vAlign, Color color, Thickness padding);

        // todo:
        void DrawString(Rect clientBounds, SpriteFont font, string text, Vector2 stretch,
            HorizontalAlignment hAlign, VerticalAlignment vAlign, Color color, Thickness padding, Vector2 offset);
    }
}
