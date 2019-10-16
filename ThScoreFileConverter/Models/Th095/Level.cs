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
        One,

        /// <summary>
        /// Represents level 2.
        /// </summary>
        [EnumAltName("2", LongName = "02")]
        Two,

        /// <summary>
        /// Represents level 3.
        /// </summary>
        [EnumAltName("3", LongName = "03")]
        Three,

        /// <summary>
        /// Represents level 4.
        /// </summary>
        [EnumAltName("4", LongName = "04")]
        Four,

        /// <summary>
        /// Represents level 5.
        /// </summary>
        [EnumAltName("5", LongName = "05")]
        Five,

        /// <summary>
        /// Represents level 6.
        /// </summary>
        [EnumAltName("6", LongName = "06")]
        Six,

        /// <summary>
        /// Represents level 7.
        /// </summary>
        [EnumAltName("7", LongName = "07")]
        Seven,

        /// <summary>
        /// Represents level 8.
        /// </summary>
        [EnumAltName("8", LongName = "08")]
        Eight,

        /// <summary>
        /// Represents level 9.
        /// </summary>
        [EnumAltName("9", LongName = "09")]
        Nine,

        /// <summary>
        /// Represents level 10.
        /// </summary>
        [EnumAltName("0", LongName = "10")]
        Ten,

        /// <summary>
        /// Represents level Extra.
        /// </summary>
        [EnumAltName("X", LongName = "ex")]
        Extra,
    }
}
