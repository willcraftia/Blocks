#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public enum GetAuthTokenStatus
    {
        [XmlEnum("get_auth_token_ok")]
        GetAuthTokenOk,

        [XmlEnum("application_restricted")]
        ApplicationRestricted,

        [XmlEnum("not_logged_in")]
        NotLoggedIn,

        [XmlEnum("get_auth_token_error")]
        GetAuthTokenError
    }
}
