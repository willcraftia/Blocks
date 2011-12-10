#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    public class InputManager : GameComponent, IInputService
    {
        IMouse mouse;

        public IMouse Mouse
        {
            get { return mouse; }
        }

        public InputManager(Game game)
            : base(game)
        {
            Game.Services.AddService(typeof(IInputService), this);
        }

        public override void Initialize()
        {
            mouse = new DefaultMouse();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            mouse.Update();

            base.Update(gameTime);
        }
    }
}
