//-----------------------------------------------------------------------
// <copyright file="ThConverterEventArgs.cs" company="None">
//     (c) 2014-2015 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models
{
    using System;

    /// <summary>
    /// Represents the event data issued by the <see cref="ThConverter"/> class.
    /// </summary>
    internal class ThConverterEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThConverterEventArgs"/> class.
        /// </summary>
        /// <param name="path">The path of the last output file.</param>
        /// <param name="current">The number of the files that have been output.</param>
        /// <param name="total">The total number of the files.</param>
        public ThConverterEventArgs(string path, int current, int total)
        {
            this.Path = path;
            this.Current = current;
            this.Total = total;
        }

        /// <summary>
        /// Gets the path of the last output file.
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// Gets the number of the files that have been output.
        /// </summary>
        public int Current { get; private set; }

        /// <summary>
        /// Gets the total number of the files.
        /// </summary>
        public int Total { get; private set; }

        /// <summary>
        /// Gets a message string that represents the current instance.
        /// </summary>
        public string Message
        {
            get
            {
                return Utils.Format(
                    "({0}/{1}) {2} ", this.Current, this.Total, System.IO.Path.GetFileName(this.Path));
            }
        }
    }
}
