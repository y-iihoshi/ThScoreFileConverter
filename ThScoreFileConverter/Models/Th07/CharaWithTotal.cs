//-----------------------------------------------------------------------
// <copyright file="CharaWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th07;

/// <summary>
/// Represents player characters of PCB and total.
/// </summary>
public enum CharaWithTotal
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

    /// <summary>
    /// Izayoi Sakuya (Illusion Sign).
    /// </summary>
    [EnumAltName("SA")]
    SakuyaA,

    /// <summary>
    /// Izayoi Sakuya (Time Sign).
    /// </summary>
    [EnumAltName("SB")]
    SakuyaB,

    /// <summary>
    /// Represents total across characters.
    /// </summary>
    [EnumAltName("TL")]
    Total,
}
