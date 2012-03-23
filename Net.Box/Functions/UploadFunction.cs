#region Using

using System;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class UploadFunction
    {
        const string uriBase = "https://upload.box.net/api/1.0/upload/";

        public static bool DumpUri { get; set; }

        public static bool DumpContent { get; set; }

        public static bool DumpXml { get; set; }

        public static UploadResult Execute(string authToken, long folderId,
            UploadFile[] files, bool share, string message, string[] emails)
        {
            var uri = uriBase + authToken + "/" + folderId;
            if (DumpUri) Console.WriteLine("URI: " + uri);

            return UploadFunctionCore.Execute<UploadResult>(uri, files, share, message, emails, DumpContent, DumpXml);
        }
    }
}
