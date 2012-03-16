#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// BlockMesh の頂点情報とエフェクト情報を管理するクラスです。
    /// このクラスは、CPU メモリ上で頂点情報とエフェクト情報を管理します。
    /// </summary>
    public sealed class InterBlockMesh
    {
        /// <summary>
        /// InterBlockEffect のリスト。
        /// </summary>
        ReadOnlyCollection<InterBlockEffect> effects;

        /// <summary>
        /// LOD ごとの InterBlockMeshPart のリスト。
        /// </summary>
        ReadOnlyCollection<InterBlockMeshPart>[] lodMeshParts;

        /// <summary>
        /// InterBlockEffect のリストを取得します。
        /// </summary>
        public ReadOnlyCollection<InterBlockEffect> Effects
        {
            get { return effects; }
        }

        /// <summary>
        /// InterBlockMeshPart のリストを取得します。
        /// </summary>
        public ReadOnlyCollection<InterBlockMeshPart> MeshParts
        {
            get { return lodMeshParts[LevelOfDetail]; }
        }

        /// <summary>
        /// 管理している LOD の数を取得します。
        /// </summary>
        public int LevelOfDetailSize
        {
            get { return lodMeshParts.Length; }
        }

        /// <summary>
        /// 利用する LOD レベルを取得または設定します。
        /// </summary>
        public int LevelOfDetail { get; set; }

        /// <summary>
        /// インスタンスを生成します (内部処理用)。
        /// </summary>
        internal InterBlockMesh(int lodSize)
        {
            lodMeshParts = new ReadOnlyCollection<InterBlockMeshPart>[lodSize];
        }

        /// <summary>
        /// InterBlockEffect 配列を設定します。
        /// </summary>
        /// <param name="effects">InterBlockEffect 配列。</param>
        internal void SetEffectArray(InterBlockEffect[] effects)
        {
            this.effects = new ReadOnlyCollection<InterBlockEffect>(effects);
        }

        /// <summary>
        /// 指定の LOD の InterBlockMeshPart 配列を設定します。
        /// </summary>
        /// <param name="lod">LOD。</param>
        /// <param name="meshParts">InterBlockMeshPart 配列。</param>
        internal void SetLODMeshPartArray(int lod, InterBlockMeshPart[] meshParts)
        {
            lodMeshParts[lod] = new ReadOnlyCollection<InterBlockMeshPart>(meshParts);
        }

        /// <summary>
        /// 指定の LOD の BlockMeshPart リストに上位と同じ物を設定します。
        /// </summary>
        /// <param name="lod">LOD。</param>
        internal void InheritLODMeshParts(int lod)
        {
            if (lod == 0) throw new ArgumentOutOfRangeException("lod");

            lodMeshParts[lod] = lodMeshParts[lod - 1];
        }
    }
}
