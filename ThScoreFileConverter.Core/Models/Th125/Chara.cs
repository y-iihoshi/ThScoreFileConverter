//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th125;

/// <summary>
/// Represents playable characters of DS.
/// </summary>
public enum Chara
{
    /// <summary>
    /// Shameimaru Aya.
    /// </summary>
    [EnumAltName("A")]
    Aya,

    /// <summary>
    /// Himekaidou Hatate.
    /// </summary>
    [EnumAltName("H")]
    Hatate,
}
