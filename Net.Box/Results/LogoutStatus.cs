#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public enum LogoutStatus
    {
        [XmlEnum("logout_ok")]
        LogoutOk,

        [XmlEnum("not_logged_in")]
        NotLoggedIn,

        [XmlEnum("application_restricted")]
        ApplicationRestricted
    }
}
