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
        /// Window の SpriteSheet。
        /// </summary>
        SpriteSheet windowSpriteSheet;

        /// <summary>
        /// Window の影の SpriteSheet。
        /// </summary>
        SpriteSheet shadowSpriteSheet;

        /// <summary>
        /// Window の影の描画位置を取得または設定します。
        /// </summary>
        public Vector2 ShadowOffset { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public WindowLookAndFeel()
        {
            ShadowOffset = new Vector2(4, 4);
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

            windowSpriteSheet = Source.SpriteSheetSource.GetSpriteSheet("Window");
            shadowSpriteSheet = Source.SpriteSheetSource.GetSpriteSheet("WindowShadow");

            base.LoadContent();
        }

        public override void Draw(Control control, IDrawContext drawContext)
        {
            var window = control as Window;

            if (shadowSpriteSheet != null)
                DrawWindow(window, drawContext, shadowSpriteSheet, Color.White, ShadowOffset);

            DrawWindow(window, drawContext, windowSpriteSheet, Color.White, Vector2.Zero);
        }

        protected void DrawWindow(Window window, IDrawContext drawContext, SpriteSheet spriteSheet, Color color, Vector2 offset)
        {
            var renderSize = window.RenderSize;
            var texture = spriteSheet.Texture;

            var template = spriteSheet.Template as WindowSpriteSheetTemplate;
            int w = template.SpriteWidth;
            int h = template.SpriteHeight;

            // 計算誤差の問題から先に int 化しておきます。
            int offsetX = (int) offset.X;
            int offsetY = (int) offset.Y;
            int renderWidth = (int) renderSize.Width;
            int renderHeight = (int) renderSize.Height;

            var bounds = new Rect();
            Rectangle sourceRectangle;

            // Top Lines
            {
                var titleDrawn = (template.ContainsTitle && window.TitleContent != null);

                var topLeft = (titleDrawn) ? WindowSpriteSheetTemplate.TitleTopLeft : WindowSpriteSheetTemplate.TopLeft;
                var top = (titleDrawn) ? WindowSpriteSheetTemplate.TitleTop : WindowSpriteSheetTemplate.Top;
                var topRight = (titleDrawn) ? WindowSpriteSheetTemplate.TitleTopRight : WindowSpriteSheetTemplate.TopRight;

                int adjustedH = AdjustHeight(renderHeight, offsetY, h, offsetY);
                int adjustedW = AdjustWidth(renderWidth, offsetX, w, offsetX);
                bounds.Height = adjustedH;
                bounds.Width = adjustedW;

                sourceRectangle = spriteSheet.Template[topLeft];
                sourceRectangle.Width = adjustedW;
                sourceRectangle.Height = adjustedH;
                bounds.X = offsetX;
                bounds.Y = offsetY;
                bounds.Width = sourceRectangle.Width;
                drawContext.DrawTexture(bounds, texture, color, sourceRectangle);

                sourceRectangle = spriteSheet.Template[top];
                sourceRectangle.Height = adjustedH;
                for (int x = w + offsetX; x < renderWidth + offsetX - w; x += w)
                {
                    adjustedW = AdjustWidth(renderWidth, offsetX, w, x);

                    sourceRectangle.Width = adjustedW;
                    bounds.X = x;
                    bounds.Width = adjustedW;
                    drawContext.DrawTexture(bounds, texture, color, sourceRectangle);
                }

                sourceRectangle = spriteSheet.Template[topRight];
                sourceRectangle.Height = adjustedH;
                bounds.X = renderWidth + offsetX - w;
                bounds.Width = w;
                drawContext.DrawTexture(bounds, texture, color, sourceRectangle);
            }

            // Middle Lines
            for (int y = h + offsetY; y < renderHeight + offsetY - h; y += h)
            {
                int adjustedW = AdjustWidth(renderWidth, offsetX, w, offsetX);
                int adjustedH = AdjustHeight(renderHeight, offsetY, h, y);

                sourceRectangle = spriteSheet.Template[WindowSpriteSheetTemplate.Left];
                sourceRectangle.Width = adjustedW;
                sourceRectangle.Height = adjustedH;
                bounds.X = offsetX;
                bounds.Y = y;
                bounds.Width = adjustedW;
                bounds.Height = adjustedH;
                drawContext.DrawTexture(bounds, texture, color, sourceRectangle);

                sourceRectangle = spriteSheet.Template[WindowSpriteSheetTemplate.Fill];
                sourceRectangle.Height = adjustedH;
                for (int x = w + offsetX; x < renderWidth + offsetX - w; x += w)
                {
                    adjustedW = AdjustWidth(renderWidth, offsetX, w, x);

                    sourceRectangle.Width = adjustedW;
                    bounds.X = x;
                    bounds.Width = adjustedW;
                    drawContext.DrawTexture(bounds, texture, color, sourceRectangle);
                }

                sourceRectangle = spriteSheet.Template[WindowSpriteSheetTemplate.Right];
                sourceRectangle.Height = adjustedH;
                bounds.X = renderWidth + offsetX - w;
                bounds.Width = w;
                drawContext.DrawTexture(bounds, texture, color, sourceRectangle);
            }

            // Bottom Lines
            {
                int adjustedW = AdjustWidth(renderWidth, offsetX, w, offsetX);

                sourceRectangle = spriteSheet.Template[WindowSpriteSheetTemplate.BottomLeft];
                sourceRectangle.Width = adjustedW;
                bounds.X = offsetX;
                bounds.Y = renderHeight + offsetY - h;
                bounds.Width = adjustedW;
                bounds.Height = h;
                drawContext.DrawTexture(bounds, texture, color, sourceRectangle);

                sourceRectangle = spriteSheet.Template[WindowSpriteSheetTemplate.Bottom];
                for (int x = w + offsetX; x < renderWidth + offsetX - w; x += w)
                {
                    adjustedW = AdjustWidth(renderWidth, offsetX, w, x);

                    sourceRectangle.Width = adjustedW;
                    bounds.X = x;
                    bounds.Width = adjustedW;
                    drawContext.DrawTexture(bounds, texture, color, sourceRectangle);
                }
                bounds.Width = w;

                sourceRectangle = spriteSheet.Template[WindowSpriteSheetTemplate.BottomRight];
                bounds.X = renderWidth + offsetX - w;
                drawContext.DrawTexture(bounds, texture, color, sourceRectangle);
            }
        }

        int AdjustWidth(int windowWidth, int windowOffsetX, int spriteWidth, int currentOffsetX)
        {
            int adjustedWidth = spriteWidth;
            if (windowWidth + windowOffsetX - spriteWidth < currentOffsetX + spriteWidth)
            {
                adjustedWidth -= (currentOffsetX + spriteWidth) - (windowWidth + windowOffsetX - spriteWidth);
            }
            return adjustedWidth;
        }

        int AdjustHeight(int windowHeight, int windowOffsetY, int spriteHeight, int currentOffsetY)
        {
            int adjustedHeight = spriteHeight;
            if (windowHeight + windowOffsetY - spriteHeight < currentOffsetY + spriteHeight)
            {
                adjustedHeight -= (currentOffsetY + spriteHeight) - (windowHeight + windowOffsetY - spriteHeight);
            }
            return adjustedHeight;
        }
    }
}
