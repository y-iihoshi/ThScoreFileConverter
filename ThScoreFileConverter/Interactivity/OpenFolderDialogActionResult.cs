//-----------------------------------------------------------------------
// <copyright file="OpenFolderDialogActionResult.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using CommunityToolkit.Diagnostics;

namespace ThScoreFileConverter.Interactivity;

/// <summary>
/// Represents a result of <see cref="OpenFolderDialogAction"/>.
/// </summary>
public sealed class OpenFolderDialogActionResult
{
    /// <summary>
    /// Initializes a new instance of the <see cref="OpenFolderDialogActionResult"/> class.
    /// </summary>
    /// <param name="folderName">A full path string.</param>
    public OpenFolderDialogActionResult(string folderName)
    {
        Guard.IsNotNull(folderName);

        this.FolderName = folderName;
    }

    /// <summary>
    /// Gets the full path of the folder selected by <see cref="OpenFolderDialogAction"/>.
    /// </summary>
    public string FolderName { get; }
}
