#region Using

using System;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework
{
    /// <summary>
    /// GameServiceContainer の拡張クラスです。
    /// </summary>
    public static class GameServiceContainerExtension
    {
        /// <summary>
        /// サービスを取得します。
        /// サービスが存在しない場合には InvalidOperationException を発生させます。
        /// </summary>
        /// <param name="container">GameServiceContainer。</param>
        /// <param name="serviceType">サービスの型。</param>
        /// <returns>サービス。</returns>
        public static object GetRequiredService(this GameServiceContainer container, Type serviceType)
        {
            var service = container.GetService(serviceType);
            if (service == null) throw new InvalidOperationException(string.Format("Service '{0}' not found", serviceType));
            return service;
        }

        /// <summary>
        /// サービスを取得します。
        /// </summary>
        /// <typeparam name="T">サービスの型。</typeparam>
        /// <param name="container">GameServiceContainer。</param>
        /// <returns>サービス。</returns>
        public static T GetService<T>(this GameServiceContainer container) where T : class
        {
            return container.GetService(typeof(T)) as T;
        }

        /// <summary>
        /// サービスを取得します。
        /// サービスが存在しない場合には ServiceNotFoundException を発生させます。
        /// </summary>
        /// <typeparam name="T">サービスの型。</typeparam>
        /// <param name="container">GameServiceContainer。</param>
        /// <returns>サービス。</returns>
        public static T GetRequiredService<T>(this GameServiceContainer container) where T : class
        {
            return GetRequiredService(container, typeof(T)) as T;
        }
    }
}
