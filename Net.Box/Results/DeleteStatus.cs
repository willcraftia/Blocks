#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public enum DeleteStatus
    {
        [XmlEnum("s_delete_node")]
        SDeleteNode,

        [XmlEnum("not_logged_in")]
        NotLoggedIn,

        [XmlEnum("application_restricted")]
        ApplicationRestricted,

        [XmlEnum("e_delete_node")]
        EDeleteNode
    }
}
