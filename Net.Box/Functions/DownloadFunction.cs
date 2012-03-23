#region Using

using System;
using System.IO;
using System.Net;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public sealed class DownloadFunction
    {
        const string uriBase = "https://www.box.net/api/1.0/download/";

        public static bool DumpUri { get; set; }

        public static Stream Execute(string authToken, long fileId)
        {
            var uri = uriBase + authToken + "/" + fileId;
            if (DumpUri) Console.WriteLine("URI: " + uri);

            var request = WebRequest.Create(uri);
            var response = request.GetResponse();
            return response.GetResponseStream();
        }
    }
}
