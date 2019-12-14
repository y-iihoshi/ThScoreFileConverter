//-----------------------------------------------------------------------
// <copyright file="Level.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

namespace ThScoreFileConverter.Models.Th155
{
    /// <summary>
    /// Represents levels of AoCF.
    /// </summary>
    public enum Level
    {
        /// <summary>
        /// Represents level Easy.
        /// </summary>
        [EnumAltName("E")]
        Easy,

        /// <summary>
        /// Represents level Normal.
        /// </summary>
        [EnumAltName("N")]
        Normal,

        /// <summary>
        /// Represents level Hard.
        /// </summary>
        [EnumAltName("H")]
        Hard,

        /// <summary>
        /// Represents level Lunatic.
        /// </summary>
        [EnumAltName("L")]
        Lunatic,

        /// <summary>
        /// Represents level OverDrive.
        /// </summary>
        [EnumAltName("D", LongName = "Over Drive")]
        OverDrive,
    }
}
