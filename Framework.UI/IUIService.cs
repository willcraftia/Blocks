#region Using

using System;

#endregion

namespace Willcraftia.Xna.Framework.UI
{
    /// <summary>
    /// UI サービスのインタフェースです。
    /// </summary>
    public interface IUIService
    {
        /// <summary>
        /// 指定された T 型が表す Screen を表示します。
        /// T で指定する型の Type.AssemblyQualifiedName プロパティが Screen の名前として用いられます。
        /// </summary>
        /// <typeparam name="T">Screen の型。</typeparam>
        void Show<T>() where T : Screen;

        /// <summary>
        /// 指定された型が表す Screen を表示します。
        /// type で指定する型の Type.AssemblyQualifiedName プロパティが Screen の名前として用いられます。
        /// </summary>
        /// <param name="type">Screen の型。</param>
        void Show(Type type);

        /// <summary>
        /// 指定の名前で定義された Screen を表示します。
        /// </summary>
        /// <remarks>
        /// 指定した Screen は即座に表示されるのではなく、次の Update メソッドの呼び出しまで表示を待機します。
        /// </remarks>
        /// <param name="screenName">Screen の名前。</param>
        void Show(string screenName);

        /// <summary>
        /// 指定の Screen を表示します。
        /// </summary>
        /// <remarks>
        /// 指定した Screen は即座に表示されるのではなく、次の Update メソッドの呼び出しまで表示を待機します。
        /// </remarks>
        /// <param name="screen">Screen。</param>
        void Show(Screen screen);
    }
}
