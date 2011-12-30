#region Using

using System;
using System.Reflection;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.Animations
{
    /// <summary>
    /// Control の 1 つのプロパティをアニメーションさせるクラスです。
    /// </summary>
    public class PropertyLerpAnimation : LerpAnimation
    {
        /// <summary>
        /// 対象とするオブジェクトを取得または設定します。
        /// </summary>
        public object Target { get; set; }

        /// <summary>
        /// 対象とするプロパティの名前を取得または設定します。
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// 対象とするプロパティ。
        /// </summary>
        PropertyInfo property;

        protected override void Update(GameTime gameTime, TimeSpan playElapsedTime, float current)
        {
            if (Target == null) throw new InvalidOperationException("Target is null.");

            // プロパティを取得します。
            if (property == null) property = Target.GetType().GetProperty(PropertyName);

            // プロパティに現在の値を設定します。
            property.SetValue(Target, current, null);
        }
    }
}
