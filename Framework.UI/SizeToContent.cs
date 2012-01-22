#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// サイズをコンテンツのサイズに合わせて自動調整する方法を列挙します。
    /// </summary>
    public enum SizeToContent
    {
        /// <summary>
        /// 自動調整せずに明示的にサイズを指定します。
        /// </summary>
        Manual,
        /// <summary>
        /// 幅を自動調節し、高さは明示的に指定します。
        /// </summary>
        Width,
        /// <summary>
        /// 高さを自動調節し、幅は明示的に指定します。
        /// </summary>
        Height,
        /// <summary>
        /// 幅と高さを自動調整します。
        /// </summary>
        WidthAndHeight
    }
}
