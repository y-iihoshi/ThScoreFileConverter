//-----------------------------------------------------------------------
// <copyright file="OpenFileDialogActionResult.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;

namespace ThScoreFileConverter.Actions
{
    /// <summary>
    /// Represents a result of <see cref="OpenFileDialogAction"/>.
    /// </summary>
    public sealed class OpenFileDialogActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OpenFileDialogActionResult"/> class.
        /// </summary>
        /// <param name="fileName">A file name selected in the file dialog box.</param>
        /// <param name="fileNames">The file names of all selected files in the dialog box.</param>
        public OpenFileDialogActionResult(string fileName, IEnumerable<string> fileNames)
        {
            this.FileName = fileName;
            this.FileNames = fileNames.ToArray();
        }

        /// <summary>
        /// Gets a string containing the file name selected in the file dialog box.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Gets the file names of all selected files in the dialog box.
        /// </summary>
        public IEnumerable<string> FileNames { get; }
    }
}
