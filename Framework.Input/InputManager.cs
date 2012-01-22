#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    /// <summary>
    /// ユーザ入力を管理するクラスです。
    /// </summary>
    public class InputManager : GameComponent, IInputService
    {
        // I/F
        public MouseDevice MouseDevice { get; private set; }

        // I/F
        public KeyboardDevice KeyboardDevice { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game">Game。</param>
        public InputManager(Game game)
            : base(game)
        {
            Game.Services.AddService(typeof(IInputService), this);
        }

        public override void Initialize()
        {
            MouseDevice = new MouseDevice();
            KeyboardDevice = new KeyboardDevice();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            MouseDevice.Update();
            KeyboardDevice.Update();

            base.Update(gameTime);
        }
    }
}
