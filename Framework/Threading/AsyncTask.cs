#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Threading
{
    /// <summary>
    /// 非同期に処理したいメソッドとコールバックの組を表します。
    /// </summary>
    public struct AsyncTask
    {
        /// <summary>
        /// 非同期に処理したいメソッド。
        /// </summary>
        public Action Action;

        /// <summary>
        /// 非同期処理の完了で呼び出されるコールバック メソッド。
        /// </summary>
        public AsyncTaskCallback Callback;
    }
}
