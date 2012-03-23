#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    [XmlRoot("response")]
    public sealed class GetUserIdResult
    {
        [XmlElement("status")]
        public GetUserIdStatus Status { get; set; }

        [XmlElement("id")]
        public long Id { get; set; }

        public override string ToString()
        {
            return "[Status=" + Status + ", Id=" + Id + "]";
        }
    }
}
