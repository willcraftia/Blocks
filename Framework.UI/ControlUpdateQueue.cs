#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Game Thread 以外からの Control の更新を Game Thread で同期させるためのキューです。
    /// </summary>
    internal sealed class ControlUpdateQueue
    {
        #region Item

        /// <summary>
        /// 更新処理を表します。
        /// </summary>
        struct Item
        {
            /// <summary>
            /// Control の更新を行うメソッド。
            /// </summary>
            public Delegate Delegate;

            /// <summary>
            /// Delegate のパラメータ。
            /// </summary>
            public object[] Parameters;
        }

        #endregion

        /// <summary>
        /// Item のキュー。
        /// </summary>
        Queue<Item> queue = new Queue<Item>();

        /// <summary>
        /// 更新処理をキューへ追加します。
        /// </summary>
        /// <param name="d">Control の更新を行うメソッド。</param>
        /// <param name="parameters">Delegate のパラメータ。</param>
        internal void Enqueue(Delegate d, params object[] parameters)
        {
            lock (queue)
            {
                var item = new Item { Delegate = d, Parameters = parameters };
                queue.Enqueue(item);
            }
        }

        /// <summary>
        /// キューから 1 つの Item を取り出し、更新処理を実行します。
        /// </summary>
        /// <param name="gameTime"></param>
        internal void Update(GameTime gameTime)
        {
            Item item;
            lock (queue)
            {
                if (queue.Count == 0) return;

                item = queue.Dequeue();
            }

            if (item.Delegate == null) return;

            if (item.Delegate is MethodInvoker)
            {
                (item.Delegate as MethodInvoker)();
            }
            else
            {
                item.Delegate.DynamicInvoke(item.Parameters);
            }
        }
    }
}
