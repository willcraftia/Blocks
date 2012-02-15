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
        /// 対象とする Control への弱参照。
        /// </summary>
        WeakReference targetReference = new WeakReference(null);

        /// <summary>
        /// 対象とするプロパティ。
        /// </summary>
        PropertyInfo property;

        /// <summary>
        /// 対象とするオブジェクトを取得または設定します。
        /// </summary>
        public object Target
        {
            get { return targetReference.Target; }
            set { targetReference.Target = value; }
        }

        /// <summary>
        /// 対象とするプロパティの名前を取得または設定します。
        /// </summary>
        public string PropertyName { get; set; }

        protected override void Update(GameTime gameTime, TimeSpan playElapsedTime, float current)
        {
            if (targetReference.Target == null) return;

            // プロパティを取得します。
            if (property == null) property = Target.GetType().GetProperty(PropertyName);

            // プロパティに現在の値を設定します。
            property.SetValue(Target, current, null);
        }
    }
}
