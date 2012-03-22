#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box
{
    public enum Role
    {
        [XmlEnum("editor")]
        Editor,

        [XmlEnum("viewer")]
        Viewer
    }

    public static class RoleExtension
    {
        public static string ToParameterValue(this Role role)
        {
            switch (role)
            {
                case Role.Editor:
                    return "editor";
                case Role.Viewer:
                    return "viewer";
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
