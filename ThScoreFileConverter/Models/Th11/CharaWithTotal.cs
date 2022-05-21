//-----------------------------------------------------------------------
// <copyright file="CharaWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using ThScoreFileConverter.Core.Models;

namespace ThScoreFileConverter.Models.Th11;

/// <summary>
/// Represents playable characters of SA and total.
/// </summary>
public enum CharaWithTotal
{
    /// <summary>
    /// Hakurei Reimu with Yakumo Yukari.
    /// </summary>
    [EnumAltName("RY")]
    ReimuYukari,

    /// <summary>
    /// Hakurei Reimu with Ibuki Suika.
    /// </summary>
    [EnumAltName("RS")]
    ReimuSuika,

    /// <summary>
    /// Hakurei Reimu with Shameimaru Aya.
    /// </summary>
    [EnumAltName("RA")]
    ReimuAya,

    /// <summary>
    /// Kirisame Marisa with Alice Margatroid.
    /// </summary>
    [EnumAltName("MA")]
    MarisaAlice,

    /// <summary>
    /// Kirisame Marisa with Patchouli Knowledge.
    /// </summary>
    [EnumAltName("MP")]
    MarisaPatchouli,

    /// <summary>
    /// Kirisame Marisa with Kawashiro Nitori.
    /// </summary>
    [EnumAltName("MN")]
    MarisaNitori,

    /// <summary>
    /// Represents total across characters.
    /// </summary>
    [EnumAltName("TL")]
    Total,
}
