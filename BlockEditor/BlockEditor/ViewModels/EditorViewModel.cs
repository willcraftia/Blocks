#region Using

using System;
using Willcraftia.Xna.Blocks.BlockEditor.Models;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.ViewModels
{
    public sealed class EditorViewModel
    {
        Editor editor;

        public SectionViewModel SectionViewModel { get; private set; }

        public EditorViewModel(Editor editor)
        {
            if (editor == null) throw new ArgumentNullException("editor");
            this.editor = editor;

            SectionViewModel = new SectionViewModel(editor);
        }
    }
}
