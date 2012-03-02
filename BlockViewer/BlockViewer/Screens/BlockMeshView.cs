#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Input;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Blocks.BlockViewer.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class BlockMeshView : Control
    {
        int mouseOffsetX;
        
        int mouseOffsetY;

        BlockMeshViewModel ViewModel
        {
            get { return DataContext as BlockMeshViewModel; }
        }

        public BlockMeshView(Screen screen) : base(screen) { }

        protected override void OnMouseMove(ref RoutedEventContext context)
        {
            if (!ViewModel.CameraMovable) return;

            var mouseDevice = Screen.MouseDevice;
            var mouseState = mouseDevice.MouseState;

            if (mouseDevice.IsButtonPressed(MouseButtons.Right))
            {
                // OnMouseDown で判定しようとすると OnMouseMove の後になるため、
                // ここで押下判定を行なっています。
                mouseOffsetX = mouseState.X;
                mouseOffsetY = mouseState.Y;
            }

            if (mouseState.RightButton == ButtonState.Pressed)
            {
                // 指定の角度方向へカメラを移動させます。
                var angleSign = new Vector2
                {
                    X = Math.Sign(mouseState.Y - mouseOffsetY),
                    Y = Math.Sign(mouseState.X - mouseOffsetX)
                };
                ViewModel.MoveCamera(angleSign);

                mouseOffsetX = mouseState.X;
                mouseOffsetY = mouseState.Y;
            }

            base.OnMouseMove(ref context);
        }

        public override void Draw(GameTime gameTime, IDrawContext drawContext)
        {
            base.Draw(gameTime, drawContext);

            var bounds = new Rect(0, 0, RenderSize.Width, RenderSize.Height);
            using (var draw3d = drawContext.BeginDraw3D())
            {
                using (var localViewport = drawContext.BeginViewport(bounds))
                {
                    using (var localClipping = drawContext.BeginNewClip(bounds))
                    {
                        Draw3D(gameTime, drawContext);
                    }
                }
            }
        }

        void Draw3D(GameTime gameTime, IDrawContext drawContext)
        {
            ViewModel.Draw();
        }
    }
}
