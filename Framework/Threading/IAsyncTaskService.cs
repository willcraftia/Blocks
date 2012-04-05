#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Threading
{
    /// <summary>
    /// AsyncTask で表すことのできる簡単な処理を、
    /// 非同期に呼び出すサービスへのインタフェースです。
    /// AsyncTask.Callback に設定されたコールバック メソッドは、
    /// Game Thread で呼び出されます。
    /// </summary>
    public interface IAsyncTaskService
    {
        /// <summary>
        /// AsyncTask をキューへ入れます。
        /// キューに入れられた AsyncTask は即座に非同期に処理されるとは限らず、
        /// Thread へ割り当てられるまで待機させられる可能性があります。
        /// </summary>
        /// <param name="task">AsyncTask。</param>
        void Enqueue(AsyncTask task);
    }
}
