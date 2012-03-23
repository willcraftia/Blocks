#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    [XmlRoot("response")]
    public sealed class GetAuthTokenResult
    {
        [XmlElement("status")]
        public GetAuthTokenStatus Status { get; set; }

        [XmlElement("auth_token")]
        public string AuthToken { get; set; }

        [XmlElement("user")]
        public User User { get; set; }

        public override string ToString()
        {
            return "[Status=" + Status + ", AuthToken=" + AuthToken + ", User=" + User + "]";
        }
    }
}
