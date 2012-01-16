#region

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    /// <summary>
    /// 入力デバイスのインタフェースです。
    /// </summary>
    public interface IInputDevice
    {
        /// <summary>
        /// デバイスが有効かどうか。
        /// </summary>
        /// <value>true (デバイスが有効な場合)、false (それ以外の場合)。</value>
        bool Enabled { get; }

        /// <summary>
        /// 人がデバイスを識別するための名前。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// デバイスの状態を更新します。
        /// </summary>
        void Update();
    }
}
