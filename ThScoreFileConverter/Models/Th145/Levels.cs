//-----------------------------------------------------------------------
// <copyright file="Levels.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;

namespace ThScoreFileConverter.Models.Th145;

/// <summary>
/// Represents bit flags of levels.
/// </summary>
[Flags]
public enum Levels
{
    /// <summary>
    /// Represents that any level is not related.
    /// </summary>
    None = 0,

    /// <summary>
    /// Represents level Easy.
    /// </summary>
    Easy = 1,

    /// <summary>
    /// Represents level Normal.
    /// </summary>
    Normal = 2,

    /// <summary>
    /// Represents level Hard.
    /// </summary>
    Hard = 4,

    /// <summary>
    /// Represents level Lunatic.
    /// </summary>
    Lunatic = 8,
}
