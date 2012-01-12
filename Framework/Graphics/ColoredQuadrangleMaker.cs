#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// 頂点色を含む四角形の頂点データを表し、VertexSource への追加を行う構造体です。
    /// </summary>
    public struct ColoredQuadrangleMaker
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
        public void Make(VertexSource<VertexPositionNormalColor, ushort> source)
        {
            source.AddIndex((ushort) (source.CurrentVertex + 0));
            source.AddIndex((ushort) (source.CurrentVertex + 1));
            source.AddIndex((ushort) (source.CurrentVertex + 2));
            source.AddIndex((ushort) (source.CurrentVertex + 0));
            source.AddIndex((ushort) (source.CurrentVertex + 2));
            source.AddIndex((ushort) (source.CurrentVertex + 3));

            source.AddVertex(new VertexPositionNormalColor(Position0, Normal, Color));
            source.AddVertex(new VertexPositionNormalColor(Position1, Normal, Color));
            source.AddVertex(new VertexPositionNormalColor(Position2, Normal, Color));
            source.AddVertex(new VertexPositionNormalColor(Position3, Normal, Color));
        }
    }
}
