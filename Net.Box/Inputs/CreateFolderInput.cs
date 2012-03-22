#region Using

using System;
using System.Text;

#endregion

namespace Willcraftia.Net.Box.Inputs
{
    public struct CreateFolderInput
    {
        public const string UriBase = InputConstants.RestUriBase + "action=create_folder";

        public string ApiKey;

        public string AuthToken;

        public long ParentId;

        public string Name;

        public bool Share;

        public string ToUri()
        {
            var sb = new StringBuilder();
            sb.Append(UriBase);
            sb.Append("&api_key=").Append(ApiKey);
            sb.Append("&auth_token=").Append(AuthToken);
            sb.Append("&parent_id=").Append(ParentId);
            sb.Append("&name=").Append(Uri.EscapeUriString(Name));
            sb.Append("&share=").Append(Share ? "1" : "0");
            return sb.ToString();
        }
    }
}
