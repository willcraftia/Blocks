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

            if (button.MouseDirectlyOver)
            {
                // TODO: 色を汎用的に指定するにはどうしたらよいだろうか？
                drawContext.DrawRectangle(new Rect(control.RenderSize), control.ForegroundColor * 0.5f);
            }
        }
    }
}
