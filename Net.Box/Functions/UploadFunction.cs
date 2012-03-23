#region Using

using System;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    /// <summary>
    /// upload API。
    /// </summary>
    public static class UploadFunction
    {
        /// <summary>
        /// 基礎となる URI。
        /// </summary>
        const string uriBase = UploadFunctionCore.UriBase + "upload/";

        /// <summary>
        /// URI を Console へダンプするかどうかを示す値を取得または設定します。
        /// </summary>
        /// <value>
        /// true (URI を Console へダンプする場合)、false (それ以外の場合)。
        /// </value>
        public static bool DumpUri { get; set; }

        /// <summary>
        /// multipart/form-data を Console へダンプするどうかを示す値を取得または設定します。
        /// </summary>
        /// <value>
        /// true (multipart/form-data を Console へダンプする場合)、false (それ以外の場合)。
        /// </value>
        public static bool DumpContent { get; set; }

        /// <summary>
        /// XML を Console へダンプするかを示す値を取得または設定します。
        /// </summary>
        /// <value>
        /// true (XML を Console へダンプする場合)、false (それ以外の場合)。
        /// </value>
        public static bool DumpXml { get; set; }

        /// <summary>
        /// 実行します。
        /// </summary>
        /// <param name="authToken">Auth-token。</param>
        /// <param name="folderId">アップロード先フォルダの ID。</param>
        /// <param name="files">UploadFile の配列。</param>
        /// <param name="share">
        /// true (ファイルを共有可とする場合)、false (それ以外の場合)。
        /// </param>
        /// <param name="message">通知メールに含めるメッセージ。</param>
        /// <param name="emails">
        /// 共有ファイルの情報を通知するユーザのメールアドレスの配列。
        /// </param>
        /// <returns></returns>
        public static UploadResult Execute(string authToken, long folderId,
            UploadFile[] files, bool share, string message, string[] emails)
        {
            var uri = uriBase + authToken + "/" + folderId;
            if (DumpUri) Console.WriteLine("URI: " + uri);

            return UploadFunctionCore.Execute<UploadResult>(uri, files, share, message, emails, DumpContent, DumpXml);
        }
    }
}
