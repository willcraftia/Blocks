#region Using

using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

#endregion

namespace Willcraftia.Xna.Framework.Content.Pipeline
{
    /// <summary>
    /// テキスト ファイルで定義された文字で SpriteFont をビルドするプロセッサです。
    /// </summary>
    [ContentProcessor(DisplayName = "Character Resource Font Processor")]
    public class CharacterResourceFontProcessor : FontDescriptionProcessor
    {
        /// <summary>
        /// 文字を定義したテキスト ファイルのパスを取得または設定します。
        /// </summary>
        [DefaultValue("Fonts/CharacterResource.txt")]
        public string CharacterResourcePath { get; set; }

        public override SpriteFontContent Process(FontDescription input, ContentProcessorContext context)
        {
            // テキスト ファイルのパスを解決します。
            var path = (!string.IsNullOrEmpty(CharacterResourcePath)) ? CharacterResourcePath : "Fonts/CharacterResource.txt";
            var fullPath = Path.GetFullPath(path);

            // テキスト ファイルを依存ファイルとして登録します。
            context.AddDependency(fullPath);
            
            // ファイル内の文字定義を読み込みます。
            string characters = File.ReadAllText(fullPath, Encoding.UTF8);

            // FontDescription に文字を登録します。
            foreach (var c in characters) input.Characters.Add(c);

            return base.Process(input, context);
        }
    }
}
