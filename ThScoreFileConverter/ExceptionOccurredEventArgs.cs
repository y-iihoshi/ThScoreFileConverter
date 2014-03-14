//-----------------------------------------------------------------------
// <copyright file="ExceptionOccurredEventArgs.cs" company="None">
//     (c) 2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter
{
    using System;

    /// <summary>
    /// Represents the event data that indicates occurring of an exception.
    /// </summary>
    internal class ExceptionOccurredEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionOccurredEventArgs"/> class.
        /// </summary>
        /// <param name="ex">The exception data.</param>
        public ExceptionOccurredEventArgs(Exception ex)
        {
            this.Exception = ex;
        }

        /// <summary>
        /// Gets the exception data.
        /// </summary>
        public Exception Exception { get; private set; }
    }
}
