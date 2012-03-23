#region Using

using System;

#endregion

namespace Willcraftia.Net.Box
{
    /// <summary>
    /// アップロードするファイルの情報を表すクラスです。
    /// </summary>
    public sealed class UploadFile
    {
        /// <summary>
        /// ファイル名を取得または設定します。
        /// </summary>
        /// <remarks>
        /// Overwrite や NewCopy の場合でも、
        /// Content-Disposition の filename を設定するために必要です。
        /// </remarks>
        public string Name { get; set; }

        /// <summary>
        /// Content-Type を取得または設定します。
        /// 例えば、XML ならば、"text/xml;charset=utf-8" を指定します。
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// ファイルの内容を取得または設定します。
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Content プロパティ以外を文字列情報に含めて返します。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "[Name=" + Name + ", ContentType=" + ContentType + "]";
        }
    }
}
