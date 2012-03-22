#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box
{
    public enum Target
    {
        [XmlEnum("folder")]
        Folder,

        [XmlEnum("file")]
        File
    }

    public static class TargetExtension
    {
        public static string ToParameterValue(this Target target)
        {
            switch (target)
            {
                case Target.Folder:
                    return "folder";
                case Target.File:
                    return "file";
                default:
                    throw new InvalidOperationException();
            }
        }
    }
}
