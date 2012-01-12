#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Debug
{
    /// <summary>
    /// Time Ruler サービスのインタフェースです。
    /// </summary>
    public interface ITimeRulerService
    {
        /// <summary>
        /// TimeRulerMarker を作成します。
        /// </summary>
        /// <returns>作成された TimeRulerMarker。</returns>
        TimeRulerMarker CreateMarker();
        
        /// <summary>
        /// TimeRulerMarker を破棄します。
        /// </summary>
        /// <param name="marker">破棄する TimeRulerMarker。</param>
        void ReleaseMarker(TimeRulerMarker marker);

        /// <summary>
        /// フレームの開始を Time Ruler へ通知します。
        /// </summary>
        void StartFrame();
    }
}
