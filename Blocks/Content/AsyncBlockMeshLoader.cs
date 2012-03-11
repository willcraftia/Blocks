#region Using

using System;
using System.Collections.Generic;
using System.Threading;
using Willcraftia.Xna.Blocks.Graphics;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    public delegate void AsyncBlockMeshLoaderCallback(string name, BlockMesh blockMesh);

    public sealed class AsyncBlockMeshLoader
    {
        IBlockMeshLoader loader;

        int queueSize;

        List<BlockMeshLoadRequest> requestQueue;

        Thread thread;

        AutoResetEvent beginEvent;

        bool exit;

        public AsyncBlockMeshLoader(IBlockMeshLoader loader, int queueSize)
        {
            if (loader == null) throw new ArgumentNullException("loader");
            if (queueSize < 1) throw new ArgumentOutOfRangeException("queueSize");
            this.loader = loader;
            this.queueSize = queueSize;

            requestQueue = new List<BlockMeshLoadRequest>(queueSize);
            beginEvent = new AutoResetEvent(false);
            thread = new Thread(Run);
            thread.Start();
        }

        public void Load(BlockMeshLoadRequest request)
        {
            if (request.Name == null || request.Callback == null)
                throw new InvalidOperationException("An invalid request is specified.");

            lock (requestQueue)
            {
                requestQueue.Add(request);

                beginEvent.Set();
            }
        }

        public void Load(ICollection<BlockMeshLoadRequest> requests)
        {
            lock (requestQueue)
            {
                foreach (var request in requests)
                {
                    if (request.Name == null || request.Callback == null)
                        throw new InvalidOperationException("An invalid request is specified.");

                    requestQueue.Add(request);
                }

                if (!thread.IsAlive) thread.Start();
            }
        }

        public void Cancel(BlockMeshLoadRequest request)
        {
            lock (requestQueue)
            {
                requestQueue.Remove(request);
            }
        }

        public void Cancel(ICollection<BlockMeshLoadRequest> requests)
        {
            lock (requestQueue)
            {
                foreach (var request in requests) requestQueue.Remove(request);
            }
        }

        public void CancelAll()
        {
            lock (requestQueue) requestQueue.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// AsyncBlockMeshLoader が不要になるタイミングで必ず Stop() を呼び出します。
        /// </remarks>
        public void Stop()
        {
            if (thread.ThreadState == ThreadState.Stopped) return;

            exit = true;

            lock (requestQueue)
            {
                requestQueue.Clear();
            }

            if (thread.ThreadState == ThreadState.WaitSleepJoin) beginEvent.Set();
            thread.Join();
        }

        void Run()
        {
            BlockMeshLoadRequest request = new BlockMeshLoadRequest();

            bool requestExists;
            while (true)
            {
                requestExists = false;

                lock (requestQueue)
                {
                    if (0 < requestQueue.Count)
                    {
                        // Dequeue。
                        request = requestQueue[0];
                        requestQueue.RemoveAt(0);
                        
                        requestExists = true;
                    }
                }

                if (requestExists)
                {
                    // ロードします。
                    var mesh = loader.LoadBlockMesh(request.Name);
                    request.Callback(request.Name, mesh);
                }
                else
                {
                    // ロード要求が無いならば Thread を待機させます。
                    beginEvent.WaitOne();

                    if (exit) break;
                }
            }
        }
    }
}
