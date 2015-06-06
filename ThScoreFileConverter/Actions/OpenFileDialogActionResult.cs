//-----------------------------------------------------------------------
// <copyright file="OpenFileDialogActionResult.cs" company="None">
//     (c) 2015 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Actions
{
    using System.Collections.Generic;
    using System.Linq;

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
        public string FileName { get; private set; }

        /// <summary>
        /// Gets the file names of all selected files in the dialog box.
        /// </summary>
        public IEnumerable<string> FileNames { get; private set; }
    }
}
