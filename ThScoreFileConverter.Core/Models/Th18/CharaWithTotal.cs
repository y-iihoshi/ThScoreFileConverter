﻿//-----------------------------------------------------------------------
// <copyright file="CharaWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th18;

/// <summary>
/// Represents player characters of UM and total.
/// </summary>
public enum CharaWithTotal
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
    /// Izayoi Sakuya.
    /// </summary>
    [Pattern("SK")]
    [Character(nameof(Sakuya))]
    Sakuya,

    /// <summary>
    /// Kochiya Sanae.
    /// </summary>
    [Pattern("SN")]
    [Character(nameof(Sanae))]
    Sanae,

    /// <summary>
    /// Represents total across characters.
    /// </summary>
    [Pattern("TL")]
    Total,
}
