﻿//-----------------------------------------------------------------------
// <copyright file="FolderBrowserDialogActionResult.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace ThScoreFileConverter.Interactivity
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
            this.SelectedPath = selectedPath ?? throw new ArgumentNullException(nameof(selectedPath));
        }

        /// <summary>
        /// Gets the path selected by <see cref="FolderBrowserDialogAction"/>.
        /// </summary>
        public string SelectedPath { get; }
    }
}
