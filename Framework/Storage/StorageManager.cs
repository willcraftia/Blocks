#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Storage;

#endregion

namespace Willcraftia.Xna.Framework.Storage
{
    /// <summary>
    /// IStorageService の実装クラスです。
    /// </summary>
    public sealed class StorageManager : GameComponent, IStorageService
    {
        // I/F
        public event EventHandler ContainerSelected = delegate { };

        /// <summary>
        /// StorageContainer。
        /// </summary>
        StorageContainer container;

        /// <summary>
        /// Select(string) を非同期に呼び出す場合のための同期オブジェクト。
        /// </summary>
        readonly object syncRoot = new object();

        /// <summary>
        /// true (StorageContainer が変更された場合)、false (それ以外の場合)。
        /// </summary>
        bool containerChanged;

        /// <summary>
        /// インスタンスを生成し、Game のサービスとして登録します。
        /// </summary>
        /// <param name="game">Game。</param>
        public StorageManager(Game game)
            : base(game)
        {
            Game.Services.AddService(typeof(IStorageService), this);
        }

        // I/F
        public StorageDirectory RootDirectory { get; private set; }

        // I/F
        public void Select(string storageName)
        {
            var showSelectorResult = StorageDevice.BeginShowSelector(null, null);
            showSelectorResult.AsyncWaitHandle.WaitOne();

            var storageDevice = StorageDevice.EndShowSelector(showSelectorResult);
            showSelectorResult.AsyncWaitHandle.Close();

            var openContainerResult = storageDevice.BeginOpenContainer(storageName, null, null);
            openContainerResult.AsyncWaitHandle.WaitOne();

            container = storageDevice.EndOpenContainer(openContainerResult);
            openContainerResult.AsyncWaitHandle.Close();

            // 非同期呼び出しの可能性があるため同期を取ります。
            lock (syncRoot)
            {
                containerChanged = true;
            }
        }

        public override void Update(GameTime gameTime)
        {
            // RootDirectory の反映とイベント送信は、Game Thread で行うことを保証します。
            lock (syncRoot)
            {
                if (containerChanged)
                {
                    RootDirectory = StorageDirectory.GetRootDirectory(container);

                    containerChanged = false;
                    ContainerSelected(this, EventArgs.Empty);
                }
            }

            base.Update(gameTime);
        }
    }
}
