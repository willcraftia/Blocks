﻿#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Blocks.Serialization;
using Willcraftia.Xna.Blocks.BlockEditor.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Screens
{
    public sealed class SectionEditControl : Control
    {
        public Color GridColor { get; set; }

        public float CellSize { get; set; }

        SectionViewModel ViewModel
        {
            get { return DataContext as SectionViewModel; }
        }

        public SectionEditControl(Screen screen)
            : base(screen)
        {
            GridColor = Color.White;
            CellSize = 12;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var w = CellSize * 16 + 1;
            var h = CellSize * 16 + 1;
            var m = Margin;

            return new Size
            {
                Width = w + m.Left + m.Right,
                Height = h + m.Top + m.Bottom
            };
        }

        public override void Draw(GameTime gameTime, IDrawContext drawContext)
        {
            base.Draw(gameTime, drawContext);

            DrawGrid(drawContext);
            DrawElements(drawContext);
        }

        void DrawGrid(IDrawContext drawContext)
        {
            var rect = new Rect();

            rect.X = 0;
            rect.Width = RenderSize.Width;
            rect.Height = 1;
            for (int y = 0; y <= 16; y++)
            {
                rect.Y = CellSize * y;
                drawContext.DrawRectangle(rect, GridColor);
            }

            rect.Y = 0;
            rect.Width = 1;
            rect.Height = RenderSize.Height;
            for (int x = 0; x <= 16; x++)
            {
                rect.X = CellSize * x;
                drawContext.DrawRectangle(rect, GridColor);
            }
        }

        void DrawElements(IDrawContext drawContext)
        {
            var rect = new Rect
            {
                Width = CellSize - 1,
                Height = CellSize - 1
            };

            for (int y = 0; y < 16; y++)
            {
                rect.Y = CellSize * y + 1;
                for (int x = 0; x < 16; x++)
                {
                    var material = ViewModel.GetMaterial(x, y);
                    if (material == null) continue;

                    rect.X = CellSize * x + 1;
                    var color = material.DiffuseColor.ToColor();

                    drawContext.DrawRectangle(rect, color);
                }
            }
        }
    }
}
