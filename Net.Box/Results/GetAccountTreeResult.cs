#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    [XmlRoot("response")]
    public sealed class GetAccountTreeResult
    {
        [XmlElement("status")]
        public GetAccountTreeStatus Status { get; set; }

        [XmlElement("tree")]
        public AccountTree Tree { get; set; }

        public override string ToString()
        {
            return "[Status=" + Status + ", Tree=" + Tree + "]";
        }
    }
}
