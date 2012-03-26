#region Using

using System;
using System.Collections.Generic;
using System.IO;
using Willcraftia.Net.Box.Functions;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Service
{
    /// <summary>
    /// Ticket 認証済みのユーザの Box 連携セッションを表すクラスです。
    /// </summary>
    public sealed class BoxSession
    {
        /// <summary>
        /// API Key を取得します。
        /// </summary>
        public string ApiKey { get; private set; }

        /// <summary>
        /// Auth-token を取得します。
        /// </summary>
        public string AuthToken { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="apiKey">API Key。</param>
        /// <param name="authToken">Auth-token。</param>
        internal BoxSession(string apiKey, string authToken)
        {
            if (apiKey == null) throw new ArgumentNullException("apiKey");
            if (authToken == null) throw new ArgumentNullException("authToken");
            ApiKey = apiKey;
            AuthToken = authToken;
        }

        /// <summary>
        /// ルート フォルダを基点としたフォルダ ツリーを取得します。
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Folder GetAccountTreeRoot(params string[] parameters)
        {
            return GetAccountTree(0, parameters);
        }

        /// <summary>
        /// 指定のフォルダを基点としたフォルダ ツリーを取得します。
        /// </summary>
        /// <param name="folderId">フォルダ ID。</param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Folder GetAccountTree(long folderId, params string[] parameters)
        {
            var result = GetAccountTreeFunction.Execute(ApiKey, AuthToken, folderId, parameters);

            if (result.Status != GetAccountTreeStatus.ListingOk)
                throw new BoxStatusException<GetAccountTreeStatus>(result.Status);

            return result.Tree.Folder;
        }

        /// <summary>
        /// 新規フォルダを作成します。
        /// </summary>
        /// <param name="parentId">親フォルダの ID。</param>
        /// <param name="name">作成するフォルダの名前。</param>
        /// <param name="share">
        /// true (作成するフォルダを共有する場合)、false (それ以外の場合)。
        /// </param>
        /// <returns>作成されたフォルダの情報。</returns>
        public CreatedFolder CreateFolder(long parentId, string name, bool share)
        {
            var result = CreateFolderFunction.Execute(ApiKey, AuthToken, parentId, name, share);

            if (result.Status != CreateFolderStatus.CreateOk)
                throw new BoxStatusException<CreateFolderStatus>(result.Status);

            return result.Folder;
        }

        /// <summary>
        /// フォルダを削除します。
        /// </summary>
        /// <param name="folderId">削除するフォルダの ID。</param>
        public void DeleteFolder(long folderId)
        {
            var result = DeleteFunction.Execute(ApiKey, AuthToken, Target.Folder, folderId);

            if (result.Status != DeleteStatus.SDeleteNode)
                throw new BoxStatusException<DeleteStatus>(result.Status);
        }

        /// <summary>
        /// ファイルを削除します。
        /// </summary>
        /// <param name="fileId">削除するファイルの ID。</param>
        public void DeleteFile(long fileId)
        {
            var result = DeleteFunction.Execute(ApiKey, AuthToken, Target.File, fileId);

            if (result.Status != DeleteStatus.SDeleteNode)
                throw new BoxStatusException<DeleteStatus>(result.Status);
        }

        /// <summary>
        /// ファイルの情報を取得します。
        /// </summary>
        /// <param name="fileId">取得するファイルの ID。</param>
        /// <returns>取得したファイルの情報。</returns>
        public Info GetFileInfo(long fileId)
        {
            var result = GetFileInfoFunction.Execute(ApiKey, AuthToken, fileId);

            if (result.Status != GetFileInfoStatus.SGetFileInfo)
                throw new BoxStatusException<GetFileInfoStatus>(result.Status);

            return result.Info;
        }

        /// <summary>
        /// ファイルをアップロードします。
        /// </summary>
        /// <param name="folderId">アップロード先フォルダの ID。</param>
        /// <param name="files">アップロードするファイルの情報 (複数可)。</param>
        /// <param name="share">true (アップロードしたファイルを共有する場合)、false (それ以外の場合)。</param>
        /// <param name="message">通知メールに含めるメッセージ。</param>
        /// <param name="emails">共有ファイルの情報を通知するユーザのメールアドレスの配列。</param>
        /// <returns>アップロードしたファイルの情報。</returns>
        public List<UploadedFile> Upload(long folderId, IEnumerable<UploadFile> files, bool share, string message, string[] emails)
        {
            var result = UploadFunction.Execute(AuthToken, folderId, files, share, message, emails);

            if (result.Status != UploadStatus.UploadOk)
                throw new BoxStatusException<UploadStatus>(result.Status);

            return result.Files;
        }

        /// <summary>
        /// ファイルを上書きアップロードします。
        /// </summary>
        /// <param name="fileId">上書きするファイルの ID。</param>
        /// <param name="file">アップロードするファイルの情報。</param>
        /// <param name="share">true (アップロードしたファイルを共有する場合)、false (それ以外の場合)。</param>
        /// <param name="message">通知メールに含めるメッセージ。</param>
        /// <param name="emails">共有ファイルの情報を通知するユーザのメールアドレスの配列。</param>
        /// <returns>アップロードしたファイルの情報。</returns>
        public UploadedFile Overwrite(long fileId, UploadFile file, bool share, string message, string[] emails)
        {
            var result = OverwriteFunction.Execute(AuthToken, fileId, file, share, message, emails);

            if (result.Status != UploadStatus.UploadOk)
                throw new BoxStatusException<UploadStatus>(result.Status);

            return result.Files[0];
        }

        /// <summary>
        /// ファイルをダウンロードします。
        /// ダウンロード操作でエラーが発生した場合には、その情報が Stream に書き込まれています。
        /// このため、正常にダウンロードしたかどうかは、
        /// 呼び出し側で Stream を読み取り、判定する必要があります。
        /// </summary>
        /// <param name="fileId">ダウンロードするファイルの ID。</param>
        /// <returns>ダウンロードしたファイル、あるいは、エラー情報を提供する Stream。</returns>
        public Stream Download(long fileId)
        {
            return DownloadFunction.Execute(AuthToken, fileId);
        }

        /// <summary>
        /// ユーザを Collaborator として招待します。
        /// </summary>
        /// <param name="folderId">招待先のフォルダの ID。</param>
        /// <param name="userIds">招待するユーザの ID。</param>
        /// <param name="emails">招待するユーザのメールアドレス。</param>
        /// <param name="itemRole">招待するユーザに与えるロール。</param>
        /// <param name="resendInvite">
        /// true (招待するユーザに対して通知メールを再送する場合)、false (それ以外の場合)。
        /// </param>
        /// <param name="noEmail">
        /// true (招待するユーザに対して通知メールを送信しない場合)、false (それ以外の場合)。
        /// </param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public List<InvitedCollaborator> InviteCollaboratorsToFolder(long folderId,
            long[] userIds, string[] emails, Role itemRole, bool resendInvite, bool noEmail,
            params string[] parameters)
        {
            var result = InviteCollaboratorsFunction.Execute(ApiKey, AuthToken,
                Target.Folder, folderId, userIds, emails, itemRole, resendInvite, noEmail,
                parameters);

            if (result.Status != InviteCollaboratorsStatus.SInviteCollaborators)
                throw new BoxStatusException<InviteCollaboratorsStatus>(result.Status);

            return result.InvitedCollaborators;
        }

        /// <summary>
        /// 指定のメールアドレスに対するユーザ ID を取得します。
        /// この操作は、指定するメールアドレスのユーザが Collaborator でなければ、
        /// そのユーザ ID を解決できません。
        /// </summary>
        /// <param name="email">メールアドレス。</param>
        /// <returns>取得したユーザ ID。</returns>
        public long GetUserId(string email)
        {
            var result = GetUserIdFunction.Execute(ApiKey, AuthToken, email);

            if (result.Status != GetUserIdStatus.SGetUserId)
                throw new BoxStatusException<GetUserIdStatus>(result.Status);

            return result.Id;
        }
    }
}
