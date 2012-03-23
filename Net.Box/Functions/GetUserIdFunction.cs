#region Using

using System;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class GetUserIdFunction
    {
        const string uriBase = GetFunctionCore.UriBase + "action=get_user_id";

        public static bool DumpUri { get; set; }

        public static bool DumpXml { get; set; }

        public static GetUserIdResult Execute(string apiKey, string authToken, string login)
        {
            var uri = uriBase + "&api_key=" + apiKey + "&auth_token=" + authToken + "&login=" + login;
            if (DumpUri) Console.WriteLine("URI: " + uri);

            return GetFunctionCore.Execute<GetUserIdResult>(uri, DumpXml);
        }
    }
}
