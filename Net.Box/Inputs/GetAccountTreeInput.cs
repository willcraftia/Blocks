#region Using

using System;
using System.Text;

#endregion

namespace Willcraftia.Net.Box.Inputs
{
    public struct GetAccountTreeInput
    {
        public const string UriBase = InputConstants.RestUriBase + "action=get_account_tree";

        public string ApiKey;

        public string AuthToken;

        public long FolderId;

        public string[] Parameters;

        public string ToUri()
        {
            var sb = new StringBuilder();
            sb.Append(UriBase);
            sb.Append("&api_key=").Append(ApiKey);
            sb.Append("&auth_token=").Append(AuthToken);
            sb.Append("&folder_id=").Append(FolderId);
            if (Parameters != null)
            {
                foreach (var p in Parameters) sb.Append("&params[]=").Append(p);
            }
            return sb.ToString();
        }
    }
}
