//-----------------------------------------------------------------------
// <copyright file="Work.cs" company="None">
//     (c) 2015 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models
{
    /// <summary>
    /// Represents a Touhou work.
    /// </summary>
    public sealed class Work
    {
        /// <summary>
        /// Gets or sets a number string (e.g. <c>"TH06"</c>).
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets a title string.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the work is supported by this tool.
        /// </summary>
        public bool IsSupported { get; set; }
    }
}
