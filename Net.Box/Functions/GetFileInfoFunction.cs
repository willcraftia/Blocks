#region Using

using System;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class GetFileInfoFunction
    {
        const string uriBase = GetFunctionCore.UriBase + "action=get_file_info";

        public static bool DumpUri { get; set; }

        public static bool DumpXml { get; set; }

        public static GetFileInfoResult Execute(string apiKey, string authToken, long fileId)
        {
            var uri = uriBase + "&api_key=" + apiKey + "&auth_token=" + authToken + "&file_id=" + fileId;
            if (DumpUri) Console.WriteLine("URI: " + uri);

            return GetFunctionCore.Execute<GetFileInfoResult>(uri, DumpXml);
        }
    }
}
