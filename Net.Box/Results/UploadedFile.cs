#region Using

using System;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public sealed class UploadedFile
    {
        [XmlAttribute("file_name")]
        public string FileName { get; set; }

        [XmlAttribute("id")]
        public long Id { get; set; }

        [XmlAttribute("folder_id")]
        public long FolderId { get; set; }

        [XmlIgnore]
        public bool Shared { get; set; }

        /// <summary>
        /// string へマッピングさせてから解析した値を Shared プロパティへ設定します。
        /// </summary>
        [XmlAttribute("shared")]
        public string SharedString
        {
            get { return Shared ? "1" : "0"; }
            set { Shared = ("1" == value); }
        }

        [XmlAttribute("public_name")]
        public string PublicName { get; set; }

        [XmlAttribute("error")]
        public string Error { get; set; }

        public override string ToString()
        {
            return "[FileName=" + FileName +
                ", Id=" + Id +
                ", FolderId=" + FolderId +
                ", Shared=" + Shared +
                ", PublicName=" + PublicName +
                ", Error=" + Error +
                "]";
        }
    }
}
