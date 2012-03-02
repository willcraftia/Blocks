#region Using

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// XNA Color クラスのヘルパ クラスです。
    /// </summary>
    public static class ColorHelper
    {
        #region PropertyInfoNameComparer

        /// <summary>
        /// PropertyInfo を Name プロパティで比較する IComparer です。
        /// </summary>
        class PropertyInfoNameComparer : IComparer<PropertyInfo>
        {
            /// <summary>
            /// シングルトン。
            /// </summary>
            public static PropertyInfoNameComparer Instance = new PropertyInfoNameComparer();

            /// <summary>
            /// シングルトンとするためにコンストラクタを非公開にします。
            /// </summary>
            PropertyInfoNameComparer() { }

            /// <summary>
            /// Name で比較します。
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <returns></returns>
            public int Compare(PropertyInfo x, PropertyInfo y)
            {
                return x.Name.CompareTo(y.Name);
            }
        }

        #endregion

        /// <summary>
        /// 定義済み Color の PropertyInfo のリスト (Name で昇順ソート済み)。
        /// </summary>
        static List<PropertyInfo> colorPropertyInfos;

        /// <summary>
        /// 静的初期化。
        /// </summary>
        static ColorHelper()
        {
            colorPropertyInfos = EnumerateColorPropertyInfos();
            colorPropertyInfos.Sort(PropertyInfoNameComparer.Instance);
        }

        public static List<Color> EnumerateColorsOrderedByName()
        {
            var colors = new List<Color>();
            foreach (var property in colorPropertyInfos)
            {
                var color = (Color) property.GetValue(null, null);
                Console.WriteLine("Name=" + property.Name + ", Color=" + color);
                colors.Add(color);
            }
            return colors;
        }

        /// <summary>
        /// 定義済み Color の PropertyInfo リスト (未ソート) を取得します。
        /// </summary>
        /// <returns></returns>
        static List<PropertyInfo> EnumerateColorPropertyInfos()
        {
            var result = new List<PropertyInfo>();
            var properties = typeof(Color).GetProperties(BindingFlags.Public | BindingFlags.Static);
            foreach (var property in properties)
            {
                if (property.GetGetMethod() != null && property.PropertyType == typeof(Color))
                {
                    result.Add(property);
                }
            }
            return result;
        }
    }
}
