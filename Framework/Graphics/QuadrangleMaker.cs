#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// 四角形の頂点データを表し、VertexSource への追加を行う構造体です。
    /// </summary>
    public struct QuadrangleMaker
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
        /// VertexSource へ頂点データを設定します。
        /// </summary>
        /// <param name="source">頂点データが設定される VertexSource。</param>
        public void Make(VertexSource<VertexPositionNormal, ushort> source)
        {
            source.AddIndex((ushort) (source.CurrentVertex + 0));
            source.AddIndex((ushort) (source.CurrentVertex + 1));
            source.AddIndex((ushort) (source.CurrentVertex + 2));
            source.AddIndex((ushort) (source.CurrentVertex + 0));
            source.AddIndex((ushort) (source.CurrentVertex + 2));
            source.AddIndex((ushort) (source.CurrentVertex + 3));

            source.AddVertex(new VertexPositionNormal(Position0, Normal));
            source.AddVertex(new VertexPositionNormal(Position1, Normal));
            source.AddVertex(new VertexPositionNormal(Position2, Normal));
            source.AddVertex(new VertexPositionNormal(Position3, Normal));
        }
    }
}
