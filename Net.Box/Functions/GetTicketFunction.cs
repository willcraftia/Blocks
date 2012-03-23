#region Using

using System;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class GetTicketFunction
    {
        const string uriBase = GetFunctionCore.UriBase + "action=get_ticket";

        public static bool DumpUri { get; set; }

        public static bool DumpXml { get; set; }

        public static GetTicketResult Execute(string apiKey)
        {
            var uri = uriBase + "&api_key=" + apiKey;
            if (DumpUri) Console.WriteLine("URI: " + uri);

            return GetFunctionCore.Execute<GetTicketResult>(uri, DumpXml);
        }
    }
}
