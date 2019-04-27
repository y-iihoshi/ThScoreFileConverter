//-----------------------------------------------------------------------
// <copyright file="Work.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
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
