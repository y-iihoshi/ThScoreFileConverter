using System;
using System.IO;

namespace ThScoreFileConverter
{
    /// <summary>
    /// ストリームからビット単位で読み出すクラス
    /// </summary>
    public class BitReader : IDisposable
    {
        /// <summary>
        /// 読み出し対象のストリーム
        /// </summary>
        private Stream stream;
        
        /// <summary>
        /// 破棄済みかどうかを示すフラグ
        /// </summary>
        private bool disposed;

        /// <summary>
        /// 読み出し中のバイト
        /// </summary>
        private int current;

        /// <summary>
        /// 読み出しビット位置を示すマスク値
        /// </summary>
        private byte mask;

        /// <summary>
        /// インスタンスを生成する
        /// </summary>
        /// <param Name="stream">読み出し対象のストリーム</param>
        public BitReader(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException("stream");
            if (!stream.CanRead)
                throw new ArgumentException("stream");

            this.stream = stream;
            this.disposed = false;
            this.current = 0;
            this.mask = 0x80;
        }

        /// <summary>
        /// 破棄処理（Dispose パターン参照）
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }
        
        /// <summary>
        /// 破棄処理（Dispose パターン参照）
        /// </summary>
        /// <param Name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                    this.stream.Dispose();
                this.disposed = true;
            }
        }

        /// <summary>
        /// デストラクター（Dispose パターン参照）
        /// </summary>
        ~BitReader()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// 指定されたビット数分読み出す
        /// </summary>
        /// <param Name="num">読み出すビット数</param>
        /// <returns>読み出した値</returns>
        public int ReadBits(int num)
        {
            if (this.disposed)
                throw new ObjectDisposedException(this.ToString());

            var value = 0;
            for (var i = 0; i < num; i++)
            {
                if (this.mask == 0x80)
                {
                    this.current = this.stream.ReadByte();
                    if (this.current < 0)   // EOF
                        this.current = 0;
                }
                value <<= 1;
                if (((byte)this.current & this.mask) != 0)
                    value |= 1;
                this.mask >>= 1;
                if (this.mask == 0)
                    this.mask = 0x80;
            }
            return value;
        }
    }
}
