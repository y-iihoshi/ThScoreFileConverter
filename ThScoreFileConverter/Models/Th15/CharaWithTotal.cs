//-----------------------------------------------------------------------
// <copyright file="CharaWithTotal.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th15
{
    /// <summary>
    /// Represents playable characters of LoLK and total.
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
        /// Kochiya Sanae.
        /// </summary>
        [EnumAltName("SN")]
        Sanae,

        /// <summary>
        /// Reisen Udongein Inaba.
        /// </summary>
        [EnumAltName("RS")]
        Reisen,

        /// <summary>
        /// Represents total across characters.
        /// </summary>
        [EnumAltName("TL")]
        Total,
    }
}
