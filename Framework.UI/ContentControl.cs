#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class ContentControl : Control
    {
        Control content;

        public Control Content
        {
            get { return content; }
            set
            {
                if (content == value) return;

                if (content != null) RemoveChild(content);

                content = value;

                if (content != null) AddChild(content);
            }
        }

        public override int ChildrenCount
        {
            get { return (content != null) ? 1 : 0; }
        }

        protected ContentControl(Screen screen) : base(screen) { }

        protected override Control GetChild(int index)
        {
            if (index != 0) throw new ArgumentOutOfRangeException("index");

            return content;
        }
    }
}
