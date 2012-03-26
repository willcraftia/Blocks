#region Using

using System;

#endregion

namespace Willcraftia.Xna.Blocks.BlockViewer.Models.Box
{
    /// <summary>
    /// Box との接続情報を管理するクラスです。
    /// </summary>
    public sealed class BoxSettings
    {
        /// <summary>
        /// Auth-token を取得または設定します。
        /// </summary>
        public string AuthToken { get; set; }

        /// <summary>
        /// Blocks Data フォルダの ID を取得または設定します。
        /// デフォルト値は -1 (未設定) です。
        /// </summary>
        public long BlocksFolderId { get; set; }

        /// <summary>
        /// BlockMeshes フォルダの ID を取得または設定します。
        /// デフォルト値は -1 (未設定) です。
        /// </summary>
        public long MeshesFolderId { get; set; }

        /// <summary>
        /// インスタンスを生成します。
        /// </summary>
        public BoxSettings()
        {
            BlocksFolderId = -1;
            MeshesFolderId = -1;
        }
    }
}
