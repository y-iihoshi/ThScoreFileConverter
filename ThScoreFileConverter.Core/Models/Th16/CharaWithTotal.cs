//-----------------------------------------------------------------------
// <copyright file="CharaWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th16;

/// <summary>
/// Represents playable characters of HSiFS and total.
/// </summary>
public enum CharaWithTotal
{
    /// <summary>
    /// Hakurei Reimu.
    /// </summary>
    [EnumAltName("RM")]
    Reimu,

    /// <summary>
    /// Cirno.
    /// </summary>
    [EnumAltName("CI")]
    Cirno,

    /// <summary>
    /// SHameimaru Aya.
    /// </summary>
    [EnumAltName("AY")]
    Aya,

    /// <summary>
    /// Kirisame Marisa.
    /// </summary>
    [EnumAltName("MR")]
    Marisa,

    /// <summary>
    /// Represents total across characters.
    /// </summary>
    [EnumAltName("TL")]
    Total,
}
