﻿#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Cameras
{
    /// <summary>
    /// 注視点を中心にした球面上を移動するカメラの View 行列を管理するクラスです。
    /// カメラは常に注視点を向くため、その姿勢における Yaw/Pitch はカメラ位置から決定されます。
    /// 現時点ではカメラの姿勢の Roll を考慮する予定がないため、Roll には対応していません。
    /// </summary>
    public class ChaseView : ViewBase
    {
        /// <summary>
        /// 球面の半径。
        /// </summary>
        float distance = 1;

        /// <summary>
        /// 球面上の位置の角度。
        /// </summary>
        Vector2 angle = Vector2.Zero;

        /// <summary>
        /// 球面の原点。
        /// </summary>
        Vector3 target = Vector3.Zero;

        /// <summary>
        /// カメラの位置。
        /// Update() メソッドの呼出で更新されます。
        /// </summary>
        Vector3 position;

        /// <summary>
        /// 注視点からカメラまでの距離 (球面の半径) を取得または設定します。
        /// </summary>
        public float Distance
        {
            get { return distance; }
            set
            {
                if (distance == value) return;

                distance = value;
                MatrixDirty = true;
            }
        }

        /// <summary>
        /// 球面上のカメラの位置の角度を取得または設定します。
        /// ベクトルの X 要素は X 軸に対する角度、Y 要素は Y 軸に対する角度を表します。
        /// 角度は、位置 [0, 0, -1] を角度 [0, 0] として設定します。
        /// </summary>
        public Vector2 Angle
        {
            get { return angle; }
            set
            {
                if (angle == value) return;

                angle = value;
                MatrixDirty = true;
            }
        }

        /// <summary>
        /// 注視点 (球面の原点) を取得または設定します。
        /// </summary>
        public Vector3 Target
        {
            get { return target; }
            set
            {
                if (target == value) return;

                target = value;
                MatrixDirty = true;
            }
        }

        /// <summary>
        /// カメラの位置を取得します。
        /// </summary>
        /// <remarks>
        /// カメラの位置は Update() メソッドの呼出で更新されます。
        /// </remarks>
        public Vector3 Position
        {
            get { return position; }
        }

        public ChaseView()
        {
            Update();
        }

        protected override void UpdateOverride()
        {
            Matrix scale;
            Matrix rotation;
            Matrix translation;

            Matrix.CreateTranslation(ref target, out translation);
            Matrix.CreateFromYawPitchRoll(angle.Y, angle.X, 0, out rotation);
            Matrix.CreateScale(distance, out scale);

            // 変換行列を算出します。
            Matrix transform;
            Matrix.Multiply(ref scale, ref rotation, out transform);
            Matrix.Multiply(ref transform, ref translation, out transform);

            // 座標を算出します。
            Vector3 baseDirection = Vector3.Backward;
            //Vector3 position;
            Vector3.Transform(ref baseDirection, ref transform, out position);

            // UP を算出します。
            Vector3 baseUp = Vector3.Up;
            Vector3 up;
            Vector3.Transform(ref baseUp, ref rotation, out up);
            up.Normalize();

            // View 行列を算出します。
            Matrix.CreateLookAt(ref position, ref target, ref up, out Matrix);

            MatrixDirty = false;
        }
    }
}
