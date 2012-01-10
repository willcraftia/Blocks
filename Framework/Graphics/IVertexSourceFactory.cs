#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// VertexSource を生成するクラスのインタフェースです。
    /// </summary>
    /// <typeparam name="T">頂点構造体の型。</typeparam>
    public interface IVertexSourceFactory<T> where T : struct
    {
        /// <summary>
        /// VertexSource を生成します。
        /// </summary>
        /// <returns>生成された VertexSource。</returns>
        VertexSource<T> CreateVertexSource();
    }
}
