//-----------------------------------------------------------------------
// <copyright file="CharaWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Core.Models.Th11;

/// <summary>
/// Represents playable characters of SA and total.
/// </summary>
public enum CharaWithTotal
{
    /// <summary>
    /// Hakurei Reimu with Yakumo Yukari.
    /// </summary>
    [Pattern("RY")]
    [Character("Reimu")]
    [Character("Yukari", 1)]
    ReimuYukari,

    /// <summary>
    /// Hakurei Reimu with Ibuki Suika.
    /// </summary>
    [Pattern("RS")]
    [Character("Reimu")]
    [Character("Suika", 1)]
    ReimuSuika,

    /// <summary>
    /// Hakurei Reimu with Shameimaru Aya.
    /// </summary>
    [Pattern("RA")]
    [Character("Reimu")]
    [Character("Aya", 1)]
    ReimuAya,

    /// <summary>
    /// Kirisame Marisa with Alice Margatroid.
    /// </summary>
    [Pattern("MA")]
    [Character("Marisa")]
    [Character("Alice", 1)]
    MarisaAlice,

    /// <summary>
    /// Kirisame Marisa with Patchouli Knowledge.
    /// </summary>
    [Pattern("MP")]
    [Character("Marisa")]
    [Character("Patchouli", 1)]
    MarisaPatchouli,

    /// <summary>
    /// Kirisame Marisa with Kawashiro Nitori.
    /// </summary>
    [Pattern("MN")]
    [Character("Marisa")]
    [Character("Nitori", 1)]
    MarisaNitori,

    /// <summary>
    /// Represents total across characters.
    /// </summary>
    [Pattern("TL")]
    Total,
}
