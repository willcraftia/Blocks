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
        public GetFileInfoStatus Status { get; set; }

        [XmlElement("info")]
        public Info Info { get; set; }

        public override string ToString()
        {
            return "[Status=" + Status + ", Info=" + Info + "]";
        }
    }
}
