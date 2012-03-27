#region Using

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    [XmlRoot("response")]
    public sealed class UploadResult
    {
        [XmlElement("status")]
        public string Status { get; set; }

        [XmlArray("files")]
        [XmlArrayItem("file")]
        public List<UploadedFile> Files { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("[Status=").Append(Status);
            sb.Append(", Files=[");
            for (int i = 0; i < Files.Count; i++)
            {
                sb.Append(Files[i]);
                if (i < Files.Count - 1) sb.Append(", ");
            }
            sb.Append("]");
            sb.Append("]");
            return sb.ToString();
        }
    }
}
