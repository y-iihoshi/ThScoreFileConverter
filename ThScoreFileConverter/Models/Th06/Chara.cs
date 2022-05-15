//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th06;

/// <summary>
/// Represents playable characters of EoSD.
/// </summary>
public enum Chara
{
    /// <summary>
    /// Hakurei Reimu (Spirit Sign).
    /// </summary>
    [EnumAltName("RA")]
    ReimuA,

    /// <summary>
    /// Hakurei Reimu (Dream Sign).
    /// </summary>
    [EnumAltName("RB")]
    ReimuB,

    /// <summary>
    /// Kirisame Marisa (Magic Sign).
    /// </summary>
    [EnumAltName("MA")]
    MarisaA,

    /// <summary>
    /// Kirisame Marisa (Love Sign).
    /// </summary>
    [EnumAltName("MB")]
    MarisaB,
}
