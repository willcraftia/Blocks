#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    /// <summary>
    /// Window 描画用の LaF です。
    /// </summary>
    public class WindowLaf : SpriteControlLafBase
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
        public WindowLaf()
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

            var color = window.BackgroundColor * drawContext.Opacity;
            var bounds = drawContext.Bounds;
            var spriteBatch = drawContext.SpriteBatch;
            var texture = spriteSheet.Texture;

            // 全てのスプライト イメージが同サイズであることを強制します。
            // SpriteSheet には異なるサイズで指定できますが、このクラスでは異なるサイズを取り扱うことができません。
            // 異なるサイズを扱おうとすると、どのスプライト イメージのサイズに基準を揃えるかの制御が複雑になります。
            int w = SpriteWidth;
            int h = SpriteHeight;

            var destinationRectangle = new Rectangle();
            Rectangle sourceRectangle;

            // Top Lines
            sourceRectangle = spriteSheet.SourceRectangles["TopLeft"];
            destinationRectangle.X = bounds.X;
            destinationRectangle.Y = bounds.Y;
            destinationRectangle.Width = w;
            destinationRectangle.Height = h;
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);

            sourceRectangle = spriteSheet.SourceRectangles["Top"];
            for (int x = w; x < bounds.Width - w; x += w)
            {
                destinationRectangle.X = bounds.X + x;
                spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
            }
            
            sourceRectangle = spriteSheet.SourceRectangles["TopRight"];
            destinationRectangle.X = bounds.X + bounds.Width - w;
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);

            // Middle Lines
            for (int y = h; y < bounds.Height - h; y += h)
            {
                sourceRectangle = spriteSheet.SourceRectangles["Left"];
                destinationRectangle.X = bounds.X;
                destinationRectangle.Y = bounds.Y + y;
                spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);

                sourceRectangle = spriteSheet.SourceRectangles["Fill"];
                for (int x = w; x < bounds.Width - w; x += w)
                {
                    destinationRectangle.X = bounds.X + x;
                    spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
                }

                sourceRectangle = spriteSheet.SourceRectangles["Right"];
                destinationRectangle.X = bounds.X + bounds.Width - w;
                spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
            }

            // Bottom Lines
            sourceRectangle = spriteSheet.SourceRectangles["BottomLeft"];
            destinationRectangle.X = bounds.X;
            destinationRectangle.Y = bounds.Y + bounds.Height - h;
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);

            sourceRectangle = spriteSheet.SourceRectangles["Bottom"];
            for (int x = w; x < bounds.Width - w; x += w)
            {
                destinationRectangle.X = bounds.X + x;
                spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
            }
            
            sourceRectangle = spriteSheet.SourceRectangles["BottomRight"];
            destinationRectangle.X = bounds.X + bounds.Width - w;
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, color);
        }
    }
}
