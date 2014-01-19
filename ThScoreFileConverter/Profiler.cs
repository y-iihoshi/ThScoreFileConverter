using System;
using System.Diagnostics;

namespace ThScoreFileConverter
{
    /// <summary>
    /// Dispose パターンを利用した処理時間計測クラス
    /// </summary>
    class Profiler : IDisposable
    {
        /// <summary>
        /// 破棄済みかどうかを示すフラグ
        /// </summary>
        protected bool disposed;

        /// <summary>
        /// 処理時間計測に使う Stopwatch インスタンス
        /// </summary>
        protected Stopwatch watch;

        /// <summary>
        /// 計測開始時の処理
        /// </summary>
        protected Action preprocess;

        /// <summary>
        /// 計測終了時の処理
        /// </summary>
        protected Action<TimeSpan> postprocess;

        /// <summary>
        /// インスタンスを生成する
        /// </summary>
        /// <param name="preprocess">計測開始時の処理</param>
        /// <param name="postprocess">計測終了時の処理</param>
        public Profiler(Action preprocess, Action<TimeSpan> postprocess)
        {
            if (preprocess == null)
                throw new ArgumentNullException("preprocess");
            if (postprocess == null)
                throw new ArgumentNullException("postprocess");

            this.disposed = false;
            this.watch = new Stopwatch();
            this.preprocess = preprocess;
            this.postprocess = postprocess;

            this.preprocess();
            this.watch.Start();
        }

        /// <summary>
        /// インスタンスを生成する
        /// </summary>
        /// <param name="message">計測終了時に出力する文字列</param>
        public Profiler(string message)
            : this(null, (elapsed) => Console.WriteLine("{0}: Elapsed = {1}", message, elapsed))
        {
        }

        /// <summary>
        /// デストラクター（Dispose パターン参照）
        /// </summary>
        ~Profiler()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// インスタンスを破棄する（Dispose パターン参照）
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
        }

        /// <summary>
        /// インスタンスを破棄する（Dispose パターン参照）
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // If some members have Dispose() methods, call them here.
                }

                this.watch.Stop();
                this.postprocess(this.watch.Elapsed);
                this.disposed = true;
            }
        }
    }
}
