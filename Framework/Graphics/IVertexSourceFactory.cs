#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// VertexSource を生成するクラスのインタフェースです。
    /// </summary>
    /// <typeparam name="TVertex">頂点構造体の型。</typeparam>
    /// <typeparam name="TIndex">インデックスの型。</typeparam>
    [Obsolete("", true)]
    public interface IVertexSourceFactory<TVertex, TIndex>
        where TVertex : struct
        where TIndex : struct
    {
        /// <summary>
        /// VertexSource を生成します。
        /// </summary>
        /// <returns>生成された VertexSource。</returns>
        VertexSource<TVertex, TIndex> CreateVertexSource();
    }
}
