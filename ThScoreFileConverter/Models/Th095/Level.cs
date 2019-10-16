//-----------------------------------------------------------------------
// <copyright file="Level.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th095
{
    /// <summary>
    /// Represents levels of StB.
    /// </summary>
    public enum Level
    {
        /// <summary>
        /// Represents level 1.
        /// </summary>
        [EnumAltName("1", LongName = "01")]
        Lv1,

        /// <summary>
        /// Represents level 2.
        /// </summary>
        [EnumAltName("2", LongName = "02")]
        Lv2,

        /// <summary>
        /// Represents level 3.
        /// </summary>
        [EnumAltName("3", LongName = "03")]
        Lv3,

        /// <summary>
        /// Represents level 4.
        /// </summary>
        [EnumAltName("4", LongName = "04")]
        Lv4,

        /// <summary>
        /// Represents level 5.
        /// </summary>
        [EnumAltName("5", LongName = "05")]
        Lv5,

        /// <summary>
        /// Represents level 6.
        /// </summary>
        [EnumAltName("6", LongName = "06")]
        Lv6,

        /// <summary>
        /// Represents level 7.
        /// </summary>
        [EnumAltName("7", LongName = "07")]
        Lv7,

        /// <summary>
        /// Represents level 8.
        /// </summary>
        [EnumAltName("8", LongName = "08")]
        Lv8,

        /// <summary>
        /// Represents level 9.
        /// </summary>
        [EnumAltName("9", LongName = "09")]
        Lv9,

        /// <summary>
        /// Represents level 10.
        /// </summary>
        [EnumAltName("0", LongName = "10")]
        Lv10,

        /// <summary>
        /// Represents level Extra.
        /// </summary>
        [EnumAltName("X", LongName = "ex")]
        Extra,
    }
}
