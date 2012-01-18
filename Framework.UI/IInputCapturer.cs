#region Using

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// キャプチャした入力を受信者へ通知するクラスのインタフェースです。
    /// キャプチャした入力の全てが通知されるわけではなく、どれを通知するかは実装クラスが決定します。
    /// </summary>
    public interface IInputCapturer
    {
        /// <summary>
        /// キャプチャした入力の通知先となる IInputReceiver を取得または設定します。
        /// </summary>
        IInputReceiver InputReceiver { get; set; }
    }
}
