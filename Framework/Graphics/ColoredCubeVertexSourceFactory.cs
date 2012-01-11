#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// 頂点色を持つ立方体の VertexSource を生成するクラスです。
    /// </summary>
    public sealed class ColoredCubeVertexSourceFactory : IVertexSourceFactory<VertexPositionNormalColor, ushort>
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

        #endregion

        /// <summary>
        /// 立方体の辺のサイズを取得または設定します。
        /// </summary>
        public float Size { get; set; }

        /// <summary>
        /// 法線 [0, 1, 0] の面の色を取得または設定します。
        /// </summary>
        public Color TopSurfaceColor { get; set; }

        /// <summary>
        /// 法線 [0, -1, 0] の面の色を取得または設定します。
        /// </summary>
        public Color BottomSurfaceColor { get; set; }

        /// <summary>
        /// 法線 [0, 0, 1] の面の色を取得または設定します。
        /// </summary>
        public Color NorthSurfaceColor { get; set; }

        /// <summary>
        /// 法線 [0, 0, -1] の面の色を取得または設定します。
        /// </summary>
        public Color SouthSurfaceColor { get; set; }

        /// <summary>
        /// 法線 [1, 0, 0] の面の色を取得または設定します。
        /// </summary>
        public Color EastSurfaceColor { get; set; }

        /// <summary>
        /// 法線 [-1, 0, 0] の面の色を取得または設定します。
        /// </summary>
        public Color WestSurfaceColor { get; set; }
        
        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public ColoredCubeVertexSourceFactory()
        {
            Size = 1;
            TopSurfaceColor = Color.White;
            BottomSurfaceColor = Color.White;
            NorthSurfaceColor = Color.White;
            SouthSurfaceColor = Color.White;
            EastSurfaceColor = Color.White;
            WestSurfaceColor = Color.White;
        }

        // I/F
        public VertexSource<VertexPositionNormalColor, ushort> CreateVertexSource()
        {
            var source = new VertexSource<VertexPositionNormalColor, ushort>();
            var maker = new SurfaceMaker();
            float s = Size * 0.5f;

            // REFERENCE: 立方体頂点計算アルゴリズムは XNA の Primitive3D サンプル コードより。

            Vector3[] normals =
            {
                new Vector3(0, 0, 1),
                new Vector3(0, 0, -1),
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, -1, 0),
            };

            Color[] colors =
            {
                NorthSurfaceColor,
                SouthSurfaceColor,
                WestSurfaceColor,
                EastSurfaceColor,
                TopSurfaceColor,
                BottomSurfaceColor
            };

            for (int i = 0; i < normals.Length; i++)
            {
                var normal = normals[i];
                var side1 = new Vector3(normal.Y, normal.Z, normal.X);
                var side2 = Vector3.Cross(normal, side1);

                maker.Position0 = (normal - side1 - side2) * s;
                maker.Position1 = (normal - side1 + side2) * s;
                maker.Position2 = (normal + side1 + side2) * s;
                maker.Position3 = (normal + side1 - side2) * s;
                maker.Normal = normal;
                maker.Color = colors[i];
                maker.Make(source);
            }

            return source;
        }
    }
}
