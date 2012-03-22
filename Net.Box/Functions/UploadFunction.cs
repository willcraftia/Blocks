#region Using

using System;
using Willcraftia.Net.Box.Inputs;
using Willcraftia.Net.Box.Results;

#endregion

namespace Willcraftia.Net.Box.Functions
{
    public static class UploadFunction
    {
        public static bool DumpXml { get; set; }

        public static UploadResult Execute(string authToken, long folderId, UploadContent content)
        {
            var input = new UploadInput
            {
                AuthToken = authToken,
                FolderId = folderId
            };
            var uri = input.ToUri();
            Console.WriteLine("URI: " + uri);

            return UploadFunctionCore.Execute<UploadResult>(uri, content, DumpXml);
        }
    }
}
