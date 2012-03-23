#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    [XmlRoot("response")]
    public sealed class GetTicketResult
    {
        [XmlElement("status")]
        public GetTicketStatus Status { get; set; }

        [XmlElement("ticket")]
        public string Ticket { get; set; }

        public override string ToString()
        {
            return "[Status=" + Status + ", Ticket=" + Ticket + "]";
        }
    }
}
