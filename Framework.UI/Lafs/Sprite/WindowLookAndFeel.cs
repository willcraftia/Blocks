#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    /// <summary>
    /// Window の Look and Feel です。
    /// </summary>
    public class WindowLookAndFeel : LookAndFeelBase
    {
        /// <summary>
        /// Window 描画用の SpriteSheet を取得または設定します。
        /// </summary>
        public SpriteSheet spriteSheet { get; set; }

        /// <summary>
        /// SpriteSheet 内の各スプライト イメージの幅を取得または設定します。
        /// </summary>
        public int SpriteWidth { get; set; }

        /// <summary>
        /// SpriteSheet 内の各スプライト イメージの高さを取得または設定します。
        /// </summary>
        public int SpriteHeight { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public WindowLookAndFeel()
        {
            SpriteWidth = 16;
            SpriteHeight = 16;
        }

        protected override void LoadContent()
        {
            //----------------------------------------------------------------
            // TODO test code
            //
            //System.Net.WebClient webClient = new System.Net.WebClient();
            //webClient.DownloadFile("http://blocks/Framework.UI.Demo/Framework.UI.DemoContent/UI/Sprite/WindowTopLeft.png", "Content/UI/Sprite/WindowTopLeft.png");

            //topLeft = Texture2D.FromStream(Source.GraphicsDevice, new System.IO.FileStream("Content/UI/Sprite/WindowTopLeft.png", System.IO.FileMode.Open));
            //
            //----------------------------------------------------------------

            var spriteSheetTexture = Source.Content.Load<Texture2D>("Window");
            spriteSheet = new SpriteSheet(spriteSheetTexture);

            var w = SpriteWidth;
            var h = SpriteHeight;
            spriteSheet.SourceRectangles["TopLeft"] = new Rectangle(0, 0, w, h);
            spriteSheet.SourceRectangles["Top"] = new Rectangle(w, 0, w, h);
            spriteSheet.SourceRectangles["TopRight"] = new Rectangle(w * 2, 0, w, h);
            spriteSheet.SourceRectangles["Left"] = new Rectangle(0, h, w, h);
            spriteSheet.SourceRectangles["Fill"] = new Rectangle(w, h, w, h);
            spriteSheet.SourceRectangles["Right"] = new Rectangle(w * 2, h, w, h);
            spriteSheet.SourceRectangles["BottomLeft"] = new Rectangle(0, h * 2, w, h);
            spriteSheet.SourceRectangles["Bottom"] = new Rectangle(w, h * 2, w, h);
            spriteSheet.SourceRectangles["BottomRight"] = new Rectangle(w * 2, h * 2, w, h);

            base.LoadContent();
        }

        public override void Draw(Control control, IDrawContext drawContext)
        {
            var window = control as Window;
            if (window == null) return;

            var renderSize = control.RenderSize;
            var texture = spriteSheet.Texture;
            var color = Color.White;

            // 全てのスプライト イメージが同サイズであることを強制します。
            // SpriteSheet には異なるサイズで指定できますが、このクラスでは異なるサイズを取り扱うことができません。
            // 異なるサイズを扱おうとすると、どのスプライト イメージのサイズに基準を揃えるかの制御が複雑になります。
            int w = SpriteWidth;
            int h = SpriteHeight;

            var bounds = new Rect();
            Rectangle sourceRectangle;

            // Top Lines
            sourceRectangle = spriteSheet.SourceRectangles["TopLeft"];
            bounds.X = 0;
            bounds.Y = 0;
            bounds.Width = w;
            bounds.Height = h;
            drawContext.DrawTexture(bounds, texture, color, sourceRectangle);

            sourceRectangle = spriteSheet.SourceRectangles["Top"];
            for (int x = w; x < renderSize.Width - w; x += w)
            {
                bounds.X = x;
                drawContext.DrawTexture(bounds, texture, color, sourceRectangle);
            }
            
            sourceRectangle = spriteSheet.SourceRectangles["TopRight"];
            bounds.X = renderSize.Width - w;
            drawContext.DrawTexture(bounds, texture, color, sourceRectangle);

            // Middle Lines
            for (int y = h; y < renderSize.Height - h; y += h)
            {
                sourceRectangle = spriteSheet.SourceRectangles["Left"];
                bounds.X = 0;
                bounds.Y = y;
                drawContext.DrawTexture(bounds, texture, color, sourceRectangle);

                sourceRectangle = spriteSheet.SourceRectangles["Fill"];
                for (int x = w; x < renderSize.Width - w; x += w)
                {
                    bounds.X = x;
                    drawContext.DrawTexture(bounds, texture, color, sourceRectangle);
                }

                sourceRectangle = spriteSheet.SourceRectangles["Right"];
                bounds.X = renderSize.Width - w;
                drawContext.DrawTexture(bounds, texture, color, sourceRectangle);
            }

            // Bottom Lines
            sourceRectangle = spriteSheet.SourceRectangles["BottomLeft"];
            bounds.X = 0;
            bounds.Y = renderSize.Height - h;
            drawContext.DrawTexture(bounds, texture, color, sourceRectangle);

            sourceRectangle = spriteSheet.SourceRectangles["Bottom"];
            for (int x = w; x < renderSize.Width - w; x += w)
            {
                bounds.X = x;
                drawContext.DrawTexture(bounds, texture, color, sourceRectangle);
            }
            
            sourceRectangle = spriteSheet.SourceRectangles["BottomRight"];
            bounds.X = renderSize.Width - w;
            drawContext.DrawTexture(bounds, texture, color, sourceRectangle);
        }
    }
}
