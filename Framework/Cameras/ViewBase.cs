#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.Cameras
{
    /// <summary>
    /// View 実装の基底クラスです。
    /// </summary>
    public abstract class ViewBase : View
    {
        /// <summary>
        /// Matrix プロパティの値が有効であるかどうかを示す値を取得します。
        /// Matrix プロパティの再計算を必要とするプロパティが変更されると、
        /// このプロパティが true に設定されます。
        /// このプロパティが true の場合にのみ、
        /// Update メソッドは Matrix プロパティの再計算を行います。
        /// サブクラスで MatrixDirty プロパティによる制御が不要な場合は
        /// MatrixDirty プロパティが常に true であるように実装します。
        /// </summary>
        public bool MatrixDirty { get; protected set; }

        /// <summary>
        /// Matrix プロパティを更新します。
        /// </summary>
        public void Update()
        {
            if (MatrixDirty) UpdateOverride();
        }

        /// <summary>
        /// MatrixDirty プロパティが true の場合に Update メソッドから呼び出されます。
        /// </summary>
        protected abstract void UpdateOverride();
    }
}
