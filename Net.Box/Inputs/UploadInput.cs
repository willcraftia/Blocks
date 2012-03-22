#region Using

using System;

#endregion

namespace Willcraftia.Net.Box.Inputs
{
    public struct UploadInput
    {
        public const string UriBase = "https://upload.box.net/api/1.0/upload/";

        public string AuthToken;

        public long FolderId;

        public string ToUri()
        {
            return UriBase + AuthToken + "/" + FolderId;
        }
    }
}
