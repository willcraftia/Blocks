#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// 立方体 (Cube) の GeometricPrimitive を生成するファクトリです。
    /// </summary>
    public sealed class CubePrimitiveFactory : GeometricPrimitiveFactory
    {
        /// <summary>
        /// 立方体の辺のサイズを取得または設定します。
        /// </summary>
        public float Size { get; set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public CubePrimitiveFactory()
        {
            Size = 1;
        }

        // Impl
        protected override void Initialize(VertexSource source)
        {
            Vector3[] normals =
            {
                new Vector3(0, 0, 1),
                new Vector3(0, 0, -1),
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, -1, 0),
            };

            foreach (var normal in normals)
            {
                var side1 = new Vector3(normal.Y, normal.Z, normal.X);
                var side2 = Vector3.Cross(normal, side1);

                source.AddIndex(source.CurrentVertex + 0);
                source.AddIndex(source.CurrentVertex + 1);
                source.AddIndex(source.CurrentVertex + 2);

                source.AddIndex(source.CurrentVertex + 0);
                source.AddIndex(source.CurrentVertex + 2);
                source.AddIndex(source.CurrentVertex + 3);

                var v = (normal - side1 - side2) * Size * 0.5f;
                source.AddVertex((normal - side1 - side2) * Size * 0.5f, normal);
                source.AddVertex((normal - side1 + side2) * Size * 0.5f, normal);
                source.AddVertex((normal + side1 + side2) * Size * 0.5f, normal);
                source.AddVertex((normal + side1 - side2) * Size * 0.5f, normal);
            }
        }
    }
}
