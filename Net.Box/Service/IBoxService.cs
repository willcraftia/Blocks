﻿#region Using

using System;

#endregion

namespace Willcraftia.Net.Box.Service
{
    /// <summary>
    /// Box 連携サービスへのインタフェースです。
    /// </summary>
    public interface IBoxService
    {
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
        /// このメソッドは、有効な Auth-token をアプリケーションがまだ管理していない場合に用います。
        /// </summary>
        /// <param name="ticket">Ticket。</param>
        /// <returns>BoxSession。</returns>
        BoxSession GetAuthToken(string ticket);

        /// <summary>
        /// 指定の Auth-token から BoxSession を生成します。
        /// このメソッドは、有効な Auth-token をアプリケーションが既に管理している場合に用います。
        /// </summary>
        /// <param name="authToken">Auth-token。</param>
        /// <returns>BoxSession。</returns>
        BoxSession CreateSession(string authToken);
    }
}