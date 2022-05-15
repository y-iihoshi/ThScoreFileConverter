//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th10;

/// <summary>
/// Represents playable characters of MoF.
/// </summary>
public enum Chara
{
    /// <summary>
    /// Hakurei Reimu (Homing type).
    /// </summary>
    [EnumAltName("RA")]
    ReimuA,

    /// <summary>
    /// Hakurei Reimu (Forward focus type).
    /// </summary>
    [EnumAltName("RB")]
    ReimuB,

    /// <summary>
    /// Hakurei Reimu (Sealing type).
    /// </summary>
    [EnumAltName("RC")]
    ReimuC,

    /// <summary>
    /// Kirisame Marisa (High-power type).
    /// </summary>
    [EnumAltName("MA")]
    MarisaA,

    /// <summary>
    /// Kirisame Marisa (Piercing type).
    /// </summary>
    [EnumAltName("MB")]
    MarisaB,

    /// <summary>
    /// Kirisame Marisa (Magic-user type).
    /// </summary>
    [EnumAltName("MC")]
    MarisaC,
}
