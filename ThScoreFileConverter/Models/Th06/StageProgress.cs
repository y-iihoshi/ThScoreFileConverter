//-----------------------------------------------------------------------
// <copyright file="StageProgress.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th06
{
    /// <summary>
    /// Represents a stage progress of a gameplay.
    /// </summary>
    public enum StageProgress
    {
        /// <summary>
        /// Not played yet.
        /// </summary>
        [EnumAltName("-------")]
        None,

        /// <summary>
        /// Lost at stage 1.
        /// </summary>
        [EnumAltName("Stage 1")]
        St1,

        /// <summary>
        /// Lost at stage 2.
        /// </summary>
        [EnumAltName("Stage 2")]
        St2,

        /// <summary>
        /// Lost at stage 3.
        /// </summary>
        [EnumAltName("Stage 3")]
        St3,

        /// <summary>
        /// Lost at stage 4.
        /// </summary>
        [EnumAltName("Stage 4")]
        St4,

        /// <summary>
        /// Lost at stage 5.
        /// </summary>
        [EnumAltName("Stage 5")]
        St5,

        /// <summary>
        /// Lost at stage 6.
        /// </summary>
        [EnumAltName("Stage 6")]
        St6,

        /// <summary>
        /// Lost at Extra stage.
        /// </summary>
        [EnumAltName("Extra Stage")]
        Extra,

        /// <summary>
        /// All cleared.
        /// </summary>
        [EnumAltName("All Clear")]
        Clear = 99,
    }
}
