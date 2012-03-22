#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public enum GetUserIdResultStatus
    {
        [XmlEnum("s_get_user_id")]
        SGetUserId,

        [XmlEnum("e_get_user_id")]
        EGetUserId
    }
}
