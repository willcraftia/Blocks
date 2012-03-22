#region Using

using System;

#endregion

namespace Willcraftia.Net.Box.Inputs
{
    public struct LogoutInput
    {
        public const string UriBase = InputConstants.RestUriBase + "action=logout";

        public string ApiKey;

        public string AuthToken;

        public string ToUri()
        {
            return UriBase + "&api_key=" + ApiKey + "&auth_token=" + AuthToken;
        }
    }
}
