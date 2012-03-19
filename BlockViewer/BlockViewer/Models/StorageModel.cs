#region Using

using System;
using Microsoft.Xna.Framework.Storage;
using Willcraftia.Xna.Blocks.Storage;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Models
{
    public sealed class StorageModel
    {
        public event EventHandler ContainerChanged = delegate { };

        public bool Selected
        {
            get { return Container != null; }
        }

        public StorageContainer Container { get; private set; }

        public StorageBlockLoader BlockLoader { get; private set; }

        public void SelectStorage()
        {
            StorageContainer oldContainer = Container;

            var showSelectorResult = StorageDevice.BeginShowSelector(null, null);
            showSelectorResult.AsyncWaitHandle.WaitOne();
            var storageDevice = StorageDevice.EndShowSelector(showSelectorResult);
            showSelectorResult.AsyncWaitHandle.Close();

            var openContainerResult = storageDevice.BeginOpenContainer("BlockViewer", null, null);
            openContainerResult.AsyncWaitHandle.WaitOne();
            Container = storageDevice.EndOpenContainer(openContainerResult);
            openContainerResult.AsyncWaitHandle.Close();

            if (oldContainer != Container)
            {
                BlockLoader = new StorageBlockLoader(Container, null);

                ContainerChanged(this, EventArgs.Empty);
            }
        }

        public string[] GetBlockMeshFileNames()
        {
            EnsureContainer();

            return Container.GetFileNames("*.blockmesh.xml");
        }

        void EnsureContainer()
        {
            if (!Selected) throw new InvalidOperationException("A storage container is not selected.");
        }
    }
}
