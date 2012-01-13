#region Using

using System;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Graphics.Demo
{
    /// <summary>
    /// ゲームオブジェクト構造体
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct GameObject : IVertexType
    {
        // CPUで更新し、GPUで読み込まれる変数
        // 座標
        public Vector3 Position;
        // スケール
        public float Scale;
        // 回転軸
        public Vector3 RotateAxis;
        // 回転
        public float Rotation;

        // CPUのみで使用する変数
        // 速度
        public Vector3 Velocity;
        // 回転速度
        public float RotationSpeed;
        // 寿命
        public float LifeTime;

        /// <summary>
        /// 頂点宣言
        /// </summary>
        public readonly static VertexDeclaration VertexDeclaration = new VertexDeclaration(
            Marshal.SizeOf(typeof(GameObject)),
            // Position, Scale (16バイト)
            new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 1),
            // RotateAxis, Rotation (16バイト)
            new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 2)
        );

        // I/F
        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }

        /// <summary>
        /// ゲームオブジェクトの初期化
        /// </summary>
        public void Initialize(Random rnd)
        {
            // 出現位置、移動速度などをランダムに生成する
            float w = BlockModelViewGame.Sandbox.Max.X - BlockModelViewGame.Sandbox.Min.X;
            float h = BlockModelViewGame.Sandbox.Max.Y - BlockModelViewGame.Sandbox.Min.Y;

            Position = new Vector3(
                BlockModelViewGame.Sandbox.Min.X + w * (float) rnd.NextDouble(),
                BlockModelViewGame.Sandbox.Min.Y + h * (float) rnd.NextDouble(),
                (float) rnd.NextDouble() * -300.0f);
            //Position = new Vector3(
            //    BlockModelViewGame.Sandbox.Min.X + w * (float) rnd.NextDouble(),
            //    BlockModelViewGame.Sandbox.Min.Y + h * (float) rnd.NextDouble(),
            //    30 - 90);

            float rad = (float) rnd.NextDouble() * MathHelper.TwoPi;
            Velocity = new Vector3((float) Math.Cos(rad), (float) Math.Sin(rad), 0) * 3.0f;

            float theta = (float) rnd.NextDouble() * MathHelper.TwoPi;
            float phi = (float) rnd.NextDouble() * MathHelper.TwoPi;
            RotateAxis = Vector3.Transform(Vector3.UnitX, Matrix.CreateFromYawPitchRoll(theta, 0, phi));
            Rotation = (float) rnd.NextDouble() * MathHelper.TwoPi;
            RotationSpeed = (float) rnd.NextDouble() * MathHelper.TwoPi;

            //Scale = 0.5f + (float) rnd.NextDouble();
            Scale = 1;
            LifeTime = 20.0f + 10.0f * (float) rnd.NextDouble();
        }

        /// <summary>
        /// ゲームオブジェクトの更新
        /// </summary>
        /// <param name="dt">更新時間</param>
        /// <returns>寿命が尽きたらfalseを返す</returns>
        public bool Update(float dt)
        {
            // 座標と回転の更新
            Position.X += Velocity.X * dt;
            Position.Y += Velocity.Y * dt;
            Position.Z += Velocity.Z * dt;
            Rotation = NormalizeRotation(Rotation + RotationSpeed * dt);

            // 移動範囲の端に来たら跳ね返る処理
            if ((Position.X < BlockModelViewGame.Sandbox.Min.X && Velocity.X < 0) ||
                (Position.X > BlockModelViewGame.Sandbox.Max.X && Velocity.X > 0))
            {
                Velocity.X = -Velocity.X;
            }

            if ((Position.Y < BlockModelViewGame.Sandbox.Min.Y && Velocity.Y < 0) ||
                (Position.Y > BlockModelViewGame.Sandbox.Max.Y && Velocity.Y > 0))
            {
                Velocity.Y = -Velocity.Y;
            }

            // 寿命の更新
            LifeTime -= dt;

            return LifeTime > 0.0f;
        }

        /// <summary>
        /// ラジアン角度を0～TwoPiの範囲に正規化する
        /// </summary>
        /// <param name="value"></param>
        /// <remarks>
        /// x86系のCPUの場合、SinやCosに0～TwoPi以外の極端に大きいまたは小さい値を
        /// 使って計算すると時間が掛かるのと、テイラー展開する場合にも
        /// 角度の値は0～TwoPiの間に限定されていると計算しやすいので正規化する。
        /// </remarks>
        static float NormalizeRotation(float value)
        {
            // 剰余算(%)を使った方が簡潔に書けるが、元々の角度が0～TwoPiに近い値の場合、
            // 以下の方法の方が速い
            while (value < 0.0f) value += MathHelper.TwoPi;
            while (value > MathHelper.TwoPi) value -= MathHelper.TwoPi;

            return value;
        }
    }
}
