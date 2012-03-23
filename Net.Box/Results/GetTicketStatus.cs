#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public enum GetTicketStatus
    {
        [XmlEnum("get_ticket_ok")]
        GetTicketOk,

        [XmlEnum("application_restricted")]
        ApplicationRestricted,

        [XmlEnum("wrong_input")]
        WrongInput
    }
}
