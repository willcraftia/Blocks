#region Using

using System;
using System.Collections.Generic;
using System.Text;
using Willcraftia.Xna.Blocks.Serialization;

#endregion

namespace Willcraftia.Xna.Blocks.Content
{
    /// <summary>
    /// Block から BlockMesh を生成する際に利用する、Block の中間データを表すクラスです。
    /// </summary>
    internal sealed class InterBlock
    {
        /// <summary>
        /// 最大 LOD のグリッド サイズ。
        /// </summary>
        public const int MaxDetailLevelGridSize = 16;

        /// <summary>
        /// 最大 LOD の Element サイズ。
        /// </summary>
        public const float MaxDefailLevelElementSize = 0.1f;

        /// <summary>
        /// 最大の LOD サイズ。
        /// </summary>
        public const int MaxDetailLevelSize = 4;

        /// <summary>
        /// グリッド サイズを取得します。
        /// </summary>
        public int GridSize { get; private set; }

        /// <summary>
        /// Material のリストを取得します。
        /// </summary>
        public List<Material> Materials { get; private set; }

        /// <summary>
        /// Element のコレクションを取得します。
        /// </summary>
        public InterElementCollection Elements { get; private set; }

        /// <summary>
        /// Element のサイズを取得します。
        /// </summary>
        public float ElementSize { get; private set; }

        /// <summary>
        /// インスタンス生成は CreateInterBlock あるいは CreateLowDetailLevelInterBlock メソッドで行います。
        /// </summary>
        InterBlock() { }

        /// <summary>
        /// 指定された数の LOD でそれぞれの LOD を持つ InterBlock の配列を生成します。
        /// </summary>
        /// <param name="block">Block。</param>
        /// <param name="lodSize">LOD のサイズ。</param>
        /// <returns>生成された InterBlock の配列。</returns>
        public static InterBlock[] CreateInterBlock(Block block, int lodSize)
        {
            if (lodSize < 1 || MaxDetailLevelSize < lodSize) throw new ArgumentOutOfRangeException("lodSize");

            var interBlocks = new InterBlock[lodSize];

            // インデックス 0 は常に最大 LOD です。
            interBlocks[0] = CreateMaxDetailLevelInterBlock(block);

            // 要求された分の下位 LOD を生成します。
            for (int i = 1; i < lodSize; i++) interBlocks[i] = CreateLowDetailLevelInterBlock(interBlocks[i - 1]);

            return interBlocks;
        }

        /// <summary>
        /// Block から最大 LOD の InterBlock を作成します。
        /// </summary>
        /// <param name="block">Block。</param>
        /// <returns>生成された InterBlock。</returns>
        static InterBlock CreateMaxDetailLevelInterBlock(Block block)
        {
            var interBlock = new InterBlock();

            // 最大 LOD として作成します。
            interBlock.GridSize = MaxDetailLevelGridSize;

            // Block の Material をそのままコピーします。
            interBlock.Materials = new List<Material>(block.Materials.Count);
            foreach (var material in block.Materials) interBlock.Materials.Add(material);

            // Block の Element をそのままコピーします。
            interBlock.Elements = new InterElementCollection();
            foreach (var element in block.Elements) interBlock.Elements.Add(element);

            // 最大 LOD の Element サイズを設定します。
            interBlock.ElementSize = MaxDefailLevelElementSize;

            return interBlock;
        }

