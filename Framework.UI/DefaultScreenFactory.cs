#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// デフォルトの IScreenFactory 実装クラスです。
    /// </summary>
    public class DefaultScreenFactory : IScreenFactory
    {
        // I/F
        public bool Initialized { get; private set; }

        /// <summary>
        /// Game を取得します。
        /// </summary>
        public Game Game { get; private set; }

        /// <summary>
        /// ScreenDefinition コレクションを取得します。
        /// </summary>
        public ScreenDefinitionCollection Definitions { get; private set; }

        /// <summary>
        /// デフォルトの SpriteFont を取得または設定します。
        /// Font プロパティは、このインスタンスから生成する全ての Screen の Font プロパティのデフォルト値となります。
        /// </summary>
        /// <remarks>
        /// LoadContent メソッドの呼び出しでこのプロパティにインスタンスが設定されていない場合、
        /// アセット名 "Font/Default" の SpriteFont が自動的にロードされて設定されます。
        /// この時、SpriteFont のロードには Game の Content プロパティが利用されます。
        /// 他のアセット名の SpriteFont を設定したい場合には、サブクラスの LoadContent メソッドで設定します。
        /// </remarks>
        public SpriteFont Font { get; protected set; }

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
        public void Initialize()
        {
            LoadContent();

            // LoadContent メソッドの終了までに Font プロパティが設定されていないならば、デフォルトの振る舞いで設定します。
            if (Font == null) Font = Game.Content.Load<SpriteFont>("Fonts/Default");

            Initialized = true;
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
            // プロパティをインスタンスに設定します。
            PopulateProperties(definition, screen);
            // インスタンスを初期化します。
            InitializeScreenInstance(definition, screen);

            return screen;
        }

        /// <summary>
        /// コンテンツをロードします。
        /// </summary>
        protected virtual void LoadContent() { }

        /// <summary>
        /// コンテンツをアンロードします。
        /// </summary>
        protected virtual void UnloadContent() { }

        /// <summary>
        /// Screen をインスタンス化します。
        /// </summary>
        /// <param name="definition">ScreenDefinition。</param>
        /// <returns>Screen インスタンス。</returns>
        protected virtual Screen CreateScreenInstance(ScreenDefinition definition)
        {
            return Activator.CreateInstance(definition.Type, Game) as Screen;
        }

        /// <summary>
        /// Screen インスタンスのプロパティを設定します。
        /// </summary>
        /// <param name="definition">ScreenDefinition。</param>
        /// <param name="screen">Screen インスタンス。</param>
        protected virtual void PopulateProperties(ScreenDefinition definition, Screen screen)
        {
            // Screen にデフォルトの SpriteFont を設定します。
            // Screen の Font プロパティは、ScreenDefinition のプロパティ定義や、
            // Screen の Initialize メソッド内で、独自の SpriteFont へオーバライドされる可能性があります。
            screen.Font = Font;

            foreach (var property in definition.Properties)
            {
                var propertyInfo = screen.GetType().GetProperty(property.Key);
                if (propertyInfo == null) throw new InvalidOperationException(
                    string.Format("Property '{0}' could not be found in '{1}'.", property.Key, screen.GetType()));

                propertyInfo.SetValue(screen, property.Value, null);
            }

            // Screen が IScreenFactoryAware ならば自分を通知します。
            var screenFactoryAware = screen as IScreenFactoryAware;
            if (screenFactoryAware != null) screenFactoryAware.ScreenFactory = this;
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

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        bool disposed;

        ~DefaultScreenFactory()
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
