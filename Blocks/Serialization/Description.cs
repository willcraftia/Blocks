#region Using

using System;
using System.Text;
using System.Xml.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    [XmlRoot("D")]
    public sealed class Description<T> : IEquatable<Description<T>>
    {
        //
        // MEMO
        //
        // コンテンツに付属させる名前やタイムスタンプは、
        // 実ゲームでは取り扱う必要がないため、
        // 一対一の関係を持つ Description として分離させ、
        // 別ファイルの XML として管理します。
        //
        // また、コンテンツの形式は実装の初期段階で確定すると思いますが、
        // エディタ用にコンテンツへ付属させる情報は、
        // 行う対応とともに形式を変化させる可能性が高いため、
        // この変化の範囲を分離させる意味もあります。
        //
        // 規約として、Content と Description は、
        // 拡張子を除いたファイル名を一致させます。
        // Description の拡張子は .description であり、
        // Content の拡張子はその型に依存します (例えば Block ならば .block)。
        // そして、それらは必ず対で管理します。
        //


        //
        // TODO
        //
        // コンテンツ サーバを構築する際にはユーザ情報なども含める。
        // 今は FileName を ID としているが、その辺りも再考する必要あり。
        //

        public const string Extension = ".description";

        [XmlAttribute("N")]
        public string Name { get; set; }

        [XmlIgnore]
        public DateTime CreationTime { get; set; }

        [XmlAttribute("CT")]
        public long CreationTimeTicks
        {
            get { return CreationTime.Ticks; }
            set { CreationTime = new DateTime(value); }
        }

        [XmlIgnore]
        public DateTime LastAccessTime { get; set; }

        [XmlAttribute("LAT")]
        public long LastAccessTimeTicks
        {
            get { return LastAccessTime.Ticks; }
            set { LastAccessTime = new DateTime(value); }
        }

        [XmlIgnore]
        public DateTime LastWriteTime { get; set; }

        [XmlAttribute("LWT")]
        public long LastWriteTimeTicks
        {
            get { return LastWriteTime.Ticks; }
            set { LastWriteTime = new DateTime(value); }
        }

        public Description()
        {
            CreationTime = DateTime.Now;
            LastAccessTime = DateTime.Now;
            LastWriteTime = DateTime.Now;
        }

        public static string ResolveFileName(string name)
        {
            return name + Extension;
        }

        #region Equatable

        public static bool operator ==(Description<T> o1, Description<T> o2)
        {
            return o1.Equals(o2);
        }

        public static bool operator !=(Description<T> o1, Description<T> o2)
        {
            return !o1.Equals(o2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType()) return false;

            return Equals((Description<T>) obj);
        }

        public bool Equals(Description<T> other)
        {
            return Name == other.Name;
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        #endregion

        #region ToString

        public override string ToString()
        {
            return "[Name=" + Name +
                ", CreationTime=" + CreationTime +
                ", LastAccessTime=" + LastAccessTime +
                ", LastWriteTime=" + LastWriteTime +
                "]";
        }

        #endregion
    }
}
