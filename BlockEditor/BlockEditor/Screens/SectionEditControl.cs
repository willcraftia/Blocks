#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Blocks.Serialization;
using Willcraftia.Xna.Blocks.BlockEditor.Models;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Screens
{
    public sealed class SectionEditControl : Control
    {
        const int gridSize = Workspace.GridSize;

        public Color GridColor { get; set; }

        public float CellSize { get; set; }

        Section Section
        {
            get { return DataContext as Section; }
        }

        public SectionEditControl(Screen screen)
            : base(screen)
        {
            GridColor = Color.White;
            CellSize = 16;
        }

        public override void Draw(GameTime gameTime, IDrawContext drawContext)
        {
            base.Draw(gameTime, drawContext);

            DrawGrid(drawContext);
            DrawElements(drawContext);
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var w = CellSize * gridSize + 1;
            var h = CellSize * gridSize + 1;
            var m = Margin;

            return new Size
            {
                Width = w + m.Left + m.Right,
                Height = h + m.Top + m.Bottom
            };
        }

        protected override void OnMouseDown(ref RoutedEventContext context)
        {
            base.OnMouseDown(ref context);

            var mouseState = Screen.MouseDevice.MouseState;
            var location = PointFromScreen(new Vector2(mouseState.X, mouseState.Y));

            int x = (int) (location.X / CellSize);
            if (gridSize < x) x = gridSize;

            int y = (int) (location.Y / CellSize);
            if (gridSize < y) y = gridSize;

            Section.SetMaterial(x, y);
        }

        void DrawGrid(IDrawContext drawContext)
        {
            var rect = new Rect();

            rect.X = 0;
            rect.Width = RenderSize.Width;
            rect.Height = 1;
            for (int y = 0; y <= gridSize; y++)
            {
                rect.Y = CellSize * y;
                drawContext.DrawRectangle(rect, GridColor);
            }

            rect.Y = 0;
            rect.Width = 1;
            rect.Height = RenderSize.Height;
            for (int x = 0; x <= gridSize; x++)
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

            for (int y = 0; y < gridSize; y++)
            {
                rect.Y = CellSize * y + 1;
                for (int x = 0; x < gridSize; x++)
                {
                    var material = Section.GetMaterial(x, y);
                    if (material == null) continue;

                    rect.X = CellSize * x + 1;
                    var color = material.DiffuseColor.ToColor();

                    drawContext.DrawRectangle(rect, color);
                }
            }
        }
    }
}
