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
    /// スプライト イメージで Control の LaF を描画するための IControlLafSource です。
    /// </summary>
    public class SpriteControlLafSource : ControlLafSourceBase
    {
        /// <summary>
        /// Control の型をキーとし、その Control の LaF を値とするディクショナリ。
        /// </summary>
        Dictionary<Type, SpriteControlLafBase> controlLafs = new Dictionary<Type, SpriteControlLafBase>();

        /// <summary>
        /// 塗り潰しに利用するテクスチャを取得します。
        /// </summary>
        public Texture2D FillTexture { get; private set; }

        /// <summary>
        /// 専用の ContentManager を取得します。
        /// </summary>
        public ContentManager Content { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game">Game。</param>
        public SpriteControlLafSource(Game game)
            : base(game)
        {
            Content = new ContentManager(Game.Services);

            // デフォルトの ControlLafBase を設定しておきます。
            RegisterControlLaf(typeof(Desktop), new DesktopLaf());
            RegisterControlLaf(typeof(Window), new WindowLaf());
            RegisterControlLaf(typeof(Controls.Label), new LabelLaf());
            RegisterControlLaf(typeof(Controls.Button), new ButtonLaf());
            RegisterControlLaf(typeof(Controls.Overlay), new OverlayLaf());
        }

        /// <summary>
        /// LaF を登録します。
        /// </summary>
        /// <param name="type">Control の型。</param>
        /// <param name="controlLaf">SpriteControlLafBase。</param>
        public void RegisterControlLaf(Type type, SpriteControlLafBase controlLaf)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (controlLaf == null) throw new ArgumentNullException("controlLaf");

            controlLafs[type] = controlLaf;
            controlLaf.Source = this;
        }

        /// <summary>
        /// LaF の登録を解除します。
        /// </summary>
        /// <param name="type">Control の型。</param>
        public void DeregisterControlLaf(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            SpriteControlLafBase controlLaf = null;
            if (controlLafs.TryGetValue(type, out controlLaf))
            {
                controlLafs.Remove(type);
                controlLaf.Dispose();
            }
        }

        public override IControlLaf GetControlLaf(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");

            var type = control.GetType();

            SpriteControlLafBase controlLaf = null;
            while (type != typeof(object))
            {
                if (controlLafs.TryGetValue(type, out controlLaf)) break;

                type = type.BaseType;
            }

            return controlLaf;
        }

        protected override void LoadContent()
        {
            FillTexture = Texture2DHelper.CreateFillTexture(Game.GraphicsDevice);

            foreach (var controlLaf in controlLafs.Values) controlLaf.Initialize();

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            if (FillTexture != null) FillTexture.Dispose();

            foreach (var controlLaf in controlLafs.Values) controlLaf.Dispose();
            controlLafs.Clear();

            if (Content != null) Content.Unload();

            base.UnloadContent();
        }
    }
}
