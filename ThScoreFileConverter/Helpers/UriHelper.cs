//-----------------------------------------------------------------------
// <copyright file="UriHelper.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Helpers;

/// <summary>
/// Provides helper functions for paths.
/// </summary>
public static class UriHelper
{
    /// <summary>
    /// Returns a relative URI string from one URI to another.
    /// </summary>
    /// <param name="relativeTo">
    /// The source URI string the result should be relative to. This is always considered to be a directory.
    /// </param>
    /// <param name="path">The destination URI string.</param>
    /// <returns>
    /// The relative URI string, or <see cref="string.Empty"/> if <paramref name="relativeTo"/> or <paramref name="path"/>
    /// don't represent URI strings.
    /// </returns>
    public static string GetRelativePath(string relativeTo, string path)
    {
        return TryGetRelativePath(relativeTo, path, out var result) ? result : string.Empty;
    }

    /// <summary>
    /// Creates a relative URI string from one URI to another.
    /// </summary>
    /// <param name="relativeTo">
    /// The source URI string the result should be relative to. This is always considered to be a directory.
    /// </param>
    /// <param name="path">The destination URI string.</param>
    /// <param name="result">The created relative URI string.</param>
    /// <returns>
    /// <see langword="true"/> if the URI string was successfully created; otherwise, <see langword="false"/>.
    /// </returns>
    public static bool TryGetRelativePath(string relativeTo, string path, out string result)
    {
        if (Uri.TryCreate(relativeTo, UriKind.Absolute, out var relativeToUri) &&
            Uri.TryCreate(path, UriKind.Absolute, out var pathUri))
        {
            result = relativeToUri.MakeRelativeUri(pathUri).OriginalString;
            return true;
        }
        else
        {
            result = string.Empty;
            return false;
        }
    }
}
