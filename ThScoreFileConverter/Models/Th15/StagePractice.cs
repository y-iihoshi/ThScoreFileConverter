//-----------------------------------------------------------------------
// <copyright file="StagePractice.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th15
{
    /// <summary>
    /// Represents stages of LoLK.
    /// </summary>
    public enum StagePractice
    {
        /// <summary>
        /// Represents stage 1.
        /// </summary>
        [EnumAltName("1")]
        St1,

        /// <summary>
        /// Represents stage 2.
        /// </summary>
        [EnumAltName("2")]
        St2,

        /// <summary>
        /// Represents stage 3.
        /// </summary>
        [EnumAltName("3")]
        St3,

        /// <summary>
        /// Represents stage 4.
        /// </summary>
        [EnumAltName("4")]
        St4,

        /// <summary>
        /// Represents stage 5.
        /// </summary>
        [EnumAltName("5")]
        St5,

        /// <summary>
        /// Represents stage 6.
        /// </summary>
        [EnumAltName("6")]
        St6,

        /// <summary>
        /// Represents Extra stage.
        /// </summary>
        [EnumAltName("X")]
        Extra,

        /// <summary>
        /// Not used.
        /// </summary>
        [EnumAltName("-")]
        NotUsed,
    }
}
