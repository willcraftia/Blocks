#region Using

using System;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public sealed class OverwriteFunction
    {
        const string uriBase = UploadFunctionCore.UriBase + "overwrite/";

        public static bool DumpUri { get; set; }

        public static bool DumpContent { get; set; }

        public static bool DumpXml { get; set; }

        public static UploadResult Execute(string authToken, long fileId,
            UploadFile file, bool share, string message, string[] emails)
        {
            var uri = uriBase + authToken + "/" + fileId;
            if (DumpUri) Console.WriteLine("URI: " + uri);

            var files = new UploadFile[]
            {
                file
            };
            return UploadFunctionCore.Execute<UploadResult>(uri, files, share, message, emails, DumpContent, DumpXml);
        }
    }
}
