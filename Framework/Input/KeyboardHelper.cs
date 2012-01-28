#region Using

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;

#endregion

namespace Willcraftia.Xna.Framework.Input
{
    public static class KeyboardHelper
    {
        #region CharacterPair

        /// <summary>
        /// キーの文字と Shift キー押下時の文字との組を保持するクラスです。
        /// </summary>
        class CharacterPair
        {
            /// <summary>
            /// キーの文字。
            /// </summary>
            public char Character;

            /// <summary>
            /// Shift キー押下時のキーの文字。
            /// </summary>
            public Nullable<char> ShiftCharacter;

            /// <summary>
            /// インスタンスを生成します。
            /// </summary>
            /// <param name="character">キーの文字。</param>
            /// <param name="shiftCharacter">Shift キーが押下された時のキーの文字。</param>
            public CharacterPair(char character, Nullable<char> shiftCharacter)
            {
                this.Character = character;
                this.ShiftCharacter = shiftCharacter;
            }
        }

        #endregion

        /// <summary>
        /// Keys をキーに CharacterPair を値とするディクショナリ。
        /// </summary>
        static readonly Dictionary<Keys, CharacterPair> keyCharacterPairMap = new Dictionary<Keys, CharacterPair>();

        /// <summary>
        /// クラスを初期化します。
        /// </summary>
        static KeyboardHelper()
        {
            InitializeKeyCharacterPairMap();
        }

        /// <summary>
        /// Keys に対応する文字を取得します。
        /// </summary>
        /// <param name="key">Keys。</param>
        /// <param name="shiftPressed">
        /// true (Shift キー押下時の文字を取得したい場合)、false (それ以外の場合)。
        /// </param>
        /// <param name="character">対応する文字。</param>
        /// <returns>
        /// true (文字の取得に成功した場合)、false (それ以外の場合)。
        /// </returns>
        public static bool KeyToCharacter(Keys key, bool shiftPressed, out char character)
        {
            character = ' ';

            if ((Keys.A <= key && key <= Keys.Z) || key == Keys.Space)
            {
                character = (shiftPressed) ? (char) key : char.ToLower((char) key);
                return true;
            }
            
            CharacterPair pair;
            if (keyCharacterPairMap.TryGetValue(key, out pair))
            {
                if (!shiftPressed)
                {
                    character = pair.Character;
                    return true;
                }

                if (pair.ShiftCharacter.HasValue)
                {
                    character = pair.ShiftCharacter.Value;
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// keyCharacterPairMap を初期化します。
        /// </summary>
        static void InitializeKeyCharacterPairMap()
        {
            RegisterCharacterPair(Keys.OemTilde, "`~");
            RegisterCharacterPair(Keys.D1, "1!");
            RegisterCharacterPair(Keys.D2, "2@");
            RegisterCharacterPair(Keys.D3, "3#");
            RegisterCharacterPair(Keys.D4, "4$");
            RegisterCharacterPair(Keys.D5, "5%");
            RegisterCharacterPair(Keys.D6, "6^");
            RegisterCharacterPair(Keys.D7, "7&");
            RegisterCharacterPair(Keys.D8, "8*");
            RegisterCharacterPair(Keys.D9, "9(");
            RegisterCharacterPair(Keys.D0, "0)");
            RegisterCharacterPair(Keys.OemMinus, "-_");
            RegisterCharacterPair(Keys.OemPlus, "=+");
            RegisterCharacterPair(Keys.OemOpenBrackets, "[{");
            RegisterCharacterPair(Keys.OemCloseBrackets, "]}");
            RegisterCharacterPair(Keys.OemPipe, "\\|");
            RegisterCharacterPair(Keys.OemSemicolon, ";:");
            RegisterCharacterPair(Keys.OemQuotes, "'\"");
            RegisterCharacterPair(Keys.OemComma, ",<");
            RegisterCharacterPair(Keys.OemPeriod, ".>");
            RegisterCharacterPair(Keys.OemQuestion, "/?");
            RegisterCharacterPair(Keys.NumPad1, "1");
            RegisterCharacterPair(Keys.NumPad2, "2");
            RegisterCharacterPair(Keys.NumPad3, "3");
            RegisterCharacterPair(Keys.NumPad4, "4");
            RegisterCharacterPair(Keys.NumPad5, "5");
            RegisterCharacterPair(Keys.NumPad6, "6");
            RegisterCharacterPair(Keys.NumPad7, "7");
            RegisterCharacterPair(Keys.NumPad8, "8");
            RegisterCharacterPair(Keys.NumPad9, "9");
            RegisterCharacterPair(Keys.NumPad0, "0");
            RegisterCharacterPair(Keys.Add, "+");
            RegisterCharacterPair(Keys.Divide, "/");
            RegisterCharacterPair(Keys.Multiply, "*");
            RegisterCharacterPair(Keys.Subtract, "-");
            RegisterCharacterPair(Keys.Decimal, ".");
        }

        /// <summary>
        /// 文字列から CharacterPair を生成して登録します。
        /// </summary>
        /// <param name="key">キー。</param>
        /// <param name="characterPair">
        /// 1 桁目に通常の文字、2 桁目に Shift キー押下時の文字を持つ文字列。
        /// Shift キー押下時の文字がない場合には 1 桁だけで指定します。
        /// </param>
        static void RegisterCharacterPair(Keys key, string characterPair)
        {
            var character = characterPair[0];
            Nullable<char> shiftCharacter = null;
            if (1 < characterPair.Length) shiftCharacter = characterPair[1];

            keyCharacterPairMap[key] = new CharacterPair(character, shiftCharacter);
        }
    }
}
