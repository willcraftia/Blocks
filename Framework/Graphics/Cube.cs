#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    public struct Cube
    {
        /// <summary>
        /// 面の法線の配列 (Backward、Forward、Right、Left、Up、Down の順)。
        /// </summary>
        static Vector3[] normals;

        static Cube()
        {
            normals = new Vector3[]
            {
                new Vector3(0, 0, 1),
                new Vector3(0, 0, -1),
                new Vector3(1, 0, 0),
                new Vector3(-1, 0, 0),
                new Vector3(0, 1, 0),
                new Vector3(0, -1, 0),
            };
        }

        /// <summary>
        /// 面の色の配列 (順序は法線の配列と同じ)。
        /// </summary>
        Color[] colors;

        /// <summary>
        /// 立方体の辺のサイズ。
        /// </summary>
        public float Size;

        /// <summary>
        /// 法線 [0, 0, 1] の面の色を取得または設定します。
        /// </summary>
        public Color BackwardColor
        {
            get
            {
                EnsureColors();
                return colors[0];
            }
            set
            {
                EnsureColors();
                colors[0] = value;
            }
        }

        /// <summary>
        /// 法線 [0, 0, -1] の面の色を取得または設定します。
        /// </summary>
        public Color ForwardColor
        {
            get
            {
                EnsureColors();
                return colors[1];
            }
            set
            {
                EnsureColors();
                colors[1] = value;
            }
        }

        /// <summary>
        /// 法線 [1, 0, 0] の面の色を取得または設定します。
        /// </summary>
        public Color RightColor
        {
            get
            {
                EnsureColors();
                return colors[2];
            }
            set
            {
                EnsureColors();
                colors[2] = value;
            }
        }

        /// <summary>
        /// 法線 [-1, 0, 0] の面の色を取得または設定します。
        /// </summary>
        public Color LeftColor
        {
            get
            {
                EnsureColors();
                return colors[3];
            }
            set
            {
                EnsureColors();
                colors[3] = value;
            }
        }

        /// <summary>
        /// 法線 [0, 1, 0] の面の色を取得または設定します。
        /// </summary>
        public Color UpColor
        {
            get
            {
                EnsureColors();
                return colors[4];
            }
            set
            {
                EnsureColors();
                colors[4] = value;
            }
        }

        /// <summary>
        /// 法線 [0, -1, 0] の面の色を取得または設定します。
        /// </summary>
        public Color DownColor
        {
            get
            {
                EnsureColors();
                return colors[5];
            }
            set
            {
                EnsureColors();
                colors[5] = value;
            }
        }

        /// <summary>
        /// VertexSource へ頂点データを設定します。
        /// </summary>
        /// <param name="source">頂点データが設定される VertexSource。</param>
        public void Make(VertexSource<VertexPositionNormal, ushort> source)
        {
            var quad = new Quadrangle();

            for (int i = 0; i < normals.Length; i++)
            {
                ResolvePositionNormal(ref quad, ref normals[i]);
                quad.Make(source);
            }
        }

        /// <summary>
        /// VertexSource へ頂点データを設定します。
        /// </summary>
        /// <param name="source">頂点データが設定される VertexSource。</param>
        public void Make(VertexSource<VertexPositionNormalColor, ushort> source)
        {
            EnsureColors();

            var quad = new Quadrangle();

            for (int i = 0; i < normals.Length; i++)
            {
                ResolvePositionNormal(ref quad, ref normals[i]);
                quad.Color = colors[i];
                quad.Make(source);
            }
        }

        void ResolvePositionNormal(ref Quadrangle quadrangle, ref Vector3 normal)
        {
            var halfSize = Size * 0.5f;
            var side1 = new Vector3(normal.Y, normal.Z, normal.X);
            var side2 = Vector3.Cross(normal, side1);

            // REFERENCE: 立方体頂点計算アルゴリズムは XNA の Primitive3D サンプル コードより。

            quadrangle.Position0 = (normal - side1 - side2) * halfSize;
            quadrangle.Position1 = (normal - side1 + side2) * halfSize;
            quadrangle.Position2 = (normal + side1 + side2) * halfSize;
            quadrangle.Position3 = (normal + side1 - side2) * halfSize;
            quadrangle.Normal = normal;
        }

        void EnsureColors()
        {
            if (colors == null) colors = new Color[6];
        }
    }
}
