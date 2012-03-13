﻿#region Using

using System;
using Willcraftia.Xna.Blocks.Content;
using Willcraftia.Xna.Blocks.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Models
{
    public sealed class BlockMeshLoaderProxy : IBlockMeshLoader
    {
        IBlockMeshLoader subject;

        public IBlockMeshLoader Subject
        {
            get
            {
                lock (this)
                {
                    return subject;
                }
            }
            set
            {
                lock (this)
                {
                    subject = value;
                }
            }
        }

        public BlockMesh LoadBlockMesh(string name)
        {
            if (subject == null) throw new InvalidOperationException("subject is null.");
            return subject.LoadBlockMesh(name);
        }
    }
}
