#region Using

using System;
using System.Threading;

#endregion

namespace Willcraftia.Xna.Framework.Threading
{
    /// <summary>
    /// 非同期に処理を呼び出し、処理結果の完了を Game Thread で受け取るサービスへのインタフェースです。
    /// </summary>
    public interface IAsyncTaskService
    {
        /// <summary>
        /// 非同期に処理したいメソッドをキューへ入れます。
        /// </summary>
        /// <param name="waitCallback">非同期に処理するメソッド。</param>
        /// <param name="state">
        /// WaitCallback 渡され、
        /// AsyncTaskResultCallback が受け取る AsyncTaskResult に設定されるオブジェクト。
        /// </param>
        /// <param name="resultCallback">
        /// WaitCallback の処理完了を受け取るコールバック メソッド。
        /// </param>
        void Enqueue(WaitCallback waitCallback, object state, AsyncTaskResultCallback resultCallback);
    }
}
