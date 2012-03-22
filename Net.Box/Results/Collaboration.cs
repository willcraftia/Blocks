#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public sealed class Collaboration
    {
        [XmlElement("id")]
        public long Id { get; set; }

        [XmlElement("item_role_name")]
        public Role ItemRole { get; set; }

        [XmlElement("status")]
        public CollaborationStatus Status { get; set; }

        [XmlElement("item_type")]
        public Target ItemType { get; set; }

        [XmlElement("item_id")]
        public long ItemId { get; set; }

        [XmlElement("item_name")]
        public string ItemName { get; set; }

        [XmlElement("item_user_id")]
        public long ItemUserId { get; set; }

        [XmlElement("item_user_name")]
        public string ItemUserName { get; set; }

        [XmlElement("user_id")]
        public long UserId { get; set; }

        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("email")]
        public string Email { get; set; }
    }
}
