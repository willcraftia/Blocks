#region Using

using System;
using Microsoft.Xna.Framework;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Blocks.BlockEditor.Models;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Screens
{
    public sealed class SectionIndexControl : Control
    {
        public Color GridColor { get; set; }

        public Color CursorColor { get; set; }

        public float CellSize { get; set; }

        Section Section
        {
            get { return DataContext as Section; }
        }

        public SectionIndexControl(Screen screen)
            : base(screen)
        {
            GridColor = Color.White;
            CursorColor = Color.Blue;
            CellSize = 16;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            var w = CellSize * 16 + 1;
            var h = CellSize + 1;
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

            var rect = new Rect();
            rect.Width = RenderSize.Width;
            rect.Height = 1;
            drawContext.DrawRectangle(rect, GridColor);

            rect.Y = RenderSize.Height - 1;
            drawContext.DrawRectangle(rect, GridColor);

            rect.Y = 0;
            rect.Width = 1;
            rect.Height = RenderSize.Height;
            for (int i = 0; i <= 16; i++)
            {
                rect.X = CellSize * i;
                drawContext.DrawRectangle(rect, GridColor);
            }

            rect.Width = CellSize - 1;
            rect.Height = CellSize - 1;
            rect.X = CellSize * Section.Index + 1;
            rect.Y = 1;
            drawContext.DrawRectangle(rect, CursorColor);
        }

        protected override void OnPreviewMouseDown(ref RoutedEventContext context)
        {
            var state = Screen.MouseDevice.MouseState;
            var point = new Vector2
            {
                X = state.X,
                Y = state.Y
            };
            point = PointFromScreen(point);

            Section.Index = (int) (point.X / CellSize);
            context.Handled = true;

            base.OnMouseDown(ref context);
        }
    }
}
