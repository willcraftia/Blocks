#region Using

using System;
using System.Text;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class GetAccountTreeFunction
    {
        const string uriBase = GetFunctionCore.UriBase + "action=get_account_tree";

        public static bool DumpUri { get; set; }

        public static bool DumpXml { get; set; }

        public static GetAccountTreeResult Execute(string apiKey, string authToken, long folderId, params string[] parameters)
        {
            var sb = new StringBuilder();
            sb.Append(uriBase);
            sb.Append("&api_key=").Append(apiKey);
            sb.Append("&auth_token=").Append(authToken);
            sb.Append("&folder_id=").Append(folderId);
            foreach (var p in parameters) sb.Append("&params[]=").Append(p);

            var uri = sb.ToString();
            if (DumpUri) Console.WriteLine("URI: " + uri);

            return GetFunctionCore.Execute<GetAccountTreeResult>(uri, DumpXml);
        }
    }
}
