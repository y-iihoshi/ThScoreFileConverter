//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th11
{
    /// <summary>
    /// Represents playable characters of SA.
    /// </summary>
    public enum Chara
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
    }
}
