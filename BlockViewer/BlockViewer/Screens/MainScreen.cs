#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Willcraftia.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Serialization;
using Willcraftia.Xna.Framework.UI;
using Willcraftia.Xna.Framework.UI.Animations;
using Willcraftia.Xna.Framework.UI.Controls;
using Willcraftia.Xna.Framework.UI.Lafs;
using Willcraftia.Xna.Framework.UI.Lafs.Debug;
using Willcraftia.Xna.Blocks.Content;
using Willcraftia.Xna.Blocks.Serialization;
using Willcraftia.Xna.Blocks.BlockViewer.Resources;
using Willcraftia.Xna.Blocks.BlockViewer.ViewModels;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Screens
{
    public sealed class MainScreen : Screen
    {
        public const int DefaultLookAndFeelIndex = 0;

        public const int DebugLookAndFeelIndex = 1;

        SelectableLookAndFeelSource selectableLookAndFeelSource = new SelectableLookAndFeelSource();

        DefaultSpriteSheetSource spriteSheetSource;

        MainViewModel mainViewModel;

        ImageButton mainMenuButton;

        BlockMeshView blockMeshView;

        MainMenuWindow mainMenuWindow;

        bool canHandleKey;

        public int SelectedLookAndFeelSourceIndex
        {
            get { return selectableLookAndFeelSource.SelectedIndex; }
            set { selectableLookAndFeelSource.SelectedIndex = value; }
        }

        public MainScreen(Game game)
            : base(game)
        {
            Content.RootDirectory = "Content";

            // Data Context
            mainViewModel = new MainViewModel(game.GraphicsDevice);
            DataContext = mainViewModel;
        }

        protected override void LoadContent()
        {
            InitializeSpriteSheet();
            InitializeLookAndFeelSource();
            InitializeControls();

            base.LoadContent();
        }

        void InitializeSpriteSheet()
        {
            var windowTemplate = new WindowSpriteSheetTemplate(32, 32);
            var windowShadowConverter = new DecoloringTexture2DConverter(new Color(0, 0, 0, 0.5f));
            var windowTexture = Content.Load<Texture2D>("UI/SpriteSheet/Window");
            var windowShadowTexture = windowShadowConverter.Convert(windowTexture);
            var lodWindowTexture = Content.Load<Texture2D>("UI/SpriteSheet/LodWindow");

            spriteSheetSource = new DefaultSpriteSheetSource();
            spriteSheetSource.SpriteSheetMap["Window"] = new SpriteSheet(windowTemplate, windowTexture);
            spriteSheetSource.SpriteSheetMap["WindowShadow"] = new SpriteSheet(windowTemplate, windowShadowTexture);
            spriteSheetSource.SpriteSheetMap["LodWindow"] = new SpriteSheet(windowTemplate, lodWindowTexture);
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
            source.LookAndFeelMap[typeof(LodWindow)] = new SpriteSheetWindowLookAndFeel
            {
                WindowSpriteSheet = spriteSheetSource.GetSpriteSheet("LodWindow")
            };
            source.LookAndFeelMap[typeof(TextBlock)] = new TextBlockLookAndFeel();
            source.LookAndFeelMap[typeof(Overlay)] = new OverlayLookAndFeel();
            source.LookAndFeelMap[typeof(Button)] = new ButtonLookAndFeel
            {
                FocusTexture = Content.Load<Texture2D>("UI/Focus"),
                MouseOverTexture = Content.Load<Texture2D>("UI/MouseOver")
            };
            source.LookAndFeelMap[typeof(Canvas)] = new CanvasLookAndFeel();

            return source;
        }

        void InitializeControls()
        {
            // TODO: テスト コード。
            mainViewModel.StoreSampleBlockMesh();

            var canvas = new Canvas(this);
            canvas.BackgroundColor = Color.CornflowerBlue;
            canvas.HorizontalAlignment = HorizontalAlignment.Stretch;
            canvas.VerticalAlignment = VerticalAlignment.Stretch;
            Desktop.Content = canvas;

            blockMeshView = new BlockMeshView(this)
            {
                Width = Desktop.Width,
                Height = Desktop.Height,
                Focusable = true,
                DataContext = mainViewModel.BlockMeshViewModel
            };
            canvas.Children.Add(blockMeshView);

            mainMenuButton = new ImageButton(this)
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            mainMenuButton.Image.Texture = Content.Load<Texture2D>("UI/MainMenuButton");
            mainMenuButton.TextBlock.Text = Strings.MainMenuButton;
            mainMenuButton.TextBlock.HorizontalAlignment = HorizontalAlignment.Right;
            mainMenuButton.TextBlock.Padding = new Thickness(4);
            mainMenuButton.TextBlock.ForegroundColor = Color.Yellow;
            mainMenuButton.TextBlock.BackgroundColor = Color.Black;
            mainMenuButton.TextBlock.ShadowOffset = new Vector2(2);
            mainMenuButton.Click += (Control s, ref RoutedEventContext c) =>
            {
                mainMenuButton.Visible = false;
                mainMenuWindow.Show();
            };
            canvas.Children.Add(mainMenuButton);

            mainMenuWindow = new MainMenuWindow(this)
            {
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Top
            };
            mainMenuWindow.VisibleChanged += (s, e) =>
            {
                mainMenuButton.Visible = !mainMenuWindow.Visible;
            };

            var overlay = new FadeOverlay(this);
            overlay.OpacityAnimation.From = 1;
            overlay.OpacityAnimation.To = 0;
            overlay.OpacityAnimation.Duration = TimeSpan.FromSeconds(1);
            overlay.OpacityAnimation.Completed += (s, e) =>
            {
                overlay.Close();
                // Desktop をアクティブにしておきます。
                Desktop.Activate();
                // キー操作が行えるようにします。
                canHandleKey = true;
            };
            overlay.Show();

            Root.KeyDown += new RoutedEventHandler(OnKeyDown);

            // BlockMeshView にフォーカスを設定しておきます。
            blockMeshView.Focus();

            // Desktop をアクティブ化します。
            Desktop.Activate();
        }

        void OnKeyDown(Control sender, ref RoutedEventContext context)
        {
            // フェード効果が終わるまでキー操作は無効になります。
            if (!canHandleKey) return;

            if (KeyboardDevice.IsKeyPressed(Keys.Y))
            {
                if (!mainMenuWindow.Visible) mainMenuWindow.Show();
            }
        }
    }
}
