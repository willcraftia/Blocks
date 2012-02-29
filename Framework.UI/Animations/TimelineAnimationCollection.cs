#region Using

using System;
using System.Collections.ObjectModel;

#endregion

namespace Willcraftia.Xna.Framework.UI.Animations
{
    public sealed class TimelineAnimationCollection : Collection<Animation>
    {
        public TimelineGroupAnimation Parent { get; private set;}

        internal TimelineAnimationCollection(TimelineGroupAnimation parent)
        {
            Parent = parent;
        }

        protected override void InsertItem(int index, Animation item)
        {
            base.InsertItem(index, item);

            Parent.AddChild(item);
        }

        protected override void SetItem(int index, Animation item)
        {
            var removedItem = base[index];
            Parent.RemoveChild(removedItem);

            base.SetItem(index, item);

            Parent.AddChild(item);
        }

        protected override void RemoveItem(int index)
        {
            var removedItem = base[index];
            Parent.RemoveChild(removedItem);

            base.RemoveItem(index);
        }
    }
}
