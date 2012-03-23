#region Using

using System;
using System.IO;
using Willcraftia.Net.Box.Functions;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Service
{
    public sealed class BoxSession
    {
        BoxManager boxManager;

        public bool Valid { get; internal set; }

        internal BoxSession(BoxManager boxManager)
        {
            this.boxManager = boxManager;

            Valid = true;
        }

        public GetAccountTreeResult GetAccountTreeRoot(params string[] parameters)
        {
            return GetAccountTree(0, parameters);
        }

        public GetAccountTreeResult GetAccountTree(long folderId, params string[] parameters)
        {
            EnsureValidSession();
            return GetAccountTreeFunction.Execute(boxManager.ApiKey, boxManager.AuthToken, folderId, parameters);
        }

        public CreateFolderResult CreateFolder(long parentId, string name, bool share)
        {
            EnsureValidSession();
            return CreateFolderFunction.Execute(boxManager.ApiKey, boxManager.AuthToken, parentId, name, share);
        }

        public DeleteResult DeleteFolder(long folderId)
        {
            EnsureValidSession();
            return DeleteFunction.Execute(boxManager.ApiKey, boxManager.AuthToken, Target.Folder, folderId);
        }

        public DeleteResult DeleteFile(long fileId)
        {
            EnsureValidSession();
            return DeleteFunction.Execute(boxManager.ApiKey, boxManager.AuthToken, Target.File, fileId);
        }

        public GetFileInfoResult GetFileInfo(long fileId)
        {
            EnsureValidSession();
            return GetFileInfoFunction.Execute(boxManager.ApiKey, boxManager.AuthToken, fileId);
        }

        public UploadResult Upload(long folderId, UploadFile[] files, bool share, string message, string[] emails)
        {
            EnsureValidSession();
            return UploadFunction.Execute(boxManager.AuthToken, folderId, files, share, message, emails);
        }

        public UploadResult Overwrite(long fileId, UploadFile file, bool share, string message, string[] emails)
        {
            EnsureValidSession();
            return OverwriteFunction.Execute(boxManager.AuthToken, fileId, file, share, message, emails);
        }

        public Stream Download(long fileId)
        {
            EnsureValidSession();
            return DownloadFunction.Execute(boxManager.AuthToken, fileId);
        }

        public InviteCollaboratorsResult InviteCollaboratorsToFolder(long folderId,
            long[] userIds, string[] emails, Role itemRole, bool resendInvite, bool noEmail,
            params string[] parameters)
        {
            EnsureValidSession();
            return InviteCollaboratorsFunction.Execute(boxManager.ApiKey, boxManager.AuthToken,
                Target.Folder, folderId, userIds, emails, itemRole, resendInvite, noEmail,
                parameters);
        }

        public GetUserIdResult GetUserId(string login)
        {
            return GetUserIdFunction.Execute(boxManager.ApiKey, boxManager.AuthToken, login);
        }

        public LogoutResult Logout()
        {
            return boxManager.Logout();
        }

        void EnsureValidSession()
        {
            if (!Valid) throw new InvalidOperationException("This session is invalidated.");
        }
    }
}
