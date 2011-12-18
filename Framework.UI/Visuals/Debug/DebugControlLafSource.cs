﻿#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Visuals.Debug
{
    public class DebugControlLafSource : ControlLafSourceBase
    {
        string contentRootDirectory;

        Dictionary<Type, DebugControlLafBase> controlLafs;

        public SpriteFont Font { get; private set; }
        
        public DebugControlLafSource() : this(null) { }

        public DebugControlLafSource(string contentRootDirectory)
            : base(contentRootDirectory)
        {
            this.contentRootDirectory = contentRootDirectory;

            controlLafs = new Dictionary<Type, DebugControlLafBase>();

            // デフォルトの ControlLafBase を設定しておきます。
            RegisterControlLaf(typeof(Window), new WindowLaf());
            RegisterControlLaf(typeof(Controls.Button), new ButtonLaf());
            RegisterControlLaf(typeof(Overlay), new OverlayLaf());
        }

        public void RegisterControlLaf(Type type, DebugControlLafBase controlLaf)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (controlLaf == null) throw new ArgumentNullException("controlLaf");

            controlLafs[type] = controlLaf;
            controlLaf.Source = this;
        }

        public void DeregisterControlLaf(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            DebugControlLafBase controlLaf = null;
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

            DebugControlLafBase controlLaf = null;
            while (type != typeof(object))
            {
                if (controlLafs.TryGetValue(type, out controlLaf)) break;

                type = type.BaseType;
            }

            return controlLaf;
        }

        protected override void LoadContent()
        {
            // UI のデフォルト フォントをロードします。
            Font = Content.Load<SpriteFont>("Default");

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