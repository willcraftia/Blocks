#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// 四角形の頂点データを表す構造体です。
    /// </summary>
    public struct Quadrangle
    {
        /// <summary>
        /// 位置 0。
        /// </summary>
        public Vector3 Position0;

        /// <summary>
        /// 位置 1。
        /// </summary>
        public Vector3 Position1;

        /// <summary>
        /// 位置 2。
        /// </summary>
        public Vector3 Position2;

        /// <summary>
        /// 位置 3。
        /// </summary>
        public Vector3 Position3;

        /// <summary>
        /// 法線。
        /// </summary>
        public Vector3 Normal;

        /// <summary>
        /// 色。
        /// </summary>
        public Color Color;

        /// <summary>
        /// VertexSource へ頂点データを設定します。
        /// </summary>
        /// <param name="source">頂点データが設定される VertexSource。</param>
        public void Make(VertexSource<VertexPositionNormal, ushort> source)
        {
            MakeIndices(source);

            source.AddVertex(new VertexPositionNormal(Position0, Normal));
            source.AddVertex(new VertexPositionNormal(Position1, Normal));
            source.AddVertex(new VertexPositionNormal(Position2, Normal));
            source.AddVertex(new VertexPositionNormal(Position3, Normal));
        }

        /// <summary>
        /// VertexSource へ頂点データを設定します。
        /// </summary>
        /// <param name="source">頂点データが設定される VertexSource。</param>
        public void Make(VertexSource<VertexPositionNormalColor, ushort> source)
        {
            MakeIndices(source);

            source.AddVertex(new VertexPositionNormalColor(Position0, Normal, Color));
            source.AddVertex(new VertexPositionNormalColor(Position1, Normal, Color));
            source.AddVertex(new VertexPositionNormalColor(Position2, Normal, Color));
            source.AddVertex(new VertexPositionNormalColor(Position3, Normal, Color));
        }

        void MakeIndices<TVertex>(VertexSource<TVertex, ushort> source)
            where TVertex : struct
        {
            source.AddIndex((ushort) (source.CurrentVertex + 0));
            source.AddIndex((ushort) (source.CurrentVertex + 1));
            source.AddIndex((ushort) (source.CurrentVertex + 2));
            source.AddIndex((ushort) (source.CurrentVertex + 0));
            source.AddIndex((ushort) (source.CurrentVertex + 2));
            source.AddIndex((ushort) (source.CurrentVertex + 3));
        }
    }
}
