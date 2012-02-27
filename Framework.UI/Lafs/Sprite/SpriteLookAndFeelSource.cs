#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Willcraftia.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    /// <summary>
    /// SpriteSheet を用いた Look and Feel を提供する ILookAndFeelSource です。
    /// </summary>
    public class SpriteLookAndFeelSource : LookAndFeelSourceBase
    {
        /// <summary>
        /// Control の型をキーとし、対応する LookAndFeelBase を値とするマップ。
        /// </summary>
        Dictionary<Type, LookAndFeelBase> lookAndFeelMap = new Dictionary<Type, LookAndFeelBase>();

        /// <summary>
        /// ContentManager を取得します。
        /// </summary>
        public ContentManager Content { get; private set; }

        /// <summary>
        /// ISpriteSheetSource を取得します。
        /// </summary>
        public ISpriteSheetSource SpriteSheetSource { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game">Game。</param>
        /// <param name="spriteSheetSource">ISpriteSheetSource。</param>
        public SpriteLookAndFeelSource(Game game, ISpriteSheetSource spriteSheetSource)
            : base(game)
        {
            if (spriteSheetSource == null) throw new ArgumentNullException("spriteSheetSource");
            SpriteSheetSource = spriteSheetSource;

            Content = new ContentManager(Game.Services);

            Register(typeof(Desktop), new DesktopLookAndFeel());
            Register(typeof(Window), new WindowLookAndFeel());
            Register(typeof(Controls.TextBlock), new TextBlockLookAndFeel());
            Register(typeof(Controls.Overlay), new OverlayLookAndFeel());
        }

        /// <summary>
        /// LookAndFeelBase を登録します。
        /// </summary>
        /// <param name="type">Control の型。</param>
        /// <param name="lookAndFeel">LookAndFeelBase。</param>
        public void Register(Type type, LookAndFeelBase lookAndFeel)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (lookAndFeel == null) throw new ArgumentNullException("lookAndFeel");

            lookAndFeelMap[type] = lookAndFeel;
            lookAndFeel.Source = this;
        }

        /// <summary>
        /// LookAndFeelBase の登録を解除します。
        /// </summary>
        /// <param name="type">Control の型。</param>
        public void Deregister(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            LookAndFeelBase lookAndFeel = null;
            if (lookAndFeelMap.TryGetValue(type, out lookAndFeel))
            {
                lookAndFeelMap.Remove(type);
                lookAndFeel.Dispose();
            }
        }

        public override ILookAndFeel GetLookAndFeel(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");

            var type = control.GetType();

            LookAndFeelBase lookAndFeel = null;
            while (type != typeof(object))
            {
                if (lookAndFeelMap.TryGetValue(type, out lookAndFeel)) break;

                type = type.BaseType;
            }

            return lookAndFeel;
        }

        protected override void LoadContent()
        {
            if (!SpriteSheetSource.Initialized) SpriteSheetSource.Initialize();

            foreach (var lookAndFeel in lookAndFeelMap.Values) lookAndFeel.Initialize();

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            foreach (var lookAndFeel in lookAndFeelMap.Values) lookAndFeel.Dispose();
            lookAndFeelMap.Clear();

            SpriteSheetSource.Dispose();
            if (Content != null) Content.Unload();

            base.UnloadContent();
        }
    }
}
