#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box
{
    public sealed class User
    {
        [XmlElement("login")]
        public string Login { get; set; }

        [XmlElement("email")]
        public string Email { get; set; }

        [XmlElement("access_id")]
        public long AccessId { get; set; }

        [XmlElement("user_id")]
        public long UserId { get; set; }

        [XmlElement("space_amount")]
        public long SpaceAmount { get; set; }

        [XmlElement("space_used")]
        public long SpaceUsed { get; set; }

        [XmlElement("max_upload_size")]
        public long MaxUploadSize { get; set; }

        public override string ToString()
        {
            return "[Login=" + Login +
                ", Email=" + Email +
                ", AccessId=" + AccessId +
                ", UserId=" + UserId +
                ", SpaceAmount=" + SpaceAmount +
                ", SpaceUsed=" + SpaceUsed +
                ", MaxUploadSize=" + MaxUploadSize +
                "]";
        }
    }
}
