#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Controls
{
    public struct WrappedTextElement
    {
        public int StartIndex;

        public int Length;

        public Vector2 Size;

        public WrappedTextElement(int startIndex, int length, Vector2 size)
        {
            StartIndex = startIndex;
            Length = length;
            Size = size;
        }
    }
}
