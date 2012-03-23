#region Using

using System;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public sealed class GetAuthTokenFunction
    {
        const string uriBase = GetFunctionCore.UriBase + "action=get_auth_token";

        public static bool DumpUri { get; set; }

        public static bool DumpXml { get; set; }

        public static GetAuthTokenResult Execute(string apiKey, string ticket)
        {
            var uri = uriBase + "&api_key=" + apiKey + "&ticket=" + ticket;
            if (DumpUri) Console.WriteLine("URI: " + uri);

            return GetFunctionCore.Execute<GetAuthTokenResult>(uri, DumpXml);
        }
    }
}
