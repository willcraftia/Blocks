#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Debug
{
    /// <summary>
    /// Time Ruler の計測要素を定義するクラスです。
    /// </summary>
    public sealed class TimeRulerMarker
    {
        /// <summary>
        /// TimeRuler。
        /// </summary>
        TimeRuler timeRuler;

        /// <summary>
        /// TimeRuler に割り当てられた ID を取得します。
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// 名前を取得または設定します。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 関連付ける Bar のインデックスを取得または設定します。
        /// </summary>
        public int BarIndex { get; set; }

        /// <summary>
        /// 色を取得または設定します。
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="timeRuler">TimeRuler。</param>
        /// <param name="id">TimeRuler に割り当てられた ID。</param>
        internal TimeRulerMarker(TimeRuler timeRuler, int id)
        {
            this.timeRuler = timeRuler;
            Id = id;
        }

        /// <summary>
        /// 計測を開始します。
        /// </summary>
        public void Begin()
        {
            timeRuler.Begin(this);
        }

        /// <summary>
        /// 計測を終了します。
        /// </summary>
        public void End()
        {
            timeRuler.End(this);
        }
    }
}
