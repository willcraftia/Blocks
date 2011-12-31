#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Lafs.Debug
{
    public class DebugControlLafSource : ControlLafSourceBase
    {
        Dictionary<Type, DebugControlLafBase> controlLafs = new Dictionary<Type, DebugControlLafBase>();

        public DebugControlLafSource(IServiceProvider serviceProvider) : this(serviceProvider, null) { }

        public DebugControlLafSource(IServiceProvider serviceProvider, string contentRootDirectory)
            : base(serviceProvider, contentRootDirectory)
        {
            // デフォルトの ControlLafBase を設定しておきます。
            RegisterControlLaf(typeof(Control), new DefaultControlLaf());
            RegisterControlLaf(typeof(Controls.Window), new WindowLaf());
            RegisterControlLaf(typeof(Controls.Button), new ButtonLaf());
            RegisterControlLaf(typeof(Controls.Overlay), new OverlayLaf());
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
