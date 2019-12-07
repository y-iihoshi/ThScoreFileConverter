//-----------------------------------------------------------------------
// <copyright file="Chara.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th17
{
    /// <summary>
    /// Represents player characters of WBWC.
    /// </summary>
    public enum Chara
    {
        /// <summary>
        /// Hakurei Reimu (Wolf spirit).
        /// </summary>
        [EnumAltName("RA")]
        ReimuA,

        /// <summary>
        /// Hakurei Reimu (Otter spirit).
        /// </summary>
        [EnumAltName("RB")]
        ReimuB,

        /// <summary>
        /// Hakurei Reimu (Eagle spirit).
        /// </summary>
        [EnumAltName("RC")]
        ReimuC,

        /// <summary>
        /// Kirisame Reimu (Wolf spirit).
        /// </summary>
        [EnumAltName("MA")]
        MarisaA,

        /// <summary>
        /// Kirisame Marisa (Otter spirit).
        /// </summary>
        [EnumAltName("MB")]
        MarisaB,

        /// <summary>
        /// Kirisame Marisa (Eagle spirit).
        /// </summary>
        [EnumAltName("MC")]
        MarisaC,

        /// <summary>
        /// Konpaku Youmu (Wolf spirit).
        /// </summary>
        [EnumAltName("YA")]
        YoumuA,

        /// <summary>
        /// Konpaku Youmu (Otter spirit).
        /// </summary>
        [EnumAltName("YB")]
        YoumuB,

        /// <summary>
        /// Konpaku Youmu (Eagle spirit).
        /// </summary>
        [EnumAltName("YC")]
        YoumuC,
    }
}
