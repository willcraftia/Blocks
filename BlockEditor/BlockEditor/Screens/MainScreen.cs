#region Using

using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Lafs;
using Willcraftia.Xna.Framework.UI.Lafs.Debug;
using Willcraftia.Xna.Blocks.BlockEditor.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockEditor.Screens
{
    public sealed class MainScreen : Screen
    {
        public const int DefaultLookAndFeelIndex = 0;

        public const int DebugLookAndFeelIndex = 1;

        SelectableLookAndFeelSource selectableLookAndFeelSource = new SelectableLookAndFeelSource();

        //DefaultSpriteSheetSource spriteSheetSource;

        BlockEditControl blockEditControl;

        public int SelectedLookAndFeelSourceIndex
        {
            get { return selectableLookAndFeelSource.SelectedIndex; }
            set { selectableLookAndFeelSource.SelectedIndex = value; }
        }

        public MainScreen(Game game)
            : base(game)
        {
        }

        protected override void LoadContent()
        {
            InitializeSounds();
            InitializeSpriteSheetSource();
            InitializeLookAndFeelSource();
            InitializeControls();

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            base.UnloadContent();
        }

        void InitializeSounds()
        {
            //RegisterSound(SoundKey.FocusNavigation, "UI/Sounds/FocusNavigation");
            //RegisterSound(SoundKey.Click, "UI/Sounds/Click");
        }

        void InitializeSpriteSheetSource()
        {
            //var windowTemplate = new WindowSpriteSheetTemplate(32, 32);
            //var windowShadowConverter = new DecoloringTexture2DConverter(new Color(0, 0, 0, 0.5f));
            //var windowTexture = Content.Load<Texture2D>("UI/SpriteSheet/Window");
            //var windowShadowTexture = windowShadowConverter.Convert(windowTexture);
            //var lodWindowTexture = Content.Load<Texture2D>("UI/SpriteSheet/LodWindow");

            //spriteSheetSource = new DefaultSpriteSheetSource();
            //spriteSheetSource.SpriteSheetMap["Window"] = new SpriteSheet(windowTemplate, windowTexture);
            //spriteSheetSource.SpriteSheetMap["WindowShadow"] = new SpriteSheet(windowTemplate, windowShadowTexture);
            //spriteSheetSource.SpriteSheetMap["LodWindow"] = new SpriteSheet(windowTemplate, lodWindowTexture);
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
            //source.LookAndFeelMap[typeof(Window)] = new SpriteSheetWindowLookAndFeel
            //{
            //    WindowSpriteSheet = spriteSheetSource.GetSpriteSheet("Window"),
            //    WindowShadowSpriteSheet = spriteSheetSource.GetSpriteSheet("WindowShadow")
            //};
            //source.LookAndFeelMap[typeof(MainMenuWindow)] = new TextureBackgroundLookAndFeel
            //{
            //    Texture = Content.Load<Texture2D>("UI/MainMenuWindow")
            //};
            //source.LookAndFeelMap[typeof(LodWindow)] = new SpriteSheetWindowLookAndFeel
            //{
            //    WindowSpriteSheet = spriteSheetSource.GetSpriteSheet("LodWindow")
            //};
            source.LookAndFeelMap[typeof(TextBlock)] = new TextBlockLookAndFeel();
            source.LookAndFeelMap[typeof(Overlay)] = new OverlayLookAndFeel();
            //source.LookAndFeelMap[typeof(Button)] = new ButtonLookAndFeel
            //{
            //    FocusTexture = Content.Load<Texture2D>("UI/Focus"),
            //    MouseOverTexture = Content.Load<Texture2D>("UI/MouseOver")
            //};
            source.LookAndFeelMap[typeof(Canvas)] = new CanvasLookAndFeel();

            return source;
        }

        void InitializeControls()
        {
            var canvas = new Canvas(this);
            canvas.BackgroundColor = Color.CornflowerBlue;
            canvas.HorizontalAlignment = HorizontalAlignment.Stretch;
            canvas.VerticalAlignment = VerticalAlignment.Stretch;
            Desktop.Content = canvas;

            blockEditControl = new BlockEditControl(this)
            {
                Width = Desktop.Width,
                Height = Desktop.Height,
                Focusable = true,
                DataContext = new BlockEditViewModel(GraphicsDevice)
            };
            canvas.Children.Add(blockEditControl);

            blockEditControl.Focus();
        }
    }
}
