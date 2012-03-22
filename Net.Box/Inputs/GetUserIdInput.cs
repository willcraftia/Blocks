#region Using

using System;

#endregion

namespace Willcraftia.Net.Box.Inputs
{
    public struct GetUserIdInput
    {
        public const string UriBase = InputConstants.RestUriBase + "action=get_user_id";

        public string ApiKey;

        public string AuthToken;

        public string Login;

        public string ToUri()
        {
            return UriBase + "&api_key=" + ApiKey + "&auth_token=" + AuthToken + "&login=" + Login;
        }
    }
}
