//-----------------------------------------------------------------------
// <copyright file="CharaWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th12;

/// <summary>
/// Represents playable characters of UFO and total.
/// </summary>
public enum CharaWithTotal
{
    /// <summary>
    /// Hakurei Reimu (Dream Sign).
    /// </summary>
    [EnumAltName("RA")]
    ReimuA,

    /// <summary>
    /// Hakurei Reimu (Spirit Sign).
    /// </summary>
    [EnumAltName("RB")]
    ReimuB,

    /// <summary>
    /// Kirisame Marisa (Love Sign).
    /// </summary>
    [EnumAltName("MA")]
    MarisaA,

    /// <summary>
    /// Kirisame Marisa (Magic Sign).
    /// </summary>
    [EnumAltName("MB")]
    MarisaB,

    /// <summary>
    /// Kochiya Sanae (Snake Sign).
    /// </summary>
    [EnumAltName("SA")]
    SanaeA,

    /// <summary>
    /// Kochiya Sanae (Frog Sign).
    /// </summary>
    [EnumAltName("SB")]
    SanaeB,

    /// <summary>
    /// Represents total across characters.
    /// </summary>
    [EnumAltName("TL")]
    Total,
}
