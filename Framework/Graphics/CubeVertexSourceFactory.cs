#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// 立方体の VertexSource を生成するクラスです。
    /// </summary>
    public sealed class CubeVertexSourceFactory : IVertexSourceFactory<VertexPositionNormal>
    {
        #region SurfaceMaker

        /// <summary>
        /// 面の頂点データを表し、VertexSource への追加を行う構造体です。
        /// </summary>
        struct SurfaceMaker
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
            public void Make(VertexSource<VertexPositionNormal> source)
            {
                source.AddIndex(source.CurrentVertex + 0);
                source.AddIndex(source.CurrentVertex + 1);
                source.AddIndex(source.CurrentVertex + 2);
                source.AddIndex(source.CurrentVertex + 0);
                source.AddIndex(source.CurrentVertex + 2);
                source.AddIndex(source.CurrentVertex + 3);

                source.AddVertex(new VertexPositionNormal(Position0, Normal));
                source.AddVertex(new VertexPositionNormal(Position1, Normal));
                source.AddVertex(new VertexPositionNormal(Position2, Normal));
                source.AddVertex(new VertexPositionNormal(Position3, Normal));
            }
        }

        #endregion

        /// <summary>
        /// 立方体の辺のサイズを取得または設定します。
        /// </summary>
        public float Size { get; set; }
        
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public CubeVertexSourceFactory()
        {
            Size = 1;
        }

        // I/F
        public VertexSource<VertexPositionNormal> CreateVertexSource()
        {
            var source = new VertexSource<VertexPositionNormal>();
            var maker = new SurfaceMaker();
            float s = Size * 0.5f;

            // Top
            maker.Position0 = new Vector3(-s, s, s);
            maker.Position1 = new Vector3(-s, s, -s);
            maker.Position2 = new Vector3(s, s, -s);
            maker.Position3 = new Vector3(s, s, s);
            maker.Normal = new Vector3(0, 1, 0);
            maker.Make(source);

            // Bottom
            maker.Position0 = new Vector3(s, -s, s);
            maker.Position1 = new Vector3(s, -s, -s);
            maker.Position2 = new Vector3(-s, -s, -s);
            maker.Position3 = new Vector3(-s, -s, s);
            maker.Normal = new Vector3(0, -1, 0);
            maker.Make(source);

            // North
            maker.Position0 = new Vector3(s, s, s);
            maker.Position1 = new Vector3(s, -s, s);
            maker.Position2 = new Vector3(-s, -s, s);
            maker.Position3 = new Vector3(-s, s, s);
            maker.Normal = new Vector3(0, 0, 1);
            maker.Make(source);

            // South
            maker.Position0 = new Vector3(-s, s, -s);
            maker.Position1 = new Vector3(-s, -s, -s);
            maker.Position2 = new Vector3(s, -s, -s);
            maker.Position3 = new Vector3(s, s, -s);
            maker.Normal = new Vector3(0, 0, -1);
            maker.Make(source);

            // East
            maker.Position0 = new Vector3(s, s, -s);
            maker.Position1 = new Vector3(s, -s, -s);
            maker.Position2 = new Vector3(s, -s, s);
            maker.Position3 = new Vector3(s, s, s);
            maker.Normal = new Vector3(1, 0, 0);
            maker.Make(source);

            // West
            maker.Position0 = new Vector3(-s, s, s);
            maker.Position1 = new Vector3(-s, -s, s);
            maker.Position2 = new Vector3(-s, -s, -s);
            maker.Position3 = new Vector3(-s, s, -s);
            maker.Normal = new Vector3(-1, 0, 0);
            maker.Make(source);

            return source;
        }
    }
}
