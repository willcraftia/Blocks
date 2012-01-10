#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// 立方体 (Cube) の GeometricPrimitive を生成するクラスです。
    /// </summary>
    public sealed class CubePrimitiveFactory : GeometricPrimitiveFactory
    {
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
        public CubePrimitiveFactory()
        {
            Size = 1;
            TopSurfaceColor = Color.White;
            BottomSurfaceColor = Color.White;
            NorthSurfaceColor = Color.White;
            SouthSurfaceColor = Color.White;
            EastSurfaceColor = Color.White;
            WestSurfaceColor = Color.White;
        }

        // Impl
        protected override void Initialize(VertexSource source)
        {
            float s = Size * 0.5f;
            Vector3 normal = new Vector3();

            // Top
            AddIndicesPerSurface(source);
            normal.X = 0;
            normal.Y = 1;
            normal.Z = 0;
            source.AddVertex(new Vector3(-s, s, s), normal, TopSurfaceColor);
            source.AddVertex(new Vector3(-s, s, -s), normal, TopSurfaceColor);
            source.AddVertex(new Vector3(s, s, -s), normal, TopSurfaceColor);
            source.AddVertex(new Vector3(s, s, s), normal, TopSurfaceColor);

            // Bottom
            AddIndicesPerSurface(source);
            normal.X = 0;
            normal.Y = -1;
            normal.Z = 0;
            source.AddVertex(new Vector3(s, -s, s), normal, BottomSurfaceColor);
            source.AddVertex(new Vector3(s, -s, -s), normal, BottomSurfaceColor);
            source.AddVertex(new Vector3(-s, -s, -s), normal, BottomSurfaceColor);
            source.AddVertex(new Vector3(-s, -s, s), normal, BottomSurfaceColor);

            // North
            AddIndicesPerSurface(source);
            normal.X = 0;
            normal.Y = 0;
            normal.Z = 1;
            source.AddVertex(new Vector3(s, s, s), normal, NorthSurfaceColor);
            source.AddVertex(new Vector3(s, -s, s), normal, NorthSurfaceColor);
            source.AddVertex(new Vector3(-s, -s, s), normal, NorthSurfaceColor);
            source.AddVertex(new Vector3(-s, s, s), normal, NorthSurfaceColor);

            // South
            AddIndicesPerSurface(source);
            normal.X = 0;
            normal.Y = 0;
            normal.Z = -1;
            source.AddVertex(new Vector3(-s, s, -s), normal, SouthSurfaceColor);
            source.AddVertex(new Vector3(-s, -s, -s), normal, SouthSurfaceColor);
            source.AddVertex(new Vector3(s, -s, -s), normal, SouthSurfaceColor);
            source.AddVertex(new Vector3(s, s, -s), normal, SouthSurfaceColor);

            // East
            AddIndicesPerSurface(source);
            normal.X = 1;
            normal.Y = 0;
            normal.Z = 0;
            source.AddVertex(new Vector3(s, s, -s), normal, EastSurfaceColor);
            source.AddVertex(new Vector3(s, -s, -s), normal, EastSurfaceColor);
            source.AddVertex(new Vector3(s, -s, s), normal, EastSurfaceColor);
            source.AddVertex(new Vector3(s, s, s), normal, EastSurfaceColor);

            // West
            AddIndicesPerSurface(source);
            normal.X = -1;
            normal.Y = 0;
            normal.Z = 0;
            source.AddVertex(new Vector3(-s, s, s), normal, WestSurfaceColor);
            source.AddVertex(new Vector3(-s, -s, s), normal, WestSurfaceColor);
            source.AddVertex(new Vector3(-s, -s, -s), normal, WestSurfaceColor);
            source.AddVertex(new Vector3(-s, s, -s), normal, WestSurfaceColor);
        }

        void AddIndicesPerSurface(VertexSource source)
        {
            source.AddIndex(source.CurrentVertex + 0);
            source.AddIndex(source.CurrentVertex + 1);
            source.AddIndex(source.CurrentVertex + 2);
            source.AddIndex(source.CurrentVertex + 0);
            source.AddIndex(source.CurrentVertex + 2);
            source.AddIndex(source.CurrentVertex + 3);
        }
    }
}
