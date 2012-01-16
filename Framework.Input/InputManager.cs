#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    /// <summary>
    /// ���[�U���͂��Ǘ�����N���X�ł��B
    /// </summary>
    public class InputManager : GameComponent, IInputService
    {
        // I/F
        public MouseDevice Mouse { get; private set; }

        // I/F
        public KeyboardDevice Keyboard { get; private set; }

        /// <summary>
        /// �C���X�^���X�𐶐����܂��B
        /// </summary>
        /// <param name="game">Game�B</param>
        public InputManager(Game game)
            : base(game)
        {
            Game.Services.AddService(typeof(IInputService), this);
        }

        public override void Initialize()
        {
            Mouse = new MouseDevice();
            Keyboard = new KeyboardDevice();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            Mouse.Update();
            Keyboard.Update();

            base.Update(gameTime);
        }
    }
}
