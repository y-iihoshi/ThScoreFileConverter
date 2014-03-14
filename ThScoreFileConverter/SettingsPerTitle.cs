//-----------------------------------------------------------------------
// <copyright file="SettingsPerTitle.cs" company="None">
//     (c) 2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter
{
    using System.Collections.Generic;

    /// <summary>
    /// Represents settings per work.
    /// </summary>
    public class SettingsPerTitle
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SettingsPerTitle"/> class.
        /// </summary>
        public SettingsPerTitle()
        {
            this.ScoreFile = string.Empty;
            this.BestShotDirectory = string.Empty;
            this.TemplateFiles = new List<string>();
            this.OutputDirectory = string.Empty;
            this.ImageOutputDirectory = string.Empty;
            this.HideUntriedCards = true;
        }

        /// <summary>
        /// Gets or sets the path of the score file.
        /// </summary>
        public string ScoreFile { get; set; }

        /// <summary>
        /// Gets or sets the path of the best shot directory.
        /// </summary>
        public string BestShotDirectory { get; set; }

        /// <summary>
        /// Gets or sets the array of the paths of template files.
        /// </summary>
        public IEnumerable<string> TemplateFiles { get; set; }

        /// <summary>
        /// Gets or sets the path of the output directory.
        /// </summary>
        public string OutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets the path of the output directory of the image files.
        /// </summary>
        public string ImageOutputDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether it hides untried cards.
        /// </summary>
        public bool HideUntriedCards { get; set; }
    }
}
