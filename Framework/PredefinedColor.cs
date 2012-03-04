#region Using

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework
{
    /// <summary>
    /// Microsoft.Xna.Framework.Color で定義済みの Color を、その名前とともに管理するクラスです。
    /// </summary>
    public sealed class PredefinedColor
    {
        /// <summary>
        /// 全ての PredefinedColor を含むリスト。
        /// </summary>
        static List<PredefinedColor> predefinedColors = new List<PredefinedColor>();

        /// <summary>
        /// 全ての PredefinedColor を含むリストを取得します。
        /// </summary>
        public static ReadOnlyCollection<PredefinedColor> PredefinedColors { get; private set; }

        /// <summary>
        /// 名前を取得します。
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Color を取得します。
        /// </summary>
        public Color Color { get; private set; }

        /// <summary>
        /// PredefinedColors プロパティを初期化します。
        /// </summary>
        static PredefinedColor()
        {
            var properties = typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (var property in properties)
            {
                if (property.GetGetMethod() != null && property.PropertyType == typeof(Color))
                {
                    var name = property.Name;
                    var color = (Color) property.GetValue(null, null);

                    var predefinedColor = new PredefinedColor(name, color);

                    predefinedColors.Add(predefinedColor);
                }
            }
            PredefinedColors = new ReadOnlyCollection<PredefinedColor>(predefinedColors);
        }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="name">名前。</param>
        /// <param name="color">Color。</param>
        PredefinedColor(string name, Color color)
        {
            Name = name;
            Color = color;
        }
    }
}
