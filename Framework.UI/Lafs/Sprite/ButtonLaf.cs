#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    /// <summary>
    /// Button 用の LaF です。
    /// </summary>
    public class ButtonLaf : SpriteControlLafBase
    {
        public override void Draw(Control control, IDrawContext drawContext)
        {
            var button = control as Controls.Button;
            if (button == null) return;

            var spriteBatch = drawContext.SpriteBatch;

            var foregroundColor = button.ForegroundColor * drawContext.Opacity;
            var bounds = drawContext.Bounds;

            if (button.MouseDirectlyOver)
            {
                // TODO: 色を汎用的に指定するにはどうしたらよいだろうか？
                spriteBatch.Draw(Source.FillTexture, bounds, foregroundColor * 0.5f);
            }
        }
    }
}
