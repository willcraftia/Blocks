#region Using

using System;
using System.IO;

#endregion

namespace Willcraftia.Net.Box.Service
{
    /// <summary>
    /// Box 連携サービスへのインタフェースです。
    /// </summary>
    public interface IBoxService
    {
        /// <summary>
        /// 作成されている BoxSession を取得します。
        /// このプロパティは、GetAuthToken(string) あるいは CreateSession(string)
        /// の呼び出しが成功するまで null を返します。
        /// </summary>
        BoxSession Session { get; }

        /// <summary>
        /// Ticket を取得します。
        /// </summary>
        /// <returns>Ticket。</returns>
        string GetTicket();

        /// <summary>
        /// 指定の Ticket をユーザが認証するためのページを標準ブラウザで表示します。
        /// </summary>
        /// <param name="ticket">Ticket。</param>
        void RedirectUserAuth(string ticket);

        /// <summary>
        /// 指定の Ticket に対する Auth-token を取得し、BoxSession を生成します。
        /// このメソッドは、Auth-token をアプリケーションがまだ管理していない場合に用います。
        /// </summary>
        /// <param name="ticket">Ticket。</param>
        /// <returns>BoxSession。</returns>
        BoxSession GetAuthToken(string ticket);

        /// <summary>
        /// 指定の Auth-token から BoxSession を生成します。
        /// このメソッドは、Auth-token をアプリケーションが既に管理している場合に用います。
        /// </summary>
        /// <param name="authToken">Auth-token。</param>
        /// <returns>BoxSession。</returns>
        BoxSession CreateSession(string authToken);
    }
}
