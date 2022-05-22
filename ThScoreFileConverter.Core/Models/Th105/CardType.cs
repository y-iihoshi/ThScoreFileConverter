//-----------------------------------------------------------------------
// <copyright file="CardType.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th105;

/// <summary>
/// Represents card types of SWR.
/// </summary>
public enum CardType
{
    /// <summary>
    /// System Card.
    /// </summary>
    [EnumAltName("Y")]
    System,

    /// <summary>
    /// Skill Card.
    /// </summary>
    [EnumAltName("K")]
    Skill,

    /// <summary>
    /// Spell Card.
    /// </summary>
    [EnumAltName("P")]
    Spell,
}
