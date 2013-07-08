using System;
using System.IO;

namespace ThScoreFileConverter
{
    /// <summary>
    /// LZSS 圧縮・展開を扱う静的クラス
    /// </summary>
    public static class Lzss
    {
        /// <summary>
        /// 辞書のサイズ
        /// </summary>
        private const int DicSize = 0x2000;

        /// <summary>
        /// LZSS 圧縮を行う（未実装）
        /// </summary>
        /// <param Name="input"></param>
        /// <param Name="output"></param>
        public static void Compress(Stream input, Stream output)
        {
            throw new NotImplementedException("LZSS complession is not supported.");
        }

        /// <summary>
        /// LZSS 展開を行う
        /// </summary>
        /// <param Name="input">展開元のストリーム</param>
        /// <param Name="output">展開結果の出力先ストリーム</param>
        public static void Extract(Stream input, Stream output)
        {
            var reader = new BitReader(input);
            var dictionary = new byte[DicSize];
            var dicIndex = 1;

            while (dicIndex < dictionary.Length)
            {
                var flag = reader.ReadBits(1);
                if (flag != 0)
                {
                    var ch = (byte)reader.ReadBits(8);
                    output.WriteByte(ch);
                    dictionary[dicIndex] = ch;
                    dicIndex = (dicIndex + 1) & 0x1FFF;
                }
                else
                {
                    var offset = reader.ReadBits(13);
                    if (offset == 0)
                        break;
                    else
                    {
                        var length = reader.ReadBits(4) + 3;
                        for (var i = 0; i < length; i++)
                        {
                            var ch = dictionary[(offset + i) & 0x1FFF];
                            output.WriteByte(ch);
                            dictionary[dicIndex] = ch;
                            dicIndex = (dicIndex + 1) & 0x1FFF;
                        }
                    }
                }
            }
        }
    }
}
