#region Using

using System;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Models
{
    public enum GridViewDirection
    {
        /// <summary>
        /// Vector3.Forward の視線。
        /// </summary>
        Forward,

        Backward,

        /// <summary>
        /// Vector3.Left の視線。
        /// </summary>
        Left,

        Right,

        /// <summary>
        /// Vector3.Down の視線。
        /// </summary>
        Down,

        Up
    }
}
