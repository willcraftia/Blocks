#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

#endregion

namespace Willcraftia.Xna.Framework.UI.ScreenFactories
{
    /// <summary>
    /// デフォルトの IScreenFactory 実装クラスです。
    /// </summary>
    public class DefaultScreenFactory : IScreenFactory
    {
        /// <summary>
        /// Game を取得します。
        /// </summary>
        public Game Game { get; private set; }

        /// <summary>
        /// ScreenDefinition コレクションを取得します。
        /// </summary>
        public ScreenDefinitionCollection Definitions { get; private set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        /// <param name="game">Game。</param>
        public DefaultScreenFactory(Game game)
        {
            if (game == null) throw new ArgumentNullException("game");
            Game = game;

            Definitions = new ScreenDefinitionCollection();
        }

        // I/F
        public Screen CreateScreen(string screenName)
        {
            if (screenName == null) throw new ArgumentNullException("screenName");
            if (!Definitions.Contains(screenName)) throw new ArgumentException("Screen definition could not be found.", "screenName");
            
            // 定義を取得します。
            var definition = Definitions[screenName];
            // Screen をインスタンス化します。
            var screen = CreateScreenInstance(definition);
            // 定義にあるプロパティをインスタンスに設定します。
            PopulateProperties(definition, screen);
            // インスタンスを初期化します。
            InitializeScreenInstance(definition, screen);

            return screen;
        }

        /// <summary>
        /// ScreenDefinition に基づいて Screen をインスタンス化します。
        /// </summary>
        /// <param name="definition">ScreenDefinition。</param>
        /// <returns>Screen インスタンス。</returns>
        protected virtual Screen CreateScreenInstance(ScreenDefinition definition)
        {
            return Activator.CreateInstance(definition.Type, Game) as Screen;
        }

        /// <summary>
        /// ScreenDefinition に基づいて Screen インスタンスのプロパティを設定します。
        /// </summary>
        /// <param name="definition">ScreenDefinition。</param>
        /// <param name="screen">Screen インスタンス。</param>
        protected virtual void PopulateProperties(ScreenDefinition definition, Screen screen)
        {
            foreach (var property in definition.Properties)
            {
                var propertyInfo = screen.GetType().GetProperty(property.Key);
                if (propertyInfo == null) throw new InvalidOperationException(
                    string.Format("Property '{0}' could not be found in '{1}'.", property.Key, screen.GetType()));

                propertyInfo.SetValue(screen, property.Value, null);
            }
        }

        /// <summary>
        /// Screen インスタンスを初期化します。
        /// </summary>
        /// <remarks>
        /// このメソッドは、PopulateProperties メソッド呼び出しの後に呼び出されます。
        /// </remarks>
        /// <param name="definition"></param>
        /// <param name="screen"></param>
        protected virtual void InitializeScreenInstance(ScreenDefinition definition, Screen screen)
        {
            screen.Initialize();
        }
    }
}
