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
        public GetTicketResultStatus Status { get; set; }

        [XmlElement("ticket")]
        public string Ticket { get; set; }

        public bool Succeeded
        {
            get { return Status == GetTicketResultStatus.GetTicketOk; }
        }

        public override string ToString()
        {
            return "[Status=" + Status +
                ", Ticket=" + Ticket +
                "]";
        }
    }
}
