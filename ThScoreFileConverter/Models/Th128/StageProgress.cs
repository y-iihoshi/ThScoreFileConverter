//-----------------------------------------------------------------------
// <copyright file="StageProgress.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable CA1707 // Identifiers should not contain underscores

namespace ThScoreFileConverter.Models.Th128
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
        /// Lost at stage A-1.
        /// </summary>
        [EnumAltName("Stage A-1")]
        A_1,

        /// <summary>
        /// Lost at stage A1-2.
        /// </summary>
        [EnumAltName("Stage A1-2")]
        A1_2,

        /// <summary>
        /// Lost at stage A1-3.
        /// </summary>
        [EnumAltName("Stage A1-3")]
        A1_3,

        /// <summary>
        /// Lost at stage A2-2.
        /// </summary>
        [EnumAltName("Stage A2-2")]
        A2_2,

        /// <summary>
        /// Lost at stage A2-3.
        /// </summary>
        [EnumAltName("Stage A2-3")]
        A2_3,

        /// <summary>
        /// Lost at stage B-1.
        /// </summary>
        [EnumAltName("Stage B-1")]
        B_1,

        /// <summary>
        /// Lost at stage B1-2.
        /// </summary>
        [EnumAltName("Stage B1-2")]
        B1_2,

        /// <summary>
        /// Lost at stage B1-3.
        /// </summary>
        [EnumAltName("Stage B1-3")]
        B1_3,

        /// <summary>
        /// Lost at stage B2-2.
        /// </summary>
        [EnumAltName("Stage B2-2")]
        B2_2,

        /// <summary>
        /// Lost at stage B2-3.
        /// </summary>
        [EnumAltName("Stage B2-3")]
        B2_3,

        /// <summary>
        /// Lost at stage C-1.
        /// </summary>
        [EnumAltName("Stage C-1")]
        C_1,

        /// <summary>
        /// Lost at stage C1-2.
        /// </summary>
        [EnumAltName("Stage C1-2")]
        C1_2,

        /// <summary>
        /// Lost at stage C1-3.
        /// </summary>
        [EnumAltName("Stage C1-3")]
        C1_3,

        /// <summary>
        /// Lost at stage C2-2.
        /// </summary>
        [EnumAltName("Stage C2-2")]
        C2_2,

        /// <summary>
        /// Lost at stage C2-3.
        /// </summary>
        [EnumAltName("Stage C2-3")]
        C2_3,

        /// <summary>
        /// Lost at stage Extra.
        /// </summary>
        [EnumAltName("Extra Stage")]
        Extra,

        /// <summary>
        /// Cleared route A1.
        /// </summary>
        [EnumAltName("A1 Clear")]
        A1Clear,

        /// <summary>
        /// Cleared route A2.
        /// </summary>
        [EnumAltName("A2 Clear")]
        A2Clear,

        /// <summary>
        /// Cleared route B1.
        /// </summary>
        [EnumAltName("B1 Clear")]
        B1Clear,

        /// <summary>
        /// Cleared route B2.
        /// </summary>
        [EnumAltName("B2 Clear")]
        B2Clear,

        /// <summary>
        /// Cleared route C1.
        /// </summary>
        [EnumAltName("C1 Clear")]
        C1Clear,

        /// <summary>
        /// Cleared route C2.
        /// </summary>
        [EnumAltName("C2 Clear")]
        C2Clear,

        /// <summary>
        /// Cleared route EX.
        /// </summary>
        [EnumAltName("Extra Clear")]
        ExtraClear,
    }
}
