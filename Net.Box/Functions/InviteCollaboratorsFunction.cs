#region Using

using System;
using System.Text;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class InviteCollaboratorsFunction
    {
        const string uriBase = GetFunctionCore.UriBase + "action=invite_collaborators";

        public static bool DumpUri { get; set; }

        public static bool DumpXml { get; set; }

        public static InviteCollaboratorsResult Execute(string apiKey, string authToken,
            Target target, long targetId,
            long[] userIds, string[] emails, Role itemRole,
            bool resendInvite, bool noEmail,
            params string[] parameters)
        {
            var sb = new StringBuilder();
            sb.Append(uriBase);
            sb.Append("&api_key=").Append(apiKey);
            sb.Append("&auth_token=").Append(authToken);
            sb.Append("&target=").Append(target.ToParameterValue());
            sb.Append("&target_id=").Append(targetId);
            if (userIds == null || userIds.Length == 0)
            {
                // 空でも必須パラメータ扱いであるようなので明示指定します。
                sb.Append("&user_ids[]=");
            }
            else
            {
                foreach (var userId in userIds) sb.Append("&user_ids[]=").Append(userId);
            }
            if (emails == null || emails.Length == 0)
            {
                // 空でも必須パラメータ扱いであるようなので明示指定します。
                sb.Append("&emails[]=");
            }
            else
            {
                foreach (var email in emails) sb.Append("&emails[]=").Append(email);
            }
            sb.Append("&item_role_name=").Append(itemRole.ToParameterValue());
            sb.Append("&resend_invite=").Append(resendInvite);
            sb.Append("&no_email=").Append(noEmail);
            foreach (var p in parameters) sb.Append("&params[]=").Append(p);

            var uri = sb.ToString();
            if (DumpUri) Console.WriteLine("URI: " + uri);

            return GetFunctionCore.Execute<InviteCollaboratorsResult>(uri, DumpXml);
        }
    }
}
