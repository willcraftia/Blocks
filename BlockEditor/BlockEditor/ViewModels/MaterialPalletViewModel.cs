#region Using

using System;
using Willcraftia.Xna.Blocks.Serialization;
using Willcraftia.Xna.Blocks.BlockEditor.Models;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.ViewModels
{
    public sealed class MaterialPalletViewModel
    {
        Editor editor;

        public MaterialPalletViewModel(Editor editor)
        {
            if (editor == null) throw new ArgumentNullException("editor");
            this.editor = editor;
        }

        public int CreateMaterial(
            MaterialColor diffuseColor,
            MaterialColor emissiveColor,
            MaterialColor specularColor, float specularPower)
        {
            return editor.CreateMaterial(diffuseColor, emissiveColor, specularColor, specularPower);
        }

        public Material GetMaterial(int materialIndex)
        {
            return editor.GetMaterial(materialIndex);
        }
    }
}
