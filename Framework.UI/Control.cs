#region Using

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// GUI コントロールの基底クラスです。
    /// </summary>
    public class Control
    {
        /// <summary>
        /// 親コントロール。
        /// </summary>
        Control parent;

        /// <summary>
        /// 親コントロールを取得または設定します。
        /// </summary>
        public Control Parent
        {
            get { return parent; }
            set
            {
                parent = value;
            }
        }

        /// <summary>
        /// 子コントロールのコレクションを取得します。
        /// </summary>
        public ControlCollection Children { get; private set; }

        /// <summary>
        /// コントロールの矩形サイズ (矩形座標は親コントロールの矩形座標からの相対位置)。
        /// </summary>
        public Rectangle Bounds;

        public Appearance Appearance { get; set; }

        /// <summary>
        /// コンストラクタ。
        /// </summary>
        public Control()
        {
            Children = new ControlCollection(this);
        }

        public Rectangle GetAbsoluteBounds()
        {
            if (parent == null) return Bounds;

            var parentAbsoluteBounds = parent.GetAbsoluteBounds();

            var absoluteBounds = Bounds;

            absoluteBounds.X += parentAbsoluteBounds.X;
            absoluteBounds.Y += parentAbsoluteBounds.Y;

            return absoluteBounds;
        }
    }
}
