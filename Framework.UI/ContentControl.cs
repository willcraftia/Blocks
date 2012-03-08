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

                var oldContent = content;

                content = value;

                if (content != null) AddChild(content);

                OnContentChanged(oldContent, content);
            }
        }

        protected override int ChildrenCount
        {
            get { return (content != null) ? 1 : 0; }
        }

        protected ContentControl(Screen screen) : base(screen) { }

        protected override Control GetChild(int index)
        {
            if (index != 0 || content == null) throw new ArgumentOutOfRangeException("index");

            return content;
        }

        protected virtual void OnContentChanged(Control oldContent, Control newContent) { }
    }
}
