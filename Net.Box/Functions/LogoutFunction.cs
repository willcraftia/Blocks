#region Using

using System;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class LogoutFunction
    {
        const string uriBase = GetFunctionCore.UriBase + "action=logout";

        public static bool DumpUri { get; set; }

        public static bool DumpXml { get; set; }

        public static LogoutResult Execute(string apiKey, string authToken)
        {
            var uri = uriBase + "&api_key=" + apiKey + "&auth_token=" + authToken;
            if (DumpUri) Console.WriteLine("URI: " + uri);

            return GetFunctionCore.Execute<LogoutResult>(uri, DumpXml);
        }
    }
}
