#region Using

using System;
using System.Runtime.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Serialization
{
    /// <summary>
    /// BlockMesh の Material を表すクラスです。
    /// </summary>
    /// <remarks>
    /// Material では透明度 (Alpha) を管理できないものとします。
    /// 
    /// Block を描画モデルへ変換する際には、Element 同士の隣接状態から最小限必要な頂点データだけが選択されますが、
    /// Material で Alpha を管理することを考えた場合、この処理をどのように行うかが問題となってしまいます。
    /// 例えば、半透明な Element は隣接していないと判定した場合、描画モデル構築における頂点データの削減が阻まれます。
    /// あるいは、半透明な Element も隣接していると判定した場合、頂点データの削減には貢献しますが、
    /// 不自然な見た目の描画モデルが構築されてしまいます。
    /// 
    /// これらの問題について深く検討したくないため、Material では透明度を管理しないものとしています。
    /// </remarks>
    [DataContract]
    public sealed class Material
    {
        /// <summary>
        /// Diffuse 色。
        /// </summary>
        [DataMember]
        public MaterialColor DiffuseColor { get; set; }

        /// <summary>
        /// Emissive 色。
        /// </summary>
        [DataMember]
        public MaterialColor EmissiveColor { get; set; }

        /// <summary>
        /// Specular 色。
        /// </summary>
        [DataMember]
        public MaterialColor SpecularColor { get; set; }

        /// <summary>
        /// Specular 係数。
        /// </summary>
        [DataMember]
        public float SpecularPower { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public Material()
        {
            // デフォルト値は BasicEffect のデフォルト値に合わせています。
            DiffuseColor = new MaterialColor(255, 255, 255);
            EmissiveColor = new MaterialColor(0, 0, 0);
            SpecularColor = new MaterialColor(255, 255, 255);
            SpecularPower = 16;
        }

        #region ToString

        public override string ToString()
        {
            return "[DiffuseColor=" + DiffuseColor +
                ", EmissiveColor=" + EmissiveColor +
                ", SpecularColor=" + SpecularColor +
                ", SpecularPower=" + SpecularPower + "]";
        }

        #endregion
    }
}
