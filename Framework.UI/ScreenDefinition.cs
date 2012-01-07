#region Using

using System;
using System.Collections.Generic;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// Screen 生成の定義を表すクラスです。
    /// </summary>
    public class ScreenDefinition
    {
        /// <summary>
        /// Screen の名前を取得します。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Screen の型を取得します。
        /// </summary>
        public Type Type { get; private set; }

        /// <summary>
        /// Screen インスタンスに設定するプロパティのコレクションを取得します。
        /// </summary>
        public IDictionary<string, object> Properties { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="name">Screen の名前。</param>
        /// <param name="type">Screen の型。</param>
        public ScreenDefinition(string name, Type type)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (type == null) throw new ArgumentNullException("type");
            Name = name;
            Type = type;

            Properties = new Dictionary<string, object>();
        }
    }
}
