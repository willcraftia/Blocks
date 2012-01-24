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
        Point Location { get; set; }

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
        /// <param name="scissorRectangle">クリップ領域。</param>
        /// <returns>クリップを制御するオブジェクト。</returns>
        IDisposable SetClip(Rect clipBounds);

        /// <summary>
        /// 新たなクリップ領域でクリップを解します。
        /// </summary>
        /// <param name="scissorRectangle">クリップ領域。</param>
        /// <returns>クリップを制御するオブジェクト。</returns>
        IDisposable SetNewClip(Rect clipBounds);

        /// <summary>
        /// Viewport の変更を開始します。
        /// </summary>
        /// <param name="viewportBounds">Viewport 領域。</param>
        /// <returns>Viewport の変更を管理するオブジェクト。</returns>
        IDisposable SetViewport(Rect viewportBounds);

        /// <summary>
        /// 指定の Control のための IControlLaf を取得します。
        /// </summary>
        /// <param name="control">Control。</param>
        /// <returns>
        /// 指定の Control のための IControlLaf。存在しない場合は null を返します。
        /// </returns>
        IControlLaf GetControlLaf(Control control);

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
    }
}
