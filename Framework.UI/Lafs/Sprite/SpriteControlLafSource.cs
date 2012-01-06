#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Sprite
{
    public class SpriteControlLafSource : ControlLafSourceBase
    {
        Dictionary<Type, SpriteControlLafBase> controlLafs = new Dictionary<Type, SpriteControlLafBase>();

        public int SpriteSize { get; set; }

        public SpriteControlLafSource(Game game)
            : base(game)
        {
            SpriteSize = 16;

            // デフォルトの ControlLafBase を設定しておきます。
            RegisterControlLaf(typeof(Controls.Window), new WindowLaf());
            RegisterControlLaf(typeof(Controls.Button), new ButtonLaf());
            RegisterControlLaf(typeof(Controls.Overlay), new OverlayLaf());
        }

        public void RegisterControlLaf(Type type, SpriteControlLafBase controlLaf)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (controlLaf == null) throw new ArgumentNullException("controlLaf");

            controlLafs[type] = controlLaf;
            controlLaf.Source = this;
        }

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
            foreach (var controlLaf in controlLafs.Values) controlLaf.Initialize();

            base.LoadContent();
        }

        protected override void UnloadContent()
        {
            foreach (var controlLaf in controlLafs.Values) controlLaf.Dispose();
            controlLafs.Clear();

            if (Content != null) Content.Unload();

            base.UnloadContent();
        }
    }
}
