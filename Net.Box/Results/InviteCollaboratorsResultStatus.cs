#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public enum InviteCollaboratorsResultStatus
    {
        [XmlEnum("s_invite_collaborators")]
        SInviteCollaborators,

        [XmlEnum("e_invite_collaborators")]
        EInviteCollaborators,

        [XmlEnum("user_already_collaborator")]
        UserAlreadyCollaborator,

        [XmlEnum("cannot_invite_subusers")]
        CannotInviteSubusers,

        [XmlEnum("e_cannot_invite_self")]
        ECannotInviteSelf,

        [XmlEnum("e_collaborators_limit_reached")]
        ECollaboratorsLimitReached,

        [XmlEnum("e_insufficient_permissions ")]
        EInsufficientPermissions
    }
}
