#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Visuals.Sprite
{
    public class SpriteControlLafSource : IControlLafSource, IDisposable
    {
        string contentRootDirectory;

        Dictionary<Type, ControlLafBase> controlLafs;

        // I/F
        public IUIContext UIContext { get; set; }

        public ContentManager Content { get; private set; }

        public int SpriteSize { get; set; }

        public SpriteFont Font { get; private set; }

        public SpriteControlLafSource() : this(null) { }

        public SpriteControlLafSource(string contentRootDirectory)
        {
            this.contentRootDirectory = contentRootDirectory;
            SpriteSize = 16;

            controlLafs = new Dictionary<Type, ControlLafBase>();

            // デフォルトの ControlLafBase を設定しておきます。
            RegisterControlLaf(typeof(Window), new WindowLaf());
            RegisterControlLaf(typeof(Controls.Button), new ButtonLaf());
            RegisterControlLaf(typeof(Overlay), new OverlayLaf());
        }

        public void RegisterControlLaf(Type type, ControlLafBase controlLaf)
        {
            if (type == null) throw new ArgumentNullException("type");
            if (controlLaf == null) throw new ArgumentNullException("controlLaf");

            controlLafs[type] = controlLaf;
            controlLaf.Source = this;
        }

        public void DeregisterControlLaf(Type type)
        {
            if (type == null) throw new ArgumentNullException("type");

            ControlLafBase controlLaf = null;
            if (controlLafs.TryGetValue(type, out controlLaf))
            {
                controlLafs.Remove(type);
                controlLaf.Dispose();
            }
        }

        // I/F
        public void Initialize()
        {
            // この LaF のための ContentManager を生成します。
            Content = UIContext.CreateContentManager();
            if (!string.IsNullOrEmpty(contentRootDirectory)) Content.RootDirectory = contentRootDirectory;

            LoadContent();
        }

        protected void LoadContent()
        {
            // UI のデフォルト フォントをロードします。
            Font = Content.Load<SpriteFont>("Default");

            foreach (var controlLaf in controlLafs.Values) controlLaf.Initialize();
        }

        protected void UnloadContent()
        {
            foreach (var controlLaf in controlLafs.Values) controlLaf.Dispose();
            controlLafs.Clear();

            if (Content != null) Content.Unload();
        }

        public IControlLaf GetControlLaf(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");

            var type = control.GetType();

            ControlLafBase controlLaf = null;
            while (type != typeof(object))
            {
                if (controlLafs.TryGetValue(type, out controlLaf)) break;

                type = type.BaseType;
            }

            return controlLaf;
        }

        public void DrawString(Rectangle bounds, SpriteFont font, Color color, string text, HorizontalAlignment hAlign, VerticalAlignment vAlign, Vector2 offset)
        {
            if (font == null) font = Font;
            var position = CalculateTextPosition(bounds, font, text, hAlign, vAlign) + offset;
            UIContext.SpriteBatch.DrawString(font, text, position, color);
        }

        public Vector2 CalculateTextPosition(Rectangle bounds, SpriteFont font, string text, HorizontalAlignment hAlign, VerticalAlignment vAlign)
        {
            if (font == null) throw new ArgumentNullException("font");
            if (text == null) throw new ArgumentNullException("text");

            Vector2 textSize = font.MeasureString(text);
            float x;
            float y;

            switch (hAlign)
            {
                case HorizontalAlignment.Left:
                    {
                        x = bounds.Left;
                        break;
                    }
                case HorizontalAlignment.Right:
                    {
                        x = bounds.Right - textSize.X;
                        break;
                    }
                case HorizontalAlignment.Center:
                default:
                    {
                        x = (bounds.Width - textSize.X) / 2.0f + bounds.Left;
                        break;
                    }
            }

            switch (vAlign)
            {
                case VerticalAlignment.Top:
                    {
                        y = bounds.Top;
                        break;
                    }
                case VerticalAlignment.Bottom:
                    {
                        y = bounds.Bottom - font.LineSpacing;
                        break;
                    }
                case VerticalAlignment.Center:
                default:
                    {
                        y = (bounds.Height - font.LineSpacing) / 2.0f + bounds.Top;
                        break;
                    }
            }

            return new Vector2(x, y);
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        bool disposed;

        ~SpriteControlLafSource()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing) UnloadContent();
                disposed = true;
            }
        }

        #endregion
    }
}
