//-----------------------------------------------------------------------
// <copyright file="Profiler.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter
{
    using System;
    using System.Diagnostics;

    /// <summary>
    /// Represents a profiler that measures the processing time by using Dispose pattern.
    /// </summary>
    public class Profiler : IDisposable
    {
        /// <summary>
        /// The flag that represents whether <see cref="Dispose(bool)"/> has been called.
        /// </summary>
        private bool disposed;

        /// <summary>
        /// The instance of the <see cref="Stopwatch"/> class for measuring the processing time.
        /// </summary>
        private Stopwatch watch;

        /// <summary>
        /// The delegate of the process called before the measuring.
        /// </summary>
        private Action preprocess;

        /// <summary>
        /// The delegate of the process called after the measuring.
        /// </summary>
        private Action<TimeSpan> postprocess;

        /// <summary>
        /// Initializes a new instance of the <see cref="Profiler"/> class.
        /// </summary>
        /// <param name="preprocess">The delegate of the process called before the measuring.</param>
        /// <param name="postprocess">The delegate of the process called after the measuring.</param>
        public Profiler(Action preprocess, Action<TimeSpan> postprocess)
        {
            this.disposed = false;
            this.watch = new Stopwatch();
            this.preprocess = preprocess;
            this.postprocess = postprocess;

            if (this.preprocess != null)
                this.preprocess();
            this.watch.Start();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Profiler"/> class.
        /// </summary>
        /// <param name="message">The string that is output after the measuring.</param>
        public Profiler(string message)
            : this(null, elapsed => Console.WriteLine("{0}: Elapsed = {1}", message, elapsed))
        {
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Profiler"/> class.
        /// </summary>
        ~Profiler()
        {
            this.Dispose(false);
        }

        /// <summary>
        /// Implements the <see cref="IDisposable.Dispose"/> method.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes the resources of the current instance.
        /// </summary>
        /// <param name="disposing">
        /// <c>true</c> if calls from the <see cref="Dispose()"/> method; <c>false</c> for the destructor.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    // If some members have Dispose() methods, call them here.
                }

                this.watch.Stop();
                if (this.postprocess != null)
                    this.postprocess(this.watch.Elapsed);
                this.disposed = true;
            }
        }
    }
}
