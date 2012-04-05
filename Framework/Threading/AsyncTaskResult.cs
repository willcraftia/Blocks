#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Threading
{
    /// <summary>
    /// AsyncTask の非同期処理完了の結果を表します。
    /// </summary>
    public struct AsyncTaskResult
    {
        /// <summary>
        /// 非同期処理内で発生した例外。
        /// </summary>
        public Exception Exception;

        /// <summary>
        /// 非同期処理完了の結果を検査します。
        /// 非同期処理内で例外が発生していた場合には、その例外を throw します。
        /// </summary>
        public void Check()
        {
            if (Exception != null) throw Exception;
        }
    }
}
