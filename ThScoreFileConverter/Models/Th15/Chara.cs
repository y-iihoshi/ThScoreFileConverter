﻿//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th15;

/// <summary>
/// Represents playable characters of LoLK.
/// </summary>
public enum Chara
{
    /// <summary>
    /// Hakurei Reimu.
    /// </summary>
    [EnumAltName("RM")]
    Reimu,

    /// <summary>
    /// Kirisame Marisa.
    /// </summary>
    [EnumAltName("MR")]
    Marisa,

    /// <summary>
    /// Kochiya Sanae.
    /// </summary>
    [EnumAltName("SN")]
    Sanae,

    /// <summary>
    /// Reisen Udongein Inaba.
    /// </summary>
    [EnumAltName("RS")]
    Reisen,
}
