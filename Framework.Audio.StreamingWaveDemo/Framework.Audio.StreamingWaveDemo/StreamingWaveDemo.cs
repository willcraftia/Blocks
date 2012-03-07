#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.Audio.StreamingWaveDemo
{
    public class StreamingWaveDemo : Game
    {
        GraphicsDeviceManager graphics;
        
        SpriteBatch spriteBatch;
        
        SpriteFont font;

        DynamicSoundEffectInstance dynamicSound;

        StreamingWave streamingWave;

        KeyboardState previousKeyboardState;

        public StreamingWaveDemo()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/Default");

            var waveFileStream = TitleContainer.OpenStream(@"Content\Sounds\48K16BSLoop.wav");

            streamingWave = new StreamingWave(waveFileStream, TimeSpan.FromMilliseconds(100));
            dynamicSound = streamingWave.DynamicSound;
        }

        protected override void UnloadContent()
        {
            streamingWave.Dispose();
        }

        protected override void Update(GameTime gameTime)
        {
            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Escape)) Exit();

            if (previousKeyboardState != keyboardState)
            {
                if (keyboardState.IsKeyDown(Keys.A)) dynamicSound.Play();
                if (keyboardState.IsKeyDown(Keys.B)) dynamicSound.Stop();
                if (keyboardState.IsKeyDown(Keys.Space)) streamingWave.Looped = !streamingWave.Looped;

                previousKeyboardState = keyboardState;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            var fontSize = font.MeasureString("X");
            var position = new Vector2(16, 32);

            spriteBatch.Begin();

            DrawString("[A]:      Call DynamicSoundEffectInstance.Play()", position);

            position.Y += fontSize.Y;
            DrawString("[B]:      Call DynamicSoundEffectInstance.Stop()", position);
            
            position.Y += fontSize.Y;
            DrawString("[Space]:  Switch StreamingWave.Looped()", position);

            position.Y += fontSize.Y;
            DrawString("[Escape]: Game.Exit()", position);

            position.Y += 32;

            position.Y += fontSize.Y;
            DrawString(string.Format("DynamicSoundEffectInstance.State: {0}", dynamicSound.State), position);

            position.Y += fontSize.Y;
            DrawString(string.Format("StreamingWave.Looped:             {0}", streamingWave.Looped), position);

            spriteBatch.End();

            base.Draw(gameTime);
        }

        void DrawString(string text, Vector2 position)
        {
            spriteBatch.DrawString(font, text, position, Color.White);
        }
    }
}
