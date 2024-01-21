//-----------------------------------------------------------------------
// <copyright file="StreamHelper.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System.IO;

namespace ThScoreFileConverter.Helpers;

/// <summary>
/// Provides helper functions for streams.
/// </summary>
public static class StreamHelper
{
    /// <summary>
    /// Creates a new instance of <see cref="FileStream"/> or <see cref="MemoryStream"/>.
    /// </summary>
    /// <param name="path">The first parameter of <see cref="FileStream(string, FileMode, FileAccess)"/>.</param>
    /// <param name="mode">The second parameter of <see cref="FileStream(string, FileMode, FileAccess)"/>.</param>
    /// <param name="access">The third parameter of <see cref="FileStream(string, FileMode, FileAccess)"/>.</param>
    /// <returns>
    /// An instance of <see cref="FileStream"/> if debug configuration; otherwise <see cref="MemoryStream"/>.
    /// </returns>
    public static Stream Create(string path, FileMode mode, FileAccess access)
    {
#if DEBUG
        return new FileStream(path, mode, access);
#else
        return new MemoryStream();
#endif
    }
}
