#region Using

using System;

#endregion

namespace Willcraftia.Net.Box.Inputs
{
    public struct GetAuthTokenInput
    {
        public const string UriBase = InputConstants.RestUriBase + "action=get_auth_token";

        public string ApiKey;

        public string Ticket;

        public string ToUri()
        {
            return UriBase + "&api_key=" + ApiKey + "&ticket=" + Ticket;
        }
    }
}
