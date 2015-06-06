//-----------------------------------------------------------------------
// <copyright file="FolderBrowserDialogActionResult.cs" company="None">
//     (c) 2015 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Actions
{
    /// <summary>
    /// Represents a result of <see cref="FolderBrowserDialogAction"/>.
    /// </summary>
    public sealed class FolderBrowserDialogActionResult
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FolderBrowserDialogActionResult"/> class.
        /// </summary>
        /// <param name="selectedPath">A path string.</param>
        public FolderBrowserDialogActionResult(string selectedPath)
        {
            this.SelectedPath = selectedPath;
        }

        /// <summary>
        /// Gets the path selected by <see cref="FolderBrowserDialogAction"/>.
        /// </summary>
        public string SelectedPath { get; private set; }
    }
}
