#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public struct RoutedEventContext
    {
        public Control Source { get; private set; }

        public bool Handled { get; set; }

        public RoutedEventContext(Control source)
            : this()
        {
            Source = source;
        }
    }
}
