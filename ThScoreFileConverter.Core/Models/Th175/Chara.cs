//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th175;

/// <summary>
/// Represents player characters of SFW.
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
    /// Yasaka Kanako.
    /// </summary>
    [EnumAltName("KN")]
    Kanako,

    /// <summary>
    /// Murasa Minamitsu.
    /// </summary>
    [EnumAltName("MI")]
    Minamitsu,

    /// <summary>
    /// Yorigami Joon and Yorigami Shion.
    /// </summary>
    [EnumAltName("JS")]
    JoonShion,

    /// <summary>
    /// Flandre Scarlet.
    /// </summary>
    [EnumAltName("FL")]
    Flandre,
}
