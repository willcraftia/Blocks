#region Using

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
        public float CellSize { get; set; }

        SectionViewModel ViewModel
        {
            get { return DataContext as SectionViewModel; }
        }

        public SectionEditControl(Screen screen)
            : base(screen)
        {
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return new Size
            {
                Width = CellSize * 16 + 1,
                Height = CellSize * 16 + 1
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
                drawContext.DrawRectangle(rect, ForegroundColor);
            }

            rect.Y = 0;
            rect.Width = 1;
            rect.Height = RenderSize.Height;
            for (int x = 0; x <= 16; x++)
            {
                rect.X = CellSize * x;
                drawContext.DrawRectangle(rect, ForegroundColor);
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
