#region Using

using System;
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

        public UploadResult Upload(long folderId, UploadFile[] files, bool share, string message, string[] emails)
        {
            EnsureValidSession();
            return UploadFunction.Execute(boxManager.AuthToken, folderId, files, share, message, emails);
        }

        public InviteCollaboratorsResult InviteCollaborators(Target target, long targetId,
            long[] userIds, string[] emails, Role itemRole, bool resendInvite, bool noEmail,
            params string[] parameters)
        {
            EnsureValidSession();
            return InviteCollaboratorsFunction.Execute(boxManager.ApiKey, boxManager.AuthToken,
                target, targetId, userIds, emails, itemRole, resendInvite, noEmail,
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
