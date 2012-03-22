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
        public GetUserIdResultStatus Status { get; set; }

        [XmlElement("id")]
        public long Id { get; set; }

        public bool Succeeded
        {
            get { return Status == GetUserIdResultStatus.SGetUserId; }
        }

        public override string ToString()
        {
            return "[Status=" + Status +
                ", Id=" + Id +
                "]";
        }
    }
}
