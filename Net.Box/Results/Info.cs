#region Using

using System;
using System.Text;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Net.Box.Results
{
    public sealed class Info
    {
        [XmlElement("file_id")]
        public long FileId { get; set; }

        [XmlElement("file_name")]
        public string FileName { get; set; }

        [XmlElement("folder_id")]
        public long FolderId { get; set; }

        [XmlIgnore]
        public bool Shared { get; set; }

        /// <summary>
        /// string へマッピングさせてから解析した値を Shared プロパティへ設定します。
        /// </summary>
        [XmlElement("shared")]
        public string SharedString
        {
            get { return Shared ? "1" : "0"; }
            set { Shared = ("1" == value); }
        }

        [XmlElement("shared_name")]
        public string SharedName { get; set; }

        [XmlElement("size")]
        public long Size { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("sha1")]
        public string Sha1 { get; set; }

        [XmlElement("created")]
        public long Created { get; set; }

        [XmlElement("updated")]
        public long Updated { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("[FileId=").Append(FileId);
            sb.Append(", FileName=").Append(FileName);
            sb.Append(", FolderId=").Append(FolderId);
            sb.Append(", Shared=").Append(Shared);
            sb.Append(", SharedName=").Append(SharedName);
            sb.Append(", Size=").Append(Size);
            sb.Append(", Description=").Append(Description);
            sb.Append(", Sha1=").Append(Sha1);
            sb.Append(", Created=").Append(Created);
            sb.Append(", Updated=").Append(Updated);
            sb.Append("]");
            return sb.ToString();
        }
    }
}
