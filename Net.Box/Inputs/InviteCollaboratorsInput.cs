#region Using

using System;
using System.Text;

#endregion

namespace Willcraftia.Net.Box.Inputs
{
    public struct InviteCollaboratorsInput
    {
        public const string UriBase = InputConstants.RestUriBase + "action=invite_collaborators";

        public string ApiKey;

        public string AuthToken;

        public Target Target;

        public long TargetId;

        public long[] UserIds;

        public string[] Emails;

        public Role ItemRole;

        public bool ResendInvite;

        public bool NoEmail;

        public string[] Parameters;

        public string ToUri()
        {
            var sb = new StringBuilder();
            sb.Append(UriBase);
            sb.Append("&api_key=").Append(ApiKey);
            sb.Append("&auth_token=").Append(AuthToken);
            sb.Append("&target=").Append(Target.ToParameterValue());
            sb.Append("&target_id=").Append(TargetId);
            if (UserIds == null || UserIds.Length == 0)
            {
                // 空でも必須パラメータ扱いであるようなので明示指定します。
                sb.Append("&user_ids[]=");
            }
            else
            {
                foreach (var userId in UserIds) sb.Append("&user_ids[]=").Append(userId);
            }
            if (Emails == null || Emails.Length == 0)
            {
                // 空でも必須パラメータ扱いであるようなので明示指定します。
                sb.Append("&emails[]=");
            }
            else
            {
                foreach (var email in Emails) sb.Append("&emails[]=").Append(email);
            }
            sb.Append("&item_role_name=").Append(ItemRole.ToParameterValue());
            sb.Append("&resend_invite=").Append(ResendInvite);
            sb.Append("&no_email=").Append(NoEmail);
            if (Parameters != null)
            {
                foreach (var p in Parameters) sb.Append("&params[]=").Append(p);
            }
            return sb.ToString();
        }
    }
}
