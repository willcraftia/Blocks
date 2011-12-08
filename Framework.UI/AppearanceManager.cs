#region Using

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    public class AppearanceManager : DrawableGameComponent, IAppearanceService
    {
        public SpriteBatch SpriteBatch { get; private set; }

        public Texture2D FillTexture { get; private set; }

        public Screen Screen { get; set; }

        public AppearanceManager(Game game)
            : base(game)
        {
            // サービスとして登録
            Game.Services.AddService(typeof(IAppearanceService), this);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            FillTexture = new Texture2D(GraphicsDevice, 1, 1);
            FillTexture.SetData<Color>(new Color[] { Color.White });

            base.LoadContent();
        }

        public override void Draw(GameTime gameTime)
        {
            if (Screen == null) return;

            DrawControl(Screen);
        }

        void DrawControl(Control control)
        {
            if (control.Appearance != null)
            {
                control.Appearance.Draw(control);
            }

            foreach (var child in control.Children)
            {
                DrawControl(child);
            }
        }
    }
}
