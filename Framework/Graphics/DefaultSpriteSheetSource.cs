#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.Graphics
{
    /// <summary>
    /// ISpriteSheetSource のデフォルト実装です。
    /// </summary>
    public class DefaultSpriteSheetSource : ISpriteSheetSource
    {
        /// <summary>
        /// SpriteSheet の ID をキーに SpriteSheet を値とするマップ。
        /// </summary>
        Dictionary<string, SpriteSheet> spriteSheetMap = new Dictionary<string, SpriteSheet>();

        /// <summary>
        /// Game を取得します。
        /// </summary>
        public Game Game { get; private set; }

        /// <summary>
        /// SpriteSheet の ID をキーに SpriteSheetDefinition を値とするマップを取得します。
        /// </summary>
        public Dictionary<string, SpriteSheetDefinition> DefinitionMap { get; private set; }

        /// <summary>
        /// ContentManager を取得します。
        /// </summary>
        public ContentManager Content { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game">Game。</param>
        public DefaultSpriteSheetSource(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");
            Game = game;
            DefinitionMap = new Dictionary<string, SpriteSheetDefinition>();
            Content = new ContentManager(Game.Services);
        }

        // I/F
        public bool Initialized { get; private set; }

        // I/F
        public void Initialize()
        {
            LoadContent();

            Initialized = true;
        }

        // I/F
        public SpriteSheet GetSpriteSheet(string id)
        {
            if (id == null) throw new ArgumentNullException("id");

            SpriteSheet spriteSheet;
            spriteSheetMap.TryGetValue(id, out spriteSheet);
            return spriteSheet;
        }

        /// <summary>
        /// Initialize メソッドから呼び出されます。
        /// </summary>
        protected virtual void LoadContent()
        {
            foreach (var entry in DefinitionMap)
            {
                var id = entry.Key;
                var definition = entry.Value;

                var texture = Content.Load<Texture2D>(definition.Location);
                var converter = definition.Converter;
                if (converter != null) texture = converter.Convert(texture);

                spriteSheetMap[id] = new SpriteSheet(definition.Template, texture);
            }
        }

        /// <summary>
        /// Dispose メソッドから呼び出されます。
        /// </summary>
        protected virtual void UnloadContent()
        {
            Content.Unload();
        }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        bool disposed;

        ~DefaultSpriteSheetSource()
        {
            Dispose(false);
        }

        void Dispose(bool disposing)
        {
            if (disposed) return;

            if (disposing) UnloadContent();

            disposed = true;
        }

        #endregion
    }
}
