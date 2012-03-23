#region Using

using System;

#endregion

namespace Willcraftia.Net.Box
{
    public sealed class UploadFile
    {
        public string Name { get; set; }

        // text/xml;charset=utf-8
        public string ContentType { get; set; }

        public string Content { get; set; }

        public override string ToString()
        {
            return "[Name=" + Name + ", ContentType=" + ContentType + "]";
        }
    }
}
