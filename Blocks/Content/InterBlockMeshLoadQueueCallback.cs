#region Using

using System;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// InterBlockMesh のロード完了で呼び出されるコールバック メソッドを定義します。
    /// </summary>
    /// <param name="name">ロードされた Block の名前。</param>
    /// <param name="result">ロードされた InterBlockMesh。</param>
    public delegate void InterBlockMeshLoadQueueCallback(string name, InterBlockMesh result);

}
