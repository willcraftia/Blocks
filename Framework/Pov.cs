#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework
{
    /// <summary>
    /// POV (Point of View) を表すクラスです。
    /// </summary>
    public sealed class Pov
    {
        /// <summary>
        /// デフォルトの FOV。
        /// </summary>
        public const float DefaultFov = MathHelper.PiOver4;

        /// <summary>
        /// 1 : 1 のアスペクト比。
        /// </summary>
        /// <remarks>
        /// この比率は、Shadow Map 生成で用いるライト カメラのアスペクト比などで利用されます。
        /// </remarks>
        public const float AspectRatio1x1 = 1.0f;

        /// <summary>
        /// 4 : 3 のアスペクト比。
        /// </summary>
        /// <remarks>
        /// この比率は、解像度 800x600 のスクリーンに相当します。
        /// </remarks>
        public const float AspectRatio4x3 = 4.0f / 3.0f;

        /// <summary>
        /// 16 : 9 のアスペクト比。
        /// </summary>
        /// <remarks>
        /// この比率は、解像度 1280x720 のスクリーンに相当します。
        /// </remarks>
        public const float AspectRatio16x9 = 16.0f / 9.0f;

        /// <summary>
        /// 近くのビュー プレーンとの距離のデフォルト。
        /// </summary>
        public const float DefaultNearPlaneDistance = 0.1f;

        /// <summary>
        /// 遠くのビュー プレーンとの距離のデフォルト。
        /// </summary>
        public const float DefaultFarPlaneDistance = 1000.0f;

        /// <summary>
        /// デフォルトの焦点距離。
        /// </summary>
        public const float DefaultFocusDistance = 3.0f;

        /// <summary>
        /// デフォルトの焦点範囲。
        /// </summary>
        public const float DefaultFocusRange = 300.0f;

        /// <summary>
        /// View 行列。
        /// </summary>
        /// <remarks>
        /// このフィールドには Update メソッドによる View 行列演算結果が設定されます。
        /// View 行列は極めて頻繁に用いられるため、隠蔽よりも速度を重視して公開フィールドとしています。
        /// また、このクラス以外のクラスが、この公開フィールドへ直接値を設定しないことを規約としておきます。
        /// </remarks>
        public Matrix View = Matrix.Identity;

        /// <summary>
        /// Projection 行列。
        /// </summary>
        /// <remarks>
        /// このフィールドには Update メソッドによる Projection 行列演算結果が設定されます。
        /// View 行列は極めて頻繁に用いられるため、隠蔽よりも速度を重視して公開フィールドとしています。
        /// また、このクラス以外のクラスが、この公開フィールドへ直接値を設定しないことを規約としておきます。
        /// </remarks>
        public Matrix Projection = Matrix.Identity;

        /// <summary>
        /// View * Projection 行列。
        /// </summary>
        /// <remarks>
        /// このフィールドには Update メソッドによる View * Projection 行列演算結果が設定されます。
        /// View * Projection 行列は極めて頻繁に用いられるため、隠蔽よりも速度を重視して公開フィールドとしています。
        /// また、このクラス以外のクラスが、この公開フィールドへ直接値を設定しないことを規約としておきます。
        /// </remarks>
        public Matrix ViewProjection = Matrix.Identity;

        /// <summary>
        /// 座標ベクトル。
        /// </summary>
        /// <remarks>
        /// このフィールドは極めて頻繁に用いられるものの、行列演算を考慮した更新フラグ制御を行う必要から、
        /// out/ref を用いた getter/setter メソッドにより取得および設定を行います。
        /// </remarks>
        Vector3 Position = Vector3.Zero;

        /// <summary>
        /// 姿勢行列。
        /// </summary>
        /// <remarks>
        /// このフィールドは極めて頻繁に用いられるものの、行列演算を考慮した更新フラグ制御を行う必要から、
        /// out/ref を用いた getter/setter メソッドにより取得および設定を行います。
        /// </remarks>
        Matrix Orientation = Matrix.Identity;

        /// <summary>
        ///  y 方向の視野角 (ラジアン単位)。
        /// </summary>
        float fov = DefaultFov;

        /// <summary>
        /// アスペクト比。
        /// </summary>
        float aspectRatio = AspectRatio4x3;

        /// <summary>
        /// 近くのビュー プレーンとの距離。
        /// </summary>
        float nearPlaneDistance = DefaultNearPlaneDistance;

        /// <summary>
        /// 遠くのビュー プレーンとの距離。
        /// </summary>
        float farPlaneDistance = DefaultFarPlaneDistance;

        /// <summary>
        /// 焦点距離。
        /// </summary>
        float focusDistance = DefaultFocusDistance;

        /// <summary>
        /// 焦点範囲。
        /// </summary>
        float focusRange = DefaultFocusRange;

        // View 更新フラグ。
        bool viewDirty = true;

        // Projection 更新フラグ。
        bool projectionDirty = true;

        /// <summary>
        /// y 方向の視野角 (ラジアン単位)。
        /// </summary>
        /// <remarks>
        /// 値を変更した場合、同時に Projection 更新フラグが ON に変更されます。
        /// </remarks>
        public float Fov
        {
            get { return fov; }
            set
            {
                if (fov == value) return;

                fov = value;
                projectionDirty = true;
            }
        }

        /// <summary>
        /// アスペクト比。
        /// </summary>
        /// <remarks>
        /// 値を変更した場合、同時に Projection 更新フラグが ON に変更されます。
        /// 0 以下の値を設定した場合、
        /// 実行時の Viewport のアスペクト比が自動的に設定されるように制御する必要があります。
        /// </remarks>
        public float AspectRatio
        {
            get { return aspectRatio; }
            set
            {
                if (aspectRatio == value) return;

                aspectRatio = value;
                projectionDirty = true;
            }
        }

        /// <summary>
        /// 近くのビュー プレーンとの距離。
        /// </summary>
        /// <remarks>
        /// 値を変更した場合、同時に Projection 更新フラグが ON に変更されます。
        /// </remarks>
        public float NearPlaneDistance
        {
            get { return nearPlaneDistance; }
            set
            {
                if (nearPlaneDistance == value) return;

                nearPlaneDistance = value;
                projectionDirty = true;
            }
        }

        /// <summary>
        /// 遠くのビュー プレーンとの距離。
        /// </summary>
        /// <remarks>
        /// 値を変更した場合、同時に Projection 更新フラグが ON に変更されます。
        /// </remarks>
        public float FarPlaneDistance
        {
            get { return farPlaneDistance; }
            set
            {
                if (farPlaneDistance == value) return;

                farPlaneDistance = value;
                projectionDirty = true;
            }
        }

        /// <summary>
        /// 焦点距離。
        /// </summary>
        public float FocusDistance
        {
            get { return focusDistance; }
            set
            {
                if (focusDistance == value) return;

                focusDistance = value;
                projectionDirty = true;
            }
        }

        /// <summary>
        /// 焦点範囲。
        /// </summary>
        public float FocusRange
        {
            get { return focusRange; }
            set
            {
                if (focusRange == value) return;

                focusRange = value;
                projectionDirty = true;
            }
        }

        /// <summary>
        /// 位置ベクトルを取得します。
        /// </summary>
        /// <param name="result">位置ベクトル。</param>
        public void GetPosition(out Vector3 result)
        {
            result = Position;
        }

        /// <summary>
        /// 位置ベクトルを更新し、View 更新フラグを ON にします。
        /// </summary>
        /// <param name="position">位置ベクトル。</param>
        public void SetPosition(ref Vector3 position)
        {
            Position = position;
            viewDirty = true;
        }

        /// <summary>
        /// 姿勢行列を取得します。
        /// </summary>
        /// <param name="result">姿勢行列。</param>
        public void GetOrientation(out Matrix result)
        {
            result = Orientation;
        }

        /// <summary>
        /// 姿勢行列を更新し、View 更新フラグを ON にします。
        /// </summary>
        /// <param name="orientation">姿勢行列。</param>
        public void SetOrientation(ref Matrix orientation)
        {
            Orientation = orientation;
            viewDirty = true;
        }

        /// <summary>
        /// View 更新フラグによらず View 行列を更新します。
        /// </summary>
        /// <remarks>
        /// View 行列が更新されると View * Projection 行列も同時に更新されます。
        /// </remarks>
        public void UpdateView()
        {
            UpdateView(true);
        }

        /// <summary>
        /// View 行列を更新します。
        /// </summary>
        /// <param name="force">
        /// true (必ず View 行列を更新する場合)、false (View 更新フラグが ON の場合にのみ View 行列を更新する場合)。
        /// </param>
        /// <remarks>
        /// View 行列が更新されると View * Projection 行列も同時に更新されます。
        /// </remarks>
        public void UpdateView(bool force)
        {
            if (force || viewDirty)
            {
                UpdateViewOnly();
                UpdateViewProjectionOnly();
            }
        }

        /// <summary>
        /// Projection 更新フラグによらず Projection 行列を更新します。
        /// </summary>
        /// <remarks>
        /// View 行列が更新されると View * Projection 行列も同時に更新されます。
        /// </remarks>
        public void UpdateProjection()
        {
            UpdateProjection(true);
        }

        /// <summary>
        /// Projection 行列を更新します。
        /// </summary>
        /// <param name="force">
        /// true (必ず Projection 行列を更新する場合)、
        /// false (Projection 更新フラグが ON の場合にのみ Projection 行列を更新する場合)。
        /// </param>
        /// <remarks>
        /// View 行列が更新されると View * Projection 行列も同時に更新されます。
        /// </remarks>
        public void UpdateProjection(bool force)
        {
            if (force || projectionDirty)
            {
                UpdateProjectionOnly();
                UpdateViewProjectionOnly();
            }
        }

        /// <summary>
        /// View 更新フラグと Projection 更新フラグによらず、View 行列と Projection 行列を更新します。
        /// </summary>
        public void Update()
        {
            Update(true);
        }

        /// <summary>
        /// View 行列と Projection 行列を更新します。
        /// </summary>
        /// <param name="force">
        /// true (必ず View 行列と Projection 行列を更新する場合)、
        /// false (View 更新フラグと Projection 更新フラグに従って更新の有無を制御する場合)。
        /// </param>
        public void Update(bool force)
        {
            bool viewProjectionDirty = false;
            if (force || viewDirty)
            {
                UpdateViewOnly();
                viewProjectionDirty = true;
            }
            if (force || projectionDirty)
            {
                UpdateProjectionOnly();
                viewProjectionDirty = true;
            }
            if (force || viewProjectionDirty)
            {
                UpdateViewProjectionOnly();
            }
        }

        /// <summary>
        /// View 行列のみを更新します。
        /// </summary>
        /// <remarks>
        /// View 更新フラグの値によらず View 行列を更新した後、View 更新フラグを OFF に設定します。
        /// </remarks>
        void UpdateViewOnly()
        {
            var target = Position + Orientation.Forward;
            var up = Orientation.Up;
            Matrix.CreateLookAt(ref Position, ref target, ref up, out View);
            viewDirty = false;
        }

        /// <summary>
        /// Projection 行列のみを更新します。
        /// </summary>
        /// <remarks>
        /// Projection 更新フラグの値によらず Projection 行列を更新した後、Projection 更新フラグを OFF に設定します。
        /// </remarks>
        void UpdateProjectionOnly()
        {
            Matrix.CreatePerspectiveFieldOfView(Fov, AspectRatio, NearPlaneDistance, FarPlaneDistance, out Projection);
            projectionDirty = false;
        }

        /// <summary>
        /// View * Projection 行列のみを更新します。
        /// </summary>
        /// <remarks>
        /// View 行列と Projection 行列は更新しないため、それらの更新も必要ならば、
        /// このメソッドの呼び出し前に更新しておく必要があります。
        /// </remarks>
        void UpdateViewProjectionOnly()
        {
            Matrix.Multiply(ref View, ref Projection, out ViewProjection);
        }
    }
}
