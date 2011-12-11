#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI.Visuals.Sprite
{
    public class SpriteControlLafSource : IControlLafSource
    {
        bool contentLoaded;

        Dictionary<Type, IControlLaf> controlLafs;

        // I/F
        public IUIContext UIContext { get; set; }

        public ContentManager Content { get; set; }

        public int SpriteSize { get; set; }

        public SpriteFont Font { get; private set; }

        public SpriteControlLafSource()
        {
            controlLafs = new Dictionary<Type, IControlLaf>();
            SpriteSize = 16;
        }

        public void LoadContent()
        {
            if (contentLoaded) return;

            // この LaF のための ContentManager が未設定ならば生成します。
            if (Content == null) Content = UIContext.CreateContentManager();

            // UI のデフォルト フォントをロードします。
            Font = Content.Load<SpriteFont>("Default");

            // WindowLaf
            var windowLaf = new WindowLaf(this);
            controlLafs[typeof(Window)] = windowLaf;

            // ButtonLaf
            var buttonLaf = new ButtonLaf(this);
            controlLafs[typeof(Controls.Button)] = buttonLaf;

            contentLoaded = true;
        }

        public void UnloadContent()
        {
            if (!contentLoaded) return;

            controlLafs.Clear();

            Content.Unload();

            contentLoaded = false;
        }

        public IControlLaf GetControlLaf(Control control)
        {
            if (control == null) throw new ArgumentNullException("control");

            var type = control.GetType();

            IControlLaf controlLaf = null;
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
    }
}
