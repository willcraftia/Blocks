#region Using

using System;
using Willcraftia.Xna.Blocks.Serialization;
using Willcraftia.Xna.Blocks.BlockEditor.Models;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.ViewModels
{
    public sealed class SectionViewModel
    {
        Editor editor;

        public SectionViewModel(Editor editor)
        {
            if (editor == null) throw new ArgumentNullException("editor");
            this.editor = editor;

            editor.SectionIndex = 8;
        }

        public void SetElement(int x, int y, int materialIndex)
        {
            editor.SetElement(x, y, materialIndex);
        }

        public Element GetElement(int x, int y)
        {
            return editor.GetElement(x, y);
        }

        public void RemoveElement(int x, int y)
        {
            editor.RemoveElement(x, y);
        }

        public Material GetMaterial(int x, int y)
        {
            var element = editor.GetElement(x, 15 - y);
            if (element == null) return null;

            return editor.GetMaterial(element.MaterialIndex);
        }
    }
}
