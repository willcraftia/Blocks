#region Using

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    [XmlRoot("response")]
    public sealed class InviteCollaboratorsResult
    {
        public sealed class Item
        {
            [XmlText]
            public string Content { get; set; }

            public override string ToString()
            {
                return "[Content=" + Content + "]";
            }
        }

        [XmlElement("status")]
        public InviteCollaboratorsResultStatus Status { get; set; }

        //[XmlArray("collaborations")]
        //[XmlArrayItem("collaboration")]
        //public List<Collaboration> Collaborations { get; set; }
        [XmlArray("invited_collaborators")]
        [XmlArrayItem("item")]
        public List<Item> InvitedCollaborators { get; set; }

        [XmlArray("unsuccessful_invitations")]
        [XmlArrayItem("item")]
        public List<Item> UnsuccessfulInvitations { get; set; }

        public bool Succeeded
        {
            get { return Status == InviteCollaboratorsResultStatus.SInviteCollaborators; }
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("[Status=").Append(Status);
            sb.Append(", InvitedCollaborators=[");
            for (int i = 0; i < InvitedCollaborators.Count; i++)
            {
                sb.Append(InvitedCollaborators[i]);
                if (i < InvitedCollaborators.Count - 1) sb.Append(", ");
            }
            sb.Append("]");
            sb.Append(", UnsuccessfulInvitations=[");
            for (int i = 0; i < UnsuccessfulInvitations.Count; i++)
            {
                sb.Append(UnsuccessfulInvitations[i]);
                if (i < UnsuccessfulInvitations.Count - 1) sb.Append(", ");
            }
            sb.Append("]");
            sb.Append("]");
            return sb.ToString();
        }
    }
}
