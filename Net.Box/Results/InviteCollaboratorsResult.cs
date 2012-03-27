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
        [XmlElement("status")]
        public string Status { get; set; }

        [XmlArray("invited_collaborators")]
        [XmlArrayItem("item")]
        public List<InvitedCollaborator> InvitedCollaborators { get; set; }

        [XmlArray("unsuccessful_invitations")]
        [XmlArrayItem("item")]
        public List<InvitedCollaborator> UnsuccessfulInvitations { get; set; }

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
