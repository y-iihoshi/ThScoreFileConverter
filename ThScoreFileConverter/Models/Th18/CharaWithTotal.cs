//-----------------------------------------------------------------------
// <copyright file="CharaWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th18
{
    /// <summary>
    /// Represents player characters of UM and total.
    /// </summary>
    public enum CharaWithTotal
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
        /// Izayoi Sakuya.
        /// </summary>
        [EnumAltName("SK")]
        Sakuya,

        /// <summary>
        /// Kochiya Sanae.
        /// </summary>
        [EnumAltName("SN")]
        Sanae,

        /// <summary>
        /// Represents total across characters.
        /// </summary>
        [EnumAltName("TL")]
        Total,
    }
}
