using System;
using System.IO;

namespace ThScoreFileConverter
{
    /// <summary>
    /// 東方 Project で使用されている暗号化形式を扱う静的クラス
    /// Thanks to Touhou Toolkit 5
    /// </summary>
    public static class ThCrypt
    {
        /// <summary>
        /// 暗号化する
        /// </summary>
        /// <param Name="input">暗号化対象のストリーム</param>
        /// <param Name="output">暗号化後の出力先ストリーム</param>
        /// <param Name="size">暗号化対象のサイズ（単位: Byte）</param>
        /// <param Name="key">暗号化キー</param>
        /// <param Name="step">FIXME</param>
        /// <param Name="block">FIXME</param>
        /// <param Name="limit">FIXME</param>
        public static void Encrypt(
            Stream input, Stream output, int size, byte key, byte step, int block, int limit)
        {
            throw new NotImplementedException("Encryption is not supported.");
        }

        /// <summary>
        /// 復号化する
        /// </summary>
        /// <param Name="input">復号化対象のストリーム</param>
        /// <param Name="output">復号化後の出力先ストリーム</param>
        /// <param Name="size">復号化対象のサイズ（単位: Byte）</param>
        /// <param Name="key">復号化キー</param>
        /// <param Name="step">FIXME</param>
        /// <param Name="block">FIXME</param>
        /// <param Name="limit">FIXME</param>
        public static void Decrypt(
            Stream input, Stream output, int size, byte key, byte step, int block, int limit)
        {
            var inBlock = new byte[block];
            var outBlock = new byte[block];
            int addup;

            addup = size % block;
            if (addup >= block / 4)
                addup = 0;
            addup += size % 2;
            size -= addup;

            while ((size > 0) && (limit > 0))
            {
                if (size < block)
                    block = size;
                if (input.Read(inBlock, 0, block) != block)
                    return;
                var inIndex = 0;
                for (var j = 0; j < 2; ++j)
                {
                    var outIndex = block - j - 1;
                    for (var i = 0; i < (block - j + 1) / 2; ++i)
                    {
                        outBlock[outIndex] = (byte)(inBlock[inIndex] ^ key);
                        inIndex++;
                        outIndex -= 2;
                        key += step;
                    }
                }
                output.Write(outBlock, 0, block);
                limit -= block;
                size -= block;
            }
            size += addup;
            if (size > 0)
            {
                var restbuf = new byte[size];
                if (input.Read(restbuf, 0, size) != size)
                    return;
                output.Write(restbuf, 0, size);
            }
        }
    }
}
