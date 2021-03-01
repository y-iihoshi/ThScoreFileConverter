//-----------------------------------------------------------------------
// <copyright file="ISettings.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.Collections.Generic;

namespace ThScoreFileConverter
{
    /// <summary>
    /// Defines the interface of the settings of this application.
    /// </summary>
    public interface ISettings
    {
        /// <summary>
        /// Gets or sets the font family name used for the UI of this application.
        /// </summary>
        string FontFamilyName { get; set; }

        /// <summary>
        /// Gets or sets the font size used for the UI of this application.
        /// </summary>
        double? FontSize { get; set; }

        /// <summary>
        /// Gets or sets the code page identifier for input files.
        /// </summary>
        int? InputCodePageId { get; set; }

        /// <summary>
        /// Gets or sets the culture name.
        /// </summary>
        string? Language { get; set; }

        /// <summary>
        /// Gets or sets the last selected work.
        /// </summary>
        string LastTitle { get; set; }

        /// <summary>
        /// Gets or sets the code page identifier for output files.
        /// </summary>
        int? OutputCodePageId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether numeric values is output with thousand separator
        /// characters.
        /// </summary>
        bool? OutputNumberGroupSeparator { get; set; }

        /// <summary>
        /// Loads the settings from the specified XML file.
        /// </summary>
        /// <param name="path">The path of the XML file to load.</param>
        void Load(string path);

        /// <summary>
        /// Saves the settings to the specified XML file.
        /// </summary>
        /// <param name="path">The path of the XML file to save.</param>
        void Save(string path);
    }
}
