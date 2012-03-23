#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    [XmlRoot("response")]
    public sealed class GetFileInfoResult
    {
        [XmlElement("status")]
        public GetFileInfoResultStatus Status { get; set; }

        [XmlElement("info")]
        public Info Info { get; set; }

        public bool Succeeded
        {
            get { return Status == GetFileInfoResultStatus.SGetFileInfo; }
        }

        public override string ToString()
        {
            return "[Status=" + Status + ", Info=" + Info + "]";
        }
    }
}
