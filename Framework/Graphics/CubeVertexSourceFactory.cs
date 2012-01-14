#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// 立方体の VertexSource を生成するクラスです。
    /// </summary>
    public sealed class CubeVertexSourceFactory : IVertexSourceFactory<VertexPositionNormal, ushort>
    {
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
        public VertexSource<VertexPositionNormal, ushort> CreateVertexSource()
        {
            var source = new VertexSource<VertexPositionNormal, ushort>();
            var quadrangle = new Quadrangle();
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

            foreach (var normal in normals)
            {
                var side1 = new Vector3(normal.Y, normal.Z, normal.X);
                var side2 = Vector3.Cross(normal, side1);

                quadrangle.Position0 = (normal - side1 - side2) * s;
                quadrangle.Position1 = (normal - side1 + side2) * s;
                quadrangle.Position2 = (normal + side1 + side2) * s;
                quadrangle.Position3 = (normal + side1 - side2) * s;
                quadrangle.Normal = normal;
                quadrangle.Make(source);
            }

            return source;
        }
    }
}
