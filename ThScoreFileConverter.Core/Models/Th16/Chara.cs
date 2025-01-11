//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th16;

/// <summary>
/// Represents playable characters of HSiFS.
/// </summary>
public enum Chara
{
    /// <summary>
    /// Hakurei Reimu.
    /// </summary>
    [Pattern("RM")]
    [Character(nameof(Reimu))]
    Reimu,

    /// <summary>
    /// Cirno.
    /// </summary>
    [Pattern("CI")]
    [Character($"Th16.{nameof(Cirno)}")]  // FIXME
    Cirno,

    /// <summary>
    /// SHameimaru Aya.
    /// </summary>
    [Pattern("AY")]
    [Character(nameof(Aya))]
    Aya,

    /// <summary>
    /// Kirisame Marisa.
    /// </summary>
    [Pattern("MR")]
    [Character(nameof(Marisa))]
    Marisa,
}
