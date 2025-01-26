//-----------------------------------------------------------------------
// <copyright file="PlayableStages.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th08;

/// <summary>
/// Represents which stages are playable.
/// </summary>
[Flags]
public enum PlayableStages
{
#pragma warning disable format
    /// <summary>
    /// Represents stage 1 is playable.
    /// </summary>
    Stage1   = 0x0001,

    /// <summary>
    /// Represents stage 2 is playable.
    /// </summary>
    Stage2   = 0x0002,

    /// <summary>
    /// Represents stage 3 is playable.
    /// </summary>
    Stage3   = 0x0004,

    /// <summary>
    /// Represents stage 4 Uncanny is playable.
    /// </summary>
    Stage4A  = 0x0008,

    /// <summary>
    /// Represents stage 4 Powerful is playable.
    /// </summary>
    Stage4B  = 0x0010,

    /// <summary>
    /// Represents stage 5 is playable.
    /// </summary>
    Stage5   = 0x0020,

    /// <summary>
    /// Represents stage Final A is playable.
    /// </summary>
    Stage6A  = 0x0040,

    /// <summary>
    /// Represents stage Final B is playable.
    /// </summary>
    Stage6B  = 0x0080,

    /// <summary>
    /// Represents Extra stage is playable.
    /// </summary>
    Extra    = 0x0100,

    /// <summary>
    /// Unknown...
    /// </summary>
    Unknown  = 0x4000,

    /// <summary>
    /// Represents all stages are playable.
    /// </summary>
    AllClear = 0x8000,
#pragma warning restore format
}
