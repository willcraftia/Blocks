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
        /// Window を描画するための SpriteSheet。
        /// </summary>
        SpriteSheet windowSpriteSheet;

        /// <summary>
        /// Window の影を描画するための SpriteSheet。
        /// </summary>
        SpriteSheet shadowSpriteSheet;

        /// <summary>
        /// Window を描画するための SpriteSheet のアセット名を取得または設定します。
        /// null を指定した場合、アセット名はデフォルトで "Window" が仮定されます。
        /// デフォルトは null です。
        /// </summary>
        public string WindowSpriteSheetName { get; set; }

        /// <summary>
        /// SpriteSheet 内の各スプライト イメージの幅を取得または設定します。
        /// デフォルトは 16 です。
        /// </summary>
        public int SpriteWidth { get; set; }

        /// <summary>
        /// SpriteSheet 内の各スプライト イメージの高さを取得または設定します。
        /// デフォルトは 16 です。
        /// </summary>
        public int SpriteHeight { get; set; }

        /// <summary>
        /// Window を描画するための SpriteSheet が、
        /// タイトル描画用スプライト イメージを含むかどうかを示す値を取得または設定します。
        /// タイトル描画用スプライト イメージは、
        /// Window.Title プロパティが null または空以外の場合に用いられます。
        /// </summary>
        /// <value>
        /// true (Window を描画するための SpriteSheet がタイトル描画用スプライト イメージを含む場合)、
        /// false (それ以外の場合)。
        /// </value>
        public bool TitleEnabled { get; set; }

        /// <summary>
        /// Window の影を描画するかどうかを示す値を取得または設定します。
        /// </summary>
        /// <value>
        /// true (Window の影を描画する場合)、false (それ以外の場合)。
        /// </value>
        public bool ShadowEnabled { get; set; }

        /// <summary>
        /// Window の影の色を取得または設定します。
        /// </summary>
        public Color ShadowColor { get; set; }

        /// <summary>
        /// Window の影の透明度を取得または設定します。
        /// </summary>
        public float ShadowOpacity { get; set; }

        /// <summary>
        /// Window の影の描画位置を取得または設定します。
        /// </summary>
        public Vector2 ShadowOffset { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public WindowLookAndFeel()
        {
            SpriteWidth = 16;
            SpriteHeight = 16;
            TitleEnabled = false;
            ShadowEnabled = true;
            ShadowColor = Color.Black;
            ShadowOpacity = 0.5f;
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

            var template = new WindowSpriteSheetTemplate(SpriteWidth, SpriteHeight, TitleEnabled);

            var windowSpriteSheetAssetName = WindowSpriteSheetName ?? "Window";
            var windowSpriteSheetTexture = Source.Content.Load<Texture2D>(windowSpriteSheetAssetName);
            windowSpriteSheet = new SpriteSheet(template, windowSpriteSheetTexture);

            if (ShadowEnabled)
            {
                var shadowSpriteSheetTexture = CreateShadowSpriteSheetTexture(windowSpriteSheetTexture);
                shadowSpriteSheet = new SpriteSheet(template, shadowSpriteSheetTexture);
            }

            base.LoadContent();
        }

        public override void Draw(Control control, IDrawContext drawContext)
        {
            var window = control as Window;

            if (ShadowEnabled)
                DrawWindow(window, drawContext, shadowSpriteSheet, ShadowColor, ShadowOffset);

            DrawWindow(window, drawContext, windowSpriteSheet, Color.White, Vector2.Zero);
        }

        protected void DrawWindow(Window window, IDrawContext drawContext, SpriteSheet spriteSheet, Color color, Vector2 offset)
        {
            var renderSize = window.RenderSize;
            var texture = spriteSheet.Texture;

            // 全てのスプライト イメージが同サイズであることを強制します。
            // SpriteSheet には異なるサイズで指定できますが、このクラスでは異なるサイズを取り扱うことができません。
            // 異なるサイズを扱おうとすると、どのスプライト イメージのサイズに基準を揃えるかの制御が複雑になります。
            int w = SpriteWidth;
            int h = SpriteHeight;

            // 計算誤差の問題から先に int 化しておきます。
            int offsetX = (int) offset.X;
            int offsetY = (int) offset.Y;
            int renderWidth = (int) renderSize.Width;
            int renderHeight = (int) renderSize.Height;

            var bounds = new Rect();
            Rectangle sourceRectangle;

            // Top Lines
            {
                var titleDrawn = (TitleEnabled && window.TitleContent != null);

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

        Texture2D CreateShadowSpriteSheetTexture(Texture2D texture)
        {
            var colors = new Color[texture.Width * texture.Height];
            texture.GetData(colors);

            for (int i = 0; i < colors.Length; i++)
            {
                // 元画像内の不透明部分を、透明度が ShadowOpacity の白にした画像を作成します。
                var alpha = colors[i].ToVector4().W;
                if (alpha != 0) alpha = ShadowOpacity;
                colors[i] = new Color(1, 1, 1, alpha);
            }
            var shadowTexture = new Texture2D(texture.GraphicsDevice, texture.Width, texture.Height, false, SurfaceFormat.Color);
            shadowTexture.SetData(colors);
            return shadowTexture;
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
