﻿#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    /// <summary>
    /// Button 用の LaF です。
    /// </summary>
    public class ButtonLaf : DefaultControlLaf
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            base.Draw(control, drawContext);

            var button = control as Controls.Button;
            if (button == null) return;

            var spriteBatch = drawContext.SpriteBatch;
            var opacity = drawContext.Opacity;
            var bounds = drawContext.Bounds;
            var foregroundColor = button.ForegroundColor * opacity;

            if (button.MouseDirectlyOver)
            {
                // TODO: 色を汎用的に指定するにはどうしたらよいだろうか？
                spriteBatch.Draw(Source.FillTexture, bounds, foregroundColor * 0.5f);
            }
        }
    }
}
