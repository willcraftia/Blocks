#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public struct Thickness
    {
        public float Left;
        public float Right;
        public float Top;
        public float Bottom;

        public Thickness(float uniformLength)
        {
            Left = uniformLength;
            Right = uniformLength;
            Top = uniformLength;
            Bottom = uniformLength;
        }

        public Thickness(float left, float top, float right, float bottom)
        {
            Left = left;
            Right = right;
            Top = top;
            Bottom = bottom;
        }
    }
}
