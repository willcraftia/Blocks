﻿#region Using

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
        /// Window の影を描画するための SpriteSheet のアセット名を取得または設定します。
        /// null を指定した場合、影の描画は行いません。
        /// デフォルトは null です。
        /// </summary>
        public string ShadowSpriteSheetName { get; set; }

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
        public Point ShadowOffset { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public WindowLookAndFeel()
        {
            SpriteWidth = 16;
            SpriteHeight = 16;
            ShadowColor = Color.Black;
            ShadowOpacity = 1;
            ShadowOffset = new Point(8, 8);
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

            var windowSpriteSheetAssetName = WindowSpriteSheetName ?? "Window";
            var windowSpriteSheetTexture = Source.Content.Load<Texture2D>(windowSpriteSheetAssetName);
            windowSpriteSheet = new SpriteSheet(windowSpriteSheetTexture);
            PrepareSpriteSheet(windowSpriteSheet);

            if (!string.IsNullOrEmpty(ShadowSpriteSheetName))
            {
                var shadowSpriteSheetTexture = Source.Content.Load<Texture2D>(ShadowSpriteSheetName);
                shadowSpriteSheet = new SpriteSheet(shadowSpriteSheetTexture);
                PrepareSpriteSheet(shadowSpriteSheet);
            }

            base.LoadContent();
        }

        public override void Draw(Control control, IDrawContext drawContext)
        {
            if (shadowSpriteSheet != null)
                DrawWindow(control, drawContext, shadowSpriteSheet, ShadowColor * ShadowOpacity, ShadowOffset);

            DrawWindow(control, drawContext, windowSpriteSheet, Color.White, Point.Zero);
        }

        protected void DrawWindow(Control control, IDrawContext drawContext, SpriteSheet spriteSheet, Color color, Point offset)
        {
            var renderSize = control.RenderSize;
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
            sourceRectangle = spriteSheet.SourceRectangles["TopLeft"];
            bounds.X = offsetX;
            bounds.Y = offsetY;
            bounds.Width = w;
            bounds.Height = h;
            drawContext.DrawTexture(bounds, texture, color, sourceRectangle);

            sourceRectangle = spriteSheet.SourceRectangles["Top"];
            for (int x = w + offsetX; x < renderWidth + offsetX - w; x += w)
            {
                int adjustedWidth = w;
                if (renderWidth + offsetX - w < x + w)
                {
                    adjustedWidth -= (x + w) - (renderWidth + offsetX - w);
                }
                sourceRectangle.Width = adjustedWidth;
                bounds.Width = adjustedWidth;
                bounds.X = x;
                drawContext.DrawTexture(bounds, texture, color, sourceRectangle);
            }
            bounds.Width = w;

            sourceRectangle = spriteSheet.SourceRectangles["TopRight"];
            bounds.X = renderWidth + offsetX - w;
            drawContext.DrawTexture(bounds, texture, color, sourceRectangle);

            // Middle Lines
            for (int y = h + offsetY; y < renderHeight + offsetY - h; y += h)
            {
                int adjustedHeight = h;
                if (renderHeight + offsetY - h < y + h)
                {
                    adjustedHeight -= (y + h) - (renderHeight + offsetY - h);
                }
                bounds.Height = adjustedHeight;

                sourceRectangle = spriteSheet.SourceRectangles["Left"];
                sourceRectangle.Height = adjustedHeight;
                bounds.X = offsetX;
                bounds.Y = y;
                drawContext.DrawTexture(bounds, texture, color, sourceRectangle);

                sourceRectangle = spriteSheet.SourceRectangles["Fill"];
                sourceRectangle.Height = adjustedHeight;
                for (int x = w + offsetX; x < renderWidth + offsetX - w; x += w)
                {
                    int adjustedWidth = w;
                    if (renderWidth + offsetX - w < x + w)
                    {
                        adjustedWidth -= (x + w) - (renderWidth + offsetX - w);
                    }
                    sourceRectangle.Width = adjustedWidth;
                    bounds.Width = adjustedWidth;
                    bounds.X = x;
                    drawContext.DrawTexture(bounds, texture, color, sourceRectangle);
                }
                bounds.Width = w;

                sourceRectangle = spriteSheet.SourceRectangles["Right"];
                sourceRectangle.Height = adjustedHeight;
                bounds.X = renderWidth + offsetX - w;
                drawContext.DrawTexture(bounds, texture, color, sourceRectangle);
            }
            bounds.Height = h;

            // Bottom Lines
            sourceRectangle = spriteSheet.SourceRectangles["BottomLeft"];
            bounds.X = offsetX;
            bounds.Y = renderHeight + offsetY - h;
            drawContext.DrawTexture(bounds, texture, color, sourceRectangle);

            sourceRectangle = spriteSheet.SourceRectangles["Bottom"];
            for (int x = w + offsetX; x < renderWidth + offsetX - w; x += w)
            {
                int adjustedWidth = w;
                if (renderWidth + offsetX - w < x + w)
                {
                    adjustedWidth -= (x + w) - (renderWidth + offsetX - w);
                }
                sourceRectangle.Width = adjustedWidth;
                bounds.Width = adjustedWidth;
                bounds.X = x;
                drawContext.DrawTexture(bounds, texture, color, sourceRectangle);
            }
            bounds.Width = w;

            sourceRectangle = spriteSheet.SourceRectangles["BottomRight"];
            bounds.X = renderWidth + offsetX - w;
            drawContext.DrawTexture(bounds, texture, color, sourceRectangle);
        }

        protected void PrepareSpriteSheet(SpriteSheet spriteSheet)
        {
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
        }
    }
}
