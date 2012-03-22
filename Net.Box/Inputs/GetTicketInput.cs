#region Using

using System;
using System.Text;

#endregion

namespace Willcraftia.Net.Box.Inputs
{
    public struct GetTicketInput
    {
        public const string UriBase = InputConstants.RestUriBase + "action=get_ticket";

        public string ApiKey;

        public string ToUri()
        {
            return UriBase + "&api_key=" + ApiKey;
        }
    }
}
