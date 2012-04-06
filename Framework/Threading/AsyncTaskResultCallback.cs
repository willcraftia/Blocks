#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Threading
{
    /// <summary>
    /// AsyncTask の非同期処理の完了で呼び出されるコールバック メソッドです。
    /// </summary>
    /// <param name="result">AsyncTaskResult。</param>
    public delegate void AsyncTaskResultCallback(AsyncTaskResult result);
}
