#region Using

using System;
using System.Text;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class CreateFolderFunction
    {
        const string uriBase = GetFunctionCore.UriBase + "action=create_folder";

        public static bool DumpUri { get; set; }

        public static bool DumpXml { get; set; }

        public static CreateFolderResult Execute(string apiKey, string authToken, long parentId, string name, bool share)
        {
            var sb = new StringBuilder();
            sb.Append(uriBase);
            sb.Append("&api_key=").Append(apiKey);
            sb.Append("&auth_token=").Append(authToken);
            sb.Append("&parent_id=").Append(parentId);
            sb.Append("&name=").Append(Uri.EscapeUriString(name));
            sb.Append("&share=").Append(share ? "1" : "0");

            var uri = sb.ToString();
            if (DumpUri) Console.WriteLine("URI: " + uri);

            return GetFunctionCore.Execute<CreateFolderResult>(uri, DumpXml);
        }
    }
}
