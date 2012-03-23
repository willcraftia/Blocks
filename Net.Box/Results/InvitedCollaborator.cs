#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public sealed class InvitedCollaborator
    {
        [XmlText]
        public string Content { get; set; }

        public override string ToString()
        {
            return "[Content=" + Content + "]";
        }
    }
}
