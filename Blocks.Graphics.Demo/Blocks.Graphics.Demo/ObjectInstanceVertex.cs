#region Using

using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics.Demo
{
    /// <summary>
    /// オブジェクトインスタンス格納用の頂点データ
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ObjectInstanceVertex : IVertexType
    {
        // 座標
        public Vector3 Position;
        // スケール
        public float Scale;
        // 回転軸
        public Vector3 RotateAxis;
        // 回転
        public float Rotation;

        /// <summary>
        /// 頂点宣言
        /// </summary>
        public readonly static VertexDeclaration VertexDecl = new VertexDeclaration(
            Marshal.SizeOf(typeof(ObjectInstanceVertex)),
            // Position, Scale (16バイト)
            new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1),
            // RotateAxis, Rotation (16バイト)
            new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 2)
        );

        /// <summary>
        /// 頂点宣言の取得
        /// </summary>
        public VertexDeclaration VertexDeclaration
        {
            get { return VertexDecl; }
        }
    }
}
