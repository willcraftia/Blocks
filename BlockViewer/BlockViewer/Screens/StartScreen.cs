#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Lafs;
using Willcraftia.Xna.Framework.UI.Lafs.Debug;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class StartScreen : Screen
    {
        public const int DefaultLookAndFeelIndex = 0;

        public const int DebugLookAndFeelIndex = 1;

        SelectableLookAndFeelSource selectableLookAndFeelSource = new SelectableLookAndFeelSource();

        DefaultSpriteSheetSource spriteSheetSource;

        public int SelectedLookAndFeelSourceIndex
        {
            get { return selectableLookAndFeelSource.SelectedIndex; }
            set { selectableLookAndFeelSource.SelectedIndex = value; }
        }

        public StartScreen(Game game)
            : base(game)
        {
            Content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            InitializeSpriteSheetSource();
            InitializeLookAndFeelSource();
            InitializeControls();

            base.LoadContent();
        }

        void InitializeSpriteSheetSource()
        {
            var windowTemplate = new WindowSpriteSheetTemplate(32, 32);
            var windowShadowConverter = new DecoloringTexture2DConverter(new Color(0, 0, 0, 0.5f));
            var windowTexture = Content.Load<Texture2D>("UI/SpriteSheet/Window");
            var windowShadowTexture = windowShadowConverter.Convert(windowTexture);

            spriteSheetSource = new DefaultSpriteSheetSource();
            spriteSheetSource.SpriteSheetMap["Window"] = new SpriteSheet(windowTemplate, windowTexture);
            spriteSheetSource.SpriteSheetMap["WindowShadow"] = new SpriteSheet(windowTemplate, windowShadowTexture);
        }

        void InitializeLookAndFeelSource()
        {
            selectableLookAndFeelSource.Items.Add(CreateDefaultLookAndFeelSource());
            selectableLookAndFeelSource.Items.Add(DebugLooAndFeelUtil.CreateLookAndFeelSource(Game));
            selectableLookAndFeelSource.SelectedIndex = 0;

            LookAndFeelSource = selectableLookAndFeelSource;
        }

        ILookAndFeelSource CreateDefaultLookAndFeelSource()
        {
            var source = new DefaultLookAndFeelSource();

            source.LookAndFeelMap[typeof(Desktop)] = new DesktopLookAndFeel();
            source.LookAndFeelMap[typeof(Window)] = new SpriteSheetWindowLookAndFeel
            {
                WindowSpriteSheet = spriteSheetSource.GetSpriteSheet("Window"),
                WindowShadowSpriteSheet = spriteSheetSource.GetSpriteSheet("WindowShadow")
            };
            source.LookAndFeelMap[typeof(MainMenuWindow)] = new TextureBackgroundLookAndFeel
            {
                Texture = Content.Load<Texture2D>("UI/MainMenuWindow")
            };
            source.LookAndFeelMap[typeof(TextBlock)] = new TextBlockLookAndFeel();
            source.LookAndFeelMap[typeof(Overlay)] = new OverlayLookAndFeel();
            source.LookAndFeelMap[typeof(Button)] = new ButtonLookAndFeel
            {
                FocusTexture = Content.Load<Texture2D>("UI/Focus"),
                MouseOverTexture = Content.Load<Texture2D>("UI/MouseOver")
            };

            return source;
        }

        void InitializeControls()
        {
            var startMenuWindow = new StartMenuWindow(this);
            startMenuWindow.Show();

            var overlay = new FadeOverlay(this);
            overlay.OpacityAnimation.From = 1;
            overlay.OpacityAnimation.To = 0;
            overlay.OpacityAnimation.Duration = TimeSpan.FromSeconds(0.5d);
            overlay.OpacityAnimation.Completed += (s, e) =>
            {
                overlay.Close();
                startMenuWindow.Activate();
            };
            overlay.Show();
        }
    }
}
