#region Using

using System;
using System.Collections.ObjectModel;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// ScreenDefinition のコレクションです。
    /// ScreenDefinition は、ScreenDefinition.Name プロパティをキーとして管理されます。
    /// </summary>
    public sealed class ScreenDefinitionCollection : KeyedCollection<string, ScreenDefinition>
    {
        /// <summary>
        /// ScreenDefinition を登録します。
        /// Screen の名前には、Screen の型の Type.AssemblyQualifiedName が用いられます。
        /// </summary>
        /// <typeparam name="T">Screen の型。</typeparam>
        public void Add<T>() where T : Screen
        {
            Add<T>(typeof(T).AssemblyQualifiedName);
        }

        /// <summary>
        /// ScreenDefinition を登録します。
        /// </summary>
        /// <typeparam name="T">Screen の型。</typeparam>
        /// <param name="name">Screen の名前。</param>
        public void Add<T>(string name) where T : Screen
        {
            Add(name, typeof(T));
        }

        /// <summary>
        /// ScreenDefinition を登録します。
        /// </summary>
        /// <param name="name">Screen の名前。</param>
        /// <param name="type">Screen の型。</param>
        public void Add(string name, Type type)
        {
            Add(new ScreenDefinition(name, type));
        }

        protected override string GetKeyForItem(ScreenDefinition item)
        {
            return item.Name;
        }
    }
}
