//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th15;

/// <summary>
/// Represents playable characters of LoLK.
/// </summary>
public enum Chara
{
    /// <summary>
    /// Hakurei Reimu.
    /// </summary>
    [Pattern("RM")]
    [Character(nameof(Reimu))]
    Reimu,

    /// <summary>
    /// Kirisame Marisa.
    /// </summary>
    [Pattern("MR")]
    [Character(nameof(Marisa))]
    Marisa,

    /// <summary>
    /// Kochiya Sanae.
    /// </summary>
    [Pattern("SN")]
    [Character(nameof(Sanae))]
    Sanae,

    /// <summary>
    /// Reisen Udongein Inaba.
    /// </summary>
    [Pattern("RS")]
    [Character(nameof(Reisen))]
    Reisen,
}