        /// <summary>
        /// 指定された InterBlock の LOD の 1 レベル下位の詳細情報を持つ InterBlock を作成します。
        /// 生成可能な最小の詳細情報を持つ InterBlock のグリッド サイズは 2 です。
        /// </summary>
        /// <param name="highBlock">生成する InterBlock の 1 レベル上位の詳細情報を持つ InterBlock。</param>
        /// <returns>生成された InterBlock。</returns>
        static InterBlock CreateLowDetailLevelInterBlock(InterBlock highBlock)
        {
            if (highBlock.GridSize == 2) throw new ArgumentException("A specified InterBlock has a minimum LOD.");

            var interBlock = new InterBlock();

            // 上位の半分のグリッド数で作成します。
            interBlock.GridSize = highBlock.GridSize / 2;
            interBlock.Materials = highBlock.Materials;
            interBlock.Elements = new InterElementCollection();

            // グリッド位置の最大と最小を計算します。
            int maxGrid = interBlock.GridSize / 2;
            int minGrid = -interBlock.GridSize / 2;

            // 下位は上位 8 グリッドを 1 つのグリッドとします。

            // 判定中に使用する上位 8 グリッドの位置情報を一時的に格納する配列です。
            Position[] hPositions = new Position[8];
            // 判定中に使用する上位 8 グリッドの Material 情報を一時的に格納する配列です。
            int[] hMaterials = new int[8];

            // 上位 8 グリッドずつ判定しながら下位グリッドの情報を決定します。
            for (int z = minGrid; z < maxGrid; z++)
            {
                for (int y = minGrid; y < maxGrid; y++)
                {
                    for (int x = minGrid; x < maxGrid; x++)
                    {
                        int hX = x * 2;
                        int hY = y * 2;
                        int hZ = z * 2;

                        hPositions[0] = new Position(hX, hY, hZ);
                        hPositions[1] = new Position(hX + 1, hY, hZ);
                        hPositions[2] = new Position(hX, hY + 1, hZ);
                        hPositions[3] = new Position(hX + 1, hY + 1, hZ);
                        hPositions[4] = new Position(hX, hY, hZ + 1);
                        hPositions[5] = new Position(hX + 1, hY, hZ + 1);
                        hPositions[6] = new Position(hX, hY + 1, hZ + 1);
                        hPositions[7] = new Position(hX + 1, hY + 1, hZ + 1);

                        for (int i = 0; i < 8; i++)
                        {
                            Element hElement;
                            highBlock.Elements.TryGetItem(hPositions[i], out hElement);
                            hMaterials[i] = (hElement != null) ? hElement.MaterialIndex : -1;
                        }

                        // 出現頻度が最大の Material を調べます。
                        Array.Sort(hMaterials);

                        int maxMaterialIndex = -1;
                        int maxCount = 0;
                        int countingMaterialIndex = -1;
                        int count = 0;
                        for (int i = 0; i < 8; i++)
                        {
                            if (hMaterials[i] == countingMaterialIndex)
                            {
                                count++;
                            }
                            else
                            {
                                if (maxCount <= count)
                                {
                                    maxCount = count;
                                    maxMaterialIndex = countingMaterialIndex;
                                }
                                countingMaterialIndex = hMaterials[i];
                                count = 1;
                            }
                        }
                        // 最後の要素も有効にするために判定します。
                        if (maxCount <= count)
                        {
                            maxCount = count;
                            maxMaterialIndex = countingMaterialIndex;
                        }

                        // 出現頻度が最大の Material で、このグリッドのための Element を作成します。
                        // maxMaterialIndex が -1 の場合はグリッドが空であることを表し、Element を作成しません。
                        if (maxMaterialIndex != -1)
                        {
                            var element = new Element()
                            {
                                MaterialIndex = maxMaterialIndex,
                                Position = new Position(x, y, z)
                            };
                            interBlock.Elements.Add(element);
                        }
                    }
                }
            }

            // Element サイズは上位 InterBlock の 2 倍です。
            interBlock.ElementSize = highBlock.ElementSize * 2;

            return interBlock;
        }

        #region ToString

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append("[Materials=[");
            if (Materials != null)
            {
                for (int i = 0; i < Materials.Count; i++)
                {
                    builder.Append(Materials[i]);
                    if (i < Materials.Count - 1) builder.Append(", ");
                }
            }
            builder.Append("], Elements=[");
            if (Elements != null)
            {
                for (int i = 0; i < Elements.Count; i++)
                {
                    builder.Append(Elements[i]);
                    if (i < Elements.Count - 1) builder.Append(", ");
                }
            }
            builder.Append("]");
            return builder.ToString();
        }

        #endregion
    }
}
